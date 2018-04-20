using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class RelationshipsTitlesKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelationshipsTitlesKeysId { get; set; }

        [Required]
        public int RelationshipTitleId { get; set; }

        [Required]
        public string RelationshipId { get; set; }
    }
}
