using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class TimeContribution
    {

        [Key]
        public int TimeContributionId { get; set; }

        [StringLength(256)]
        public string TimeContributions { get; set; }

        [StringLength(256)]
        public string TimeLicenses { get; set; }

        [StringLength(256)]
        public string TotalTime { get; set; }
        



    }
}
