#region

using System.ComponentModel;
using ISSSTE.Tramites2016.Common.Util;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Enums
{
    /// <summary>
    ///     Enum para identificar los roles del sistema
    /// </summary>
    public enum RolEnum
    {


        [Description("Derechohabiente")]
        Derechohabiente = 1,
        //Jefe de Operadores
        [Description("Administrador Hipotecas")]
        JefeOperadores = 2,
        [Description("Unidad jurídica")]
        Operador = 3


    }
}