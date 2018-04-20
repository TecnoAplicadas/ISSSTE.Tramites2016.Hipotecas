using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Api
{ /// <summary>
  /// Modelo para alertas
  /// </summary>
    public class Alert
    {
        public string StatusName { get; set; }
        public string Folio { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string KidName { get; set; }
        public Guid Id { get; set; }
        public string RequestId { get; set; }
        public string NoISSSTE { get; set; }
    }
}
