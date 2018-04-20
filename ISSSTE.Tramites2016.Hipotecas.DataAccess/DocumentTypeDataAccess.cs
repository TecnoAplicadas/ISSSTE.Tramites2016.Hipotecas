using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class DocumentTypeDataAccess
    {
        private Conexion con = new Conexion();
        public List<DocumentType> GetDocuments()
        {


            List<DocumentType> documents = new List<DocumentType>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getDocumentsTypeDescription", conexion);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DocumentType documentType = new DocumentType();
                        documentType.DocumentTypeId = (int)reader["DocumentTypeId"];
                        documentType.Name = (string)reader["Name"];
                        documentType.Description = (string)reader["Description"];
                        documentType.BeneficiarieType = reader["BeneficiarieType"].ToString() == "null" || reader["BeneficiarieType"].ToString() == string.Empty ? 1 : (int)reader["BeneficiarieType"];
                        documentType.Required = (bool)reader["Required"];

                        documents.Add(documentType);
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
        public List<DocumentType> GetDocuments(string tipoCred)
        {


            List<DocumentType> documents = new List<DocumentType>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getDocumentsTypeDescriptionByType", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Type", SqlDbType.VarChar).Value = tipoCred;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DocumentType documentType = new DocumentType();
                        documentType.DocumentTypeId = (int)reader["DocumentTypeId"];
                        documentType.Name = (string)reader["Name"];
                        documentType.Description = (string)reader["Description"];
                        documentType.BeneficiarieType = reader["BeneficiarieType"].ToString() == "null" || reader["BeneficiarieType"].ToString() == string.Empty ? 1 : (int)reader["BeneficiarieType"];
                        documentType.Required = (bool)reader["Required"];

                        documents.Add(documentType);
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
    }
}
