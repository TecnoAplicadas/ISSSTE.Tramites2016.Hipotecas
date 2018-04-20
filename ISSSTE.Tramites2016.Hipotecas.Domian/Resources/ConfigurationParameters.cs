using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Resources
{
    /// <summary>
    /// Contiene los diferentes parametros de configuración que se tiene en la aplicación
    /// </summary>
    public enum ConfigurationParameters
    {
        PastDaysRequests = 1,
        PastDaysAlerts,
        RequestListRecord,
        MailSubject,
        NumberOfDates,
        CapacityDate,
        YearTo,
        IncompleteRequestStatusId = 10,
        AcceptedRequestStatusId,
        RejectedRequestStatusId,
        CanalizeRequestStatusId,
        IndicatorWarningPercentage
    }
}
