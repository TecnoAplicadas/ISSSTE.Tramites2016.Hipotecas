#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;
using Newtonsoft.Json;

#endregion

namespace ISSSTE.Tramites2016.Common.ServiceAgents.Implementation
{
    public class SirvelDataServiceAgent : BaseServiceAgent, ISirvelDataServiceAgent
    {
        #region Static Properties

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales de la funeraria del app.config
        /// </summary>
        private string MortuariesInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSMortuariesInfo"]; }
        }

        private string MortuaryInfoUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["InformixWSMortuaryInfo"];
            }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales de los productos / servicios por funerarias del app.config
        /// </summary>
        private string MortuaryProductsInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSProductsInfo"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales de un producto / servicios del app.config
        /// </summary>
        private string MortuaryProductInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSProductInfo"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los nombres de los estados de la Republica Mexicana del app.config
        /// </summary>
        private string StatesInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSStatesInfo"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa pata obtener los tpos de productos del app.config
        /// </summary>
        private string TypesProductsInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSTypesProductsInfo"]; }
        }

        #endregion

        #region ISirvelDataServiceAgent Implementation

        /// <summary>
        ///     Obtiene los datos generales de las funerarias en un determinado estado
        /// </summary>
        /// <param name="idState">Id del estado donde se buscaran las funerarias</param>
        /// <returns>Datos generales de las funerarias</returns>
        public async Task<List<MortuaryInformation>> GetMortuariesInformation(int? idState)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(MortuariesInfoUrl, idState);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var mortuaries = JsonConvert.DeserializeObject<List<MortuaryInformation>>(json);

            return mortuaries;
        }

        /// <summary>
        /// Obtiene los datos generales de una funeraria por su id
        /// </summary>
        /// <param name="idMortuary">Id del velatorio a buscar</param>
        /// <returns>Datos generales del velatorio</returns>

        public async Task<MortuaryInformation> GetMortuaryInformation(int? idMortuary)
        {
            var token = base.GetToken();

            var baseAddress = base.ServiceBaseUrl + String.Format(this.MortuaryInfoUrl, idMortuary);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var mortuaries = JsonConvert.DeserializeObject<MortuaryInformation>(json);

            return mortuaries;
            
        }

        /// <summary>
        ///     Obtiene los datos generales de los productos en un a funerarias determinada
        /// </summary>
        /// <param name="idMortuary">Id de la funeraria donde se buscaran los productos / servicios</param>
        /// <returns>Datos generales de los productos</returns>
        public async Task<List<MortuaryProductsInformation>> GetProductsInformation(int? idMortuary)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(MortuaryProductsInfoUrl, idMortuary);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<List<MortuaryProductsInformation>>(json);

            return products;
        }


        /// <summary>
        ///     Obtiene la información de un producto o servicio de una funeraria
        /// </summary>
        /// <param name="idMortuary">Id de la funeraria donde se buscara el producto o servicio</param>
        /// <param name="idProduct">Id del producto o servicio a buscar</param>
        /// <returns>Información general del producto o servicio</returns>
        public async Task<MortuaryProductsInformation> GetProductByIdAsync(int idMortuary, int idProduct)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(MortuaryProductInfoUrl, idMortuary, idProduct);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var product = JsonConvert.DeserializeObject<MortuaryProductsInformation>(json);

            return product;
        }

        /// <summary>
        ///     Obtiene los estados de la Republica Mexicana
        /// </summary>
        /// <returns>Estados de la Republica Mexicana</returns>
        public async Task<List<StateInformation>> GetStatesInformation()
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(StatesInfoUrl);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var states = new List<StateInformation>();

            JsonConvert.PopulateObject(json, states);

            return states;
        }

        public async Task<List<MortuaryTypesProductsInformation>> GetTypesProducts()
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(TypesProductsInfoUrl);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var types = JsonConvert.DeserializeObject<List<MortuaryTypesProductsInformation>>(json);

            return types;
        }

        #endregion
    }
}