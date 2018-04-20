using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model.Velacion;
using CrystalDecisions.Shared;
using System.IO;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using System.Web;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Fosa;
using ISSSTE.Tramites2016.Common.Reports.Resources;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
    public class MortuaryReportHelper : IMortuaryReportHelper
    {
        #region Implementation
        /// <summary>
        /// Obtiene el reporte de una cotización en formato Pdf
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="entitle">Datos personales del cliente</param>
        /// <param name="items">Detalle la cotización</param>
        /// <returns>Arreglo de bytes con el reporte final generado</returns>
        public byte[] GetQuotationReportPDFStream(Header header, Entitle entitle, List<ItemsQuotation> items)
        {
            var report = BuildReport(header, entitle, items);
            
            var reportQuotation = report.ExportToStream(ExportFormatType.PortableDocFormat);
            
            using (MemoryStream ms = new MemoryStream())
            {
                reportQuotation.CopyTo(ms);
                return ms.ToArray();
            }
        }


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
        public byte[] GetIndicatorsMortuaryStream(string delegation, string operador, string starDate, string endDate,
                                            int totalQuotation, int quotationEntitle, int quotationNotEntitle,
                                            ReportFormat format)
        {
            var report = BuildMortuaryIndicators(delegation, operador, starDate, endDate, totalQuotation, quotationEntitle, quotationNotEntitle);
            
            var mmortuaryIndicators = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                mmortuaryIndicators.CopyTo(ms);
                return ms.ToArray();
            }
        }
        
        #endregion


        #region Helpers

        /// <summary>
        /// Construye el reporte con los objetos proporcionados
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="entitle">Datos personales del cliente</param>
        /// <param name="items">Detalle la cotización</param>
        /// <returns>Objeto con el reporte de cotización</returns>
        private QuotationReport BuildReport(Header header, Entitle entitle, List<ItemsQuotation> items)
        {
            QuotationReport report = new QuotationReport();
            report.Subreports[1].SetDataSource(BuildHeader(header));
            report.Subreports[0].SetDataSource(BuildEntitle(entitle));
            report.Subreports[2].SetDataSource(items);
            
            return report;
        }

        /// <summary>
        /// Construye el header del reporte
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <returns>Lista de objetos header</returns>
        private List<Header> BuildHeader(Header header)
        {
            List<Header> listHeader = new List<Header>();
            listHeader.Add(header);

            return listHeader;
        }

        /// <summary>
        /// Construye la sección de datos del cliente  del reporte
        /// </summary>
        /// <param name="entitle">Datos del cliente del reporte</param>
        /// <returns>Lista de objetos entitle</returns>
        private List<Entitle> BuildEntitle(Entitle entitle)
        {
            List<Entitle> listEntitle = new List<Entitle>();
            listEntitle.Add(entitle);

            return listEntitle;
        }


        /// <summary>
        /// Construye el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="totalQuotation">N° total de cotizaciones realizadas por derechohabientes y no derechohabientes</param>
        /// <param name="quotationEntitle">N° de cotizaciones hechas por derechoahbientes</param>
        /// <param name="quotationNotEntitle">N° de cotizaciones hechas por público en general</param>
        /// <returns>Objeto con el reporte generado</returns>
        private MortuaryIndicators BuildMortuaryIndicators(string delegation, string operador, string starDate, string endDate,
                                                            int totalQuotation, int quotationEntitle, int quotationNotEntitle)
        {
            MortuaryIndicators report = new MortuaryIndicators();
            
            report.SetParameterValue(report.Parameter_Delegacion.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_Operador.ParameterFieldName, operador);
            report.SetParameterValue(report.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            report.SetParameterValue(report.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);
            report.SetParameterValue(report.Parameter_TotalCotizaciones.ParameterFieldName, totalQuotation);
            report.SetParameterValue(report.Parameter_cotizacionesDerechohabientes.ParameterFieldName, quotationEntitle);
            report.SetParameterValue(report.Parameter_CotizacionesNoDerechohabientes.ParameterFieldName, quotationNotEntitle);

            return report;
        }
    }

    #endregion
}
