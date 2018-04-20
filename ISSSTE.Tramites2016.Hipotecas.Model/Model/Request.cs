#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Request
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            Appoinments = new HashSet<Appoinment>();
            //Debtors = new HashSet<Debtor>();
            //Opinions = new HashSet<Opinion>();
            RequestStatus = new HashSet<RequestStatu>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RequestId { get; set; }

        [Required]
        [StringLength(20)]
        public string EntitleId { get; set; }

        [Required]
        [StringLength(50)]
        public string Folio { get; set; }

        public bool? IsComplete { get; set; }
               
        public DateTime Date { get; set; }

        public int? IdLegalUnit { get; set; }

        [StringLength(250)]
        public string WritingProperty { get; set; }

        public bool IsConjugalCredit { get; set; }

        //public bool IsActive { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appoinment> Appoinments { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestStatu> RequestStatus { get; set; }


        //public int AppointmentCount { get; set; }

        //[StringLength(50)]
        //public string UserId { get; set; }

        //public bool IsAssigned { get; set; }

        //public int? ValidationId { get; set; }

        // public int? TimeContributionId { get; set; }

        //   public int PensionId { get; set; }
        //  public DateTime Date { get; set; }

        //[StringLength(256)]
        //public string Description { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Debtor> Debtors { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Opinion> Opinions { get; set; }

        //public virtual Validation Validation { get; set; }

        //   public virtual TimeContribution TimeContribution { get; set; }

    }
}