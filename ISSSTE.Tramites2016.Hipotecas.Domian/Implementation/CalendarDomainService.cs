#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Implementation
{
    /// <summary>
    ///     Implementación del Dominio del Calendario
    /// </summary>
    public class CalendarDomainService : BaseDomainService, ICalendarDomainService
    {
        #region Constructor

        public CalendarDomainService(IUnitOfWork context) : base(context)
        {
            //this._context = context;
        }

        #endregion

        #region Fields

        //private IUnitOfWork _context;
        private ICommonDomainService _commonDomainService;

        #endregion

        #region ICalendarDomainService

        public async Task<List<Weekday>> GetWeekDays()
        {
            try
            {
                return _context.Weekdays.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Holyday>> GetHolydays()
        {
            try
            {
                return _context.Holydays.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<bool> IsHolyday(DateTime date)
        {
            try
            {
                return _context.Holydays.Any(r => r.Date == date);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<PagedInformation<AppointmentsResult>> GetPagetAppointmentsByRequestAsync(string roleName,
       List<int> delegationId,Guid requestId )
        {


            var pageInfo = new PagedInformation<AppointmentsResult>();
            //  pageInfo.SetElementosPorPagina = pageSize;



            var currentRole = await _context.Roles
                                .Where(r => r.Name == roleName).FirstOrDefaultAsync();

            var roleJop = (int)RolEnum.JefeOperadores;
            var roleOp = (int)RolEnum.Operador;
            var roleDer = (int)RolEnum.Derechohabiente;

            var requestsQuery = from req in _context.Requests
                                join appo in _context.Appoinments on req.RequestId equals appo.RequestId
                                join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where reqsta.IsCurrentStatus && rl.RoleId != roleDer && rl.Name == currentRole.Name   && req.RequestId == requestId
                                select new AppointmentsResult
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,
                                    AppointmentDate = appo.Date,
                                    Date = reqsta.Date,
                                    NoISSSTE = ent.NoISSSTE,
                                    RFC = ent.RFC,
                                    CURP = ent.CURP,
                                    Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name, // Mostrar
                                    Direccion = ent.Street + ", Num. int. " + ent.NumInt + ", Num. Ext. " + ent.NumExt + ", Col. " + ent.Colony + " " + ent.Birthplace + " CP " + ent.ZipCode,
                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,// Mostrar
                                    RoleId = currentRole.RoleId == roleJop ? roleJop : rl.RoleId,
                                    RoleDescription = rl.Description
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


            //if (!String.IsNullOrEmpty(query))
            //    requestsQuery =
            //        requestsQuery.Where(r => (r.Name + " " + r.Folio).ToLower().Contains(query.ToLower()));

            //if (statusId.HasValue)
            //    requestsQuery = requestsQuery.Where(r => r.StatusId == statusId);

            //if (currentRole.RoleId == (int)RolEnum.Operador)
            //    requestsQuery = requestsQuery.Where(r => r.UserId == username);


            pageInfo.resultCount = await requestsQuery.CountAsync(); //   var requestsCount = await requestsQuery.CountAsync();

            var requests = await requestsQuery
                    .OrderByDescending(r => r.Date)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.CurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();


            pageInfo.ResultList = requests;

            return pageInfo;

        }
        public async Task<PagedInformation<AppointmentsResult>> GetPagetAppointmentsAsync(int pageSize, int page, string roleName,
            List<int> delegationId, string username, DateTime? date = default(DateTime?), string query = null, TimeSpan? time = default(TimeSpan?), int? statusId = default(int?))
        {


            var pageInfo = new PagedInformation<AppointmentsResult>();
            pageInfo.SetElementosPorPagina = pageSize;



            var currentRole = await _context.Roles
                                .Where(r => r.Name == roleName).FirstOrDefaultAsync();

            var roleJop = (int)RolEnum.JefeOperadores;
            var roleOp = (int)RolEnum.Operador;
            var roleDer = (int)RolEnum.Derechohabiente;

            var requestsQuery = from req in _context.Requests
                                join appo in _context.Appoinments on req.RequestId equals appo.RequestId
                                join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
                                join reqsta in _context.RequestStatus on req.RequestId equals reqsta.RequestId
                                join sta in _context.Status on reqsta.StatusId equals sta.StatusId
                                join rl in _context.Roles on sta.RoleId equals rl.RoleId
                                where reqsta.IsCurrentStatus && rl.RoleId != roleDer && rl.Name == roleName
                                select new AppointmentsResult
                                {
                                    RequestId = req.RequestId,
                                    Folio = req.Folio,
                                    AppointmentDate = appo.Date,
                                    AppointmentTime = appo.Time,
                                    Date = reqsta.Date,
                                    NoISSSTE = ent.NoISSSTE,
                                    RFC = ent.RFC,
                                    CURP = ent.CURP,
                                    Name = ent.PaternalLastName + " " + ent.MaternalLastName + " " + ent.Name, // Mostrar
                                    Direccion = ent.Street + ", Num. int. " + ent.NumInt + ", Num. Ext. " + ent.NumExt + ", Col. " + ent.Colony + " " + ent.Birthplace + " CP " + ent.ZipCode,
                                    StatusId = sta.StatusId,
                                    StatusDescription = sta.Name,// Mostrar
                                    RoleId = currentRole.RoleId == roleJop ? roleJop : rl.RoleId,
                                    RoleDescription = rl.Description
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

            //if (statusId.HasValue)
            //    requestsQuery = requestsQuery.Where(r => r.StatusId == statusId);

            if (date != null)
                requestsQuery = requestsQuery.Where(r => r.AppointmentDate == date);

            if (time != null)
                requestsQuery = requestsQuery.Where(r => r.AppointmentTime == time);

            if (statusId.HasValue)
                requestsQuery = requestsQuery.Where(r => r.StatusId == statusId);

            if (currentRole.RoleId == (int)RolEnum.Operador)
                requestsQuery = requestsQuery.Where(r => r.UserId == username);


            pageInfo.resultCount = await requestsQuery.CountAsync(); //   var requestsCount = await requestsQuery.CountAsync();

            var requests = await requestsQuery
                    .OrderByDescending(r => r.Date)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.CurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();


            pageInfo.ResultList = requests;

            return pageInfo;

        }

        public async Task<List<Schedule>> GetAppointments(DateTime date, int delegationId)
        {
            try
            {
                var schedules = new List<Schedule>();
                var dof = date.DayOfWeek;
                var sp = _context.SpecialDays.FirstOrDefault(r => r.Date == date);
                if (sp != null)
                {
                    if (sp.IsNonWorking)
                    {
                        return schedules;
                    }
                    if (sp.IsOverrided)
                    {
                        var sps =
                            _context.SpecialDaysSchedules.Where(r => r.DelegationId == delegationId && r.Date == date)
                                .OrderBy(r => r.Time)
                                .ToList();
                        foreach (var s in sps)
                        {
                            var sche = new Schedule
                            {
                                WeekdayId = (int)dof,
                                Time = s.Time,
                                DelegationId = s.DelegationId,
                                Delegation = s.Delegation
                            };
                            sche.Capacity = sche.Capacity -
                                            _context.Appoinments.Count(
                                                r =>
                                                    r.Delegationid == delegationId && r.Date == date && r.Time == s.Time);
                            schedules.Add(sche);
                        }
                        return schedules;
                    }
                    var sces =
                        _context.Schedules.Where(r => r.DelegationId == delegationId && r.WeekdayId == (int)dof)
                            .ToList();
                    foreach (var sce in sces)
                    {
                        sce.Capacity = sce.Capacity -
                                       _context.Appoinments.Count(
                                           r => r.Delegationid == delegationId && r.Date == date && r.Time == sce.Time);
                    }
                    return sces;
                }
                return schedules;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<List<AppointmentsResult>> GetCurrentsAppointmentsByRequestId(Guid RequestId)
        {
            try
            {
                var result = await _context.Appoinments.Include(r => r.Delegation)
                                .Where(r => r.RequestId == RequestId && r.IsAttended == false && r.IsCancelled == false)
                                .Select(s => new AppointmentsResult
                                {
                                    AppointmentId = s.AppoinmentId,
                                    AppointmentDate = s.Date,
                                    AppointmentTime = s.Time,
                                    DelegationName = s.Delegation.Name,
                                    DelegationAddress = s.Delegation.Name
                                }).ToListAsync();


                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }


        public async Task<List<AppointmentsResult>> GetCancelAndNotAttendedAppointmentsByRequestId(Guid RequestId)
        {

            try
            {
                var result = await _context.Appoinments.Include(r => r.Delegation)
                                    .Where(r => r.RequestId == RequestId && r.IsAttended == false && r.IsCancelled == true)
                                    .Select(s => new AppointmentsResult
                                    {
                                        AppointmentId = s.AppoinmentId,
                                        AppointmentDate = s.Date,
                                        AppointmentTime = s.Time,
                                        DelegationName = s.Delegation.Name,
                                        DelegationAddress = s.Delegation.Name
                                    }).ToListAsync();


                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        //public async Task<List<AppointmentsResult>> GetCurrentAppointmentsByEntitleId(string NoIssste)
        //{
        //    try
        //    {
        //        var result =await (from req in _context.Requests
        //                      join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
        //                      join apo in _context.Appoinments on req.RequestId equals apo.RequestId
        //                      join del in _context.Delegations on apo.Delegationid equals del.DelegationId
        //                      where ent.NoISSSTE == NoIssste
        //                      //&& req.IsActive== true
        //                      && req.Appoinments.Any()
        //                      && apo.IsAttended==false && apo.IsCancelled==false
        //                      select new AppointmentsResult
        //                      {   AppointmentId = apo.AppoinmentId,
        //                          AppointmentDate = apo.Date,
        //                          AppointmentTime = apo.Time,
        //                          DelegationName = del.Name,
        //                          DelegationAddress = del.Name                                 
        //                       }
        //                     ).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }

        //}


        //public async Task<List<AppointmentsResult>> GetCancelAndNotAttendedAppointmentsByEntitleId(string NoIssste)
        //{

        //    try
        //    {
        //        var result = await (from req in _context.Requests
        //                            join ent in _context.Entitles on req.EntitleId equals ent.EntitleId
        //                            join apo in _context.Appoinments on req.RequestId equals apo.RequestId
        //                            join del in _context.Delegations on apo.Delegationid equals del.DelegationId
        //                            where ent.NoISSSTE == NoIssste
        //                            //&& req.IsActive== true
        //                            && req.Appoinments.Any()
        //                            && apo.IsAttended == false && apo.IsCancelled == true
        //                            select new AppointmentsResult
        //                            {
        //                                AppointmentId = apo.AppoinmentId,
        //                                AppointmentDate = apo.Date,
        //                                AppointmentTime = apo.Time,
        //                                DelegationName = del.Name,
        //                                DelegationAddress = del.Name
        //                            }
        //                     ).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }

        //}



        public async Task<List<Appoinment>> GetAppointmentsByRequestId(Guid requestId)
        {
            try
            {
                return _context.Appoinments.Where(r => r.RequestId == requestId).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Appoinment> GetAppointmentsById(Guid appointmentId)
        {
            try
            {
                return _context.Appoinments.FirstOrDefault(r => r.AppoinmentId == appointmentId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveAppointment(Appoinment appoinment)
        {
            try
            {
                foreach (var app in _context.Appoinments.Where(r => r.RequestId == appoinment.RequestId))
                {
                    app.IsCancelled = true;
                }
                _context.Appoinments.AddOrUpdate(appoinment);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Task<int> SaveSpecialDay(SpecialDay specialDay)
        {
            try
            {
                _context.SpecialDays.AddOrUpdate(specialDay);
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Task<int> SaveSpecialDaySchedule(SpecialDaysSchedule specialDaysSchedule)
        {
            try
            {
                specialDaysSchedule.Capacity =
                    Convert.ToInt32(_context.Configurations.FirstOrDefault(r => r.Name == "CapacityDate").Value);
                _context.SpecialDaysSchedules.AddOrUpdate(specialDaysSchedule);
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Task<int> SaveSchedule(Schedule schedule)
        {
            try
            {
                schedule.Capacity =
                    Convert.ToInt32(_context.Configurations.FirstOrDefault(r => r.Name == "CapacityDate").Value);
                _context.Schedules.AddOrUpdate(schedule);
                return _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void DeleteSchedulesByDelegation(int delegationId)
        {
            try
            {
                var sches = _context.Schedules.Where(r => r.DelegationId == delegationId);
                _context.Schedules.RemoveRange(sches);
                _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<SpecialDay>> GetSpecialDayByDelegationId(int delegationId)
        {
            try
            {
                return _context.SpecialDays.Where(r => r.DelegationId == delegationId).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<SpecialDaysSchedule>> GetSpecialDayScheduleByDelegationId(int delegationId)
        {
            try
            {
                return
                    _context.SpecialDaysSchedules.Where(r => r.DelegationId == delegationId)
                        .OrderBy(r => r.Date)
                        .ThenBy(r => r.Time)
                        .ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Schedule>> GetScheduleByDelegationId(int delegationId)
        {
            try
            {
                return _context.Schedules.Where(r => r.DelegationId == delegationId).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<List<Appoinment>> GetAllAppointmentsbyRequest(Guid requestId)
        {
            try
            {
                var query = _context.Appoinments.Include(r => r.Delegation).Where(r => r.RequestId == requestId);

                return await query.ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<int> CancelAppointment(Guid appointmentId, int conf)
        {

            try
            {
                int count = 0;
                Guid requestId = appointmentId;
                string userId = string.Empty;
                var appoinments = _context.Appoinments.Where(r => r.AppoinmentId.Equals(appointmentId));

                foreach (var apoin in appoinments)
                {
                    var allAppoinments = _context.Appoinments.Where(r => r.RequestId.Equals(apoin.RequestId)).ToList();

                    foreach (var all in allAppoinments)
                    {
                        count = count + 1;
                        requestId = all.RequestId;
                    }
                }
                if (count >= conf)
                {
                    /* CANCELAR Appoinments */
                    var apps = _context.Appoinments.Find(appointmentId);
                    var reqStas = new List<RequestStatu>();
                    apps.IsAttended = false;
                    apps.IsCancelled = true;

                    /* CANCELAR RequestStatus (Cancelar los IsCurrentStatus = 1 y Agregar nueva fila) */
                    var selectRequestStatus = _context.RequestStatus.Where(r => r.RequestId == requestId && r.IsCurrentStatus == true);

                    foreach (var query in selectRequestStatus)
                    {
                        query.IsCurrentStatus = false;
                    }


                    var reqStatus = new RequestStatu
                    {
                        RequestStatusId = Guid.NewGuid(),
                        RequestId = requestId,
                        StatusId = (int)StatusEnum.CitacanceladaDer,
                        Date = DateTime.Now,
                        IsCurrentStatus = true,
                        Observations = "",
                    };

                    var req = _context.Requests.Find(requestId);
                    req.RequestId = requestId;
                    req.IsComplete = false;

                    _context.RequestStatus.AddOrUpdate(reqStatus);
                    _context.Requests.AddOrUpdate(req);
                }
                else
                {
                    var app = _context.Appoinments.Find(appointmentId);
                    app.IsAttended = false;
                    app.IsCancelled = true;

                    var apps = _context.Appoinments.Find(appointmentId);
                    var reqStas = new List<RequestStatu>();
                    apps.IsAttended = false;
                    apps.IsCancelled = true;

                    /* CANCELAR RequestStatus (Cancelar los IsCurrentStatus = 1 y Agregar nueva fila) */
                    var selectRequestStatus = _context.RequestStatus.Where(r => r.RequestId == requestId && r.IsCurrentStatus == true);

                    foreach (var query in selectRequestStatus)
                    {
                        query.IsCurrentStatus = false;
                    }


                    var reqStatus = new RequestStatu
                    {
                        RequestStatusId = Guid.NewGuid(),
                        RequestId = requestId,
                        StatusId = (int)StatusEnum.EnesperadeAgendarCiraDer,
                        Date = DateTime.Now,
                        IsCurrentStatus = true,
                        Observations = "",
                    };

                    var req = _context.Requests.Find(requestId);
                    req.RequestId = requestId;
                    req.IsComplete = false;

                    _context.RequestStatus.AddOrUpdate(reqStatus);
                    _context.Requests.AddOrUpdate(req);
                    _context.Appoinments.AddOrUpdate(app);
                    //  _context.RequestStatus.AddOrUpdate(rs);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        //public async Task<int> CancelAppointment(Guid appointmentId)
        //{
        //    try
        //    {
        //        int count = 0;
        //        Guid requestId = appointmentId;
        //        string userId = string.Empty;
        //        var value = Convert.ToInt32(_context.Configurations.FirstOrDefault(r => r.Name == "NumberOfDates").Value);
        //        var appoinments = _context.Appoinments.Where(r => r.AppoinmentId.Equals(appointmentId));
        //        foreach (var apoin in appoinments)
        //        {
        //            var allAppoinments = _context.Appoinments.Where(r => r.RequestId.Equals(apoin.RequestId)).ToList();
        //            foreach (var all in allAppoinments)
        //            {
        //                count = count + 1;
        //                requestId = all.RequestId;
        //            }
        //        }
        //        if (count == value)
        //        {
        //            /* CANCELAR Appoinments */
        //            var apps = _context.Appoinments.Find(appointmentId);
        //            var reqStas = new List<RequestStatu>();
        //            apps.IsAttended = false;
        //            apps.IsCancelled = true;
        //            //_context.Appoinments.AddOrUpdate(apps);

        //            /* CANCELAR RequestStatus (Cancelar los IsCurrentStatus = 1 y Agregar nueva fila) */
        //            var selectRequestStatus = _context.RequestStatus.Where(r => r.RequestId == requestId && r.IsCurrentStatus == true);
        //            foreach (var query in selectRequestStatus)
        //            {
        //                query.IsCurrentStatus = false;
        //                //userId = query.UserId;
        //            }
        //            var reqStatus = new RequestStatu
        //            {
        //                RequestId = requestId,
        //                StatusId = 410,
        //                Date = DateTime.Now,
        //                IsCurrentStatus = true,
        //                //UserId = userId,
        //                Observations = "",
        //                //ElapsedDays = 0,
        //                //ElapsedWorkDays = 0,
        //                //Data = null
        //            };

        //            Request req = new Request();
        //            req.RequestId = reqStatus.RequestId;
        //            req.IsComplete = false;
        //            reqStas.Add(reqStatus);
        //          //  _context.Requests.AddOrUpdate(req);
        //            _context.RequestStatus.AddOrUpdate(reqStatus);
        //        }
        //        else
        //        {
        //            var app = _context.Appoinments.Find(appointmentId);
        //            app.IsAttended = false;
        //            app.IsCancelled = true;
        //            _context.Appoinments.AddOrUpdate(app);
        //        }
        //        return await _context.SaveChangesAsync();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}


        public async Task<CalendarApi> GetCalendarByMonthAndDelegation(int delegationId, DateTime montYear)


        {
            try
            {
                var calendar = new CalendarApi { Days = new DayCalendar[6, 7] };
                var initDate = new DateTime(montYear.Year, montYear.Month, 1, 0, 0, 0);
                var init = new DateTime(montYear.Year, montYear.Month, 1, 0, 0, 0);
                var day = ConverterData.GetWeekEnumDay(init.DayOfWeek);
                var space = 0;
                var occu = 0;

                for (var i = 0; i < 6; i++)
                {
                    for (var j = 0; j < 7; j++)
                    {
                        if (i == 0)
                        {
                            if (j + 1 >= (int)day)
                            {
                                init = j + 1 == (int)day ? init : init.AddDays(1);
                                space = GetFreeSpace(init, delegationId);
                                occu = GetOccupiedSpace(init, delegationId);
                                calendar.Days[i, j] = FillAvalitiyAndRate(space, occu, init);
                            }
                            else
                            {
                                calendar.Days[i, j] = FillAvalitiyAndRate(space, occu, null);
                            }
                        }
                        else
                        {
                            if (init.AddDays(1).Month == initDate.Month)
                            {
                                init = init.AddDays(1);
                                space = GetFreeSpace(init, delegationId);
                                occu = GetOccupiedSpace(init, delegationId);
                                calendar.Days[i, j] = FillAvalitiyAndRate(space, occu, init);
                            }
                            else
                            {
                                calendar.Days[i, j] = FillAvalitiyAndRate(space, occu, null);
                            }
                        }
                    }
                }
                return calendar;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int GetFreeSpace(DateTime date, int delegation)
        {
            try
            {
                var specialdays = _context.SpecialDays.Where(r => r.DelegationId == delegation && r.Date == date.Date);
                if (specialdays.Any())
                {
                    var specialDay = specialdays.FirstOrDefault();
                    if (specialDay.IsNonWorking)
                        return 0;
                    var speSche =
                        _context.SpecialDaysSchedules.Where(r => r.DelegationId == delegation && r.Date == date.Date);
                    return speSche.Any() ? speSche.Sum(r => r.Capacity) : 0;
                }
                var weekday = ConverterData.GetWeekEnumDay(date.DayOfWeek);
                var query = _context.Schedules.Where(r => r.DelegationId == delegation && r.WeekdayId == (int)weekday);
                return query.Any() ? query.Sum(r => r.Capacity) : 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int GetOccupiedSpace(DateTime date, int delegation)
        {
            try
            {
                return
                    _context.Appoinments.Count(
                        r => r.Delegationid == delegation && !r.IsCancelled && r.Date == date.Date);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<ScheduleApi>> GetTimeCalendar(DateTime date, int delegation)
        {
            try
            {
                //if (date == null)
                //    return new List<ScheduleApi>();
                var dateN = date;
                var query = new List<ScheduleApi>();
                var res = new List<ScheduleApi>();
                var apps =
                    _context.Appoinments.Where(
                        r => r.Delegationid == delegation && !r.IsCancelled && r.Date == dateN.Date);
                var specialdays = _context.SpecialDays.Where(r => r.DelegationId == delegation && r.Date == dateN.Date);
                if (specialdays.Any())
                {
                    var specialDay = specialdays.FirstOrDefault();
                    if (specialDay.IsNonWorking)
                        return new List<ScheduleApi>();
                    var speSche =
                        _context.SpecialDaysSchedules.Where(r => r.DelegationId == delegation && r.Date == dateN.Date);
                    query = (from s in speSche
                             select new ScheduleApi
                             {
                                 Date = s.Date,
                                 DelegationId = s.DelegationId,
                                 Time = s.Time,
                                 Capacity = s.Capacity
                             }).ToList();
                }
                else
                {
                    var weekday = ConverterData.GetWeekEnumDay(dateN.DayOfWeek);

                    int dia = (int)weekday;


                    // revisar por que no funciona la consulta por delegacion y dia: MFP 12-01-2017
                    query =
                        _context.Schedules
                        .Where(x => x.WeekdayId.Equals(dia))
                        .Select(s => new ScheduleApi
                        {
                            Date = dateN.Date,
                            DelegationId = s.DelegationId,
                            Time = s.Time,
                            Capacity = s.Capacity
                        }).ToList();

                }


                foreach (var api in query)
                {
                    var count = apps.Count(r => r.Date == api.Date && r.Time == api.Time);
                    if (count < api.Capacity)
                        res.Add(api);
                }
                return res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public static DayCalendar FillAvalitiyAndRate(decimal freeSpace, decimal occupiedSpace, DateTime? date)
        {
            var dayCalendar = new DayCalendar { Date = date };
            if (date == null || date < DateTime.Now.Date)
            {
                dayCalendar.Availability = AvailabilityEnum.PastDate;
                dayCalendar.FreeSpace = -1;
                dayCalendar.OccupiedSpace = -1;
                dayCalendar.Rate = -1;
                dayCalendar.Day = String.Empty;
                if (date != null)
                {
                    dayCalendar.Day = ((DateTime)date).Day.ToString();
                }
            }
            else
            {
                dayCalendar.Day = ((DateTime)date).Day.ToString();
                dayCalendar.FreeSpace = freeSpace;
                dayCalendar.OccupiedSpace = occupiedSpace;
                if (occupiedSpace == 0 && freeSpace == 0)
                    dayCalendar.Rate = -1;
                else
                {
                    if (freeSpace.Equals(0))
                    {
                        freeSpace = 1;
                    }
                    dayCalendar.Rate = (occupiedSpace / freeSpace) * 100;
                }
                if (dayCalendar.Rate <= 50)
                    dayCalendar.Availability = AvailabilityEnum.Avaliable;
                if (dayCalendar.Rate > 50)
                    dayCalendar.Availability = AvailabilityEnum.LowAvailability;
                if (dayCalendar.Rate == 100)
                    dayCalendar.Availability = AvailabilityEnum.Unavailable;
                if (dayCalendar.Rate == -1)
                    dayCalendar.Availability = AvailabilityEnum.NoService;
            }
            return dayCalendar;
        }

        public async Task<List<KeyValue>> GetMonths()
        {
            try
            {
                var months = new List<KeyValue>();
                months.Add(new KeyValue { Key = "Enero", Value = "01" });
                months.Add(new KeyValue { Key = "Febrero", Value = "02" });
                months.Add(new KeyValue { Key = "Marzo", Value = "03" });
                months.Add(new KeyValue { Key = "Abril", Value = "04" });
                months.Add(new KeyValue { Key = "Mayo", Value = "05" });
                months.Add(new KeyValue { Key = "Junio", Value = "06" });
                months.Add(new KeyValue { Key = "Julio", Value = "07" });
                months.Add(new KeyValue { Key = "Agosto", Value = "08" });
                months.Add(new KeyValue { Key = "Septiembre", Value = "09" });
                months.Add(new KeyValue { Key = "Octubre", Value = "10" });
                months.Add(new KeyValue { Key = "Noviembre", Value = "11" });
                months.Add(new KeyValue { Key = "Diciembre", Value = "12" });
                return months;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<KeyValue>> GetYears()
        {
            try
            {
                //var yearTo = 0;
                //var value = _context.Configurations.FirstOrDefault(r => r.Name == "YearTo");
                //if (value == null)
                var yearTo = DateTime.Now.Year + 2;
                //else
                //yearTo = int.Parse(value.Value);
                var years = new List<KeyValue>();
                var year = DateTime.Now.Year;
                while (year < yearTo)
                {
                    years.Add(new KeyValue { Key = year.ToString(), Value = year.ToString() });
                    year++;
                }
                return years;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Metodos Administracion de Horarios
        public async Task<List<ScheduleAdminApi>> GetScheduleByAdministrator(int id)
        {
            try
            {
                var query = from sc in _context.Schedules
                            join wk in _context.Weekdays on sc.WeekdayId equals wk.WeekdayId
                            where sc.DelegationId == id
                            select new ScheduleAdminApi
                            {
                                ScheduleId = sc.ScheduleId,
                                DelegationId = sc.DelegationId,
                                Capacity = sc.Capacity,
                                Time = sc.Time,
                                WeekDay = wk.Description,
                                WeekdayId = sc.WeekdayId
                            };
                return await query.OrderBy(r => r.WeekdayId).ThenBy(r => r.Time).ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<List<SpecialDay>> GetNonLaborableDaysAdministrator(int id)
        {
            try
            {
                var query = _context.SpecialDays.Where(r => r.IsNonWorking && r.DelegationId == id);
                return await query.OrderBy(r => r.Date).ToListAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<List<SpecialDayScheduleApi>> GetSpecialDaysDateAdministrator(int id)
        {
            try
            {
                var specialDays = new List<SpecialDayScheduleApi>();
                var overridedDays =
                    await
                        _context.SpecialDays.Where(r => r.DelegationId == id && r.IsOverrided)
                            .OrderBy(r => r.Date)
                            .ToListAsync();
                foreach (var overridedDay in overridedDays)
                {
                    var currentDay = new SpecialDayScheduleApi();
                    currentDay.Schedules = new List<SpecialDaysSchedule>();
                    currentDay.SpecialDay = new SpecialDay
                    {
                        Date = overridedDay.Date,
                        DelegationId = overridedDay.DelegationId,
                        IsNonWorking = overridedDay.IsNonWorking,
                        IsOverrided = overridedDay.IsOverrided
                    };

                    var specialSchedules =
                        await
                            _context.SpecialDaysSchedules.Where(r => r.Date == overridedDay.Date && r.DelegationId == id)
                                .ToListAsync();
                    foreach (var specialSchedule in specialSchedules)
                    {
                        currentDay.Schedules.Add(new SpecialDaysSchedule
                        {
                            Capacity = specialSchedule.Capacity,
                            Date = specialSchedule.Date,
                            DelegationId = specialSchedule.DelegationId,
                            SpecialDayScheduleId = specialSchedule.SpecialDayScheduleId,
                            Time = specialSchedule.Time
                        });
                    }
                    specialDays.Add(currentDay);
                }

                return specialDays;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<int> DeleteSchedule(Guid id)
        {
            try
            {
                var sche = _context.Schedules.Find(id);
                if (sche != null)
                {
                    _context.Schedules.Remove(sche);
                }

                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> DeleteNonLaboraleDays(int id, DateTime date)
        {
            try
            {
                var nolaborableday = _context.SpecialDays.Find(id, date);
                _context.SpecialDays.Remove(nolaborableday);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        /// <summary>
        /// Elimina el horario que haya sido removido de la lista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSpecialDaysSchedules(Guid id)
        {
            var special = _context.SpecialDaysSchedules.Find(id);
            _context.SpecialDaysSchedules.Remove(special);
            return await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Elimina las horas asociadas a la fecha especial
        /// </summary>
        /// <param name="specialScheduleDays"></param>
        /// <returns></returns>
        public async Task<int> DeleteSpecialScheduleDays(Guid specialScheduleDays)
        {
            try
            {
                var special = _context.SpecialDaysSchedules.Find(specialScheduleDays);
                _context.SpecialDaysSchedules.Remove(special);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveSchedules(List<Schedule> saves)
        {
            try
            {
                _context.Schedules.AddRange(saves);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<int> SaveNonLaborableDays(List<SpecialDay> specialDays)
        {
            try
            {
                _context.SpecialDays.AddRange(specialDays);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<int> SaveSpecialDayAndSchedule(List<SpecialDayScheduleApi> specialDaysapi)
        {
            try
            {
                foreach (var spa in specialDaysapi)
                {
                    _context.SpecialDays.AddOrUpdate(spa.SpecialDay);
                    _context.SpecialDaysSchedules.AddRange(spa.Schedules);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveSpecialSchedules(List<SpecialDaysSchedule> specialSchedules)
        {
            _context.SpecialDaysSchedules.AddRange(specialSchedules);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appoinment>> GetAppointmentsForDelegation(int Delegationid)
        {
            return
                await _context.Appoinments.Where(appointment => appointment.Delegationid == Delegationid).ToListAsync();
        }

        #endregion
    }
}