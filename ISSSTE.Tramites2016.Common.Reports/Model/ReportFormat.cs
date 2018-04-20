using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model
{
    /// <summary>
    /// Representa el formato en que se puede generar un reporte
    /// </summary>
    public enum ReportFormat
    {
        [Description(".xls")]
        Excel = 4,

        [Description(".pdf")]
        Pdf = 5
    }
}
