using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class Conexion
    {
        public SqlConnection obtenConexion()
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conStr);
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
            }
           
            
        }

        public DataSet obtenConsulta(string Consulta)
        {
            string conStr = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;
            DataSet ds = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlDataAdapter adap = new SqlDataAdapter(Consulta, sqlConnection);
            adap.Fill(ds);
            return ds;
        }

    }
}
