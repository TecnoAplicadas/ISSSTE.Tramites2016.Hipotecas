#region

using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Domian.Implementation;
using ISSSTE.Tramites2016.Hipotecas.Domian.Resources;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Implementation
{
    /// <summary>
    ///     Implementación del dominio de las solicitudes
    /// </summary>
    public class RequestDomainService : BaseDomainService, IRequestDomainService
    {
        #region Constructor

        public RequestDomainService(IUnitOfWork context, IMailService mailService, ILogger logger) : base(context)
        {
            _mailService = mailService;
            _logger = logger;
        }

        #endregion

        #region Fields

        private const string MailSubject = "Solicitud de Pension";
        //private IUnitOfWork _context;
        private readonly IMailService _mailService;
        private readonly Tramites2016.Common.Util.ILogger _logger;

        #endregion

        #region IRequestDomainService

        public async Task<int> SaveRequest(Request request)
        {


            try
            {
                if (request.RequestId == Guid.Empty)
                {
                    _context.Requests.Add(request);
                }
                else
                {
                    _context.Requests.AddOrUpdate(request);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> AsingRequest(Guid requestId, Guid userId)
        {
            try
            {
                var request = _context.Requests.Find(requestId);
                //request.IsAssigned = true;
                //request.UserId = userId;
                _context.Requests.AddOrUpdate(request);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        /// <summary>
        /// Valida todo lo necesario para el cambio de estatus y retorna un string que indica el error
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="nextStatusId"></param>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        private async Task<String> ValidateChangeRequestStatusAsync(Guid requestId, int nextStatusId, List<String> roleNames)
        {
            //Valida que 

            List<int> roleIds = _context.Roles.Where(r => roleNames.Contains(r.Name)).Select(r => r.RoleId).ToList();
            List<Document> docs;
            RequestStatu currentRequestStatus = await _context.RequestStatus
                    .Where(rs => rs.RequestId.Equals(requestId) && rs.IsCurrentStatus && rs.StatusId > 299)
                        .Include(data => data.Status)
                        .Include(data => data.Request)
                        .Where(rs => rs.Status.RoleId == roleIds.FirstOrDefault())
                        .FirstOrDefaultAsync();
            if ((await GetAvailableNextStatus(requestId, roleNames)).Where(x => x.StatusId == nextStatusId).Count() == 0)
            {
                return ("No cuentas con los permisos para avanzar al siguiente estatus.");
            }
            if (currentRequestStatus.StatusId == 300)
            {
                docs = _context.Documents.Where(d => d.RequestId == requestId).ToList();
                if (nextStatusId == 301)
                {
                    if (docs.Where(d => !(d.IsValid ?? false)).Count() > 0)
                    {
                        return "No puedes aprobar la documentación hasta que todos los documentos de la solicitud esten revisados y aprobados.";
                    }
                }
                if (nextStatusId == 302)
                {
                    if (docs.Where(d => !(d.IsValid ?? true)).Count() == 0 && docs.Count() > 0)
                    {
                        return ("No puedes rechazar la documentación hasta que todos los documentos de la solicitud esten revisados y al menos uno sea rechazado.");
                    }
                }
            }
            return ("");//sin ningún problema de validación
        }
        public async Task<String> ChangeRequestStatusAsync(Guid requestId, int nextStatusId, List<String> roleNames)
        {
            List<int> rolesIds = _context.Roles.Where(r => roleNames.Contains(r.Name)).Select(r => r.RoleId).ToList();
            String validationResult = (await ValidateChangeRequestStatusAsync(requestId, nextStatusId, roleNames));
            if (!validationResult.Equals(""))
            {
                return validationResult;
            }
            return await ForwardRequestStatus(requestId, nextStatusId, rolesIds);
        }
        private async Task<String> ForwardRequestStatus(Guid requestId, int nextStatusId, List<int> roleIds)
        {

            try
            {

                int result = 0;

                //Se cambia el estado de la solicitud actual, en el caso de agendar/cancelar cita usa la solicitud del derechohabiente
                RequestStatu currentRequestStatus = await _context.RequestStatus
                    .Where(rs => rs.RequestId.Equals(requestId) && rs.IsCurrentStatus)
                        .Include(data => data.Status)
                        .Include(data => data.Request)
                        .Where(rs => rs.Status.RoleId == roleIds.FirstOrDefault())
                        .FirstOrDefaultAsync();

                if (currentRequestStatus == null)
                {
                    return ("No cuentas con los permisos para avanzar al siguiente estatus.");
                }
                //Le indicamos que ya no será el status a mostrar en el sistema
                currentRequestStatus.IsCurrentStatus = false;
                DbEntityEntry entry = _context.Entry(currentRequestStatus);
                entry.State = EntityState.Modified;



                Status nextStatus = await _context.Status
                    .Where(s => s.StatusId == nextStatusId)
                    .FirstOrDefaultAsync();

                //agrego el siguiente estado del rol
                this._context.RequestStatus.Add(new RequestStatu()
                {
                    Date = DateTime.Now,
                    IsCurrentStatus = true,
                    RequestId = requestId,
                    RequestStatusId = Guid.NewGuid(),
                    StatusId = nextStatus.StatusId
                });
                foreach (var relatedStatus in _context.StatusRelatedStatus.Where(x => x.StatusId == nextStatus.StatusId))
                {
                    int roleId = _context.Status.Where(x => x.StatusId == relatedStatus.RelatesStatusId).FirstOrDefault()?.RoleId ?? 0;
                    var pastRequestStatuses = await _context.RequestStatus.Where(requestStatus => requestStatus.RequestId.Equals(requestId))
                        .Where(requestStatus => requestStatus.IsCurrentStatus && requestStatus.Status.RoleId == roleId)
                        .ToListAsync();

                    foreach (RequestStatu requestStatus in pastRequestStatuses)
                    {
                        requestStatus.IsCurrentStatus = false;
                        DbEntityEntry requestStatusEntry = _context.Entry(requestStatus);
                        requestStatusEntry.State = EntityState.Modified;
                    }
                }

                foreach (var relatedStatus in _context.StatusRelatedStatus.Where(x => x.StatusId == nextStatus.StatusId))
                {
                    Guid newRequestStatusId = Guid.NewGuid();
                    //se agrega this al _dataContext integración --UPG
                    this._context.RequestStatus.Add(new RequestStatu()
                    {
                        Date = DateTime.Now,
                        IsCurrentStatus = true,
                        RequestId = requestId,
                        RequestStatusId = newRequestStatusId,
                        StatusId = relatedStatus.RelatesStatusId,
                    });

                    //await SendMail(relatedStatus, beneficiaryData.Kid.Entitle.Email, beneficiaryData.Kid.Name);

                    //Validar si es final de solicitud
                }


                result = await this._context.SaveChangesAsync();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
        public async Task<List<Status>> GetAvailableNextStatus(Guid requestId, List<String> roleNames)
        {
            try
            {
                List<Status> availableCurrentStatus = new List<Status>();
                List<int> availableCurrentStatusIds = new List<int>();
                List<Status> availableNextStatus = new List<Status>();
                List<StatusNextStatu> nextStatusAux = new List<StatusNextStatu>();
                List<int> nextStatusAuxIds = new List<int>();
                List<int> rolesIds = _context.Roles.Where(r => roleNames.Contains(r.Name)).Select(r => r.RoleId).ToList();
                Request req = _context.Requests.Where(re => re.RequestId == requestId).FirstOrDefault();
                bool existsRol = rolesIds.Count > 0 != null;
                bool requestFound = req != null;
                if (existsRol && requestFound)
                {
                    availableCurrentStatus = _context.RequestStatus.Where(rs => rs.RequestId == requestId && rs.IsCurrentStatus && rs.StatusId > 299).
                        Include(r => r.Status).Where(r => rolesIds.Contains(r.Status.RoleId)).Select(ob => ob.Status).ToList();
                    availableCurrentStatusIds = availableCurrentStatus.Select(x => x.StatusId).ToList();
                    nextStatusAux = _context.StatusNextStatus.Where(s => availableCurrentStatusIds.Contains(s.StatusId)).ToList();
                    nextStatusAuxIds = nextStatusAux.Select(x => x.NextStatusId).ToList();
                    availableNextStatus = _context.Status.Where(z => nextStatusAuxIds.Contains(z.StatusId)).ToList();
                }
                return availableNextStatus;
            }
            catch (Exception ex)
            {
                return new List<Status>();
            }

        }
        public async Task<List<Status>> GetNextStatus(bool happy, bool isEntitleStatus,
            int? actualStatus, int? statusChangue)
        {
            try
            {
                var statuses = new List<Status>();
                var nextStatus = _context.StatusNextStatus.Where(r => r.StatusId == actualStatus);
                var nx = new StatusNextStatu();
                var related = new List<StatusRelatedStatu>();
                var guidjefe = (int)RolEnum.JefeOperadores;
                var guidoperador = (int)RolEnum.Operador;
                var guidDerecho = (int)RolEnum.Derechohabiente;
                var guiVar = 0;
                //if (!isAssigned && isEntitleStatus)
                //    guiVar = guidjefe;
                //if (isAssigned && isEntitleStatus)
                //    guiVar = guidoperador;
                if (!isEntitleStatus)
                    guiVar = guidDerecho;
                var idStatus = 0;
                if (actualStatus == null || statusChangue != null)
                    idStatus = (int)statusChangue;
                if (actualStatus != null && statusChangue == null)
                {
                    nx = happy ? nextStatus.FirstOrDefault() : nextStatus.LastOrDefault();
                    idStatus = nx.NextStatusId;
                }

                related = (from rs in _context.StatusRelatedStatus
                           join st in _context.Status on rs.RelatesStatusId equals st.StatusId
                           where rs.StatusId == idStatus //&& st.RoleId == guiVar
                           select rs).ToList(); //MFP 12 - 01 - 2017

                statuses.Add(_context.Status.FirstOrDefault(r => r.StatusId == idStatus));

                if (related.Any())
                {
                    var id = related.FirstOrDefault().RelatesStatusId;
                    statuses.Add(_context.Status.FirstOrDefault(r => r.StatusId == id));
                }
                // MFP 12 - 01 - 2017

                return statuses;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveStatusRequestByEntitle(Request request, bool happy, int? statusChague, string userId)
        {
            try
            {
                int idStatus = 0;
                var entitle = _context.Entitles.Find(request.EntitleId);
                var reqStas = new List<RequestStatu>();
                var status = new List<Status>();
                var guidDere = (int)RolEnum.Derechohabiente;
                var stas = _context.RequestStatus.Where(r => r.IsCurrentStatus && r.RequestId == request.RequestId);
                var stasDer = from st in stas
                              join sta in _context.Status on st.StatusId equals sta.StatusId
                              where sta.RoleId == guidDere
                              select st;
                if (!stas.Any())
                {
                    int count = 0;
                    var appoinments = _context.Appoinments.Where(r => r.RequestId.Equals(request.RequestId));
                    foreach (var apoin in appoinments)
                    {
                        var allAppoinments = _context.Appoinments.Where(r => r.RequestId.Equals(apoin.RequestId)).ToList();
                        foreach (var all in allAppoinments)
                        {
                            count = count + 1;
                        }
                    }
                    //if (count < 3)
                    status = await GetNextStatus(happy, true, null, statusChague);
                }
                else
                {
                    foreach (var st in stas)
                    {
                        if (st.StatusId == (int)StatusEnum.EnesperadeagendarcitapararecogerdocumentacionDer)
                        {
                            statusChague = (int)StatusEnum.CitaagendadapararecogerdocumentacionDer;
                        }
                        else
                        {
                            idStatus = st.StatusId;
                        }

                    }
                    if (idStatus != 310)
                    {
                        var query = from st in stas
                                    join sta in _context.Status on st.StatusId equals sta.StatusId
                                    where sta.RoleId == guidDere
                                    select st;
                        status =
                            await
                                GetNextStatus(happy, true,
                                    query.Any() ? query.First().StatusId : (int?)null, statusChague);
                    }
                }
                if (idStatus != 310)
                {

                    foreach (var sta in status)
                    {
                        var rqSta = new RequestStatu
                        {
                            RequestStatusId = Guid.NewGuid(),
                            Date = DateTime.Now,
                            IsCurrentStatus = true,
                            Observations = String.Empty,
                            RequestId = request.RequestId,
                            StatusId = sta.StatusId
                        };

                        reqStas.Add(rqSta);
                    }
                    foreach (var st in stas)
                    {
                        st.IsCurrentStatus = false;
                    }
                    if (request.RequestId != Guid.Empty)
                    {
                        _context.RequestStatus.AddRange(reqStas);
                    }
                    if (ConfigurationManager.AppSettings["ActiveMail"] == "1")
                    {
                        foreach (var actualNewNotifiyStatus in status.Where(r => r.IsNotify))
                        {
                            try
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Estimado Derechohabiente:");
                                sb.AppendLine("Su petición con el folio : " + request.Folio + ":");
                                sb.AppendLine(actualNewNotifiyStatus.NotifyMessage);
                                _mailService.SendMailAsync(entitle.Email, MailSubject,
                                    String.Format(sb.ToString(), entitle.Name));
                            }
                            catch (Exception ex)
                            {
                                //await this._logger.WriteEntryAsync(ex, "Ocurrio un error al enviar el correo");
                            }
                        }
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveStatusRequest(Request request, bool happy, int? statusChague, String userId,
            string role)
        {
            try
            {
                int idStatus = 0;
                var currentRole = await _context.Roles
                    .Where(r => r.Name == role)
                    .FirstOrDefaultAsync();
                var entitle = _context.Entitles.Find(request.EntitleId);
                var reqStas = new List<RequestStatu>();
                var status = new List<Status>();
                var stas = _context.RequestStatus.Where(r => r.IsCurrentStatus && r.RequestId == request.RequestId);
                if (!stas.Any())
                {
                    status = await GetNextStatus(happy, false, null, statusChague);
                }
                else
                {
                    int count = 0;
                    var value = Convert.ToInt32(_context.Configurations.FirstOrDefault(r => r.Name == "NumberOfDates").Value);
                    var appoinments = _context.Appoinments.Where(r => r.RequestId.Equals(request.RequestId));
                    foreach (var apoin in appoinments)
                    {
                        count = count + 1;
                    }
                    if (count == value)
                    {
                        /* CANCELAR Appoinments */
                        var apps = _context.Appoinments.Where(r => r.RequestId.Equals(request.RequestId) && r.IsCancelled.Equals(false));
                        foreach (var Appoinments in apps)
                        {
                            Appoinments.IsAttended = false;
                            Appoinments.IsCancelled = true;
                        }

                        /* Cancelar RequestStatus */
                        var cancelReqStatus = _context.RequestStatus.Where(r => r.RequestId.Equals(request.RequestId) && r.IsCurrentStatus.Equals(true));
                        foreach (var cancel in cancelReqStatus)
                        {
                            cancel.IsCurrentStatus = false;
                        }
                        var reqStatus = new RequestStatu
                        {
                            RequestStatusId = Guid.NewGuid(),
                            RequestId = request.RequestId,
                            StatusId = 203,
                            Date = DateTime.Now,
                            IsCurrentStatus = true,
                            //UserId = userId,
                            Observations = "",
                            //ElapsedDays = 0,
                            //ElapsedWorkDays = 0,
                            //Data = null
                        };
                        reqStas.Add(reqStatus);
                        _context.RequestStatus.AddOrUpdate(reqStatus);

                    }

                    foreach (var st in reqStas)
                    {
                        idStatus = st.StatusId;
                    }
                    if (idStatus != 203)
                    {
                        var query = from st in stas
                                    join sta in _context.Status on st.StatusId equals sta.StatusId
                                    where sta.RoleId == currentRole.RoleId
                                    select st;

                        status =
                            await
                                GetNextStatus(happy, false,
                                    query.Any() ? query.First().StatusId : (int?)null, statusChague);
                    }
                }
                if (idStatus != 203)
                {
                    foreach (var sta in status)
                    {
                        var rqSta = new RequestStatu
                        {
                            RequestStatusId = Guid.NewGuid(),
                            Date = DateTime.Now,
                            //ElapsedDays = 0,
                            //ElapsedWorkDays = 0,
                            IsCurrentStatus = true,
                            Observations = String.Empty,
                            RequestId = request.RequestId,
                            //UserId = userId,
                            StatusId = sta.StatusId
                        };

                        reqStas.Add(rqSta);
                    }
                    foreach (var st in stas)
                    {
                        st.IsCurrentStatus = false;
                    }
                    if (request.RequestId != Guid.Empty)
                    {
                        _context.RequestStatus.AddRange(reqStas);
                    }
                }
                if (ConfigurationManager.AppSettings["ActiveMail"] == "1")
                {
                    foreach (var actualNewNotifiyStatus in status.Where(r => r.IsNotify))
                    {
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("Estimado Derechohabiente:");
                            sb.AppendLine("Su petición con el folio : " + request.Folio + ":");
                            sb.AppendLine(actualNewNotifiyStatus.NotifyMessage);
                            _mailService.SendMailAsync(entitle.Email, MailSubject,
                                String.Format(sb.ToString(), entitle.Name));
                        }
                        catch (Exception ex)
                        {
                            //await this._logger.WriteEntryAsync(ex, "Ocurrio un error al enviar el correo");
                        }
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Task<int> SaveStatusRequestByChief(Request request, bool happy, int? statusChague, String userId)
        {
            try
            {
                var entitle = _context.Entitles.Find(request.EntitleId);
                var reqStas = new List<RequestStatu>();
                var status = new List<Status>();
                var stas = _context.RequestStatus.Where(r => r.IsCurrentStatus && r.RequestId == request.RequestId);
                if (!stas.Any())
                {
                    status = GetNextStatus(happy, false, null, statusChague).Result;
                }
                else
                {
                    var query = from st in stas
                                join sta in _context.Status on st.StatusId equals sta.StatusId
                                where sta.RoleId == (int)RolEnum.JefeOperadores
                                select st;

                    status =
                        GetNextStatus(happy, false, query.First().StatusId, statusChague).Result;
                }
                foreach (var sta in status)
                {
                    var rqSta = new RequestStatu
                    {
                        RequestStatusId = Guid.NewGuid(),
                        Date = DateTime.Now,
                        //ElapsedDays = 0,
                        //ElapsedWorkDays = 0,
                        IsCurrentStatus = true,
                        Observations = String.Empty,
                        RequestId = request.RequestId,
                        //UserId = userId,
                        StatusId = sta.StatusId
                    };
                    reqStas.Add(rqSta);
                }
                foreach (var st in stas)
                {
                    st.IsCurrentStatus = false;
                }
                if (request.RequestId == Guid.Empty)
                {
                    _context.RequestStatus.AddRange(reqStas);
                }

                if (ConfigurationManager.AppSettings["ActiveMail"] == "1")
                {
                    foreach (var actualNewNotifiyStatus in status.Where(r => r.IsNotify))
                    {
                        try
                        {

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("Estimado Derechohabiente:");
                            sb.AppendLine("Su petición con el folio : " + request.Folio + ":");
                            sb.AppendLine(actualNewNotifiyStatus.NotifyMessage);
                            _mailService.SendMailAsync(entitle.Email, MailSubject,
                                String.Format(sb.ToString(), entitle.Name));
                        }
                        catch (Exception ex)
                        {
                            //await this._logger.WriteEntryAsync(ex, "Ocurrio un error al enviar el correo");
                        }
                    }
                }
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<RequestGeneric>> GetRequestsByEntitleId(String entitleId, bool actual)
        {
            try
            {
                var derechohabiente = (int)RolEnum.Derechohabiente;
                var query = from req in _context.Requests
                            join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                            // join pe in _context.Pensions on req.PensionId equals pe.PensionId
                            //  join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                            join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                            join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                            join rl in _context.Roles on sta.RoleId equals rl.RoleId
                            where ent.CURP == entitleId && rl.RoleId == derechohabiente && reqsta.IsCurrentStatus
                            select new RequestGeneric
                            {
                                RequestId = req.RequestId,
                                Folio = req.Folio,
                                Date = reqsta.Date,
                                NoISSSTE = ent.NoISSSTE,
                                RFC = ent.RFC,
                                CURP = ent.CURP,
                                Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                StatusId = sta.StatusId,
                                StatusDescription = sta.Name,
                                RoleId = rl.RoleId,
                                RoleDescription = rl.Description
                            };
                //if (actual)
                //    query = query.Where(r =>
                //        (int)StatusEnum.Enesperadediagnostico == r.StatusId ||
                //        (int)StatusEnum.Aprobado == r.StatusId ||
                //        (int)StatusEnum.EnEsperaDeAprobacióndeDictamen == r.StatusId ||
                //        (int)StatusEnum.EnEsperadeAgendarCitaDer == r.StatusId ||
                //        (int)StatusEnum.CitaAgendadaDer == r.StatusId ||
                //        (int)StatusEnum.EnEsperadeRespuestadeIngreso == r.StatusId);
                //else
                //    query = query.Where(r =>
                //        (int)StatusEnum.InformacionIncorrecta == r.StatusId ||
                //        (int)StatusEnum.NoAprobada == r.StatusId ||
                //        (int)StatusEnum.SolicitudRechazadaDer == r.StatusId ||
                //        (int)StatusEnum.DictamenRechazadoDer == r.StatusId ||
                //        (int)StatusEnum.SolicitudAtendidaDer == r.StatusId
                //        );
                return query.OrderByDescending(r => r.Date).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<IEnumerable<IEnumerable<DocumentTypes>>> GetDocumentsForInfoAsync()
        {
            List<List<DocumentTypes>> documents = new List<List<DocumentTypes>>();
            List<DocumentTypes> complementaryScanDocuments = await _context.DocumentTypes.ToListAsync();


            documents.Add(complementaryScanDocuments);
            return documents;
        }

        public async Task<bool> UpdateDocumentByRequestId(DocumentData documentData)
        {
            try
            {

                var docs = await _context.Documents.Where(s => s.DocumentId == documentData.DocumentId).FirstOrDefaultAsync();

                var reqsStatus = await _context.RequestStatus.Where(s => s.RequestId == documentData.RequestId).FirstOrDefaultAsync();

                if (docs != null && reqsStatus != null)
                {
                    docs.Observations = documentData.Observations;
                    docs.IsValid = documentData.IsValid;

                    _context.Entry(docs).Property("Data").IsModified = false;

                    //if (documentData.IsValid)
                    //    reqsStatus.StatusId = 360;// Solicitud atendida
                    //else
                    //    reqsStatus.StatusId = 331;//Solicitud rechazada

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public async Task<Request> GetRequestByRequestId(Guid requestId)
        {
            try
            {
                return _context.Requests.Find(requestId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<RequestGeneric> GetRequestCompleteEntitleByRequestId(Guid requestId, string state)
        {
            try
            {
                var derechohabiente = (int)RolEnum.Derechohabiente;
                if (state != "F")
                {
                    var query = from req in _context.Requests
                                join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                //join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where req.RequestId == requestId && rl.RoleId == derechohabiente && reqsta.IsCurrentStatus
                                select new RequestGeneric
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,
                                    Date = reqsta.Date,
                                    NoISSSTE = ent.NoISSSTE,
                                    RFC = ent.RFC,
                                    CURP = ent.CURP,
                                    Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,
                                    RoleId = rl.RoleId,
                                    RoleDescription = rl.Description
                                };
                    return await query.FirstOrDefaultAsync();
                }
                else
                {
                    var query = from req in _context.Requests
                                join deb in _context.Debtors on req.RequestId equals deb.RequestId
                                // join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where req.RequestId == requestId && rl.RoleId == derechohabiente && reqsta.IsCurrentStatus
                                select new RequestGeneric
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,

                                    Date = reqsta.Date,
                                    NoISSSTE = "",
                                    RFC = "",
                                    CURP = req.EntitleId.ToString(),
                                    Name = deb.PaternalLastName + " " + deb.MaternalLastName + " " + deb.Name,

                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,
                                    RoleId = rl.RoleId,
                                    RoleDescription = rl.Description
                                };
                    return await query.FirstOrDefaultAsync();
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(string noIssste)
        {
            int pastAlertsDays = await base.GetConfigurationParameterAsync<int>(ConfigurationParameters.PastDaysAlerts);
            DateTime startDate = DateTime.Now.AddDays(pastAlertsDays * -1);
            var query_roles = from Role in _context.Roles
                              where Role.Name.Equals("Derechohabiente")
                              select Role;
            var derechohabiente_roleId = await query_roles.FirstAsync();
            //int entitleGuid = (int)Model.Enums.RolEnum.Derechohabiente;
            int entitleGuid = derechohabiente_roleId.RoleId;
            List<Alert> systemAlerts = new List<Alert>();
            var query = from request in _context.Requests
                        join entitle in _context.Entitles on
                          request.EntitleId equals entitle.EntitleId
                        join requestStatus in _context.RequestStatus on
                          request.RequestId equals requestStatus.RequestId
                        join status in _context.Status on
                          requestStatus.StatusId equals status.StatusId
                        where
                          entitle.NoISSSTE.Equals(noIssste)
                          && requestStatus.Date >= startDate && requestStatus.Date <= DateTime.Now
                          && requestStatus.IsCurrentStatus
                          && status.RoleId.Equals(entitleGuid)
                        select new
                        {
                            Date = requestStatus.Date,
                            Folio = request.Folio,
                            StatusName = status.Name,
                            //Url = requestStatus.Data,
                            //KidName = kid.Name,
                            Id = Guid.NewGuid(),
                            RequestStatusId = Guid.NewGuid(),
                            RequestId = request.RequestId,
                            NoISSSTE = entitle.NoISSSTE
                        };
            var alerts = await query.ToListAsync();

            foreach (var alert in alerts)
            {
                systemAlerts.Add(new Alert()
                {
                    Date = alert.Date,
                    Folio = alert.Folio,
                    StatusName = alert.StatusName,
                    //Url = alert.Url,
                    //KidName = alert.KidName,
                    Id = alert.Id,
                    RequestId = alert.RequestId + "",
                    NoISSSTE = alert.NoISSSTE
                });
            }
            return systemAlerts;
        }

        public Task<int> SaveOpinion(Opinion opinion)
        {
            try
            {
                _context.Opinions.AddOrUpdate(opinion);
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Opinion> GetOpinionByRequest(Guid requestId)
        {
            try
            {
                return _context.Opinions.FirstOrDefault(r => r.RequestId == requestId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Status>> GetStatusByRole(string role)
        {
            try
            {
                var currentRole = await _context.Roles
                    .Where(r => r.Name == role)
                    .FirstOrDefaultAsync();
                return
                    await
                        _context.Status.Include(r => r.Role).Where(rl => rl.RoleId == currentRole.RoleId).ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Status>> GetDateStatusByRole(string role)
        {
            try
            {
                var currentRole = await _context.Roles
                    .Where(r => r.Name == role)
                    .FirstOrDefaultAsync();
                return
                    await
                        _context.Status.Include(r => r.Role).Where(rl => rl.RoleId == currentRole.RoleId && rl.Name.Contains("Cita")).ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Delegation>> GetAllDelegation()
        {
            try
            {
                return _context.Delegations.Where(r => r.Name != null && r.IsActive == true).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<RequestGeneric>> GetRequestsByDelegation(String filtro, int delegationId, Status status,
            bool isChief, string userId)
        {
            try
            {
                var query = from req in _context.Requests
                            join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                            //  join pe in _context.Pensions on req.PensionId equals pe.PensionId
                            // join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                            join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                            join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                            join rl in _context.Roles on sta.RoleId equals rl.RoleId
                            //  join app in _context.Appoinments on del.DelegationId equals app.Delegationid into joinApp
                            //  from request in joinApp.DefaultIfEmpty()
                            // where del.DelegationId == delegationId && reqsta.IsCurrentStatus
                            select new RequestGeneric
                            {
                                RequestId = req.RequestId,
                                Folio = req.Folio,
                                Date = reqsta.Date,
                                NoISSSTE = ent.NoISSSTE,
                                RFC = ent.RFC,
                                CURP = ent.CURP,
                                Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                StatusId = sta.StatusId,
                                StatusDescription = sta.Name,
                                RoleId = rl.RoleId,
                                RoleDescription = rl.Description
                            };
                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(r => r.ToString().ToLower().Contains(filtro.ToLower()));
                if (isChief)
                    query = query.Where(r => r.RoleId == (int)RolEnum.JefeOperadores);
                else
                    query = query.Where(r => r.RoleId == (int)RolEnum.Operador && r.UserId == userId);
                return query.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<PagedRequestsResult> GetPagedRequestsAsync(int pageSize, int page, string roleName,
            List<int> delegationId, string username, bool orden, int? statusId = default(int?), string query = null)
        {
            try
            {
                var currentRole = await _context.Roles
                    .Where(r => r.Name == roleName)
                    .FirstOrDefaultAsync();
                var roleJop = (int)RolEnum.JefeOperadores;
                var roleOp = (int)RolEnum.Operador;
                var roleDer = (int)RolEnum.Derechohabiente;
               // string rolAdmin = RolEnum.JefeOperadores.GetEnumDescription().ToString();

                var requestsQuery = from req in _context.Requests
                                    join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                    //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                    // join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                                    join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                    join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                    join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                    where reqsta.IsCurrentStatus &&  rl.RoleId == roleJop
                                    select new RequestGeneric
                                    {
                                        RequestId = req.RequestId,
                                        Folio = req.Folio,
                                        Date = reqsta.Date,
                                        NoISSSTE = ent.NoISSSTE,
                                        RFC = ent.RFC,
                                        CURP = ent.CURP,
                                        Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                        StatusId = sta.StatusId,
                                        StatusDescription = sta.Name,
                                        RoleId = rl.RoleId, // currentRole.RoleId == roleJop ? roleJop : rl.RoleId,
                                        RoleDescription = rl.Description
                                        //   DelegationId = del.DelegationId
                                    };

                if (currentRole.RoleId == roleOp)
                {
                    requestsQuery = requestsQuery.Where(r => r.RoleId == currentRole.RoleId);
                }
                if (currentRole.RoleId == roleJop)
                {
                    requestsQuery = requestsQuery.Distinct();
                }

                if (!delegationId.Contains(-1))
                    requestsQuery = requestsQuery.Where(r => delegationId.Contains(r.DelegationId));
                if (!String.IsNullOrEmpty(query))
                    requestsQuery =
                        requestsQuery.Where(r => (r.Name + " " + r.Folio).ToLower().Contains(query.ToLower()));
                if (statusId.HasValue)
                    requestsQuery = requestsQuery.Where(r => r.StatusId == statusId);
                if (currentRole.RoleId == (int)RolEnum.Operador)
                    requestsQuery = requestsQuery.Where(r => r.UserId == username);

                var requestsCount = await requestsQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)requestsCount / pageSize);
                var currentPage = page < totalPages ? page : totalPages;

                if (currentPage.Equals(0))
                    currentPage = 1;
                var requests = new List<RequestGeneric>();
                if (orden)
                {
                    requests = requestsCount == 0
                    ? new List<RequestGeneric>()
                    : await requestsQuery
                        .OrderBy(r => r.Date)
                        .Skip(pageSize * (currentPage - 1))
                        .Take(pageSize)
                        .ToListAsync();
                }
                else
                {
                    requests = requestsCount == 0
                   ? new List<RequestGeneric>()
                   : await requestsQuery
                       .OrderByDescending(r => r.Date)
                       .Skip(pageSize * (currentPage - 1))
                       .Take(pageSize)
                       .ToListAsync();
                }


                var result = new PagedRequestsResult
                {
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Requests = requests.ToList()
                };

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<RequestGeneric> GetRequestCompleteByRequestId(Guid requestId, string role)
        {
            try
            {
                DateTime fechaCita = Convert.ToDateTime("1000-01-01");
                var appoiments = await _context.Appoinments
                    .Where(r => r.RequestId == requestId)
                    .FirstOrDefaultAsync();
                var roleDer = (int)RolEnum.Derechohabiente;
                var currentRole = await _context.Roles
                    .Where(r => r.Name == role)
                    .FirstOrDefaultAsync();
                if (appoiments != null)
                {
                    fechaCita = appoiments.Date;
                    var query = from req in _context.Requests
                                join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                //  join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                //  join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where req.RequestId == requestId && rl.RoleId != roleDer && reqsta.IsCurrentStatus
                                select new RequestGeneric
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,
                                    Date = fechaCita,
                                    NoISSSTE = ent.NoISSSTE,
                                    RFC = ent.RFC,
                                    CURP = ent.CURP,
                                    Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,
                                    RoleId = rl.RoleId,
                                    RoleDescription = rl.Description
                                };
                    return await query.FirstOrDefaultAsync();
                }
                else
                {
                    var query = from req in _context.Requests
                                join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                //   join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where req.RequestId == requestId && rl.RoleId != roleDer && reqsta.IsCurrentStatus
                                select new RequestGeneric
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,
                                    //Date = Convert.ToDateTime("0000-00-00")
                                    NoISSSTE = ent.NoISSSTE,
                                    RFC = ent.RFC,
                                    CURP = ent.CURP,
                                    Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,
                                    RoleId = rl.RoleId,
                                    RoleDescription = rl.Description
                                };
                    return await query.FirstOrDefaultAsync();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<OpinionMessage>> GetOpinionMessages()
        {
            try
            {
                return await _context.OpinionMessages.ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<PagedRequestsResult> GetPagedDatesAsync(int pageSize, int page, List<int> delegationId,
            string username, DateTime? date, TimeSpan? time, string roleName, string query = null)
        {
            try
            {
                var roleDere = RolEnum.Derechohabiente.GetEnumDescription();
                var statusId = (int)StatusEnum.CitaAgendadaDer;
                var currentRole = await _context.Roles
                    .Where(r => r.Name == roleName)
                    .FirstOrDefaultAsync();
                var roleJop = (int)RolEnum.JefeOperadores;
                var roleOp = (int)RolEnum.Operador;
                var roleDer = (int)RolEnum.Derechohabiente;
                var status = new List<int> { 200, 202, 203, 206, 207, 208, 209, 210, 303, 304, 309, 310, 311 };
                var dateNow = DateTime.Now.Date;
                //&& rl.RoleId.ToString() == roleDere
                //                    && reqsta.StatusId == statusId
                var requestsQuery = from req in _context.Requests
                                    join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                    //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                                    //   join del in _context.Delegations on ent.DelegationId equals del.DelegationId
                                    join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                    join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                    join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                    join app in _context.Appoinments on req.RequestId equals app.RequestId
                                    where
                                        reqsta.IsCurrentStatus && rl.RoleId != roleDer && status.Contains(sta.StatusId) &&
                                        !app.IsCancelled
                                    select new RequestGeneric
                                    {
                                        RequestId = req.RequestId,
                                        Folio = req.Folio,
                                        Date = reqsta.Date,
                                        NoISSSTE = ent.NoISSSTE,
                                        RFC = ent.RFC,
                                        CURP = ent.CURP,
                                        Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name,

                                        StatusId = sta.StatusId,
                                        StatusDescription = sta.Name,
                                        RoleId = rl.RoleId,
                                        RoleDescription = rl.Description,
                                        DateApp = app.Date,
                                        TimeApp = app.Time,
                                        AppointmentId = app.AppoinmentId,
                                        IsAttendent = app.IsAttended,
                                        //   DelegationId = del.DelegationId,
                                        IsAttended = app.IsAttended,
                                        StatusDate = app.IsAttended ? "Cita Atendida" : "Cita no atendida",
                                        DelegationIdApp = app.Delegationid,
                                        IsinDay = dateNow == app.Date
                                    };

                //if (currentRole.RoleId == roleOp)
                //{
                //    requestsQuery = requestsQuery.Where(r => r.RoleId == currentRole.RoleId);
                //}
                if (currentRole.RoleId == roleJop)
                {
                    requestsQuery = requestsQuery.Distinct();
                }
                if (!delegationId.Contains(-1))
                    requestsQuery = requestsQuery.Where(r => delegationId.Contains(r.DelegationIdApp));
                if (!String.IsNullOrEmpty(query))
                    requestsQuery =
                        requestsQuery.Where(r => (r.Name + " " + r.Folio).ToLower().Contains(query.ToLower()));

                if (date != null)
                    requestsQuery = requestsQuery.Where(r => r.DateApp == date);
                if (time != null)
                    requestsQuery = requestsQuery.Where(r => r.TimeApp == time);

                var requestsCount = await requestsQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)requestsCount / pageSize);
                var currentPage = page < totalPages ? page : totalPages;

                if (currentPage.Equals(0))
                    currentPage = 1;


                var requests = requestsCount == 0
                    ? new List<RequestGeneric>()
                    : await requestsQuery
                        .OrderByDescending(r => r.DateApp).ThenBy(r => r.TimeApp)
                        .Skip(pageSize * (currentPage - 1))
                        .Take(pageSize)
                        .ToListAsync();

                var result = new PagedRequestsResult
                {
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Requests = requests.ToList()
                };

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> ChangueStatusAppointment(Guid appointmentId, bool isAttended)
        {
            try
            {
                var app = _context.Appoinments.Find(appointmentId);
                app.IsAttended = isAttended;
                app.IsCancelled = isAttended ? false : true;
                _context.Appoinments.AddOrUpdate(app);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Role> GetRole(string roleName)
        {
            try
            {
                var currentRole = await _context.Roles
                    .Where(r => r.Name == roleName)
                    .FirstOrDefaultAsync();
                return currentRole;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<AspNetUser>> GetUsersOperator()
        {
            try
            {
                var role = ConfigurationManager.AppSettings["roloperador"];
                var query = from au in _context.AspNetUsers
                            join aur in _context.AspNetUserRoles on au.Id equals aur.UserId
                            join ar in _context.AspNetRoles on aur.RoleId equals ar.Id
                            where role == ar.Name
                            select au;
                return await query.ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SendEmailValidation(Guid requestId)
        {
            try
            {
                //var request = _context.Requests.Include(r => r.Validation).FirstOrDefault(r => r.RequestId == requestId); MFP 10-01-2017
                var request = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
                var entitle = _context.Entitles.Find(request.EntitleId);
                //MFP
                //if (request.Validation.IsBeneficiaries && request.Validation.IsCurpCorrect &&
                //    request.Validation.IsDebtorsCorrect && request.Validation.IsGeneralDataCorrect
                //    && request.Validation.IsWorkHistoryCorrect)
                //{
                //    return 0;
                //}
                var message = new StringBuilder(Environment.NewLine);
                var messa = new List<OpinionMessage>();
                //if (!request.Validation.IsGeneralDataCorrect) MFP
                //{
                //    message.AppendLine(ConfigurationManager.AppSettings["GeneralData"]);
                //    messa = await GetOpinionMessagesByConfig(EnumConfigMessages.DatosGenerales);
                //    foreach (var opinionMessage in messa)
                //    {
                //        message.AppendLine(opinionMessage.Message);
                //    }
                //}
                //if (!request.Validation.IsBeneficiaries)
                //{
                //    message.AppendLine(ConfigurationManager.AppSettings["Beneficiaries"]);
                //    messa = await GetOpinionMessagesByConfig(EnumConfigMessages.Benficiarios);
                //    foreach (var opinionMessage in messa)
                //    {
                //        message.AppendLine(opinionMessage.Message);
                //    }
                //}
                //if (!request.Validation.IsDebtorsCorrect)
                //{
                //    message.AppendLine(ConfigurationManager.AppSettings["Debtors"]);
                //    messa = await GetOpinionMessagesByConfig(EnumConfigMessages.Benficiarios);
                //    foreach (var opinionMessage in messa)
                //    {
                //        message.AppendLine(opinionMessage.Message);
                //    }
                //}
                //if (!request.Validation.IsWorkHistoryCorrect)
                //{
                //    message.AppendLine(ConfigurationManager.AppSettings["Laboral"]);
                //    messa = await GetOpinionMessagesByConfig(EnumConfigMessages.Benficiarios);
                //    foreach (var opinionMessage in messa)
                //    {
                //        message.AppendLine(opinionMessage.Message);
                //    }
                //}
                //if (!request.Validation.IsCurpCorrect)
                //{
                //    message.AppendLine(ConfigurationManager.AppSettings["Curp"]);
                //    messa = await GetOpinionMessagesByConfig(EnumConfigMessages.Benficiarios);
                //    foreach (var opinionMessage in messa)
                //    {
                //        message.AppendLine(opinionMessage.Message);
                //    }
                //}
                //if (ConfigurationManager.AppSettings["ActiveMail"] == "1")
                //{
                //    try
                //    {
                //        StringBuilder sb = new StringBuilder();
                //        sb.AppendLine("Estimado Derechohabiente:");
                //        sb.AppendLine("Anexamos los pasos para realizar las correcciones pertinentes:");
                //        await _mailService.SendMailAsync(entitle.Email, MailSubject, sb.ToString() + message.ToString());
                //    }
                //    catch (Exception ex)
                //    {
                //        //await this._logger.WriteEntryAsync(ex, "Ocurrio un error al enviar el correo");
                //    }
                //}

                return 1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SendEmailQuiz(Guid requestId)
        {
            try
            {

                //var request = _context.Requests.Include(r => r.Validation).FirstOrDefault(r => r.RequestId == requestId); MFP 10-01-2017
                var request = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
                var entitle = _context.Entitles.Find(request.EntitleId);

                if (ConfigurationManager.AppSettings["ActiveMail"] == "1")
                {
                    try
                    {


                        _mailService.SendMailAsync(entitle.Email, MailSubject,
                            ConfigurationManager.AppSettings["TextoEncuesta"]);
                    }
                    catch (Exception ex)
                    {
                        //await this._logger.WriteEntryAsync(ex, "Ocurrio un error al enviar el correo");
                    }
                }

                return 1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<RequestGeneric>> GetCurpRequest(String curp, bool actual)
        {
            try
            {
                var requestInfo = (from req in _context.Requests
                           .Where(req => req.EntitleId.Equals(curp) && req.IsComplete == false)
                                   select new RequestGeneric
                                   {
                                       RequestId = req.RequestId,
                                       IsComplete = req.IsComplete,
                                   }).ToList();

                return requestInfo;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //public async void BatchService()
        //{
        //    try
        //    {
        //        const int citaagendada = (int)StatusEnum.CitaAgendadaDer;
        //        const int enespera = (int)StatusEnum.EnEsperadeAgendarCitaDer;
        //        var statusliList = new List<int>();
        //        var dateNow = DateTime.Now.Date;
        //        statusliList.Add((int)StatusEnum.Enesperadediagnostico);
        //        statusliList.Add((int)StatusEnum.Aprobado);
        //        statusliList.Add((int)StatusEnum.EnEsperaDeAprobacióndeDictamen);
        //        statusliList.Add((int)StatusEnum.EnEsperadeAgendarCitaDer);
        //        var daysout = int.Parse(_context.Configurations.FirstOrDefault(r => r.Name.Equals("DayOutDate")).Value);
        //        var number = int.Parse(_context.Configurations.FirstOrDefault(r => r.Name.Equals("NumberOfDates")).Value);
        //        var citapast = from r in _context.Requests
        //                       join rs in _context.RequestStatus on r.RequestId equals rs.RequestId
        //                       join ap in _context.Appoinments on r.RequestId equals ap.RequestId
        //                       where rs.IsCurrentStatus && !ap.IsCancelled && !ap.IsAttended && ap.Date < dateNow && rs.StatusId == citaagendada
        //                       select r;




        //        foreach (var request in citapast.ToList())
        //        {
        //            await SaveStatusRequestByEntitle(request, true, (int)StatusEnum.Enesperadediagnostico, request.EntitleId.ToString());
        //        }
        //        await _context.SaveChangesAsync();
        //        var outofDate =
        //            from r in _context.Requests
        //            join rs in _context.RequestStatus on r.RequestId equals rs.RequestId
        //            where rs.IsCurrentStatus && statusliList.Contains(rs.StatusId)
        //            select new { request = r, reqStatus = rs };



        //        foreach (var request in outofDate)
        //        {
        //            var res = DateUtils.CountDays(request.reqStatus.Date, DateTime.Now.Date);
        //            if (res >= daysout)
        //            {
        //               // await SaveStatusRequestByEntitle(request.request, false, (int)StatusEnum.SolicitudRechazadaDer, request.request.EntitleId.ToString());
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //        var outofApp =
        //            from r in _context.Requests
        //            join rs in _context.RequestStatus on r.RequestId equals rs.RequestId
        //            join ap in _context.Appoinments on r.RequestId equals ap.RequestId
        //            where rs.IsCurrentStatus && rs.StatusId == enespera
        //            select r;

        //        foreach (var request in outofApp)
        //        {
        //            var sum = _context.Appoinments.Count(r => r.RequestId == request.RequestId);
        //            //if (sum >= number)
        //                //await SaveStatusRequestByEntitle(request, false, (int)StatusEnum.SolicitudRechazadaDer, request.EntitleId.ToString());
        //        }



        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        /// <summary>
        /// Obtiene la información por medio de LINQ.
        /// </summary>
        /// <param name="pension"></param>
        /// <param name="relationship"></param>
        /// <returns></returns>
        public async Task<List<RelationshipGeneric>> GetDocumentsByRelationship(int pension, string KeyRelationships)
        {
            try
            {
                int[] Relationships = KeyRelationships.Split(',').Select(int.Parse).ToArray();

                var listDocuments = (from rsd in _context.RelationshipDocuments
                                     join doc in _context.Documents on rsd.DocumentsId equals doc.DocumentId
                                     //join pen in _context.Pensions on rsd.PensionId equals pen.PensionId
                                     join rs in _context.Relationships on rsd.RelationshipId equals rs.RelationshipId
                                     where Relationships.Contains(rsd.RelationshipId)
                                     && rsd.PensionId.Equals(pension)
                                     select new RelationshipGeneric
                                     {
                                         RelationshipId = rsd.RelationshipId,
                                         DocumentId = doc.DocumentId,
                                         //TypeDocument = doc.DocumentTypeId, Descomentar MFP 06-01-2017
                                         Description = doc.Observations,
                                         NameRelationships = rs.name
                                     }).OrderBy(r => r.RelationshipId);
                return listDocuments.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        #endregion


        #region NoUsado


        public async Task<List<OpinionMessage>> GetOpinionMessagesByConfig(EnumConfigMessages config)
        {
            try
            {
                var ids = new List<int>();
                var conf = ConfigurationManager.AppSettings[config.GetEnumDescription()].Split(',');
                foreach (var id in conf)
                {
                    ids.Add(int.Parse(id));
                }
                return _context.OpinionMessages.Where(r => ids.Contains(r.OpinionMessageId)).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Consulta la modalidad de la pension
        /// </summary>
        /// <param name="KeyRelationships"></param>
        /// <returns></returns>
        public async Task<List<RelationshipGeneric>> GetModaliadPension(string KeyRelationships)
        {
            try
            {
                var listDocuments = (from rsk in _context.RelationshipsTitlesKeys
                                     join rst in _context.RelationshipTitles on rsk.RelationshipTitleId equals rst.RelationshipTitleId
                                     where rsk.RelationshipId.Equals(KeyRelationships)
                                     select new RelationshipGeneric
                                     {
                                         NameRelationshipTitles = rst.Name
                                     });
                return listDocuments.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Consulta las solicitudes creadas por el Deudo 1.
        /// </summary>
        /// <param name="entitleId"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        /// 
        public async Task<List<RequestGeneric>> GetRequestsDebtor(string entitleId, bool actual)
        {
            try
            {

                var derechohabiente = RolEnum.Derechohabiente;
                var query = from req in _context.Requests
                            join deb in _context.Debtors on req.RequestId equals deb.RequestId
                            //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                            join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                            join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                            join rl in _context.Roles on sta.RoleId equals rl.RoleId
                            where req.EntitleId == entitleId
                            && rl.RoleId.Equals(derechohabiente)
                            && reqsta.IsCurrentStatus
                            && deb.CURP.Equals(entitleId)
                            select new RequestGeneric
                            {
                                RequestId = req.RequestId,
                                Folio = req.Folio,
                                Date = reqsta.Date,

                                StatusDescription = sta.Name,
                                StatusId = sta.StatusId
                            };
                //if (actual)
                //    query = query.Where(r =>
                //        (int)StatusEnum.Enesperadediagnostico == r.StatusId ||
                //        (int)StatusEnum.Aprobado == r.StatusId ||
                //        (int)StatusEnum.EnEsperaDeAprobacióndeDictamen == r.StatusId ||
                //        (int)StatusEnum.EnEsperadeAgendarCitaDer == r.StatusId ||
                //        (int)StatusEnum.CitaAgendadaDer == r.StatusId ||
                //        (int)StatusEnum.EnEsperadeRespuestadeIngreso == r.StatusId);
                //else
                //    query = query.Where(r =>
                //        (int)StatusEnum.InformacionIncorrecta == r.StatusId ||
                //        (int)StatusEnum.NoAprobada == r.StatusId ||
                //        (int)StatusEnum.SolicitudRechazadaDer == r.StatusId ||
                //        (int)StatusEnum.DictamenRechazadoDer == r.StatusId ||
                //        (int)StatusEnum.SolicitudAtendidaDer == r.StatusId
                //        );
                return query.OrderByDescending(r => r.Date).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        /// <summary>
        /// Consulta las solicitudes creadas por el Deudo.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public async Task<List<RequestGeneric>> GetRequestDebtorsRequestId(Guid requestId, bool actual)
        {
            try
            {
                var derechohabiente = RolEnum.Derechohabiente;
                var query = from req in _context.Requests
                                //join deb in _context.Debtors on req.EntitleId equals deb.CURP //MFP 10-01-2017
                                //join pe in _context.Pensions on req.PensionId equals pe.PensionId
                            join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                            join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                            join rl in _context.Roles on sta.RoleId equals rl.RoleId
                            where req.RequestId.Equals(requestId)
                            && rl.RoleId.Equals(derechohabiente)
                            && reqsta.IsCurrentStatus
                            select new RequestGeneric
                            {
                                RequestId = req.RequestId,
                                Folio = req.Folio,
                                Date = reqsta.Date,
                                StatusDescription = sta.Name,
                                StatusId = sta.StatusId
                            };
                return query.OrderByDescending(r => r.Date).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //    public async Task<PagedReportResult> GetPagedRequestsRepo(int pageSize, int page,
        //string Gender = null, int? delID = default(int?),
        //int? pensionId = default(int?), int? statusId = default(int?), string nameEntiti = null,
        //string numIssste = null, DateTime? inicio = default(DateTime?), DateTime? final = default(DateTime?), bool? banderaSoloDelegacion = default(bool?))
        //    {
        //        try
        //        {
        //            if (numIssste == "undefined")
        //            {
        //                numIssste = null;
        //            }

        //            //var maxValue = _context.Requests.Max(x => x.Date);
        //            //var minValue = _context.Requests.Min(x => x.Date);
        //            var requestsQuery = from r in _context.Requests
        //                                join rs in _context.RequestStatus on r.RequestId equals rs.RequestId
        //                                join cp in _context.Pensions on r.PensionId equals cp.PensionId
        //                                join e in _context.Entitles on r.EntitleId equals e.EntitleId
        //                                join cd in _context.Delegations on e.DelegationId equals cd.DelegationId
        //                                join cs in _context.Status on rs.StatusId equals cs.StatusId
        //                                join ap in _context.Appoinments on r.RequestId equals ap.RequestId
        //                                orderby e.Name
        //                                select new RequestRepor
        //                                {
        //                                    RequestId = r.RequestId,
        //                                    PensionId = r.PensionId,
        //                                    DelegationName = cd.Name,
        //                                    DelegationId = cd.DelegationId,
        //                                    NameE = e.Name + " " + e.PaternalLastName + " " + e.MaternalLastName,
        //                                    NoISSSTE = e.NoISSSTE,
        //                                    Sexo = e.Gender,
        //                                    TipoPensionSolicitado = cp.Name,
        //                                    Age = e.Age,
        //                                    //PeriodoDel = (r.Date),
        //                                    PeriodoDel = (r.Date),
        //                                    PeriodoAl = (r.Date),
        //                                    // PeriodoDel =_context.Requests.Max(r=>r.Date),
        //                                    //PeriodoAl = _context.Requests.Min(r => r.Date),
        //                                    Fechainicio = r.Date,
        //                                    EstatusDeSolicitud = cs.Name,
        //                                    DateAppoinment = ap.Date,
        //                                    TimeAppoiment = ap.Time,
        //                                    R_IsComplete = r.IsComplete,
        //                                    IsAttendent = ap.IsAttended,
        //                                    IsCancelled = ap.IsCancelled,
        //                                    StatusId = rs.StatusId,
        //                                    //DiferenciaFecha = DateTime.Parse(DbFunctions.DiffDays(r.Date, ap.Date).ToString())
        //                                    DiferenciaFechasint = DbFunctions.DiffDays(r.Date, ap.Date)
        //                                } into x
        //                                group x by new
        //                                {
        //                                    x.RequestId,
        //                                    x.PensionId,
        //                                    x.DelegationName,
        //                                    x.DelegationId,
        //                                    x.NameE,
        //                                    x.NoISSSTE,
        //                                    x.Sexo,
        //                                    x.TipoPensionSolicitado,
        //                                    x.Age,
        //                                    x.PeriodoDel,
        //                                    x.PeriodoAl,
        //                                    x.Fechainicio,
        //                                    x.EstatusDeSolicitud,
        //                                    x.DateAppoinment,
        //                                    x.TimeAppoiment,
        //                                    x.R_IsComplete,
        //                                    x.IsAttendent,
        //                                    x.IsCancelled,
        //                                    x.StatusId,
        //                                    x.DiferenciaFechasint
        //                                } into y
        //                                select new RequestRepor
        //                                {
        //                                    RequestId = y.Key.RequestId,
        //                                    PensionId = y.Key.PensionId,
        //                                    DelegationName = y.Key.DelegationName,
        //                                    DelegationId = y.Key.DelegationId,
        //                                    NameE = y.Key.NameE,
        //                                    NoISSSTE = y.Key.NoISSSTE,
        //                                    Sexo = y.Key.Sexo,
        //                                    TipoPensionSolicitado = y.Key.TipoPensionSolicitado,
        //                                    Age = y.Key.Age,
        //                                    //PeriodoDel = DateTime.Parse((y.OrderBy(PeriodoDel)).FirstOrDefault()),
        //                                    PeriodoDel = y.Key.PeriodoDel,
        //                                    PeriodoAl = y.Key.PeriodoAl,

        //                                    /*
        //                                    PeriodoDel = y.Max(r => r.PeriodoDel),
        //                                    PeriodoAl = y.Min(r => r.PeriodoDel),
        //                                    */
        //                                    Fechainicio = y.Key.Fechainicio,
        //                                    EstatusDeSolicitud = y.Key.EstatusDeSolicitud,
        //                                    DateAppoinment = y.Key.DateAppoinment,
        //                                    TimeAppoiment = y.Key.TimeAppoiment,
        //                                    R_IsComplete = y.Key.R_IsComplete,
        //                                    IsAttendent = y.Key.IsAttendent,
        //                                    IsCancelled = y.Key.IsCancelled,
        //                                    StatusId = y.Key.StatusId,
        //                                    // DiferenciaFecha = (DbFunctions.DiffDays(y.Key.Fechainicio, y.Key.DateAppoinment))
        //                                    DiferenciaFechasint = DbFunctions.DiffDays(y.Key.Fechainicio, y.Key.DateAppoinment)
        //                                };
        //            if (!String.IsNullOrEmpty(Gender) && Gender != null && Gender != "null")
        //            {
        //                requestsQuery = requestsQuery.Where(r => r.Sexo == Gender).OrderBy(r => r.NameE);
        //            }
        //            if (delID > 0 && delID != null)
        //            {
        //                requestsQuery = requestsQuery.Where(r => r.DelegationId == delID);

        //            }
        //            if (pensionId > 0 && pensionId != null)
        //            {
        //                requestsQuery = requestsQuery.Where(r => r.PensionId == pensionId);
        //            }
        //            if (statusId > 99 && statusId != null)
        //            {
        //                requestsQuery = requestsQuery.Where(rs => rs.StatusId == statusId);
        //            }
        //            if (!String.IsNullOrEmpty(nameEntiti) && nameEntiti != "null")
        //            {
        //                //requestsQuery = requestsQuery.Where(r=> r.NameE == nameEntiti);
        //                requestsQuery = requestsQuery.Where(r => (r.NameE).ToLower().Contains(nameEntiti.ToLower()));
        //            }
        //            if (!String.IsNullOrEmpty(numIssste) && numIssste != "null")
        //            {
        //                requestsQuery = requestsQuery.Where(r => r.NoISSSTE == numIssste);
        //            }
        //            if (inicio.HasValue && final.HasValue && inicio != null && final != null)
        //            {
        //                requestsQuery = requestsQuery.Where(r => (r.PeriodoDel > inicio) && (r.PeriodoAl < final));
        //            }


        //            var requestsCount = await requestsQuery.CountAsync();
        //            var impretionDates = requestsQuery;
        //            var totalPages = (int)Math.Ceiling((double)requestsCount / pageSize);
        //            var currentPage = page < totalPages ? page : totalPages;
        //            if (currentPage.Equals(0))
        //                currentPage = 1;
        //            var requests = await requestsQuery
        //                    .OrderByDescending(r => r.NameE)
        //                    .Skip(pageSize * (currentPage - 1))
        //                    .Take(pageSize)
        //                    .ToListAsync();
        //            var countE = requestsQuery.Where(r => r.R_IsComplete == true).Count();
        //            var countI = requestsQuery.Where(r => r.R_IsComplete == false).Count();
        //            var countTCC = requestsQuery.Where(r => r.IsAttendent == true).Count();
        //            var countTCI = requestsQuery.Where(r => r.IsCancelled == true).Count();
        //            var countTBJ = requestsQuery.Where(r => r.PensionId == 1).Count();
        //            var countTBETS = requestsQuery.Where(r => r.PensionId == 2).Count();
        //            var countTBPC = requestsQuery.Where(r => r.PensionId == 3).Count();
        //            var countTBMT = requestsQuery.Where(r => r.PensionId == 4).Count();
        //            var countTBMP = requestsQuery.Where(r => r.PensionId == 5).Count();
        //            var countTDE = 0;
        //            var countTDI = 0;
        //            var countTDA = 0;
        //            var countTDC = 0;
        //            if (banderaSoloDelegacion == true)
        //            {
        //                countTDE = requestsQuery.Where(r => r.R_IsComplete == true).Count();
        //                countTDI = requestsQuery.Where(r => r.R_IsComplete == false).Count();
        //                countTDA = requestsQuery.Where(r => r.IsAttendent == true).Count();
        //                countTDC = requestsQuery.Where(r => r.IsCancelled == true).Count();
        //            }
        //            var result = new PagedReportResult
        //            {
        //                PageSize = pageSize,
        //                CurrentPage = currentPage,
        //                TotalPages = totalPages,
        //                Requests = requests.ToList(),
        //                TotalesPorSolicitudExitoso = countE,
        //                TotalesPorSolicitudIncorrecto = countI,
        //                TotalesPorCitaCorrecto = countTCC,
        //                TotalesPorCitaIncorrecto = countTCI,
        //                TotalesPorBeneficioJubilacion = countTBJ,
        //                TotalesPorBeneficioEdadYTiempoServicio = countTBETS,
        //                TotalesPorBeneficioCesantia = countTBPC,
        //                TotalesPorBeneficioMuerteTrabajador = countTBMT,
        //                TotalesPorBeneficioMuertePensionado = countTBMP,
        //                TotalesDelegacionExitosas = countTDE,
        //                TotalesDelegacionIncorrectas = countTDI,
        //                TotalesDelegaciónAgendada = countTDA,
        //                TotalesDelegaciónCancelada = countTDC,
        //                ParaImpresion = impretionDates.ToList()
        //            };


        //            return result;
        //        }
        //        catch (Exception exception)
        //        {
        //            throw exception;
        //        }
        //    }



        #endregion




    }
}
