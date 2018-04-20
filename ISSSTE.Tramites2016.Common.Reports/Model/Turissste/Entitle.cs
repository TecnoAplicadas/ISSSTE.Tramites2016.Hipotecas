namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Datos del derechohabiente
  /// </summary>
  public class Entitle
  {
    /// <summary>
    /// Número del ISSSTE
    /// </summary>
    public string NoISSSTE { get; set; }

    /// <summary>
    /// Número del Folio
    /// </summary>
    public string Folio { get; set; }

    /// <summary>
    /// Nombre de la agencia 
    /// </summary>
    public string Agency { get; set; }

    /// <summary>
    /// Nombre completo
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Calle
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Número exterior
    /// </summary>
    public string NumExt { get; set; }

    /// <summary>
    /// Número interior
    /// </summary>
    public string NumInt { get; set; }

    /// <summary>
    /// Colonia
    /// </summary>
    public string Colony { get; set; }
        
    /// <summary>
    /// Código postal
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// Correo electrónico
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Teléfono de casa
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Registro Federal de Causantes
    /// </summary>
    public string RFC { get; set; }

    /// <summary>
    /// Clave única del registro de población
    /// </summary>
    public string CURP { get; set; }

    /// <summary>
    /// Dependencia donde labora
    /// </summary>
    public string Denpendency { get; set; }
  }
}