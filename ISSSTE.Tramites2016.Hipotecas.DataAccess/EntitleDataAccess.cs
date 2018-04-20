using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System.Data.Common;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Common.Mail;
using System.Configuration;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class EntitleDataAccess

    {

        private Conexion con = new Conexion();
        Conection conex = new Conection();
        public DbCommand comando = null;

        public static Boolean bandera = false;
        public static String RequestId;



        





        //public String saveInformationEntitle(Entitle index)
        //{
        //    String Exp;
        //    try
        //    {
        //        DataSet ds = new DataSet();

        //        string query = "exec sp_saveInformationEntitle   @EntitleId='" + index.EntitleId
        //           + "' ,@Lada = '" + index.Lada
        //           + "', @Email = '" + index.Email
        //           + "' , @Telephone = '" + index.Telephone
        //           + "' , @Name = '" + index.Name
        //           + "' , @PaternalLastName = '" + index.PaternalLastName
        //           + "' , @MaternalLastName = '" + index.MaternalLastName
        //           + "' , @Gender = '" + index.Gender
        //           + "' , @Birthdate = '" + Convert.ToDateTime(index.Birthdate).ToString("yyyy-MM-dd")
        //           + "' , @Birthplace = '" + index.Birthplace
        //           + "' , @ZipCode = '" + index.ZipCode
        //           + "' , @City = '" + index.City
        //           + "' , @Colony = '" + index.Colony
        //           + "' , @Street = '" + index.Street
        //           + "' , @NumInt = '" + index.NumInt
        //           + "' , @NumExt = '" + index.NumExt;




        //        ds = con.ObtenerConsulta(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        Exp = ex.Message;
        //    }

        //    return "";



        //}

        //public void SaveEntitle(Entitle entitle)
        //{
        //    //string conStr = ConfigurationManager.ConnectionStrings["CadenaConexionSQL"].ConnectionString;
        //    //SqlConnection sqlConnection = new SqlConnection(conStr);

        //    string query = "sp_insertEntitle" +
        //    " @EntitleId= '" + entitle.EntitleId +
        //    "' ,@NoIssste= '" + entitle.NoISSSTE +
        //    "'  ,@PaternalLastName='" + entitle.PaternalLastName +
        //    "'  ,@MaternalLastName='" + entitle.MaternalLastName +
        //    "'  ,@Name='" + entitle.Name +
        //    "'  ,@RFC='" + entitle.RFC +
        //    "'  ,@Age =' " + entitle.Age +
        //    "'  ,@BirthDate ='" + Convert.ToDateTime(entitle.Birthdate).ToString("yyyy-MM-dd") +
        //    "'  ,@Gender='" + entitle.Gender +
        //    "'  ,@Street='" + entitle.Street +
        //    "'  ,@NumExt='" + entitle.NumExt +
        //    "'  ,@NumInt='" + entitle.NumInt +
        //    "'  ,@Colony='" + entitle.Colony +
        //    "'  ,@ZipCode='" + entitle.ZipCode +
        //    "'  ,@Email='" + entitle.Email +
        //    "'  ,@Telephone='" + entitle.Telephone +
        //    "'  ,@City='" + entitle.City +
        //    "'  ,@Curp='" + entitle.CURP +
        //    "'  ,@MaritalStatus='" + entitle.MaritalStatus +
        //    "'  ,@BirthPlace='" + entitle.Birthplace +
        //    "'  ,@Lada='" + entitle.Lada +
        //    "'  ,@State='" + entitle.State + "'";
        //    // "@RegimeType=" + entitle.RegimeType +
        //    // "@IsActive", SqlDbType.Bit).Value = entitle.IsActive +


        //    var conexion = con.ObtenerConsulta(query);
        //}

        

        public void ActualizarEntitle(string noIssste, string lada, string telefono, string email, string mobile)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_updateInformationEntitled", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NoIssste", SqlDbType.VarChar).Value = noIssste;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
            command.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = telefono;
            command.Parameters.Add("@Lada", SqlDbType.VarChar).Value = lada;
            command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = mobile;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();

        }

        public void SendMail(string RequestId, int status)
        {         //Implementacion para envio de correo electronico
            string queryMessage = "exec spS_GetMessageByStatus @StatusId = " + status;
            string queryentitle = "exec sp_getEntitleRequest @requestId ='" + RequestId + "'";

            DataTable entitle = conex.ObtenerConsulta(queryentitle).Tables[0];


            string mailMessage = conex.ObtenerConsulta(queryMessage).Tables[0].Rows[0].ItemArray[10].ToString();
            string titleMessage = conex.ObtenerConsulta(queryMessage).Tables[0].Rows[0].ItemArray[2].ToString();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Estimado Derechohabiente: {0} ");
            sb.AppendLine(mailMessage);
            MailService mail = new MailService();
            string nombre = entitle.Rows[0].ItemArray[4].ToString() + " " + entitle.Rows[0].ItemArray[3].ToString() + " " + entitle.Rows[0].ItemArray[2].ToString();
            mail.SendMailAsync(entitle.Rows[0].ItemArray[14].ToString(), titleMessage,
                   String.Format(sb.ToString(), nombre), ConfigurationManager.AppSettings["MailMasterPagePath"].ToString(), ConfigurationManager.AppSettings["MailMasterPageLogoPath"].ToString());
        }
    }
}
