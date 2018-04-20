using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Deportivas;

namespace ISSSTE.Tramites2016.Common.Reports
{
    public interface IActivitySportReportHelper
    {
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
        byte[] GetIndicatorsActivitySportStream(string delegation, string operador, string starDate, string endDate,
                                            int totalRequestActivitiesReceived, int totalRequestActivitiesReceivedAccepted,
                                            int totalRequestActivitiesReceivedRejected, int totalRequestEventsReceived,
                                            int totalRequestEventsReceivedAccepted, int totalRequestEventsReceivedRejected, IEnumerable<int> documentationDate,
                                            int documentAuditedInTime, ReportFormat format);

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
        byte[] GetCredentialPartipant(string entitleName, string noIssste, string delegation,
            string instructorName, string date, string placeAndHour);

        /// <summary>
        /// Obtiene la lista de participantes para una actividad o evento deportivo
        /// </summary>
        /// <param name="delegation">Delegación(es) participantes en la actividad o evento</param>
        /// <param name="eventActivityName">Nombre de la actividad o evento</param>
        /// <param name="phase">Etapa de la actividad o evento</param>
        /// <param name="listParticipants">Lista de participantes de la actividad o evento</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con la lista de participantes generada</returns>
        byte[] GetListOfParticipants(string delegation,
                                    string eventActivityName,
                                    string phase,
                                    IEnumerable<PartipantsActivityEvent> listParticipants,
                                    ReportFormat format);
    }
}
