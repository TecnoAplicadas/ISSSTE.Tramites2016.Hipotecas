using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class CivilState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CivilStateId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
