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
   public class UpdateStatusDataAcces
    {
      
        public static String RequestId;
        private Conexion con = new Conexion();
        public List<Model.Model.UpdateStatus> updateStatusbyId(Guid requestId)
        {
            List<Model.Model.UpdateStatus> getStatus = new List<Model.Model.UpdateStatus>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getRequestStatusById", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idRequest", requestId);
            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.Model.UpdateStatus fStatus = new Model.Model.UpdateStatus();
                        //fStatus.NextStatus = (int)reader["RelatesStatusid"];
                        //fStatus.RelatesStatusId = (int)reader["RelatesStatusId"];
                        fStatus.Name = (string)reader["name"];
                        fStatus.RelatesStatusId = (int)reader["RelatesStatusId"];
                        getStatus.Add(fStatus);
                        //..
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //reader.Close();
                conexion.Close();
                return getStatus;
            }
            catch (Exception ex)
            {
               
                return null;
            }

        }
    }
}
