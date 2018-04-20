#region

using System.ComponentModel;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Api
{
    /// <summary>
    ///     Enum para identificar los status
    /// </summary>
    public enum StatusEnum
    {

        [Description("En espera de agendar cita")]
        EnesperadeAgendarCiraDer = 200,
        [Description("En espera de revision de documentación")]
        EnesperaderevisiondedocumentacionDer =201,
        [Description("Cita agendada")]
        CitaAgendadaDer = 202,
        [Description("Cita cancelada")]
         CitacanceladaDer= 203 ,
        [Description("Documentación aceptada")]
        DocumentacionAceptadaDer = 204 ,
        [Description("En espera de carga de documentación")]
        EnesperadecargadedocumentacionDer = 205 ,
        [Description(" En espera de agendar cita para recoger documentación")]
        EnesperadeagendarcitapararecogerdocumentacionDer =206 ,
        [Description("Cita agendada para recoger documentación")]
        CitaagendadapararecogerdocumentacionDer = 207 ,
        [Description("Cita cancelada para recoger documentación")]
        CitacanceladapararecogerdocumentacionDer = 208 ,
        [Description("Cita cerrada para recoger documentación")]
        CitacerradapararecogerdocumentacionDer = 209 ,
        [Description("Cita atendida")]
        CitaatendidaDer = 210,
        [Description("En espera de aprobación de documentación")]
        EnesperadeaprobacióndedocumentacionOp = 300 ,
        [Description("Documentación aprobada")] 
        DocumentacionaprobadaOP = 301,
        [Description("Documentación rechazada")]
        DocumentacionrechazadaOP =302,
        [Description("En espera de agendar cita ")]
        EnesperadeagendarcitaOP = 303,
        [Description("Cita Agendada")]
        CitaagendadaOP = 304,
        [Description("Cita atendida (cotejo de documentos)")]
        CitaatendidacotejodedocumentosOP =305,
        [Description("Instrumento administrativo de cancelación ")]
        Instrumentoadministrativodecancelación =306,
        [Description("Ingreso al RPP el isntrumento")]
        IngresoalRPPelisntrumento = 307,
        [Description("Cancelación lista")]
        Cancelacionlista = 308,
        [Description("En espera de agendar cita para recoger documentación")]
        Enesperadeagendarcitapararecogerdocumentación = 309,
        [Description("Cita agendada para recoger documentación")]
        CitaagendadapararecogerdocumentacionOP = 310,
        [Description("Cita cerrada")]
        CitacerradaOP = 311,
             [Description("Cita atendida")]
        CitaAtendidaOP = 312,
        [Description("Cita no asistida")]
        CitaNoAsistidaOP = 313,



    }
}