using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class CalendarDataAccess
    {
        Conection con = new Conection();


        public List<Holyday> GetHolydayCA()
        {
            DataSet ds = new DataSet();
            List<Holyday> listH = new List<Holyday>();

            string query = "exec spS_Holyday";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                Holyday uc = new Holyday();
                uc.Description = DR["Description"].ToString();
                uc.Date = Convert.ToDateTime(DR["NoDeptos"].ToString());

                listH.Add(uc);

            }

            return listH;

        }

        public bool HolydayByDateDA(DateTime date)
        {
            DataSet ds = new DataSet();
            List<Holyday> listH = new List<Holyday>();

            string query = "exec spS_HolydayByDate @Date ='" + date + "'";

            ds = con.ObtenerConsulta(query);


            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Schedule> GetSpecialDaysDA()
        {
            DataSet ds = new DataSet();
            List<Schedule> listSD = new List<Schedule>();

            string query = "exec spS_SpecialDays";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                Schedule uc = new Schedule();
                uc.Capacity = Convert.ToInt32(DR["Capacity"].ToString());
                uc.DelegationId = Convert.ToInt32(DR["DelegationId"].ToString());
                //uc.ScheduleId = Convert.ToInt32(DR["ScheduleId"].ToString()); MFP 03-01-2017
                uc.Time = TimeSpan.Parse(DR["Time"].ToString());
                uc.WeekdayId = Convert.ToInt32(DR["WeekdayId"].ToString());

                listSD.Add(uc);

            }

            return listSD;
        }

        public List<Appoinment> GetAppoinmentsByRequestDA(Guid requestId)
        {

            DataSet ds = new DataSet();
            List<Appoinment> listA = new List<Appoinment>();

            string query = "exec spS_AppoinmentByRequest @RequestID='" + requestId + "'";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                Appoinment ap = new Appoinment();

                ap.AppoinmentId = Guid.Parse(DR["AppoinmentId"].ToString());
                ap.Date = Convert.ToDateTime(DR["Date"].ToString());
                ap.Delegationid = Convert.ToInt32(DR["DelegationId"].ToString());
                ap.IsAttended = Convert.ToBoolean(DR["IsAttended"].ToString());
                ap.IsCancelled = Convert.ToBoolean(DR["IsCancelled"].ToString());
                ap.RequestId = Guid.Parse(DR["RequestId"].ToString());
                ap.Time = TimeSpan.Parse(DR["Capacity"].ToString());


                listA.Add(ap);

            }

            return listA;

        }

        public Appoinment GetAppoinmentsByIdAD(int appoinmentId)
        {
            DataSet ds = new DataSet();
            Appoinment ap = new Appoinment();

            string query = "exec spS_AppoinmentByID @appoinmentId=" + appoinmentId;

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {


                ap.AppoinmentId = Guid.Parse(DR["AppoinmentId"].ToString());
                ap.Date = Convert.ToDateTime(DR["Date"].ToString());
                ap.Delegationid = Convert.ToInt32(DR["DelegationId"].ToString());
                ap.IsAttended = Convert.ToBoolean(DR["IsAttended"].ToString());
                ap.IsCancelled = Convert.ToBoolean(DR["IsCancelled"].ToString());
                ap.RequestId = Guid.Parse(DR["RequestId"].ToString());
                ap.Time = TimeSpan.Parse(DR["Capacity"].ToString());

            }

            return ap;
        }


        public void CancelAppoinmentsDA(Guid RequestId)
        {
            DataSet ds = new DataSet();

            string query = "exec spU_CancelAppoinment @RequestId = '" + RequestId + "'";

            ds = con.ObtenerConsulta(query);



        }

        public int SaveAppoinmentsDA(Appoinment appointment)
        {
            DataSet ds = new DataSet();

            string query = "exec spI_Appoinments @RequestId '= " + appointment.RequestId +
          "' , @DelegationId = " + appointment.Delegationid +
           ", @Date = '" + appointment.Date +
           "', @Time='" + appointment.Time +
          "', @IsAttended= " + appointment.IsAttended +
          " , @IsCancelled = " + appointment.IsCancelled;

            ds = con.ObtenerConsulta(query);

            return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());

        }

        public int SaveSpecialDaysDA(SpecialDay specialDay)
        {

            DataSet ds = new DataSet();

            string query = "exec spI_SpecialDays @DelegationId= " + specialDay.DelegationId +
           ", @Date =" + specialDay.Date +
           ",@IsNonWorking =" + specialDay.IsNonWorking +
           ", @IsOverrided=" + specialDay.IsOverrided;

            ds = con.ObtenerConsulta(query);

            return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());


        }

        public int SaveSpecialDaysSchedulesDA(List<SpecialDaysSchedule> specialDaysSchedule)
        {
            DataSet ds = new DataSet();
            foreach (SpecialDaysSchedule sds in specialDaysSchedule)
            {
                string query = "exec spI_SpecialDaysSchedules spI_SpecialDays @DelegationId=" + sds.DelegationId +
                           ", @Date='" + sds.Date +
                           "', @Time='" + sds.Time +
                            "', @Capacity =" + sds.Capacity;

                ds = con.ObtenerConsulta(query);
            }

            return 1;


        }


        public int SaveSpecialDaysSchedulesDA(SpecialDaysSchedule specialDaysSchedule)
        {
            DataSet ds = new DataSet();

            string query = "exec spI_SpecialDaysSchedules spI_SpecialDays @DelegationId=" + specialDaysSchedule.DelegationId +
                       ", @Date='" + specialDaysSchedule.Date +
                       "', @Time='" + specialDaysSchedule.Time +
                        "', @Capacity =" + specialDaysSchedule.Capacity;

            ds = con.ObtenerConsulta(query);


            return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());


        }

        public int SaveSchedulesDA(Schedule schedule)
        {
            DataSet ds = new DataSet();

            string query = "exec spI_Schedules @DelegationId = " + schedule.DelegationId +
               ", @WeekdayId=" + schedule.WeekdayId +
              " , @Time ='" + schedule.Time +
              "' , @Capacity = " + schedule.Capacity;

            ds = con.ObtenerConsulta(query);

            return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());

        }

        public List<SpecialDay> GetSpecialDaysDA(int DelegationId)
        {

            DataSet ds = new DataSet();
            List<SpecialDay> listA = new List<SpecialDay>();

            string query = "exec spS_SpecialDayByDelegation @DelegationId=" + DelegationId;

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                SpecialDay sd = new SpecialDay();

                sd.DelegationId = Convert.ToInt32(DR["DelegationId"].ToString());
                sd.Date = Convert.ToDateTime(DR["Date"].ToString());
                sd.IsNonWorking = Convert.ToBoolean(DR["IsNonWorking"].ToString());
                sd.IsOverrided = Convert.ToBoolean(DR["IsOverrided"].ToString());

                listA.Add(sd);

            }

            return listA;

        }

        public List<SpecialDaysSchedule> GetSpecialDaysSchedulesDA(int DelegationId)
        {
            DataSet ds = new DataSet();
            List<SpecialDaysSchedule> listA = new List<SpecialDaysSchedule>();

            string query = "exec spS_SpecialDaySchedulesByDelegation @DelegationId=" + DelegationId;

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                SpecialDaysSchedule sd = new SpecialDaysSchedule();

                sd.DelegationId = Convert.ToInt32(DR["DelegationId"].ToString());
                sd.Date = Convert.ToDateTime(DR["Date"].ToString());
                sd.Capacity = Convert.ToInt32(DR["Capacity"].ToString());
                sd.SpecialDayScheduleId = Convert.ToInt32(DR["SpecialDayScheduleId"].ToString());
                sd.Time = TimeSpan.Parse(DR["Time"].ToString());

                listA.Add(sd);

            }

            return listA;
        }

        public List<Schedule> GetSchedulesDA(int DelegationId)
        {

            DataSet ds = new DataSet();
            List<Schedule> listA = new List<Schedule>();

            string query = "exec spS_SchedulesByDelegation @DelegationId=" + DelegationId;

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                Schedule sd = new Schedule();

                sd.DelegationId = Convert.ToInt32(DR["DelegationId"].ToString());
                sd.Capacity = Convert.ToInt32(DR["Capacity"].ToString());
                sd.Time = TimeSpan.Parse(DR["Time"].ToString());
                //sd.ScheduleId = Convert.ToInt32(DR["ScheduleId"].ToString()); MFP 03-01-2017
                sd.WeekdayId = Convert.ToInt32(DR["WeekdayId"].ToString());


                listA.Add(sd);

            }

            return listA;

        }

        public void DeleteSpecialDays(int id, DateTime date)
        {
            DataSet ds = new DataSet();

            string query = "exec spD_SpecialDays @id = " + id + "', @date='" + date + "'";

            ds = con.ObtenerConsulta(query);

        }

    }
}
