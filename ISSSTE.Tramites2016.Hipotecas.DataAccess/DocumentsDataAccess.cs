using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class DocumentsDataAccess
    {
        Conection con = new Conection();
        public static String RequestId;
        public void uploadFile(byte[] file, string extencion, string requestId, int documentType)
        {

            // DataSet ds = new DataSet();

            // string query = "spI_DocumentRequest @DocumentTypeId =" + documentType +
            // ", @FileExtension ='" + extencion +
            // "' ,@Data ='" + BitConverter.ToString(file) +
            //"', @RequestId= '" + requestId + "'";

            // con.ObtenerConsulta(query);

            //con.ObtenerConsulta
            //SqlConnection connection = new SqlConnection("...");
            //connection.Open();
            SqlConnection conex2 = Conection.obtenConexion();
            conex2.Open();
            using (var sqlWrite = new SqlCommand("spI_DocumentRequest", conex2))
            {
                sqlWrite.CommandType = CommandType.StoredProcedure;
                 sqlWrite.Parameters.Add("@DocumentTypeId", SqlDbType.Int, documentType).Value = documentType;
                sqlWrite.Parameters.Add("@RequestId", SqlDbType.VarChar, 100).Value = requestId;
                sqlWrite.Parameters.Add("@FileExtension", SqlDbType.VarChar, 20).Value = extencion;
                sqlWrite.Parameters.Add("@Data", SqlDbType.VarBinary, file.Length).Value = file;

                sqlWrite.ExecuteNonQuery();
            }

            conex2.Close();

        }

    }
}
