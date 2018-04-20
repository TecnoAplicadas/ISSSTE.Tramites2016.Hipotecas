using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class RelationshipDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelationshipDocumentsId { get; set; }
        public int PensionId { get; set; }
        public int RelationshipId { get; set; }
        public int RelationshipTitleId { get; set; }
        public Guid DocumentsId { get; set; }

    }
}
