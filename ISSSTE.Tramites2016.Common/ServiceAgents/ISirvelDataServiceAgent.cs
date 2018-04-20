#region

using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;

#endregion

namespace ISSSTE.Tramites2016.Common.ServiceAgents
{
    /// <summary>
    /// Contiene métodos para consultar la información de la BD de Informix de Sirvel
    /// </summary>
    public interface ISirvelDataServiceAgent
    {
        /// <summary>
        ///     Obtiene los datos generales de las funerarias en un determinado estado de la republica
        /// </summary>
        /// <param name="idState">Id del estado donde se buscaran los velatorios</param>
        /// <returns>Datos generales de los velatorios</returns>
        Task<List<MortuaryInformation>> GetMortuariesInformation(int? idState);

        /// <summary>
        /// Obtiene los datos generales de una funeraria por su id
        /// </summary>
        /// <param name="idMortuary">Id del velatorio a buscar</param>
        /// <returns>Datos generales del velatorio</returns>

        Task<MortuaryInformation> GetMortuaryInformation(int? idMortuary);

        /// <summary>
        /// Obtiene los datos generales de los productos / servicios en una funeraria determinada
        /// </summary>
        /// <param name="idMortuary">Id del velatorio donde se buscaran los productos / servicios</param>
        /// <returns>Datos generales de los productos / servicios</returns>
        Task<List<MortuaryProductsInformation>> GetProductsInformation(int? idMortuary);

        /// <summary>
        ///     Obtiene los estados de la Republica Mexicana
        /// </summary>
        /// <returns>Los estados de la Republica Mexicana</returns>
        Task<List<StateInformation>> GetStatesInformation();

        /// <summary>
        ///     Obtiene los tipos de productos de las funerarias
        /// </summary>
        /// <returns>Tipos de productos</returns>
        Task<List<MortuaryTypesProductsInformation>> GetTypesProducts();

        /// <summary>
        ///     Obtiene la información de un producto o servicio de una funeraria
        /// </summary>
        /// <param name="idMortuary">Id de la funeraria donde se buscara el producto o servicio</param>
        /// <param name="idProduct">Id del producto o servicio a buscar</param>
        /// <returns>Información general del producto o servicio</returns>
        Task<MortuaryProductsInformation> GetProductByIdAsync(int idMortuary, int idProduct);
    }
}
