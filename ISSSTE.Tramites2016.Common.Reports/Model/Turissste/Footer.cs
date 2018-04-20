using System.Collections.Generic;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Pago registrado
  /// </summary>
  public class Payment
  {
    /// <summary>
    /// Descripción del pago
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Monto del pago
    /// </summary>
    public decimal Amount { get; set; }
  }

  /// <summary>
  /// Encabezado de la solicitud
  /// </summary>
  public class Footer
  {
    /// <summary>
    /// Prestador de servicios
    /// </summary>
    public string ServiceEnabler { get; set; }

    /// <summary>
    /// Vendedor
    /// </summary>
    public string Salesman { get; set; }

    /// <summary>
    /// Folio del cupón
    /// </summary>
    public string Coupon { get; set; }

    /// <summary>
    /// Folio del documento de servicio
    /// </summary>
    public string ServiceDocument { get; set; }

    /// <summary>
    /// Folio de la factura
    /// </summary>
    public string Invoice { get; set; }

    /// <summary>
    /// Clave Turissste
    /// </summary>
    public string TurisssteKey { get; set; }

    /// <summary>
    /// Pagos de la solicitud
    /// </summary>
    public IEnumerable<Payment> Payments { get; set; }
  }
}