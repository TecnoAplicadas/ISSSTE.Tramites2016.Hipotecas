using System;
using System.Collections.Generic;
using System.Linq;

namespace ISSSTE.Tramites2016.Common.Security.Core
{
    /// <summary>
    /// Representa la información de un usuario
    /// </summary>
    public class IsssteUser
    {
        #region Constants

        /// <summary>
        /// Valor de la delegación que representa que se puede acceder a la información todas las delegaciones
        /// </summary>
        private int AllDelegationsId = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene o asigna Id del usuario
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre de usuario del usuario
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre del usuario
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o asigna el correo electrónico del usuario
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtiene o asigna la lista de propiedades del usuario
        /// </summary>
        public List<IsssteUserProperty> Properties { get; set; }

        /// <summary>
        /// Obtiene o asigna la lista de rolew del usuario
        /// </summary>
        public List<IsssteRole> Roles { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del usuario
        /// </summary>
        public IsssteUser()
        {
            this.Properties = new List<IsssteUserProperty>();
            this.Roles = new List<IsssteRole>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene los posibles valore que un usuario tiene asignado bajo un nombre de una propiedad
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Lista de valores que el usuario tiene asignado</returns>
        public List<IsssteUserProperty> GetPropertyValues(string propertyName)
        {
            List<IsssteUserProperty> properties = new List<IsssteUserProperty>();

            if (this.Properties != null)
            {
                properties = this.Properties.Where(p => p.Type == propertyName).Select(p => p).ToList();
            }

            return properties;
        }

        /// <summary>
        /// Obtiene la lista de ids de delegaciones que el usuario tiene asignadas
        /// </summary>
        /// <returns>Lista de ids de delegaciones</returns>
        public List<int> GetDelegations()
        {
            return this.GetPropertyValues(IsssteUserPropertyTypes.Delegation).Select(p => Convert.ToInt32(p.Value)).ToList();
        }

        /// <summary>
        /// Valida si el usuario tiene permiso para acceder a la información de todas las delegaciones
        /// </summary>
        /// <returns>Resultado de la validación</returns>
        public bool HasAuthorizationToAllDelegations()
        {
            return this.GetDelegations().Any(d => d == AllDelegationsId);
        }

        #endregion
    }
}