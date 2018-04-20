using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSSTE.Tramites2016.Hipotecas.Models
{
    /// <summary>
    /// Represents the information used to create a catalog view
    /// </summary>
    public class CatalogViewModel
    {
        /// <summary>
        /// Gets or sets the catalog system name
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the catalog display name
        /// </summary>
        public string CatalogDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the catalog key property
        /// </summary>
        public string KeyProperty { get; set; }

        /// <summary>
        /// Gets or sets the properties of the catalog
        /// </summary>
        public List<CatalogPropertyInfo> Properties { get; set; }
    }
}