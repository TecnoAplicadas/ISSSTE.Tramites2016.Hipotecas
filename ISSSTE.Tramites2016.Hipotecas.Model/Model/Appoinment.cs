#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Appoinment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AppoinmentId { get; set; }

        public Guid RequestId { get; set; }
        public int Delegationid { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }
        public bool IsAttended { get; set; }
        public bool IsCancelled { get; set; }
        public virtual Delegation Delegation { get; set; }
        public virtual Request Request { get; set; }
    }
}