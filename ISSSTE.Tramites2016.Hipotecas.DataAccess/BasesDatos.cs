using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class BasesDatos
    {
        private Conexion con = new Conexion();
        
        public DbCommand comando = null;
        
        
        public DbDataReader EjecutarConsulta()
        {
            
            return this.comando.ExecuteReader();
        }

        public void Actualizar()
        {
            this.comando.ExecuteNonQuery();
        }

        

        public Entitle GetEntitleByCurp(string curp)
        {

            Entitle entitle = new Entitle();
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_getEntitles", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Curp", SqlDbType.VarChar).Value = curp;
            

            try
            {
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        entitle.EntitleId =reader["EntitleId"].ToString();
                        entitle.Age = (int)reader["Age"];
                        entitle.Name = reader["Name"].ToString();
                        entitle.Birthdate = (DateTime?) reader["BirthDate"];
                        entitle.Birthplace = (string) reader["BirthPlace"];
                        entitle.City = (string) reader["City"];
                        entitle.Colony = (string) reader["Colony"];
                        entitle.CURP = (string) reader["CURP"];
                        entitle.Gender = (string) reader["Gender"];
                        entitle.MaritalStatus = (string) reader["MaritalStatus"];
                        entitle.MaternalLastName = (string) reader["MaternalLastName"];
                        entitle.PaternalLastName = (string) reader["PaternalLastName"];
                        entitle.Name = (string) reader["Name"];
                        entitle.NoISSSTE = (string) reader["NoISSSTE"];
                        entitle.NumExt = (string) reader["NumExt"];
                        entitle.NumInt = (string) reader["NumInt"];
                        entitle.RegimeType = reader["RegimeType"] == DBNull.Value ? "" : (string)reader["RegimeType"];
                        entitle.RFC = (string) reader["RFC"];
                        entitle.Street = (string) reader["Street"];
                        entitle.ZipCode = (string) reader["ZipCode"];
                        entitle.IsActive = reader["IsActive"] == DBNull.Value ? false : (bool)reader["IsActive"];
                        entitle.Lada = reader["Lada"] == DBNull.Value ? "" : (string) reader["Lada"];
                        entitle.Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                        entitle.Telephone = reader["Telephone"] == DBNull.Value ? "" : (string)reader["Telephone"];
                        entitle.MobilePhone = reader["MobilePhone"] == DBNull.Value ? "" : (string)reader["MobilePhone"];
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //reader.Close();
                conexion.Close();
                return entitle;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public void UpdateEntitle(Entitle entitle)
        {
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_updateEntitled", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NoIssste", SqlDbType.VarChar).Value = entitle.NoISSSTE;
            command.Parameters.Add("@PaternalLastName", SqlDbType.VarChar).Value = entitle.PaternalLastName;
            command.Parameters.Add("@MaternalLastName", SqlDbType.VarChar).Value = entitle.MaternalLastName;
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = entitle.Name;
            command.Parameters.Add("@RFC", SqlDbType.VarChar).Value = entitle.RFC;
            command.Parameters.Add("@Age", SqlDbType.Int).Value = entitle.Age;
            command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = entitle.Birthdate;
            command.Parameters.Add("@Gender", SqlDbType.VarChar).Value = entitle.Gender;
            command.Parameters.Add("@Street", SqlDbType.VarChar).Value = entitle.Street;
            command.Parameters.Add("@NumExt", SqlDbType.VarChar).Value = entitle.NumExt;
            command.Parameters.Add("@NumInt", SqlDbType.VarChar).Value = entitle.NumInt;
            command.Parameters.Add("@Colony", SqlDbType.VarChar).Value = entitle.Colony;
            command.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = entitle.ZipCode;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = entitle.Email;
            command.Parameters.Add("@Telephone", SqlDbType.VarChar).Value = entitle.Telephone;
            command.Parameters.Add("@City", SqlDbType.VarChar).Value = entitle.City;
            command.Parameters.Add("@Curp", SqlDbType.VarChar).Value = entitle.CURP;
            command.Parameters.Add("@MaritalStatus", SqlDbType.VarChar).Value = entitle.MaritalStatus;
            command.Parameters.Add("@BirthPlace", SqlDbType.VarChar).Value = entitle.Birthplace;
            command.Parameters.Add("@Lada", SqlDbType.VarChar).Value = entitle.Lada;
            command.Parameters.Add("@RegimeType", SqlDbType.VarChar).Value = entitle.RegimeType;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = entitle.IsActive;
            command.Parameters.Add("@MobilePhone", SqlDbType.VarChar).Value = entitle.MobilePhone == null ? entitle.MobilePhone = string.Empty : entitle.MobilePhone;
            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();
        }
        public void SaveEntitle(Entitle entitle)
        {
            //string conStr = ConfigurationManager.ConnectionStrings["CadenaConexionSQL"].ConnectionString;
            //SqlConnection sqlConnection = new SqlConnection(conStr);
            var conexion = con.obtenConexion();
            SqlCommand command = new SqlCommand("sp_insertEntitled", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EntitleId", SqlDbType.VarChar).Value = entitle.EntitleId;
            command.Parameters.Add("@NoIssste", SqlDbType.VarChar).Value = entitle.NoISSSTE;
            command.Parameters.Add("@PaternalLastName", SqlDbType.VarChar).Value = entitle.PaternalLastName;
            command.Parameters.Add("@MaternalLastName", SqlDbType.VarChar).Value = entitle.MaternalLastName;
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = entitle.Name;
            command.Parameters.Add("@RFC", SqlDbType.VarChar).Value = entitle.RFC;
            command.Parameters.Add("@Age", SqlDbType.Int).Value = entitle.Age;
            command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = entitle.Birthdate;
            command.Parameters.Add("@Gender", SqlDbType.VarChar).Value = entitle.Gender;
            command.Parameters.Add("@Street", SqlDbType.VarChar).Value = entitle.Street;
            command.Parameters.Add("@NumExt", SqlDbType.VarChar).Value = entitle.NumExt;
            command.Parameters.Add("@NumInt", SqlDbType.VarChar).Value = entitle.NumInt;
            command.Parameters.Add("@Colony", SqlDbType.VarChar).Value = entitle.Colony;
            command.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = entitle.ZipCode;
            command.Parameters.Add("@MStatus", SqlDbType.VarChar).Value = entitle.MaritalStatus;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = entitle.Email == null ? entitle.Email = string.Empty : entitle.Email;
            command.Parameters.Add("@Telephone", SqlDbType.VarChar).Value = entitle.Telephone == null ? entitle.Telephone = string.Empty : entitle.Telephone;
            command.Parameters.Add("@City", SqlDbType.VarChar).Value = entitle.City;
            command.Parameters.Add("@Curp", SqlDbType.VarChar).Value = entitle.CURP;
          //  command.Parameters.Add("@MaritalStatus", SqlDbType.VarChar).Value = entitle.MaritalStatus;
            command.Parameters.Add("@BirthPlace", SqlDbType.VarChar).Value = entitle.Birthplace;
            command.Parameters.Add("@Lada", SqlDbType.VarChar).Value = entitle.Lada == null ? entitle.Lada = string.Empty : entitle.Lada;
            command.Parameters.Add("@RegimeType", SqlDbType.VarChar).Value = entitle.RegimeType;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = entitle.IsActive;
            command.Parameters.Add("@MobilePhone", SqlDbType.VarChar).Value = entitle.MobilePhone == null ? entitle.MobilePhone = string.Empty : entitle.MobilePhone;

            conexion.Open();
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
