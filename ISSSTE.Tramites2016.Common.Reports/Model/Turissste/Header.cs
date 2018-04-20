using System;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Tipo de servicio
  /// </summary>
  public enum BodyTypes
  {
    /// <summary>
    /// Paquetes turísticos
    /// </summary>
    Package = 1, 
    /// <summary>
    /// Hospedaje
    /// </summary>
    Lodgment = 2, 
    /// <summary>
    /// Transporte
    /// </summary>
    Transportation = 3
  }

  /// <summary>
  /// Encabezado del reporte
  /// </summary>
  public class Header
  {
    /// <summary>
    /// 
    /// </summary>
    public Guid RequestId;

    /// <summary>
    /// Datos del derechohabiente
    /// </summary>
    public Entitle Entitle { get; set; }

    /// <summary>
    /// Tipo de cuerpo asociado
    /// </summary>
    public BodyTypes BodyType { get; set; }
  }
}
