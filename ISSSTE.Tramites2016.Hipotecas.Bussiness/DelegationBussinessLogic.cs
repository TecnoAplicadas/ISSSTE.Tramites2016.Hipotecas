using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.Domain
{
    public class DelegationBussinessLogic
    {
        private DelegationDataAccess delegationDataAccess = new DelegationDataAccess();

        public List<Delegation> GetDelegations()
        {
            return delegationDataAccess.GetDelegations();
        }
    }
}
