#region

using System.ComponentModel;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Enums
{
    /// <summary>
    ///     Enum personalizado para los dias de la semana
    /// </summary>
    public enum WeekDayEnum
    {
        [Description("Domingo")] Domingo = 1,
        [Description("Lunes")] Lunes = 2,
        [Description("Martes")] Martes = 3,
        [Description("Miercoles")] Miercoles = 4,
        [Description("Jueves")] Jueves = 5,
        [Description("Viernes")] Viernes = 6,
        [Description("Sabado")] Sabado = 7
    }
}