#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;

#endregion

namespace ISSSTE.Tramites2016.Common.ServiceAgents
{
    /// <summary>
    /// Contiene métodos para consultar la información de la BD de Informix de Sipe Av
    /// </summary>
    public interface ISipeAvDataServiceAgent
    {
        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="isssteNumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        Task<EntitleSipeInformation> GetEntitleByNoIsssteAsync(string isssteNumber);

        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="rfc">RFC del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        Task<EntitleSipeInformation> GetEntitleByRfcAsync(string rfc);

        /// <summary>
        ///     Obtiene los datos generales de un derechohabiente
        /// </summary>
        /// <param name="curp">CURP del derechohabiente a consultar</param>
        /// <returns>Datos generales del derechohabiente</returns>
        Task<EntitleSipeInformation> GetEntitleByCurpAsync(string curp);

        /// <summary>
        ///     Obtiene los datos generales de los beneficiarios de un derechohabiente
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Datos generales de los beneficiarios</returns>
        Task<List<BeneficiarySipeInformation>> GetBeneficiariesByNoIsssteAsync(string issstenumber);

        /// <summary>
        /// Obtiene la información de los Beneficiarios que viene de la tabla beneficiarios_CI
        /// </summary>
        /// <param name="issstenumber"></param>
        /// <returns></returns>
        Task<List<RelativesSipeInformation>> GetBeneficiariesCIByNoIsssteAsync(string issstenumber);

        /// <summary>
        ///     Obtiene los deudos de los derechohabientes
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Deudos de los derechohabientes</returns>
        Task<List<RelativesSipeInformation>> GetRelativesByNoIsssteAsync(string issstenumber);

        /// <summary>
        ///     Obtiene la historia laboral de un derechohabiente
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Historia laboral del derechohabientes</returns>
        Task<List<LaboralHistorySipeInformation>> GetLaboralHistoryByNoIsssteAsync(string issstenumber);

        /// <summary>
        ///     Obtiene el tipo de regimen de un derechohabiente
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Tipo de regimen del derechohabiente</returns>
        Task<RegimenInformation> GetRegimenByNoIsssteAsync(string issstenumber);

        /// <summary>
        ///     Obtiene los beneficiarios de los derechohabientes con edad menor o igual a 7 años
        /// </summary>
        /// <param name="issstenumber">Numero de ISSSTE del derechohabiente a consultar</param>
        /// <returns>Beneficiarios con edad menor o ogual a 7 años</returns>
        Task<List<BeneficiarySipeInformation>> GetKidsAsync(string issstenumber);

        /// <summary>
        ///     Obtiene el estado actual del derechohabiente
        /// </summary>
        /// <param name="entitle">Derechohabiente a consultar</param>
        /// <returns>Estado actual del derechohabiente</returns>
        bool GetStateEntitle(EntitleSipeInformation entitle);

        /// <summary>
        ///     Actualiza el email y el telefono del derechohabiente
        /// </summary>
        /// <param name="curp">CURP del derechohabiente</param>
        /// <param name="telephone">Telefono del derechohabiente</param>
        /// <param name="email">Correo delectronico del derechohabiente</param>
        Task UpdateEntitledInfoAsync(string curp, EntitleViewModel info);

        bool CurpIsValid(String curp);
    }
}