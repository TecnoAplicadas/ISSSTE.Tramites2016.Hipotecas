using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Xml;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.ComponentModel;
namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class Conection
    {
        static public SqlConnection obtenConexion()
        {
            SqlConnection.ClearAllPools();

            SqlConnectionStringBuilder context = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["HipotecasConection"].ConnectionString);

            return new SqlConnection(context.ToString());
        }

        //static public SqlConnection obtenConexion()
        //{
        //    SqlConnection.ClearAllPools();
        //    SqlConnectionStringBuilder conn2 = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["HipotecasConection"].ConnectionString);
        //    //SqlConnectionStringBuilder conn2 = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["SosConnectionStringLalo"].ConnectionString);
        //    string pass = conn2.Password;
        //    string[] usuario = conn2.UserID.Split("\\".ToCharArray()[0]);
        //    // El primer parametro es el usuario, el segundo el dominio, el tercero la contraseña
        //    WrapperImpersonationContext context = new WrapperImpersonationContext(usuario[0], usuario[1], pass);
        //    //WrapperImpersonationContext context = new WrapperImpersonationContext(usuario[1], usuario[0], pass);
        //    context.Enter();
        //    return new SqlConnection(conn2.ConnectionString);
        //}
        public class WrapperImpersonationContext
        {
            [DllImport("advapi32.dll", SetLastError = true)]
            public static extern bool LogonUser(String lpszUsername, String lpszDomain,
            String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public extern static bool CloseHandle(IntPtr handle);

            private const int LOGON32_PROVIDER_DEFAULT = 0; private const int LOGON32_LOGON_INTERACTIVE = 2;

            private string m_Domain;
            private string m_Password;
            private string m_Username;
            private IntPtr m_Token;

            private WindowsImpersonationContext m_Context = null;

            protected bool IsInContext
            {
                get { return m_Context != null; }
            }

            public WrapperImpersonationContext(string domain, string username, string password)
            {
                m_Domain = domain;
                m_Username = username;
                m_Password = password;
            }

            [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
            public void Enter()
            {
                if (this.IsInContext) return;
                m_Token = new IntPtr(0);
                try
                {
                    m_Token = IntPtr.Zero;
                    bool logonSuccessfull = LogonUser(
                       m_Username,
                       m_Domain,
                       m_Password,
                       LOGON32_LOGON_INTERACTIVE,
                       LOGON32_PROVIDER_DEFAULT,
                       ref m_Token);
                    if (logonSuccessfull == false)
                    {
                        int error = Marshal.GetLastWin32Error();
                        throw new Win32Exception(error);
                    }
                    WindowsIdentity identity = new WindowsIdentity(m_Token);
                    m_Context = identity.Impersonate();
                }
                catch (Exception)
                {
                    throw;
                }
            }


            [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
            public void Leave()
            {
                if (this.IsInContext == false) return;
                m_Context.Undo();

                if (m_Token != IntPtr.Zero) CloseHandle(m_Token);
                m_Context = null;
            }
        }


        public DataSet ObtenerConsulta(string consulta) //Obtiene un dataset de una consulta a la base de datos sosconnectionstring
        {
            DataSet DS = new DataSet();
            SqlConnection conex2 = obtenConexion();
            conex2.Open();
            SqlDataAdapter adap = new SqlDataAdapter(consulta, conex2);
            adap.Fill(DS);
            return DS;
        }

        public SqlCommand ObtenerSelect(string consulta) //Obtiene un dataset de una consulta a la base de datos sosconnectionstring
        {
            String DS;
            SqlConnection conex2 = obtenConexion();
            conex2.Open();
            SqlCommand adap = new SqlCommand(consulta, conex2);

            return adap;
        }

        //    public SqlCommand ConectionFile(String Source, String id, String Password) //Obtiene un dataset de una consulta a la base de datos sosconnectionstring
        //    {
        //        SqlConnection conex2 = new SqlConnection();
        //        conex2.ConnectionString = "Data Source="+ Source+ "; Persist Security info=false; User id="+ id + "; password="+Password+"; Initial Catalog=TramiteHipotecas";
        //        conex2.Open();
        //        SqlCommand coman = new SqlCommand();
        //        coman.Connection = conex2;
        //        return coman;
        //    }
        //}
    }
}