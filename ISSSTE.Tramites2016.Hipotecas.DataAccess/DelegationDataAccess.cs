using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class DelegationDataAccess
    {
        private Conexion con = new Conexion();
        public List<Delegation> GetDelegations()
        {
            List<Delegation> lstDelegations = new List<Delegation>();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getDelegations", conexion);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Delegation delegation = new Delegation();
                        delegation.DelegationId = (int) reader["DelegationId"];
                        delegation.Name = (string)reader["Name"];
                        lstDelegations.Add(delegation);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                //reader.Close();
                conexion.Close();
                return lstDelegations;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
