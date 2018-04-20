#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using Newtonsoft.Json;
using ISSSTE.Tramites2016.Common.Util.Estancias;

#endregion

namespace ISSSTE.Tramites2016.Common.ServiceAgents.Implementation
{
    public class SipeAvDataServiceAgent : BaseServiceAgent, ISipeAvDataServiceAgent
    {
        #region Static Properties

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales del derechohabiente por NoISSSTE del app.config
        /// </summary>
        private string EntitleByNoIsssteInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSEntitle"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales del derechohabiente por RFC del app.config
        /// </summary>
        private string EntitleByRfcInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSEntitleByRfc"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales del derechohabiente por CURP del app.config
        /// </summary>
        private string EntitleByCurpInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSEntitleByCurp"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales de los beneficiarios del app.config
        /// </summary>
        private string BeneficiariesInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSBeneficiaries"]; }
        }

        /// <summary>
        /// Obtiene la URl para obtener los datos generales de los beneficiarios CI del app.config
        /// </summary>
        private string BeneficiariesCIInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSBeneficiariesCI"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener los datos generales de los deudos del app.config
        /// </summary>
        private string RelativesInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSRelatives"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener la historia laboral del derechohabiente del app.config
        /// </summary>
        private string LaboralHistoryInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSLaboralHistory"]; }
        }

        /// <summary>
        ///     Obtiene la URL relativa para obtener el tipo de regimen del derechohabiente del app.config
        /// </summary>
        private string RegimenInfoUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSRegimen"]; }
        }

        #endregion

        #region ISipeAvDataServiceAgent Implementation

        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="isssteNumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        public async Task<EntitleSipeInformation> GetEntitleByNoIsssteAsync(string isssteNumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(EntitleByNoIsssteInfoUrl, isssteNumber);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var directo = JsonConvert.DeserializeObject<List<EntitleSipeInformation>>(json);

            var firstDirecto = directo.FirstOrDefault();

            if (firstDirecto != null)
                firstDirecto.Age =
                    DateTimeSpan.CompareDates(Convert.ToDateTime(firstDirecto.BirthDate), DateTime.Now).Years;

            return firstDirecto;
        }


        public EntitleSipeInformation GetEntitleByNoIssste(string isssteNumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(EntitleByNoIsssteInfoUrl, isssteNumber);

            var http = BuildHttpClient(baseAddress, token);
            ////----
            HttpResponseMessage response = http.GetAsync(baseAddress).Result;
            //string stateInfo = response.Content.ReadAsStringAsync().Result;


            ////----
            //var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync();
            json.Wait();
            var directo = JsonConvert.DeserializeObject<List<EntitleSipeInformation>>(json.Result);

            var firstDirecto = directo.FirstOrDefault();

            if (firstDirecto != null)
                firstDirecto.Age =
                    DateTimeSpan.CompareDates(Convert.ToDateTime(firstDirecto.BirthDate), DateTime.Now).Years;

            return firstDirecto;

        }
        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="rfc">RFC del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        public async Task<EntitleSipeInformation> GetEntitleByRfcAsync(string rfc)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(EntitleByRfcInfoUrl, rfc);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var directo = JsonConvert.DeserializeObject<List<EntitleSipeInformation>>(json);

            var firstDirecto = directo.FirstOrDefault();

            if (firstDirecto != null)
                firstDirecto.Age =
                    DateTimeSpan.CompareDates(Convert.ToDateTime(firstDirecto.BirthDate), DateTime.Now).Years;

            return firstDirecto;
            ;
        }

        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="curp">CURP del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        public async Task<EntitleSipeInformation> GetEntitleByCurpAsync(string curp)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(EntitleByCurpInfoUrl, curp);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var directo = JsonConvert.DeserializeObject<List<EntitleSipeInformation>>(json);

            var firstDirecto = directo.FirstOrDefault();

            if (firstDirecto != null)
                firstDirecto.Age =
                    DateTimeSpan.CompareDates(Convert.ToDateTime(firstDirecto.BirthDate), DateTime.Now).Years;

            return firstDirecto;
        }

        /// <summary>
        ///     Obtiene los datos generales de los beneficiarios
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Datos generales de los beneficiarios</returns>
        public async Task<List<BeneficiarySipeInformation>> GetBeneficiariesByNoIsssteAsync(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(BeneficiariesInfoUrl, issstenumber);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var indirecto = JsonConvert.DeserializeObject<List<BeneficiarySipeInformation>>(json);

            foreach (var beneficiary in indirecto)
            {
                beneficiary.Age = DateTimeSpan.CompareDates(Convert.ToDateTime(beneficiary.BirthDate), DateTime.Now);

                beneficiary.AgeYears = beneficiary.Age.Years;
            }

            return indirecto.OrderByDescending(o => o.BirthDate).ToList();
        }

        public async Task<List<RelativesSipeInformation>> GetBeneficiariesCIByNoIsssteAsync(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(BeneficiariesCIInfoUrl, issstenumber);

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var indirecto = JsonConvert.DeserializeObject<List<RelativesSipeInformation>>(json);

            const int AGE_MINIMUM = 18;
            const int AGE_MAXIMUM = 25;

            var relativesList = new List<RelativesSipeInformation>();

            if (indirecto != null)
            {
                foreach (var relative in indirecto)
                {
                    if (relative.VersionKey.IsEmpty())
                    {
                        relative.Age = DateTimeSpan.CompareDates(Convert.ToDateTime(relative.BirthDate), DateTime.Now).Years;
                        relativesList.Add(relative);
                    }
                    else
                    {
                        relative.Age = DateTimeSpan.CompareDates(Convert.ToDateTime(relative.BirthDate), DateTime.Now).Years;

                        if (String.Equals(relative.VersionKey.ToUpper(), "P"))
                        {
                            relativesList.Add(relative);
                        }
                        else if (relative.Age < AGE_MINIMUM)
                        {
                            relativesList.Add(relative);
                        }
                        else if (relative.Age >= AGE_MINIMUM && relative.Age <= AGE_MAXIMUM &&
                                 String.Equals(relative.VersionKey.ToUpper(), "T"))
                        {
                            relativesList.Add(relative);
                        }
                    }
                }
            }

            return indirecto.OrderByDescending(o => o.BirthDate).ToList();
        }

        /// <summary>
        ///     Obtiene la historia laboral de un derechohabiente
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Historia laboral del derechohabientes</returns>
        public async Task<List<LaboralHistorySipeInformation>> GetLaboralHistoryByNoIsssteAsync(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(LaboralHistoryInfoUrl, issstenumber);
            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var cuenta = JsonConvert.DeserializeObject<List<LaboralHistorySipeInformation>>(json);

            // CAP if (cuenta != null)
            //{
            //    foreach (var item in cuenta)
            //    {

            //            if (item.StartDate != null && item.EndDate != null)
            //            {
            //                var tmp = DateTimeSpan.CompareDates(Convert.ToDateTime(item.StartDate),
            //                    Convert.ToDateTime(item.EndDate));

            //                item.YearsOfContributions = tmp.Years.ToString("D2") + "." + tmp.Months.ToString("D2") + "." +
            //                                            tmp.Days.ToString("D2");
            //            }


            //    }


            //}

            return cuenta.OrderBy(c => c.StartDate).ToList();
        }

        /// <summary>
        ///     Obtiene el tipo de regimen de un derechohabiente
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Tipo de regimen del derechohabiente</returns>
        public async Task<RegimenInformation> GetRegimenByNoIsssteAsync(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(RegimenInfoUrl, issstenumber);
            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var regimen = JsonConvert.DeserializeObject<List<RegimenInformation>>(json);

            return regimen.FirstOrDefault();
        }


        public RegimenInformation GetRegimenByNoIssste(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(RegimenInfoUrl, issstenumber);
            var http = BuildHttpClient(baseAddress, token);

            //var response = http.GetAsync(baseAddress);
            HttpResponseMessage response = http.GetAsync(baseAddress).Result;
            //string stateInfo = response.Content.ReadAsStringAsync().Result;


            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync();
            json.Wait();
            var regimen = JsonConvert.DeserializeObject<List<RegimenInformation>>(json.Result);

            return regimen.FirstOrDefault();
        }

        /// <summary>
        ///     Obtiene los beneficiarios de los derechohabientes con edad menor o igual a 7 años
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Beneficiarios con edad menor o ogual a 6 años</returns>
        public async Task<List<BeneficiarySipeInformation>> GetKidsAsync(string issstenumber)
        {
            var kids = new List<BeneficiarySipeInformation>();

            foreach (var beneficiary in await GetBeneficiariesByNoIsssteAsync(issstenumber))
            {
                if (EstanciasUtils.ValidateAge(beneficiary.Age))
                    kids.Add(beneficiary);
            }

            return kids.OrderByDescending(o => o.BirthDate).ToList();
        }

        /// <summary>
        ///     Obtiene el estado actual del derechohabiente
        /// </summary>
        /// <param name="entitle">Derechohabiente a consultar</param>
        /// <returns>Estado actal del derechohabiente</returns>
        public bool GetStateEntitle(EntitleSipeInformation entitle)
        {
            if (entitle != null)
            {
                if (String.Equals(entitle.State.ToUpper(), "A"))
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Obtiene los deudos de los derechohabientes
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Deudos de los derechohabientes</returns>
        public async Task<List<RelativesSipeInformation>> GetRelativesByNoIsssteAsync(string issstenumber)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(RelativesInfoUrl, issstenumber);
            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var relatives = JsonConvert.DeserializeObject<List<RelativesSipeInformation>>(json);

            const int AGE_MINIMUM = 18;
            const int AGE_MAXIMUM = 25;

            var relativesList = new List<RelativesSipeInformation>();

            if (relatives != null)
            {
                foreach (var relative in relatives)
                {
                    if (relative.VersionKey.IsEmpty())
                    {
                        relative.Age = DateTimeSpan.CompareDates(Convert.ToDateTime(relative.BirthDate), DateTime.Now).Years;
                        relativesList.Add(relative);
                    }
                    else
                    {
                        relative.Age = DateTimeSpan.CompareDates(Convert.ToDateTime(relative.BirthDate), DateTime.Now).Years;

                        if (String.Equals(relative.VersionKey.ToUpper(), "P"))
                        {
                            relativesList.Add(relative);
                        }
                        else if (relative.Age < AGE_MINIMUM)
                        {
                            relativesList.Add(relative);
                        }
                        else if (relative.Age >= AGE_MINIMUM && relative.Age <= AGE_MAXIMUM &&
                                 String.Equals(relative.VersionKey.ToUpper(), "T"))
                        {
                            relativesList.Add(relative);
                        }
                    }
                }
            }

            return relativesList.OrderByDescending(r => r.BirthDate).ToList();
        }

        /// <summary>
        ///     Actualiza el email y el telefono del derechohabiente
        /// </summary>
        /// <param name="curp">CURP del derechohabiente</param>
        /// <param name="EntitleViewModel">Información a actualizar</param>
        public async Task UpdateEntitledInfoAsync(string curp, EntitleViewModel info)
        {
            var token = GetToken();

            var baseAddress = ServiceBaseUrl + String.Format(EntitleByCurpInfoUrl, curp);
            var http = BuildHttpClient(baseAddress, token);

            var response =
                await
                    http.PutAsync(baseAddress,
                        new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, HttpContants.ContentTypes.Json));

            response.EnsureSuccessStatusCode();
        }

        public bool CurpIsValid(String curp)
        {
            //Metodo de validacion
            return true;
        }

        #endregion
    }
}