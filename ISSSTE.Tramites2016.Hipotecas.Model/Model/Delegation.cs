#region

using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Hipotecas.Model.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "Delegation")]
    public class Delegation
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Delegation()
        {
            Appoinments = new HashSet<Appoinment>();
            Entitles = new HashSet<Entitle>();
            Schedules = new HashSet<Schedule>();
            SpecialDays = new HashSet<SpecialDay>();
            SpecialDaysSchedules = new HashSet<SpecialDaysSchedule>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DelegationId { get; set; }

        //public string Description { get; set; }

        [Required]
        [StringLength(5)]
        public string StateKey { get; set; }

        [Required]
        [StringLength(150)]
        [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "Delegation_Name", IsLabel = true, ShowInListView = true)]
        public string Name { get; set; }

        [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "Delegation_IsActive", ShowInListView = true)]
        public bool IsActive { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appoinment> Appoinments { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entitle> Entitles { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedules { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialDay> SpecialDays { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialDaysSchedule> SpecialDaysSchedules { get; set; }
    }
}