using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Escrituracion.Models.Model;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class DocumentDataAccess
    {
        private Conexion con = new Conexion();
        public void SaveDocument(byte[] file, string extencion, string requestId, int documentType)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_insertDocument", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DocumentId", Guid.NewGuid());
            command.Parameters.Add("@DocumentTypeId", SqlDbType.Int).Value = documentType;
            //command.Parameters.Add("@IsValid", SqlDbType.VarChar).Value = null;
            command.Parameters.AddWithValue("@RequestId", requestId);
            command.Parameters.Add("@Observations", SqlDbType.VarChar).Value = "";
            command.Parameters.Add("@Data", SqlDbType.VarBinary).Value = file;
            command.Parameters.Add("@FileExtension", SqlDbType.VarChar).Value = extencion;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();

        }

        public List<DocumentsAdministrator> GetListDocumentsByRequestId(Guid requestId)
        {
            List<DocumentsAdministrator> documents = new List<DocumentsAdministrator>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getDocumentsByRequestId", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestId", requestId);

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DocumentsAdministrator document = new DocumentsAdministrator();
                        document.DocumentId = (Guid)reader["DocumentId"];
                        document.DocumentTypeId = (int)reader["DocumentTypeId"];
                        var isValid = reader["IsValid"];
                        if (isValid == null || isValid.ToString() == string.Empty)
                        {
                            document.IsValid = false;
                        }
                        else
                        {
                            document.IsValid = Convert.ToBoolean(reader["IsValid"]);
                        }
                        document.RequestId = (Guid)reader["RequestId"];
                        document.Observations = (string)reader["Observations"];
                        var image = (byte[])reader["Data"];
                        //byte[] result = Convert.FromBase64String(image);
                        document.Data = image;
                        document.Description = (string)reader["Description"];

                        documents.Add(document);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //reader.Close();
                conexion.Close();
                return documents;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Documents GetImage(Guid documentId, int documentTypeId)
        {
            Documents document = new Documents();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getImageByDocumentId", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DocumentId", documentId);
            command.Parameters.AddWithValue("@DocumentTypeId", documentTypeId);

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        document.DocumentId = (Guid)reader["DocumentId"];
                        document.FileExtension = (string)reader["FileExtension"];
                        var image = (byte[])reader["Data"];
                        document.Data = image;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //reader.Close();
                conexion.Close();
                return document;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<Seguimiento> GetResult(Guid RequestId)
        {
            List<Seguimiento> documents = new List<Seguimiento>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sps_Seguimiento", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestId", RequestId);

            
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Seguimiento document = new Seguimiento();
                        document.RequestId = (Guid)reader["RequestId"];
                        document.Name = reader["Name"].ToString();
                        document.Folio = reader["Folio"].ToString();
                        document.Date = Convert.ToDateTime(reader["Date"].ToString());
                        // document.DocumentType = (string)reader["Description"];
                        //string isValid = reader["IsValid"].ToString();
                        //if (isValid == null)
                        //{
                        //    document.IsValid = "En Proceso";
                        //}
                        //else if (isValid.Equals("0"))
                        //{
                        //    document.IsValid = "Rechazado";
                        //}
                        //else if (isValid.Equals("1"))
                        //{
                        //    document.IsValid = "Aprobado";
                        //}
                        //if (reader["Observations"] == DBNull.Value)
                        //{
                        //    document.Observations = "";
                        //}
                        //else
                        //{
                        //    document.Observations = (string)reader["Observations"];
                        //}

                        documents.Add(document);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //reader.Close();
                conexion.Close();
                return documents;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

        }


    }
}
