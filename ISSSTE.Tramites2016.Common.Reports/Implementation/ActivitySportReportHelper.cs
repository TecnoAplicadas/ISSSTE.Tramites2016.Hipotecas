using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Deportivas;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using ISSSTE.Tramites2016.Common.Reports.Resources;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
    public class ActivitySportReportHelper : IActivitySportReportHelper
    {
        #region Implementation

        /// <summary>
        /// Obtiene el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="totalRequestActivitiesReceived">N° total de solicitudes recibidas a actividades deportivas permanentes</param>
        /// <param name="totalRequestActivitiesReceivedAccepted">N° de solicitudes recibidas aceptadas a actividades deportivas permanentes</param>
        /// <param name="totalRequestActivitiesReceivedRejected">N° de solicitudes recibidas rechazadas a actividades deportivas permanentes</param>
        /// <param name="totalRequestEventsReceived">N° total de solicitudes recibidas a eventos deportivos</param>
        /// <param name="totalRequestEventsReceivedAccepted">N° total de solicitudes recibidas aceptadas a eventos deportivos</param>
        /// <param name="totalRequestEventsReceivedRejected">N° total de solicitudes recibidas rechazadas a eventos deportivos</param>
        /// <param name="documentAuditedInTime">N° de tramites dictaminados en tiempo</param>
        /// <param name="documentationDate">Lista con resultado de (Fecha dictaminación de la documentación - Fecha de recepción de documentación)</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        public byte[] GetIndicatorsActivitySportStream(string delegation, string operador, string starDate, string endDate,
             int totalRequestActivitiesReceived, int totalRequestActivitiesReceivedAccepted,
             int totalRequestActivitiesReceivedRejected, int totalRequestEventsReceived, int totalRequestEventsReceivedAccepted,
             int totalRequestEventsReceivedRejected, IEnumerable<int> documentationDate, int documentAuditedInTime, ReportFormat format)
        {
            var report = BuildActivitySportIndicators(delegation, operador, starDate, endDate,
            totalRequestActivitiesReceived, totalRequestActivitiesReceivedAccepted,
            totalRequestActivitiesReceivedRejected, totalRequestEventsReceived, totalRequestEventsReceivedAccepted,
            totalRequestEventsReceivedRejected, documentationDate, documentAuditedInTime);
            
            var reportQuotation = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                reportQuotation.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene la credencial del particpante para una actividad o evento deportivo
        /// </summary>
        /// <param name="entitleName">Nombre completo del participante</param>
        /// <param name="noIssste">Numero de Isste del participante</param>
        /// <param name="delegation">Delegacion donde de llevara a cabo la actividad/evento</param>
        /// <param name="instructorName">Nombre completo del instructor de la actividad</param>
        /// <param name="date">Fecha en la que se llevara a acabp la actividad / evento</param>
        /// <param name="placeAndHour">Lugar, hora e información relevante sobre la actividad/evento</param>
        /// <returns>Arreglo de bytes con la crdencial generada</returns>
        public byte[] GetCredentialPartipant(string entitleName, 
                                            string noIssste, 
                                            string delegation, 
                                            string instructorName, 
                                            string date,
                                            string placeAndHour)
        {
            var credential = BuildCredentialParticipant(entitleName,noIssste,delegation,instructorName,date,placeAndHour);

            var credentialGenerated = credential.ExportToStream(ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                credentialGenerated.CopyTo(ms);
                return ms.ToArray();
            }
        }

        

        /// <summary>
        /// Obtiene la lista de participantes para una actividad o evento deportivo
        /// </summary>
        /// <param name="delegation">Delegación(es) participantes en la actividad o evento</param>
        /// <param name="eventActivityName">Nombre de la actividad o evento</param>
        /// <param name="phase">Etapa de la actividad o evento</param>
        /// <param name="listParticipants">Lista de participantes de la actividad o evento</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con la lista de participantes generada</returns>
        public byte[] GetListOfParticipants(string delegation, 
                                            string eventActivityName, 
                                            string phase, 
                                            IEnumerable<PartipantsActivityEvent> listParticipants,
                                            ReportFormat format)
        {
            var report = BuildListOfParticipants(delegation, eventActivityName, phase, listParticipants);

            var reportQuotation = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                reportQuotation.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Construye el reporte de indicadores 
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="totalRequestActivitiesReceived">N° total de solicitudes recibidas a actividades deportivas permanentes</param>
        /// <param name="totalRequestActivitiesReceivedAccepted">N° de solicitudes recibidas aceptadas a actividades deportivas permanentes</param>
        /// <param name="totalRequestActivitiesReceivedRejected">N° de solicitudes recibidas rechazadas a actividades deportivas permanentes</param>
        /// <param name="totalRequestEventsReceived">N° total de solicitudes recibidas a eventos deportivos</param>
        /// <param name="totalRequestEventsReceivedAccepted">N° total de solicitudes recibidas aceptadas a eventos deportivos</param>
        /// <param name="totalRequestEventsReceivedRejected">N° total de solicitudes recibidas rechazadas a eventos deportivos</param>
        /// <param name="documentAuditedInTime">N° de tramites dictaminados en tiempo</param>
        /// <param name="documentationDate">Lista con resultado de (Fecha dictaminación de la documentación - Fecha de recepción de documentación)</param>
        /// <returns>Objeto con el reporte generado</returns>
        private ActivitySportIndicators BuildActivitySportIndicators(string delegation, string operador, string starDate, 
                                                                string endDate, int totalRequestActivitiesReceived, 
                                                                int totalRequestActivitiesReceivedAccepted, int totalRequestActivitiesReceivedRejected, 
                                                                int totalRequestEventsReceived, int totalRequestEventsReceivedAccepted,
                                                                int totalRequestEventsReceivedRejected, IEnumerable<int> documentationDate, 
                                                                int documentAuditedInTime)
        {
            ActivitySportIndicators report = new ActivitySportIndicators();

            //Se cargan los datos del titulo de propiedad
            int differenceDocumentationDate = documentationDate.Sum();

            report.SetParameterValue(report.Parameter_Delegacion.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_Operador.ParameterFieldName, operador);
            report.SetParameterValue(report.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            report.SetParameterValue(report.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);
            report.SetParameterValue(report.Parameter_TotalSolicitudesRecibidasActividades.ParameterFieldName, totalRequestActivitiesReceived);
            report.SetParameterValue(report.Parameter_SolicitudesRecibidasActividadesAceptadas.ParameterFieldName, totalRequestActivitiesReceivedAccepted);
            report.SetParameterValue(report.Parameter_SolicitudesRecibidasActividadesRechazadas.ParameterFieldName, totalRequestActivitiesReceivedRejected);
            report.SetParameterValue(report.Parameter_TotalSolicitudesRecibidasEventos.ParameterFieldName, totalRequestEventsReceived);
            report.SetParameterValue(report.Parameter_SolicitudesRecibidasEventosAceptadas.ParameterFieldName, totalRequestEventsReceivedAccepted);
            report.SetParameterValue(report.Parameter_SolicitudesRecibidasEventosRechazadas.ParameterFieldName, totalRequestEventsReceivedRejected);
            report.SetParameterValue(report.Parameter_TramitesDictaminadosTiempo.ParameterFieldName, documentAuditedInTime);
            report.SetParameterValue(report.Parameter_FechaDictaminacionFechaReccepcion.ParameterFieldName, differenceDocumentationDate);


            return report;
        }

        /// <summary>
        /// Construye la credencial del particpante para una actividad o evento deportivo
        /// </summary>
        /// <param name="entitleName">Nombre completo del participante</param>
        /// <param name="noIssste">Numero de Isste del participante</param>
        /// <param name="delegation">Delegacion donde de llevara a cabo la actividad/evento</param>
        /// <param name="instructorName">Nombre completo del instructor de la actividad</param>
        /// <param name="date">Fecha en la que se llevara a acabp la actividad / evento</param>
        /// <param name="placeAndHour">Lugar, hora e información relevante sobre la actividad/evento</param>
        /// <returns>Objeto con la crdencial generada</returns>
        private ActivitySportCredential BuildCredentialParticipant(string entitleName,
                                                                    string noIssste,
                                                                    string delegation,
                                                                    string instructorName,
                                                                    string date,
                                                                    string placeAndHour)
        {
            ActivitySportCredential credential = new ActivitySportCredential();

            credential.SetParameterValue(credential.Parameter_NombreDerechohabiente.ParameterFieldName, entitleName);
            credential.SetParameterValue(credential.Parameter_NoIssste.ParameterFieldName, noIssste);
            credential.SetParameterValue(credential.Parameter_Delegacion.ParameterFieldName, delegation);
            credential.SetParameterValue(credential.Parameter_NombreInstructor.ParameterFieldName, instructorName);
            credential.SetParameterValue(credential.Parameter_Fecha.ParameterFieldName, date);
            credential.SetParameterValue(credential.Parameter_HoraLugar.ParameterFieldName, placeAndHour);

            return credential;
        }

        /// <summary>
        /// Construye la lista de participantes para una actividad o evento deportivo
        /// </summary>
        /// <param name="delegation">Delegación(es) participantes en la actividad o evento</param>
        /// <param name="eventActivityName">Nombre de la actividad o evento</param>
        /// <param name="phase">Etapa de la actividad o evento</param>
        /// <param name="listParticipants">Lista de participantes de la actividad o evento</param>
        /// <returns>Objeto con la lista de participantes generada</returns>
        private ListOfParticipantsAndTeams BuildListOfParticipants(string delegation,
                                                string eventActivityName,
                                                string phase,
                                                IEnumerable<PartipantsActivityEvent> listParticipants)
        {
            ListOfParticipantsAndTeams listOfParticipants = new ListOfParticipantsAndTeams();
            
            listOfParticipants.SetDataSource(listParticipants);

            listOfParticipants.SetParameterValue(listOfParticipants.Parameter_Delegacion.ParameterFieldName, delegation);
            listOfParticipants.SetParameterValue(listOfParticipants.Parameter_NombreEventoActividad.ParameterFieldName, eventActivityName);
            listOfParticipants.SetParameterValue(listOfParticipants.Parameter_Etapa.ParameterFieldName, phase);

            return listOfParticipants;
        }
        #endregion


    }
}
