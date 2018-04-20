using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.Bussiness
{
    public class UpdateStatusBussiness
    {
        private UpdateStatusDataAcces updateNew = new UpdateStatusDataAcces();
        public List<UpdateStatus> updateStatusbyId(Guid requestId)
        {
            return updateNew.updateStatusbyId(requestId);
            //..
        }

    }
}
