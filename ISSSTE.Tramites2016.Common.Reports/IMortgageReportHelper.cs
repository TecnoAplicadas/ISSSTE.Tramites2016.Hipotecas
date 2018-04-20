using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model.Hipoteca;

namespace ISSSTE.Tramites2016.Common.Reports
{
    public interface IMortgageReportHelper
    {
        /// <summary>
        /// obtiene el reporte de cancelación de hipoteca en formato pdf
        /// </summary>
        /// <param name="date"></param>
        /// <param name="mortgage"></param>
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        byte[] GetMortgageCancel(MortgageCancel mortgage);
    }
}
