#region

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Util
{
    /// <summary>
    ///     Metodos de extension para diversas clases
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Obtiene la descripción de un valor de un enumerador
        /// </summary>
        /// <param name="value">Valor del enumerador</param>
        /// <returns>Descipción del valor del enumerador</returns>
        public static string GetEnumDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        /// <summary>
        /// Obtiene el <see cref="Guid"/> del valor del enumerador
        /// </summary>
        /// <param name="value">Valor del enumerador</param>
        /// <returns><see cref="Guid"/> del valor del enumerador</returns>
        public static Guid GetGuidAttribute(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                var attribute = (EnumGuidAttribute)
                    fi.GetCustomAttributes(typeof(EnumGuidAttribute), false)
                        .FirstOrDefault();
                return attribute.Guid;
            }

            return Guid.NewGuid();
        }

        /// <summary>
        /// Sets the entity as updated in a way that only only its not null properties wiill be sent to the database when <see cref="SaveChangesAsync"/> is called
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <param name="entity">The entity</param>
        public static void SetEntityAsPartialyUpdated<T>(this DbContext dbContext, T entity) where T : class
        {
            //Se agrega al contexto y se marca como modificada
            var dbEntry = dbContext.Entry(entity);
            dbEntry.State = EntityState.Modified;

            foreach (var name in dbEntry.CurrentValues.PropertyNames)
            {
                //Si el nombre de la pripiedad coincied con la lista de propiedades, se marca como modificada
                if (dbEntry.Property(name).CurrentValue == null)
                {
                    dbEntry.Property(name).IsModified = false;

                    var property = typeof(T).GetProperty(name);

                    if (property.CustomAttributes.Any(a => a.AttributeType == typeof(RequiredAttribute)))
                    {
                        object value = null;

                        if (property.PropertyType == typeof(String))
                            value = "";

                        property.SetValue(entity, value);
                    }
                }
            }
        }

        /// <summary>
        /// Defines a method which resolves a concurrency exception in EF
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="currentValues">The <see cref="DbContext"/> entityvalues</param>
        /// <param name="databaseValues">The values in the database</param>
        public delegate T ResolveConcurrency<T>(T currentValues, T databaseValues);


        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database, and applying custom logic if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException"/> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <param name="concurrencyResolution">The logic to use resolving the concurrency exception</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyAsync<T>(this DbContext dbContext, ResolveConcurrency<T> concurrencyResolution)
            where T : class
        {
            int result = 0;
            bool saveFailed;

            do
            {
                saveFailed = false;

                try
                {
                    result = await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Get the current entity values and the values in the database 
                    // as instances of the entity type 
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    T databaseValuesAsObject = databaseValues != null ? (T)databaseValues.ToObject() : null;

                    // Have the user choose what the resolved values should be 
                    var resolvedValuesAsBlog = concurrencyResolution((T)entry.Entity, databaseValuesAsObject);

                    // Update the original values with the database values and 
                    // the current values with whatever the user choose. 
                    if (databaseValues != null)
                        entry.OriginalValues.SetValues(databaseValues);
                    entry.CurrentValues.SetValues(resolvedValuesAsBlog);
                }

            } while (saveFailed);

            return result;
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database, and applying "database wins" approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException"/> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyDatabaseWinsAsync<T>(this DbContext dbContext)
            where T : class
        {
            return await Extensions.SaveChangesHandlingOptimisticConcurrencyAsync<T>(dbContext, (currentValues, databaseValues) =>
            {
                return databaseValues;
            });
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database, and applying "client wins" approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException"/> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyClientWinsAsync<T>(this DbContext dbContext)
            where T : class
        {
            return await Extensions.SaveChangesHandlingOptimisticConcurrencyAsync<T>(dbContext, (currentValues, databaseValues) =>
            {
                // Update original values from the database 
                return currentValues;
            });
        }
    }
}