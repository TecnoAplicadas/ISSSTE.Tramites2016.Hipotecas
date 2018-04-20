
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
//using ISSSTE.Tramites2016.Hipotecas.DataAccess;


namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
   public class UpdateStatus
    {
       // private Conexion con = new Conexion();
        public UpdateStatus()
        {
                
        }
        public int NextStatus { get; set; }
        public int RelatesStatusId { get; set; }
        public string Name { get; set; }

        //public List<UpdateStatus> GetListDocumentsByRequestId(Guid requestId)
        //{
        //    List<UpdateStatus> getStatus = new List<UpdateStatus>();
        //    var conexion = con.obtenConexion();
        //    SqlCommand command = new SqlCommand("sp_getRequestStatusById", conexion);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddWithValue("@RequestId", requestId);
        //    try
        //    {
        //        conexion.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                UpdateStatus fStatus = new UpdateStatus();
        //                fStatus.NextStatus = (int)reader["NextStatusId"];
        //                //fStatus.RelatesStatusId = (int)reader["RelatesStatusId"];
        //                fStatus.Name = (string)reader["Name"];
        //                var isValid = reader["IsValid"];
        //                getStatus.Add(fStatus);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No rows found.");
        //        }
        //        //reader.Close();
        //        conexion.Close();
        //        return getStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

       // }

    }
}
