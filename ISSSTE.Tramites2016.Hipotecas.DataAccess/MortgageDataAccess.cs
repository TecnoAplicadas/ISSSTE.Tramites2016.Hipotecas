using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class MortgageDataAccess
    {

        private Conexion con = new Conexion();

        //Llamada al StoreProcedure que obtiene los datos para el reporte de Cancelación de Hipoteca
        public MortgageCancelReportData GetMortgageCancelByRequestId(Guid RequestId)
        {
            Model.Pocos.MortgageCancelReportData data = new Model.Pocos.MortgageCancelReportData();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getMortgageCancelByRequestId", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@RequestId", SqlDbType.UniqueIdentifier).Value = RequestId;          

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Name = (string)reader["Name"];
                        data.City = (string)reader["City"];
                        data.Date = (DateTime)reader["Date"];
                        data.WritingProperty = (string)reader["WritingProperty"];
                        data.Telephone = (string)reader["Telephone"];
                        data.MobileTelephone = (string)reader["MobilePhone"];
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                conexion.Close();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}
