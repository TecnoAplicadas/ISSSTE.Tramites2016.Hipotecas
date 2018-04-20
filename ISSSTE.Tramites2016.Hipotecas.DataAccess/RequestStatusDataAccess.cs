using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Common.Mail;
using System.Configuration;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class RequestStatusDataAccess
    {
        private Conexion con = new Conexion();


        public void SaveRequestStatus(Guid requestId, int statusId)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_insertRequestStatus", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestStatusId", Guid.NewGuid());
            command.Parameters.AddWithValue("@RequestId", requestId);
            command.Parameters.Add("@StatusId", SqlDbType.Int).Value = statusId;
            command.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@IsCurrentStatus", SqlDbType.Bit).Value = true;
            command.Parameters.Add("@Observations", SqlDbType.VarChar).Value = string.Empty;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();




        }

        public void UpdateRequestStatus(int status, string requestId)
        {
            var conexion = con.obtenConexion();
            Guid reGuid = new Guid(requestId);
            SqlCommand command = new SqlCommand("sp_updateRequestStatus", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@StatusId", SqlDbType.Int).Value = status;
            command.Parameters.AddWithValue("@RequestId", reGuid);
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();

        }
        public void SaveStatusNext(int status, string requestId)
        {
            var conexion = con.obtenConexion();
            Guid reGuid = new Guid(requestId);
            SqlCommand command = new SqlCommand("spSU_NextStatus", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@StatusId", SqlDbType.Int).Value = status;
            command.Parameters.AddWithValue("@RequestId", reGuid);
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();

        }
        public int GetActivesRequests(string noissste)
        {
            DataSet ds = new DataSet();
            string query = "spS_ActiveRequest @noissste=" + noissste;
            ds = con.obtenConsulta(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int dt = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());

                return dt;
            }
            else { return 0; }
        }



        public DataTable RelatedRequestStatus(Guid newRequestId, int status)
        {
            DataSet ds = new DataSet();
            string query = "spS_StatusRelatedStatus @StatusId=" + status;
            ds = con.obtenConsulta(query);

            //var conexion = con.obtenConexion();
            //SqlCommand command = new SqlCommand("spS_StatusRelatedStatus", conexion);
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.Add("@StatusId", SqlDbType.Int).Value = status;
            ////command.Parameters.AddWithValue("@RequestId", reGuid);
            //conexion.Open();
            //SqlDataReader reader = command.ExecuteReader();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                SaveRequestStatus(newRequestId, Convert.ToInt32(dr["RelatesStatusId"].ToString()));
            }
            return dt;
        }


    }
}
