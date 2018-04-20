#region

using System;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Domian.Resources;
using System.Linq;
using System.Data.Entity;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Implementation
{
    /// <summary>
    ///     Implementacion del dominicio base
    /// </summary>
    public class BaseDomainService : IDisposable
    {
        #region Fields

        public IUnitOfWork _context;

        #endregion

        #region Constructor

        public BaseDomainService(IUnitOfWork context)
        {
            _context = context;
        }

        #endregion

        #region IDisposable Implementation

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    _context.Dispose();
                }
                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Obtiene el valir de una configuración intentandola castear al tipo especificado
        /// </summary>
        /// <param name="parameter">Configuración a obtener</param>
        /// <returns>Valor de la configuración</returns>
        protected async Task<T> GetConfigurationParameterAsync<T>(ConfigurationParameters parameter)
        {
            T result = default(T);

            var parameterValue = await this.GetConfigurationParameterAsync(parameter);

            if (!String.IsNullOrEmpty(parameterValue))
                result = (T)Convert.ChangeType(parameterValue, typeof(T));

            return result;

        }

        /// <summary>
        /// Obtiene el valir de una configuración como cadena
        /// </summary>
        /// <param name="parameter">Configuración a obtener</param>
        /// <returns>Valor de la configuración</returns>
        protected async Task<string> GetConfigurationParameterAsync(ConfigurationParameters parameter)
        {
            int configurationId = (int)parameter;

            string result = await _context.Configurations
                .Where(c => c.ConfiguratonId == configurationId)
                .Select(c => c.Value)
                .FirstOrDefaultAsync();

            return result;
        }
        #endregion
    }
}