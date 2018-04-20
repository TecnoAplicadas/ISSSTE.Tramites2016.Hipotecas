#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class RequestStatu
    {

        [Column(Order =0),Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RequestStatusId { get; set; }
        //RequestStatusId
        public Guid RequestId { get; set; }

        [Column(Order = 1),Key]
        public int StatusId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public bool IsCurrentStatus { get; set; }

        //No existen:

        //[Required]
        //[StringLength(70)]
        //public string UserId { get; set; }

        [StringLength(256)]
        public string Observations { get; set; }

        //public int ElapsedDays { get; set; }
        //public int ElapsedWorkDays { get; set; }

        //[StringLength(1024)]
        //public string Data { get; set; }

        public virtual Request Request { get; set; }
        public virtual Status Status { get; set; }
    }
}