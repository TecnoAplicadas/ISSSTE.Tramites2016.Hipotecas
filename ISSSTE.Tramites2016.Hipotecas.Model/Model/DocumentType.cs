using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Hipotecas.Model.Resources;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "DocumentType")]
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(50)]
        [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "DocumentType_Name", IsLabel = true, ShowInListView = true)]
        public string Name { get; set; }

        [StringLength(256)]
        [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "DocumentType_Description", ShowInListView = true)]
        public string Description { get; set; }

        
        [StringLength(1)]
        public int BeneficiarieType { get; set; }

        [CatalogDisplay(ResourceType = typeof(DisplayNames), DisplayNameResourceId = "DocumentType_Required", ShowInListView = true)]
        public bool Required { get; set; }

    }
}
