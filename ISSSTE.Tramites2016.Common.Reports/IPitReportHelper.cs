using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model.Fosa;
using ISSSTE.Tramites2016.Common.Reports.Model.Velacion;
using ISSSTE.Tramites2016.Common.Reports.Model;

namespace ISSSTE.Tramites2016.Common.Reports
{
    public interface IPitReportHelper
    {
        /// <summary>
        /// Obtiene el titulo de propiedad en formato Pdf
        /// </summary>
        /// <param name="name">Nombre del titular</param>
        /// <param name="noTitle">Número del titulo de propiedad</param>
        /// <param name="date">Fecha en la que sexpide el titulo de propiedad</param>
        /// <param name="beneficiaries">Lista de beneficiarios registrados</param>
        /// <returns>Arreglo de bytes con el titulo de propiedad generado</returns>
        byte[] GetPropertyTitlePDFStream(string name, string noTitle, DateTime date, List<string> beneficiaries);

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
        byte[] GetIndicatorsPitStream(string delegation, string operador, string starDate, string endDate,
                                        int documentationRuledInTime, int acceptedRequests, int rejectedRequests, int totalReceivedRequests, int pitsSoldViaWeb,
                                        int requestsInTime, IEnumerable<int> documentationRuleTimes, ReportFormat format);


    }
}
