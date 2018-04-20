#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.Renapo;
using ISSSTE.Tramites2016.Common.ServiceAgents;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Bussiness;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using ISSSTE.Tramites2016.Common.Reports.Model.Hipoteca;
using ISSSTE.Tramites2016.Common.Reports;
using ISSSTE.Tramites2016.Common.ServiceAgents.Implementation;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Implementation
{
    /// <summary>
    ///     Implementación del dominio del derechohabiente
    /// </summary>
    public class EntitleDomainService : BaseDomainService, IEntitleDomainService
    {
        #region Fields

        //private IUnitOfWork _context;
        private readonly ISipeAvDataServiceAgent _sipeAv;
        private readonly IMortgageReportHelper _mortgageReport;

        DataAccess.HipotecasContext _penContext = new HipotecasContext();
        #endregion

        #region Constructor

        public EntitleDomainService(IUnitOfWork context, ISipeAvDataServiceAgent sipeAv, IMortgageReportHelper mortgageReport)
            : base(context)
        {
            //this._context = context;
            _sipeAv = sipeAv;
            _mortgageReport = mortgageReport;
        }

        #endregion

        #region IEntitleDomainService
        /// <summary>
        /// Revisa en informix si existe un derechohabiente con el número de issste proporcionado
        /// </summary>
        /// <param name="issteNumber">Número de issste del derechohabiente</param>
        /// <returns>Booleano indicando si el número corresponde a un derechohabiente</returns>
        public async Task<bool> IsIsssteNumberValid(string issteNumber)
        {
            SipeAvDataServiceAgent agent = new SipeAvDataServiceAgent();
            var entitleInfo = await agent.GetEntitleByNoIsssteAsync(issteNumber);

            return entitleInfo != null;
        }
        /// <summary>
        /// Realiza una búsqueda del derechohabiente por su curp en informix
        /// </summary>
        /// <param name="curp">CURP del derechohabiente</param>
        /// <returns>Los datos del derechohabiente o nulo si no es encontrado</returns>
        public async Task<string> GetEntitleIsssteNumberByCurp(string curp)
        {
            SipeAvDataServiceAgent agent = new SipeAvDataServiceAgent();
            string result = null;

            var entitleInfo = await agent.GetEntitleByCurpAsync(curp);

            if (entitleInfo != null)
                result = entitleInfo.NumIssste;

            return result;
        }
        /// <summary>
        /// Realiza una búsqueda del derechohabiente por su rfc en informix
        /// </summary>
        /// <param name="rfc">RFC del derechohabiente</param>
        /// <returns>Los datos del derechohabiente o nulo si no es encontrado</returns>
        public async Task<string> GetEntitleIsssteNumberByRfc(string rfc)
        {
            SipeAvDataServiceAgent agent = new SipeAvDataServiceAgent();
            string result = null;

            var entitleInfo = await agent.GetEntitleByRfcAsync(rfc);

            if (entitleInfo != null)
                result = entitleInfo.NumIssste;

            return result;
        }

        public async Task<Entitle> GetEntitleByNoIssste(string noIssste)
        {
            try
            {
                var entitle = await _sipeAv.GetEntitleByNoIsssteAsync(noIssste);
                if (entitle == null)
                    return null;

                if (entitle.DirectType == "ER")
                {
                    //return null;
                    string mensaje = _penContext.Messages.ToList().Where(x => x.Key == ConfigurationManager.AppSettings["tDirectoER"]).Select(x => new Message
                    {
                        Description = x.Description,
                        MessageId = x.MessageId,
                        Key = x.Key,

                    }).FirstOrDefault().Description;
                }
                var ent = ConverterData.EntitleConverter(entitle);
                //if (_context.Entitles.Select(x => x.DelegationId) != null)
                //{
                var entdb = _context.Entitles.FirstOrDefault(r => r.NoISSSTE == noIssste);


                //var regimen = await _sipeAv.GetRegimenByNoIsssteAsync(noIssste);

                //if (regimen != null)
                //{
                //    ent.RegimeType = regimen.RegimenDescription;
                //    ent.RegimeKey = regimen.RegimenKey;
                //}
                if (entdb != null && String.IsNullOrEmpty(ent.Email) && String.IsNullOrEmpty(ent.Telephone))
                {
                    ent.Email = entdb.Email;
                    ent.Telephone = entdb.Telephone;
                }
                //    }

                var res = await SaveEntitle(ent);

                return ent;


            }
            catch (Exception exception)
            {
                throw (exception);
            }

        }

        public Task<int> SaveEntitle(Entitle entitle)
        {
            try
            {
                _context.Entitles.AddOrUpdate(entitle);

                if (!String.IsNullOrEmpty(entitle.Email) && !String.IsNullOrEmpty(entitle.Telephone))
                {
                    var entitleViewModel = new EntitleViewModel
                    {
                        Email = entitle.Email,
                        Telephone = entitle.Telephone
                    };
                    _sipeAv.UpdateEntitledInfoAsync(entitle.CURP, entitleViewModel);
                }
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Debtor>> GetBeneficiariesCIbyNoIssste(string noIssste)
        {
            try
            {
                var debtorsSipe = await _sipeAv.GetBeneficiariesCIByNoIsssteAsync(noIssste);
                return debtorsSipe.Select(debtor => ConverterData.RelativeConverter(debtor)).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Debtor>> GetDebtorsbyNoIssste(string noIssste)
        {
            try
            {
                var debtorsSipe = await _sipeAv.GetRelativesByNoIsssteAsync(noIssste);
                return debtorsSipe.Select(debtor => ConverterData.RelativeConverter(debtor)).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveDebtors(List<Debtor> debtors, EntitledData entitledData)
        {
            try
            {
                foreach (var deb in debtors)
                {
                    deb.RequestId = entitledData.RequestId; //  reemplazar esta linea
                    deb.NoISSSTE = entitledData.Entitle.NoISSSTE; // agregar esta linea
                    _context.Debtors.AddOrUpdate(deb);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<CURPStruct> ValidateCurp(string curp)
        {
            try
            {
                //if (ConfigurationManager.AppSettings["UseRenapo"] == "0")
                //    return _sipeAv.CurpIsValid(curp);
                return Renapo.ValidateCurp(curp);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Debtor>> GetDebtorsbyRequest(Guid requestId)
        {
            try
            {
                return _context.Debtors.Where(r => r.RequestId == requestId).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Entitle> GetEntitleById(string noIssste)
        {
            try
            {
                return _context.Entitles.First(r => r.NoISSSTE == noIssste);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Entitle> GetEntitleByCurp(string curp)
        {
            try
            {
                var entitle = await _sipeAv.GetEntitleByCurpAsync(curp);
                if (entitle == null)
                    return null;

                if (entitle.DirectType == "ER")
                {
                    string mensaje = _penContext.Messages.ToList().Where(x => x.Key == ConfigurationManager.AppSettings["tDirectoER"]).Select(x => new Message
                    {
                        MessageId = x.MessageId,
                        Key = x.Key,
                        Description = x.Description,

                    }).FirstOrDefault().Description;
                }
                var ent = ConverterData.EntitleConverter(entitle);
                var entdb = _context.Entitles.FirstOrDefault(r => r.CURP == curp);
                //if (String.IsNullOrEmpty(entitle.DelegationCode))
                //{
                //    ent.Delegation = new Delegation();
                //    ent.Delegation.DelegationId = -999;
                //    return ent;
                //}
                //var delegationCode = int.Parse(entitle.DelegationCode);
                //var delegation = _context.Delegations.FirstOrDefault(r => r.DelegationId == delegationCode);
                //if (delegation != null)
                //{
                //    ent.DelegationId = delegation.DelegationId;
                //    ent.Delegation = delegation;
                //}
                //var regimen = await _sipeAv.GetRegimenByNoIsssteAsync(noIssste);
                //if (regimen != null)
                //    ent.RegimeType = regimen.RegimenDescription;
                if (entdb != null && String.IsNullOrEmpty(ent.Email) && String.IsNullOrEmpty(ent.Telephone))
                {
                    ent.Email = entdb.Email;
                    ent.Telephone = entdb.Telephone;
                }
                var res = await SaveEntitle(ent);
                return ent;
                // return _context.Entitles.Find(curp);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //public Task<int> SaveValidation(Validation validation)
        //{
        //    try
        //    {
        //        //_context.Validations.AddOrUpdate(validation);
        //        return _context.SaveChangesAsync();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        public Task<int> SaveTimeContribution(TimeContribution time)
        {
            try
            {
                _context.TimeContributions.AddOrUpdate(time);
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<DownloadFileResult> GetMortgageCancel(Guid RequestId)
        {
            try
            {
                //Llamar al método de consulta del store procedure que obtiene la información del request
                Hipotecas.Bussiness.MortgageBussiness BL = new MortgageBussiness();
                Model.Pocos.MortgageCancelReportData datos = BL.GetMortgageCancelByRequestId(RequestId);

                //convertimos la fecha obtenida para darle el formato requerido por el reporte
                string[] splitDate = datos.Date.ToLongDateString().Split(',');

                //Se asigna la información obtenida para generar el reporte
                MortgageCancel mortgage = new MortgageCancel()
                {
                    Name = datos.Name.ToUpper(),
                    City = datos.City.ToUpper(),
                    Date = splitDate[1].ToUpper(),
                    Property = datos.WritingProperty.ToUpper(),
                    Telephone = datos.Telephone,
                    MobileTelephone = datos.MobileTelephone
                };

                DownloadFileResult result = null;
                var data = _mortgageReport.GetMortgageCancel(mortgage);
                result = new DownloadFileResult
                {
                    FileName = "CancelacionHipoteca.PDF",
                    Data = data
                };
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //public async Task<Validation> GetValidationByRequest(Guid requestId)
        //{
        //    try
        //    {
        //        var req = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
        //        return _context.Validations.FirstOrDefault(r => r.ValidationId == req.ValidationId);
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        //public async Task<TimeContribution> GetTimeContributionByRequest(Guid requestId)
        //{
        //    try
        //    {
        //        var req = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
        //        return _context.TimeContributions.FirstOrDefault(r => r.TimeContributionId == req.TimeContributionId);
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        #endregion

        #region Methods


        #endregion
    }
}