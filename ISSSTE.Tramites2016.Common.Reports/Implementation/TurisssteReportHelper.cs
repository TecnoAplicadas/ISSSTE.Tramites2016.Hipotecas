using System.Collections.Generic;
using System.IO;
using System.Linq;
using CrystalDecisions.Shared;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using ISSSTE.Tramites2016.Common.Reports.Resources;
using System;
using System.Collections;
using System.Security.Cryptography;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports.ViewerObjectModel;
using CrystalDecisions.Windows.Forms;
using ISSSTE.Tramites2016.Common.Reports.Model.Turissste;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
  /// <summary>
  /// Implementación del generador de indicadores para el reporte
  /// </summary>
  public class TurisssteReportHelper : ITurisssteReportHelper
  {
    #region Implementation

    /// <summary>
    /// Obtiene el reporte de indicadores en formato pdf o excel
    /// </summary>
    /// <param name="agency">Agencia turistica donde se consultaran los indicadores</param>
    /// <param name="operador">Operador que realiza el proceso</param>
    /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
    /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="quoteAnsewerdInTime">Numero de tramites atendidas en tiempo</param>
    /// <param name="totalReceivedRequestsViaWeb">Lista con numero total de solicitudes recibidas via web para paquetes, hospedaje y transporte</param>
        /// <param name="quoteAnswerTimes">Lista con resultado de (Fecha envío de solicitud - Fecha de envío de cotización)</param>
    /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
    public byte[] GetIndicatorsTurisssteStream(string agency, string operador, string starDate, string endDate,
            int quoteAnsewerdInTime, IEnumerable<int> totalReceivedRequestsViaWeb, IEnumerable<int> quoteAnswerTimes,
        ReportFormat format)
    {
            var report = BuildTurisssteIndicators(agency, operador, starDate, endDate, quoteAnsewerdInTime, totalReceivedRequestsViaWeb,
                                              quoteAnswerTimes);
      
            var turIsssteIndicators = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                turIsssteIndicators.CopyTo(ms);
                return ms.ToArray();
            }
    }

    /// <summary>
    /// Obtiene el formato de la solicitud para que el derechohabiente la firme
    /// </summary>
    /// <param name="header">Encabezado del reporte</param>
    /// <param name="bodyPackage">Cuerpo del formato para paquetes turísticos</param>
    /// <param name="bodyLodgment">Cuerpo del formato para paquetes de hospedaje</param>
    /// <param name="bodyTransportation">Cuerpo del formato para paquetes de transporte</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
    public byte[] GetTurissteFormatStream(Header header, 
                                          BodyPackage bodyPackage, 
                                          BodyLodgment bodyLodgment, 
                                          BodyTransportation bodyTransportation)
    {
            ReportClass report;

            if (header.BodyType.ToString().ToLower().Equals(ReportValues.PackgeTypeFormat))
            {
                    report = BuildTurisssteFormatPackage(header, bodyPackage);
            }
            else if (header.BodyType.ToString().ToLower().Equals(ReportValues.LodgmentTypeFormat))
            {
                    report = BuildTurisssteFormatLodgment(header, bodyLodgment);
            }
            else
            {
                    report = BuildTurisssteFormatTransportation(header, bodyTransportation);
            }

            var turisssteFormat = report.ExportToStream(ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                turisssteFormat.CopyTo(ms);
                return ms.ToArray();
            }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Construye el reporte de indicadores en formato pdf o excel
    /// </summary>
    /// <param name="agency">Agencia turistica donde se consultaran los indicadores</param>
    /// <param name="operador">Operador que realiza el proceso</param>
    /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
    /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="quoteAnswerdInTime">Numero de cotizaciones atendidas en tiempo</param>
    /// <param name="totalReceivedRequestsViaWeb">Lista con numero total de solicitudes recibidas via web para paquetes, hospedaje y transporte</param>
        /// <param name="quoteAnswerTimes">Lista con resultado de (Fecha envío de solicitud - Fecha de envío de cotización)</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
    private TurIsssteIndicators BuildTurisssteIndicators(string agency, string operador, string starDate, string endDate,
                                                            int quoteAnswerdInTime, IEnumerable<int> totalReceivedRequestsViaWeb,
                                                            IEnumerable<int> quoteAnswerTimes)
    {
            TurIsssteIndicators report = new TurIsssteIndicators();

            report.SetParameterValue(report.Parameter_Agencia.ParameterFieldName, agency);
            report.SetParameterValue(report.Parameter_Operador.ParameterFieldName, operador);
            report.SetParameterValue(report.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            report.SetParameterValue(report.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);
            report.SetParameterValue(report.Parameter_SolicitdesViaWebPaqete.ParameterFieldName, totalReceivedRequestsViaWeb.ElementAtOrDefault(0));
            report.SetParameterValue(report.Parameter_SolicitdesViaWebHospedaje.ParameterFieldName, totalReceivedRequestsViaWeb.ElementAtOrDefault(1));
            report.SetParameterValue(report.Parameter_SolicitdesViaWebTransporteAereo.ParameterFieldName, totalReceivedRequestsViaWeb.ElementAtOrDefault(2));
            report.SetParameterValue(report.Parameter_SolicitdesViaWebTransporteTerrestre.ParameterFieldName, totalReceivedRequestsViaWeb.ElementAtOrDefault(3));
            report.SetParameterValue(report.Parameter_SolicitdesRevisadasEnTiempo.ParameterFieldName, quoteAnswerdInTime);
            report.SetParameterValue(report.Parameter_SolicitudesRevisadasFueraDeTiempo.ParameterFieldName, quoteAnswerTimes.Count() - quoteAnswerdInTime);
      
            return report;
    }

    /// <summary>
    /// Construye el reporte de indicadores en formato pdf o excel
    /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="bodyTransportation">Cuerpo del formato para paquetes de transporte</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        private TurIsssteFormatServiceTransportation BuildTurisssteFormatTransportation(Header header, BodyTransportation bodyTransportation)
        {
            TurIsssteFormatServiceTransportation formatServicePackage = new TurIsssteFormatServiceTransportation();

            formatServicePackage.Subreports[ReportValues.NameSubreportHeader].SetDataSource(new List<Entitle>() { header.Entitle });

            formatServicePackage.Subreports[ReportValues.NameSubreportTransportationData].SetDataSource(new List<BodyTransportation>() { bodyTransportation });

            formatServicePackage.Subreports[ReportValues.NameSubreportTransportationPerson].SetDataSource(bodyTransportation.Travelers);

            formatServicePackage.Subreports[ReportValues.NameSubreportTransportationItinerary].SetDataSource(bodyTransportation.Itinerary);

            return formatServicePackage;
        }

        /// <summary>
        /// Construye el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="bodyLodgment">Cuerpo del formato para paquetes de hospedaje</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        private TurIsssteFormatServiceLodgment BuildTurisssteFormatLodgment(Header header, BodyLodgment bodyLodgment)
        {
            TurIsssteFormatServiceLodgment formatServicePackage = new TurIsssteFormatServiceLodgment();

            formatServicePackage.Subreports[ReportValues.NameSubreportHeader].SetDataSource(new List<Entitle>() { header.Entitle });

            formatServicePackage.Subreports[ReportValues.NameSubreportLodgmentData].SetDataSource(new List<BodyLodgment>() { bodyLodgment });

            formatServicePackage.Subreports[ReportValues.NameSubreportLodgmentPerson].SetDataSource(bodyLodgment.Guests);

            return formatServicePackage;
        }

        /// <summary>
        /// Construye el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="header">Encabezado del reporte</param>
        /// <param name="bodyPackage">Cuerpo del formato para paquetes turísticos</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
        private TurIsssteFormatServicePackage BuildTurisssteFormatPackage(Header header, BodyPackage bodyPackage)
        {
            TurIsssteFormatServicePackage formatServicePackage = new TurIsssteFormatServicePackage();

            formatServicePackage.Subreports[ReportValues.NameSubreportHeader].SetDataSource(new List<Entitle>() { header.Entitle });

            formatServicePackage.Subreports[ReportValues.NameSubreportPackgeData].SetDataSource(new List<BodyPackage>() { bodyPackage });

            formatServicePackage.Subreports[ReportValues.NameSubreportPackgePerson].SetDataSource(bodyPackage.PackagePersons);

            return formatServicePackage;
        }

    #endregion
  }
}
