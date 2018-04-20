using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;

namespace ISSSTE.Tramites2016.Hipotecas.Bussiness
{
    public class MortgageBussiness
    {
        //objeto de tipo MortgageDataAccess
        private MortgageDataAccess mortgageDA = new DataAccess.MortgageDataAccess();

        //Llamada al método que obtiene los datos para el reporte
        public MortgageCancelReportData GetMortgageCancelByRequestId(Guid RequestId)
        {
            return mortgageDA.GetMortgageCancelByRequestId(RequestId);
        }
    }
}
