using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Enums
{
    public enum EnumBeneficiario
    {
        [Description("Derechohabiente")]
        Derechohabiente = 1,
        [Description("Apoderado")]
        Apoderado = 2,
        [Description("NoTitular")]
        NoTitular = 3,
                    [Description("Conyugal")]
        Conyugal = 4
    }
}
