#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Common.Renapo;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domians
{
    public interface IEntitleDomainService
    {
        /// <summary>
        /// Revisa en informix si existe un derechohabiente con el número de issste proporcionado
        /// </summary>
        /// <param name="issteNumber">Número de issste del derechohabiente</param>
        /// <returns>Booleano indicando si el número corresponde a un derechohabiente</returns>
        Task<bool> IsIsssteNumberValid(string issteNumber);

        /// <summary>
        /// Realiza una búsqueda del derechohabiente por su curp en informix
        /// </summary>
        /// <param name="curp">CURP del derechohabiente</param>
        /// <returns>Los datos del derechohabiente o nulo si no es encontrado</returns>
        Task<string> GetEntitleIsssteNumberByCurp(string curp);

        /// <summary>
        /// Realiza una búsqueda del derechohabiente por su rfc en informix
        /// </summary>
        /// <param name="rfc">RFC del derechohabiente</param>
        /// <returns>Los datos del derechohabiente o nulo si no es encontrado</returns>
        Task<string> GetEntitleIsssteNumberByRfc(string rfc);
        /// <summary>
        ///     Obtiene el derechohabiente por no ISSSTE
        /// </summary>
        /// <param name="noIssste"></param>
        /// <returns></returns>
        Task<Entitle> GetEntitleByNoIssste(string noIssste);

        /// <summary>
        ///     Guarda los datos del derechohabiente
        /// </summary>
        /// <param name="entitle"></param>
        /// <returns></returns>
        Task<int> SaveEntitle(Entitle entitle);
      
        /// <summary>
        ///     Obtiene los beneficiarios CI por numero de issste
        /// </summary>
        /// <param name="noIssste"></param>
        /// <returns></returns>
        Task<List<Debtor>> GetBeneficiariesCIbyNoIssste(string noIssste);
      
        /// <summary>
        ///     Obtiene a los deudos por numero de issste
        /// </summary>
        /// <param name="noIssste"></param>
        /// <returns></returns>
        Task<List<Debtor>> GetDebtorsbyNoIssste(string noIssste);


       Task<DownloadFileResult> GetMortgageCancel(Guid requestId);

        /// <summary>
        ///     Valida el CURP
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        Task<CURPStruct> ValidateCurp(string curp);
       
        /// <summary>
        ///     Obtiene a los dedudos por id de solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<List<Debtor>> GetDebtorsbyRequest(Guid requestId);


        /// <summary>
        ///     Obtiene al derechohabiente por node issste
        /// </summary>
        /// <param name="noIssste"></param>
        /// <returns></returns>
        Task<Entitle> GetEntitleById(string noIssste);

     

        /// <summary>
        ///     Guarda a los deduos por solicitud
        /// </summary>
        /// <param name="debtors"></param>
        /// <param name="IdRequest"></param>
        /// <returns></returns>
        Task<int> SaveDebtors(List<Debtor> debtors, EntitledData IdRequest);

        /// <summary>
        ///     Guarda el resultado de la validacion  de la solicitud
        /// </summary>
        /// <param name="validation"></param>
        /// <returns></returns>
        //Task<int> SaveValidation(Validation validation);


        Task<int> SaveTimeContribution(TimeContribution timeContribution);
        
        
        /// <summary>
        ///     Obtiene la validacion por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
      //  Task<Validation> GetValidationByRequest(Guid requestId);

        /// <summary>
        /// Obtine el tiempo de Contibuciones por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
      //  Task<TimeContribution> GetTimeContributionByRequest(Guid requestId);

        /// <summary>
        ///     Obtiene al derechohabiente por CURP
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        Task<Entitle> GetEntitleByCurp(string curp);
    }
}