using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.DataAccess;
using System.Reflection;
using System.Data.SqlClient;

namespace ISSSTE.Tramites2016.Common.Catalogs
{
    /// <summary>
    /// Defines methods for quering a generic entity in a database
    /// </summary>
    public class CatalogRepository : ICatalogRepository
    {
        #region Fields

        /// <summary>
        /// Represents the <see cref="DbContext"/> of the application
        /// </summary>
        protected IDbContext _dataContext;
        private readonly IConection _conexion;

        #endregion

        #region Constructor

        /// <summary>
        /// The contructor requires an open DataContext to work with
        /// </summary>
        /// <param name="context">An open DataContext</param>
        public CatalogRepository(IConection conexion, ILogger logger)
        {
            _conexion = conexion;
        }

        #endregion

        #region ICatalogDomainService

        public async Task<TObject> GetAsync<TObject>(params object[] keyValues) where TObject : class
        {
            TObject result = (TObject)Activator.CreateInstance(typeof(TObject));

            var conexion = _conexion.obtenConexion();

            if (keyValues[1].Equals("DocumentTypes"))
            {
                SqlCommand sql = new SqlCommand("sp_getDocumentType", conexion);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("@DocumentTypeId", SqlDbType.Int).Value = keyValues[0];

                try
                {
                    Type fieldsType = result.GetType();

                    PropertyInfo name = fieldsType.GetProperty("Name");
                    PropertyInfo documentTypeId = fieldsType.GetProperty("DocumentTypeId");
                    PropertyInfo required = fieldsType.GetProperty("Required");
                    PropertyInfo description = fieldsType.GetProperty("Description");

                    conexion.Open();
                    SqlDataReader reader = sql.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            description.SetValue(result, reader["Description"].ToString());
                            name.SetValue(result, reader["Name"].ToString());
                            documentTypeId.SetValue(result, Convert.ToInt32(reader["DocumentTypeId"].ToString()));
                            required.SetValue(result, Convert.ToBoolean(reader["Required"].ToString()));
                        }
                    }
                }
                catch (Exception e)
                {

                }

            }
            else if (keyValues[1].Equals("Delegations"))
            {
                SqlCommand sql = new SqlCommand("sp_getDelegation", conexion);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("@DelegationId", SqlDbType.Int).Value = keyValues[0];


                try
                {
                    Type fieldsType = result.GetType();

                    PropertyInfo name = fieldsType.GetProperty("Name");
                    PropertyInfo delegationId = fieldsType.GetProperty("DelegationId");
                    PropertyInfo isActive = fieldsType.GetProperty("IsActive");

                    conexion.Open();
                    SqlDataReader reader = sql.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            name.SetValue(result, reader["Name"].ToString());
                            delegationId.SetValue(result, Convert.ToInt32(reader["DelegationId"].ToString()));
                            isActive.SetValue(result, Convert.ToBoolean(reader["IsActive"].ToString()));
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            conexion.Close();

            return result;
        }

        public async Task<ICollection<TObject>> GetAllAsync<TObject>(string catalogName) where TObject : class
        {
            List<TObject> result = (List<TObject>)Activator.CreateInstance(typeof(List<TObject>)); ;

            var conexion = _conexion.obtenConexion();

            if (catalogName.Equals("DocumentTypes"))
            {
                SqlCommand sql = new SqlCommand("sp_getAllDocumentsType", conexion);
                sql.CommandType = CommandType.StoredProcedure;

                conexion.Open();
                SqlDataReader reader = sql.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TObject dtype = (TObject)Activator.CreateInstance(typeof(TObject));

                        Type fieldsType = dtype.GetType();

                        PropertyInfo name = fieldsType.GetProperty("Name");
                        PropertyInfo documentTypeId = fieldsType.GetProperty("DocumentTypeId");
                        PropertyInfo required = fieldsType.GetProperty("Required");
                        PropertyInfo description = fieldsType.GetProperty("Description");

                        description.SetValue(dtype, reader["Description"].ToString());
                        name.SetValue(dtype, reader["Name"].ToString());
                        documentTypeId.SetValue(dtype, Convert.ToInt32(reader["DocumentTypeId"].ToString()));
                        required.SetValue(dtype, Convert.ToBoolean(reader["Required"].ToString()));

                        result.Add(dtype);

                    }
                }

            } 
            if (catalogName.Equals("Delegations"))
            {
                SqlCommand sql = new SqlCommand("sp_getDelegations", conexion);
                sql.CommandType = CommandType.StoredProcedure;

                conexion.Open();
                SqlDataReader reader = sql.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TObject dtype = (TObject)Activator.CreateInstance(typeof(TObject));

                        Type fieldsType = dtype.GetType();

                        PropertyInfo name = fieldsType.GetProperty("Name");
                        PropertyInfo delegationId = fieldsType.GetProperty("DelegationId");
                        PropertyInfo isActive = fieldsType.GetProperty("IsActive");

                        name.SetValue(dtype, reader["Name"].ToString());
                        delegationId.SetValue(dtype, Convert.ToInt32(reader["DelegationId"].ToString()));
                        isActive.SetValue(dtype, Convert.ToBoolean(reader["IsActive"].ToString()));

                        result.Add(dtype);

                    }
                }
            }

            conexion.Close();

            return result;
        }

        public async Task<TObject> FindAsync<TObject>(Expression<Func<TObject, bool>> match) where TObject : class
        {
            return await _dataContext.Set<TObject>().SingleOrDefaultAsync(match);
        }

        public async Task<ICollection<TObject>> FindAllAsync<TObject>(Expression<Func<TObject, bool>> match) where TObject : class
        {
            return await _dataContext.Set<TObject>().Where(match).ToListAsync();
        }

        public async Task<TObject> AddAsync<TObject>(TObject t) where TObject : class
        {
            _dataContext.Set<TObject>().Add(t);

            await _dataContext.SaveChangesAsync();
            return t;
        }

        public async Task<IEnumerable<TObject>> AddAllAsync<TObject>(IEnumerable<TObject> tList) where TObject : class
        {
            _dataContext.Set<TObject>().AddRange(tList);
            await _dataContext.SaveChangesAsync();
            return tList;
        }

        public async Task<TObject> UpdateAsync<TObject>(TObject updated, int key) where TObject : class
        {
            if (updated == null)
                return null;

            TObject existing = await _dataContext.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                _dataContext.Entry(existing).CurrentValues.SetValues(updated);
                await _dataContext.SaveChangesAsync();
            }
            return existing;
        }

        public async Task<TObject> AddOrUpdateAsync<TObject>(TObject updated, string catalogName) where TObject : class
        {
            var conexion = _conexion.obtenConexion();
            SqlCommand sql;

            Type fieldsType = updated.GetType(); 

            if (catalogName.Equals("DocumentTypes"))
            {

                PropertyInfo documentTypeId = fieldsType.GetProperty("DocumentTypeId");
                PropertyInfo name = fieldsType.GetProperty("Name");
                PropertyInfo required = fieldsType.GetProperty("Required");
                PropertyInfo description = fieldsType.GetProperty("Description");

                sql = new SqlCommand("sp_getDocumentType", conexion);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("@DocumentTypeId", SqlDbType.Int).Value = documentTypeId.GetValue(updated);

                conexion.Open();
                SqlDataReader reader = sql.ExecuteReader();

                if (reader.HasRows)
                {
                    conexion.Close();

                    sql = new SqlCommand("sp_updateDocumentType", conexion);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.Add("@DocumentTypeId", SqlDbType.Int).Value = documentTypeId.GetValue(updated);
                    sql.Parameters.Add("@Name", SqlDbType.VarChar). Value = name.GetValue(updated);
                    sql.Parameters.Add("@Description", SqlDbType.VarChar).Value = description.GetValue(updated);
                    sql.Parameters.Add("@Required", SqlDbType.Bit).Value = Convert.ToByte(required.GetValue(updated));

                    conexion.Open();

                    sql.ExecuteNonQuery();
                } 
                else
                {
                    conexion.Close();

                    sql = new SqlCommand("sp_insertDocumentType", conexion);
                    sql.CommandType = CommandType.StoredProcedure;
                    sql.Parameters.Add("@Name", SqlDbType.VarChar).Value = name.GetValue(updated);
                    sql.Parameters.Add("@Description", SqlDbType.VarChar).Value = description.GetValue(updated);
                    sql.Parameters.Add("@Required", SqlDbType.Bit).Value = Convert.ToByte(required.GetValue(updated));

                    conexion.Open();

                    sql.ExecuteNonQuery();
                }
            }
            else if (catalogName.Equals("Delegations"))
            {
                PropertyInfo name = fieldsType.GetProperty("Name");
                PropertyInfo delegationId = fieldsType.GetProperty("DelegationId");
                PropertyInfo isActive = fieldsType.GetProperty("IsActive");

                sql = new SqlCommand("sp_getDelegation", conexion);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("@DelegationId", SqlDbType.Int).Value = delegationId.GetValue(updated);

                conexion.Open();
                SqlDataReader reader = sql.ExecuteReader();

                if (reader.HasRows)
                {
                    conexion.Close();

                    sql = new SqlCommand("sp_updateDelegation", conexion);
                    sql.CommandType = CommandType.StoredProcedure;

                    sql.Parameters.Add("@DelegationId", SqlDbType.Int).Value = delegationId.GetValue(updated);
                    sql.Parameters.Add("@StateKey", SqlDbType.VarChar).Value = "";
                    sql.Parameters.Add("@IsActive", SqlDbType.Bit).Value = Convert.ToByte(isActive.GetValue(updated));
                    sql.Parameters.Add("@Name", SqlDbType.VarChar).Value = name.GetValue(updated);

                    conexion.Open();

                    sql.ExecuteNonQuery();

                }
                else
                {
                    conexion.Close();

                    sql = new SqlCommand("sp_insertDelegation", conexion);
                    sql.CommandType = CommandType.StoredProcedure;

                    sql.Parameters.Add("@IsActive", SqlDbType.Bit).Value = Convert.ToByte(isActive.GetValue(updated));
                    sql.Parameters.Add("@StateKey", SqlDbType.VarChar).Value = "";
                    sql.Parameters.Add("@Name", SqlDbType.VarChar).Value = name.GetValue(updated);

                    conexion.Open();

                    sql.ExecuteNonQuery();
                }
            }
                conexion.Close();

            return updated;
        }

        public async Task<int> DeleteAsync<TObject>(TObject t, string catalogName) where TObject : class
        {
            DataSet ds = new DataSet();
            if (catalogName.Equals("DocumentTypes"))
            {
                Type fieldsType = t.GetType();

                PropertyInfo documentTypeId = fieldsType.GetProperty("DocumentTypeId");

                string queryDocuments = "DELETE FROM Documents WHERE DocumentTypeId = {0} ";
                ds = _conexion.obtenConsulta(String.Format(queryDocuments, documentTypeId.GetValue(t)));
                string query = "DELETE FROM DocumentTypes WHERE DocumentTypeId = {0} ";
                ds = _conexion.obtenConsulta(String.Format(query, documentTypeId.GetValue(t)));
            }
            else if (catalogName.Equals("Delegations"))
            {
                Type fieldsType = t.GetType();

                PropertyInfo delegationId = fieldsType.GetProperty("DelegationId");

                string query = "DELETE FROM Delegations WHERE DelegationId = {0} ";
                ds = _conexion.obtenConsulta(String.Format(query, delegationId.GetValue(t)));
            }


            return 1;
        }

        public async Task<int> CountAsync<TObject>() where TObject : class
        {
            return await _dataContext.Set<TObject>().CountAsync();
        }

        #endregion
    }
}
