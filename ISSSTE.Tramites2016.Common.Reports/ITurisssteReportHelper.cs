using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Turissste;

namespace ISSSTE.Tramites2016.Common.Reports
{
  /// <summary>
  /// 
  /// </summary>
  public interface ITurisssteReportHelper
  {
    /// <summary>
    /// Obtiene el reporte de indicadores en formato pdf o excel
    /// </summary>
    /// <param name="agency">Agencia turistica donde se consultaran los indicadores</param>
    /// <param name="operador">Operador que realiza el proceso</param>
    /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
    /// <param name="endDate">Limite superior para la consulta de los indicadores</param>
        /// <param name="quoteAnsewerdInTime">Numero de tramites atendidas en tiempo</param>
    /// <param name="totalReceivedRequestsViaWeb">Lista con numero total de solicitudes recibidas via web para paquetes, hospedaje y transporte</param>
        /// <param name="quoteAnswerTimes">Lista con resultado de (Fecha de envío de cotización - Fecha envío de solicitud)</param>
    /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
    byte[] GetIndicatorsTurisssteStream(string agency, string operador, string starDate, string endDate,
                                        int quoteAnsewerdInTime, IEnumerable<int> totalReceivedRequestsViaWeb,
                                            IEnumerable<int> quoteAnswerTimes, ReportFormat format);

    /// <summary>
    /// Obtiene el formato de la solicitud para que el derechohabiente la firme
    /// </summary>
    /// <param name="header">Encabezado del reporte</param>
    /// <param name="bodyPackage">Cuerpo del formato para paquetes turísticos</param>
    /// <param name="bodyLodgment">Cuerpo del formato para paquetes de hospedaje</param>
    /// <param name="bodyTransportation">Cuerpo del formato para paquetes de transporte</param>
    /// <param name="footer">Pie del formato</param>
    /// <returns>Arreglo de bytes con el reporte generado</returns>
    byte[] GetTurissteFormatStream(Header header, 
                                   BodyPackage bodyPackage,
                                   BodyLodgment bodyLodgment,
                                   BodyTransportation bodyTransportation);
  }
}
