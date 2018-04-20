#region

using System.ComponentModel;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Enums
{
    /// <summary>
    ///     Indica el parametro para obtener los mensajes configurables
    /// </summary>
    public enum EnumConfigMessages
    {
        [Description("Aprobado")] Aprobado = 1,
        [Description("NoAprobado")] NoAprobado = 2,
        [Description("DatosGenerales")] DatosGenerales = 3,
        [Description("HistoriaLaboral")] HistoriaLaboral = 4,
        [Description("Benficiarios")] Benficiarios = 5,
        [Description("Deudos")] Deudos = 6,
        [Description("Curp")] Curp = 7,
        [Description("Rechazada")] Rechazada = 8
    }
}