#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian
{
    public interface ICommonDomainService
    {
        /// <summary>
        /// Obtiene la infromación de la tabla de configuraciones
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Configuration> GetConfigurationAsync(String name);
        /// <summary>
        ///     Obtiene los requerimientos por pension
        /// </summary>
        /// <param name="pensionId"></param>
        /// <returns></returns>
        Task<List<Requirement>> GetRequirements(int pensionId);

        /// <summary>
        ///     Obtiene los mensajes
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetMessages();

        /// <summary>
        ///     Obtiene el mensaje por llave
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Message> GetMessage(string key);

        /// <summary>
        ///     Obtiene las configuraciones por nombre
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Configuration> GetConfiguration(String name);

        /// <summary>
        ///     Obtiene las delegaciones
        /// </summary>
        /// <returns></returns>
        Task<List<Delegation>> GetDelegations();

        /// <summary>
        ///     Obtiene las delegaciones por Id
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        Task<Delegation> GetDelegationById(int delegationId);

        /// <summary>
        ///     Obtiene los roles
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetRoles();

        /// <summary>
        ///     Obtienene le role por Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<Role> GetRole(int roleId);

        /// <summary>
        ///     Obtiene las delegaciones filtradas
        /// </summary>
        /// <param name="delegations"></param>
        /// <returns></returns>
        Task<List<Delegation>> GetDelegationFiltered(int[] delegations);

        Task<PagedMessagesResult> GetMessagesAsync(int pageSize, int page, string query = null);

        Task<IEnumerable<Message>> GetMessageById(int messsageId);


        Task<int> SaveDescription(List<Message> message);


    }
}