using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Velacion
{
    public class Header
    {
        public DateTime Date { get; set; }

        public DateTime Expired { get; set; }

        public string Folio { get; set; }

        public int MortuaryId { get; set; }

        public string MortuaryName { get; set; }

        public string MortuaryAddress { get; set; }

        public string MortuaryZipCode { get; set; }
    }
}
