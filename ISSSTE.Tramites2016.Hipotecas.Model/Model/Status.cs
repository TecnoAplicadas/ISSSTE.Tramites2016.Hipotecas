#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Status
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Status()
        {
            RequestStatus = new HashSet<RequestStatu>();
            //StatusNextStatus = new HashSet<StatusNextStatu>();
            //StatusNextStatus1 = new HashSet<StatusNextStatu>();
            //StatusRelatedStatus = new HashSet<StatusRelatedStatu>();
            //StatusRelatedStatus1 = new HashSet<StatusRelatedStatu>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StatusId { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsCancel { get; set; }
        public bool IsNotify { get; set; }
        public bool IsShowInReport { get; set; }
        public int DueDays { get; set; }
        public string NotifyMessage { get; set; }

        [StringLength(1024)]
        public string Data { get; set; }

        public bool? IsFinal { get; set; }
        public bool? IsInitial { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestStatu> RequestStatus { get; set; }

        public virtual Role Role { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StatusNextStatu> StatusNextStatus { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StatusNextStatu> StatusNextStatus1 { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StatusRelatedStatu> StatusRelatedStatus { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StatusRelatedStatu> StatusRelatedStatus1 { get; set; }
    }
}