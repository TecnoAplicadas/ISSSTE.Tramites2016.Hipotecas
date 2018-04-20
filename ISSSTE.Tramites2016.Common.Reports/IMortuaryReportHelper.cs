using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model.Velacion;
using ISSSTE.Tramites2016.Common.Reports.Model;


namespace ISSSTE.Tramites2016.Common.Reports
{
    public interface IMortuaryReportHelper
    {
        /// <summary>
        /// Obtiene el reporte de una cotización en formato Pdf
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="entitle">Datos personales del cliente</param>
        /// <param name="items">Detalle la cotización</param>
        /// <returns>Arreglo de bytes con el reporte final generado</returns>
        byte[] GetQuotationReportPDFStream(Header header, Entitle entitle, List<ItemsQuotation> items);

        /// <summary>
        /// Obtiene el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="totalQuotation">N° total de cotizaciones realizadas por derechohabientes y no derechohabientes</param>
        /// <param name="quotationEntitle">N° de cotizaciones hechas por derechoahbientes</param>
        /// <param name="quotationNotEntitle">N° de cotizaciones hechas por público en general</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        byte[] GetIndicatorsMortuaryStream(string delegation, string operador, string starDate, string endDate,
                                            int totalQuotation, int quotationEntitle , int quotationNotEntitle,
                                            ReportFormat format);
    }
}



