using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class MaximumSalary
    {
        [Key]
        public int SalaryId { get; set; }

        public decimal SalaryCap { get; set; }

        public int year { get; set; }
    }
}
