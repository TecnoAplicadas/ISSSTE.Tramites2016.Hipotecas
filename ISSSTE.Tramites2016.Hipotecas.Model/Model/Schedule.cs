#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid ScheduleId { get; set; }

        public int DelegationId { get; set; }
        public int WeekdayId { get; set; }
        public TimeSpan Time { get; set; }
        public int Capacity { get; set; }
        public virtual Delegation Delegation { get; set; }
    }
}