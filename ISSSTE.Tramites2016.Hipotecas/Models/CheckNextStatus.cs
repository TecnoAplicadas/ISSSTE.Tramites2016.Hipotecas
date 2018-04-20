using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSSTE.Tramites2016.Hipotecas.Models
{
    public class CheckNextStatus
    {
        public Guid requestId;
    }
    public class UpdateRequestStatus
    {
        public int newStatus;
        public Guid requestId;
    }
}