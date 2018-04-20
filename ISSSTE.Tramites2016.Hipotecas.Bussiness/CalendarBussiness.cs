using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Bussines
{
    public class CalendarBussiness
    {
        CalendarDataAccess calDA = new CalendarDataAccess();
        public List<Holyday> GetHolyday()
        {
            return calDA.GetHolydayCA();
        }

        public bool HolydayByDate(DateTime date)
        {
            return calDA.HolydayByDateDA(date);
        }

        public List<Schedule> GetSpecialDays()
        {
            return calDA.GetSpecialDaysDA();
        }

        public List<Appoinment> GetAppoinmentsByRequest(Guid requestId)
        {
            return calDA.GetAppoinmentsByRequestDA(requestId);
        }

        public Appoinment GetAppoinmentsById(int appoinmentId)
        {
            return calDA.GetAppoinmentsByIdAD(appoinmentId);
        }

        public void CancelAppoinments(Guid RequestId)
        {
            calDA.CancelAppoinmentsDA(RequestId);
        }

        public int SaveAppoinments(Appoinment appointment)
        {
            return calDA.SaveAppoinmentsDA(appointment);
        }

        public int SaveSpecialDays(SpecialDay specialDay)
        {
            return calDA.SaveSpecialDaysDA(specialDay);
        }

        public int SaveSpecialDaysSchedules(List<SpecialDaysSchedule> specialDaysSchedule)
        {
            return calDA.SaveSpecialDaysSchedulesDA(specialDaysSchedule);
        }

        public int SaveSpecialDaysSchedules(SpecialDaysSchedule specialDaysSchedule)
        {
            return calDA.SaveSpecialDaysSchedulesDA(specialDaysSchedule);
        }

        public int SaveSchedules(Schedule schedule)
        {

            return calDA.SaveSchedulesDA(schedule);
        }


        public void DeleteSchedules(int delegationId)
        {
        }

        public List<SpecialDay> GetSpecialDays(int DelegationId)
        {
            return calDA.GetSpecialDaysDA(DelegationId);
        }

        public List<SpecialDaysSchedule> GetSpecialDaysSchedules(int DelegationId)
        {
            return calDA.GetSpecialDaysSchedulesDA(DelegationId);
        }

        public List<Schedule> GetSchedules(int DelegationId)
        {
            return calDA.GetSchedulesDA(DelegationId);
        }


        public void DeleteSpecialDays(int id, DateTime date)
        {
           calDA.DeleteSpecialDays(id, date);
        }

      //  DeleteSpecialScheduleDays

    }


}
