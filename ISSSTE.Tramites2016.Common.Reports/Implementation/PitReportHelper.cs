using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using ISSSTE.Tramites2016.Common.Reports.Model.Fosa;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Resources;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
    public class PitReportHelper : IPitReportHelper
    {
        #region Implementation
        /// <summary>
        /// Obtiene el titulo de propiedad en formato Pdf
        /// </summary>
        /// <param name="name">Nombre del titular</param>
        /// <param name="noTitle">Número del titulo de propiedad</param>
        /// <param name="date">Fecha en la que sexpide el titulo de propiedad</param>
        /// <param name="beneficiaries">Lista de beneficiarios registrados</param>
        /// <returns>Arreglo de bytes con el titulo de propiedad generado</returns>
        public byte[] GetPropertyTitlePDFStream(string name, string noTitle, DateTime date, List<string> beneficiaries)
        {
            var report = BuildTitleProperty(name, noTitle, date, beneficiaries);

            var reportPropertyTitle = report.ExportToStream(ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                reportPropertyTitle.CopyTo(ms);
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
        /// <param name="documentationRuledInTime">Numero de tramites dictaminados en tiempo</param>
        /// <param name="acceptedRequests">Número de solicitudes aceptadas</param>
        /// <param name="rejectedRequests">Número de solicitudes rechazadas</param>
        /// <param name="totalReceivedRequests">Numero total de solicitudes recibidos</param>
        /// <param name="pitsSoldViaWeb">Número de fosas vendida vía web</param>
        /// <param name="requestsInTime">Numero trámites realizados en el tiempo establecido</param>
        /// <param name="documentationRuleTimes">Lista con  (Fecha dictaminación documentación - Fecha de recepción de documentación) </param>
        /// <param name="format">Indica el formato de salida deseado</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        public byte[] GetIndicatorsPitStream(string delegation, string operador, string starDate, string endDate,
                                        int documentationRuledInTime, int acceptedRequests, int rejectedRequests, int totalReceivedRequests, int pitsSoldViaWeb,
                                        int requestsInTime, IEnumerable<int> documentationRuleTimes, ReportFormat format)
        {
            var report = BuildPitIndicators(delegation, operador, starDate, endDate, documentationRuledInTime, acceptedRequests, rejectedRequests, totalReceivedRequests,
                                            pitsSoldViaWeb, requestsInTime, documentationRuleTimes);



            var PitIndicators = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                PitIndicators.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Construye el titulo de propiedad con la información proporcionada
        /// </summary>
        /// <param name="name">Nombre del titular</param>
        /// <param name="noTitle">Número del titulo de propiedad</param>
        /// <param name="date">Fecha en la que sexpide el titulo de propiedad</param>
        /// <param name="beneficiaries">Lista de beneficiarios registrados</param>
        /// <returns>Objeto con el titulo de propiedad</returns>
        private PropertyTitleReport BuildTitleProperty(string name, string noTitle, DateTime date, List<string> beneficiaries)
        {
            PropertyTitleReport report = new PropertyTitleReport();

            //Se cargan los datos del titulo de propiedad
            report.SetParameterValue(0, name ?? "");
            report.SetParameterValue(1, noTitle ?? "");
            report.SetParameterValue(2, beneficiaries.First() ?? "");
            report.SetParameterValue(3, beneficiaries.Count() > 1 ? beneficiaries[1] : "");
            report.SetParameterValue(4, date);

            return report;
        }

        /// <summary>
        /// Construye el reporte con los indicadores para solicitud de obtención de fosa
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="documentationRuledInTime">Numero de tramites dictaminados en tiempo</param>
        /// <param name="acceptedRequests">Número de solicitudes aceptadas</param>
        /// <param name="rejectedRequests">Número de solicitudes rechazadas</param>
        /// <param name="totalReceivedRequests">Numero total de solicitudes recibidos</param>
        /// <param name="pitsSoldViaWeb">Número de fosas vendida vía web</param>
        /// <param name="requestsInTime">Numero trámites realizados en el tiempo establecido</param>
        /// <param name="documentationRuleTimes">Lista con  (Fecha dictaminación documentación - Fecha de recepción de documentación) </param>
        /// <returns>Objeto con el reporte de indicadores para solicitud de obtención de fosa</returns>
        private PitIndicators BuildPitIndicators(string delegation, string operador, string starDate, string endDate,
                                        int documentationRuledInTime, int acceptedRequests, int rejectedRequests, int totalReceivedRequests, int pitsSoldViaWeb,
                                        int requestsInTime, IEnumerable<int> documentationRuleTimes)
        {
            PitIndicators report = new PitIndicators();

            //Se cargan los datos del titulo de propiedad
            int daysAverageValidation = documentationRuleTimes.Sum();

            report.SetParameterValue(report.Parameter_Delegacion.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_Operador.ParameterFieldName, operador);
            report.SetParameterValue(report.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            report.SetParameterValue(report.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);
            report.SetParameterValue(report.Parameter_TramitesEnTiempo.ParameterFieldName, requestsInTime);
            report.SetParameterValue(report.Parameter_SolcitudesAceptadas.ParameterFieldName, acceptedRequests);
            report.SetParameterValue(report.Parameter_SolicitudesRechazadas.ParameterFieldName, rejectedRequests);
            report.SetParameterValue(report.Parameter_DictaminadosEnTiempo.ParameterFieldName, documentationRuledInTime);
            report.SetParameterValue(report.Parameter_NoTotalSolicitudes.ParameterFieldName, totalReceivedRequests);
            report.SetParameterValue(report.Parameter_FosasVendidas.ParameterFieldName, pitsSoldViaWeb);
            report.SetParameterValue(report.Parameter_DiasPromedioValidacion.ParameterFieldName, daysAverageValidation);         


            return report;
        }

        #endregion
    }
}
