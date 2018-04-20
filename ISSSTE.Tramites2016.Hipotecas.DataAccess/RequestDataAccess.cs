using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.AccessControl;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Common.Mail;
using System.Configuration;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class RequestDataAccess
    {
        private Conexion con = new Conexion();
        Conection conex = new Conection();
        DataSet ds = new DataSet();
        public void SaveRequest(Request request)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_insertRequests", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestId", request.RequestId);
            command.Parameters.Add("@EntitleId", SqlDbType.VarChar).Value = request.EntitleId;
            command.Parameters.Add("@Folio", SqlDbType.VarChar).Value = request.Folio;
            command.Parameters.Add("@IsComplete", SqlDbType.Bit).Value = request.IsComplete;
            command.Parameters.Add("@Date", SqlDbType.DateTime).Value = request.Date;
            command.Parameters.Add("@IdLegalUnit", SqlDbType.Int).Value = request.IdLegalUnit;
            command.Parameters.Add("@WritingProperty", SqlDbType.VarChar).Value = request.WritingProperty;
            command.Parameters.Add("@IsConjugalCredit", SqlDbType.Bit).Value = request.IsConjugalCredit;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();
        }

        public void UpdateRequestNoComplete(string RequestId)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_updateRequestsNoComplete", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestId", RequestId);

            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();

           // SenMail(RequestId.ToString(), Convert.ToInt32(StatusEnum.EnesperadeAgendarCiraDer));

        }

    
        public void UpdateRequest(string RequestId, string Writing, bool TypeCredit)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_updateRequests", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RequestId", RequestId);
            command.Parameters.Add("@WritingProperty", SqlDbType.VarChar).Value = Writing;
            command.Parameters.Add("@IsConjugalCredit", SqlDbType.Bit).Value = TypeCredit;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();
        }
        /*-------------------------*/
        public void UpdateRequestStatus(string RequestId, string StatusId)
        {
            var conexion = con.obtenConexion();

           
            
            try
            {
                SqlCommand command = new SqlCommand("sp_updateRequestStatus", conexion);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequestId", RequestId);
                command.Parameters.AddWithValue("@StatusId", StatusId);
                //command.Parameters.Add("@WritingProperty", SqlDbType.VarChar).Value = StatusID;
                conexion.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                conexion.Close();
            }

        }




        /*-----------------------------*/



        public Request GetRequestByRequestId(Guid requestId)
        {
            Request request = new Request();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getRequestById", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@RequestId", SqlDbType.UniqueIdentifier).Value = requestId;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        request.EntitleId = reader["EntitleId"].ToString(); // MFP
                        request.Folio = (string)reader["Folio"];
                        request.IsComplete = Convert.ToBoolean(reader["IsComplete"].ToString());
                        request.Date = (DateTime)reader["Date"];
                        request.WritingProperty = (string)reader["WritingProperty"];
                        request.IsConjugalCredit = (bool)reader["IsConjugalCredit"];

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public Request GetRequestByEntitleId(string entitleId)
        {
            Request request = new Request();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getRequestByEntitle", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EntitleId", SqlDbType.VarChar).Value = entitleId;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        request.EntitleId = reader["EntitleId"].ToString(); // MFP
                        request.Folio = (string)reader["Folio"];
                        request.IsComplete = Convert.ToBoolean(reader["IsComplete"].ToString());
                        request.Date = (DateTime)reader["Date"];
                        request.WritingProperty = (string)reader["WritingProperty"];
                        request.IsConjugalCredit = (bool)reader["IsConjugalCredit"];

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetRequestIdByNoIssste(string NoIssste)
        {
            Request request = new Request();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getRequestIdByNoIssste", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NoIssste", SqlDbType.VarChar).Value = NoIssste;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        request.RequestId = (Guid)reader["RequestId"];


                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return request.RequestId.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CivilState> GetCivilState()
        {
            List<CivilState> lcs = new List<CivilState>();
            string query = "exec sp_getCivilState";
            ds = con.obtenConsulta(query);

            try
            {
                //conexion.Open();
                //SqlDataReader reader = command.ExecuteReader();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CivilState cvcat = new CivilState();
                    cvcat.CivilStateId = Convert.ToInt32(dr["CivilStateId"].ToString());
                    cvcat.Key = dr["Key"].ToString();
                    cvcat.Name = dr["Name"].ToString();
                    cvcat.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

                    lcs.Add(cvcat);

                }


                return lcs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //
        public List<RequestAdministrator> GetAllRequestHistoria(string NoIssste)
        {
            List<RequestAdministrator> lstRequests = new List<RequestAdministrator>();
            //var conexion = con.obtenConexion();
            //SqlCommand command = new SqlCommand("sp_getAllRequestHistory", conexion);
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.Add("@NoIssste", SqlDbType.VarChar).Value = NoIssste;

            string query = "exec sp_getAllRequestHistory @NoIssste='" + NoIssste + "'";
            ds = con.obtenConsulta(query);

            try
            {
                //conexion.Open();
                //SqlDataReader reader = command.ExecuteReader();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RequestAdministrator lector = new RequestAdministrator();

                    lector.RequestId = Guid.Parse(dr["RequestId"].ToString());
                    lector.Folio = dr["Folio"].ToString();
                    lector.CURP = dr["CURP"].ToString();
                    lector.NoISSSTE = dr["NoISSSTE"].ToString();
                    lector.Name = dr["Name"].ToString();
                    lector.Estatus = dr["estatus"].ToString();
                    lector.Date = dr["Date"].ToString();
                    lector.StatusId = Convert.ToInt32(dr["StatusId"].ToString());

                    lstRequests.Add(lector);
                }


                return lstRequests;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<RequestAdministrator> GetAllRequest()
        {
            List<RequestAdministrator> lstRequests = new List<RequestAdministrator>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getAllRequest", conexion);
            command.CommandType = CommandType.StoredProcedure;


            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        RequestAdministrator request = new RequestAdministrator();
                        request.RequestId = (Guid)reader["RequestId"];
                        request.Folio = (string)reader["Folio"];
                        request.CURP = (string)reader["CURP"];
                        request.NoISSSTE = (string)reader["NoISSSTE"];
                        request.Name = (string)reader["Name"];
                        request.Estatus = (string)reader["estatus"];
                        request.Date = (string)reader["Date"];
                        lstRequests.Add(request);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return lstRequests;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public RequestAdministrator GetHistoryRequestByRequestId(string requestId)
        {
            RequestAdministrator request = new RequestAdministrator();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getHistoryRequest", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@RequestId", SqlDbType.VarChar).Value = requestId;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {


                        request.RequestId = (Guid)reader["RequestId"];
                        request.Folio = (string)reader["Folio"];
                        request.CURP = (string)reader["CURP"];
                        request.NoISSSTE = (string)reader["NoISSSTE"];
                        request.Name = (string)reader["Name"];
                        request.Estatus = (string)reader["estatus"];
                        request.Date = (string)reader["Date"];

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
