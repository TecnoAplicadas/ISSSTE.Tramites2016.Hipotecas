#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using System.Data.Entity.Migrations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Implementation
{
    /// <summary>
    ///     Implementacion del Dominio en Común
    /// </summary>
    public class CommonDomainService : BaseDomainService, ICommonDomainService
    {
        #region Constructor

        public CommonDomainService(IUnitOfWork context) : base(context)
        {
            //this._context = context;          
        }

        #endregion

        #region Fields

        //private IUnitOfWork _context;

        #endregion

        #region ICommonDomainService


        public async Task<Configuration> GetConfigurationAsync(String name)
        {
            try
            {
                return _context.Configurations.FirstOrDefault(r => r.Name == name);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public async Task<List<Requirement>> GetRequirements(int pensionId)
        {
            try
            {
                return _context.Requirements.Where(r => r.PensionId == pensionId).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Message>> GetMessages()
        {
            try
            {
                return _context.Messages.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Message> GetMessage(string key)
        {
            try
            {
                return _context.Messages.FirstOrDefault(r => r.Key == key);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Configuration> GetConfiguration(String name)
        {
            try
            {
                return _context.Configurations.FirstOrDefault(r => r.Name == name);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Delegation>> GetDelegations()
        {
            try
            {
                return _context.Delegations.Where(r => r.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Delegation> GetDelegationById(int delegationId)
        {
            try
            {
                return _context.Delegations.FirstOrDefault(r => r.DelegationId == delegationId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Role>> GetRoles()
        {
            try
            {
                return _context.Roles.ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<Role> GetRole(int roleId)
        {
            try
            {
                return _context.Roles.FirstOrDefault(r => r.RoleId == roleId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Delegation>> GetDelegationFiltered(int[] delegations)
        {
            try
            {
                var result = _context.Delegations.AsQueryable().Where(x => x.IsActive == true);

                if (!delegations.Contains(-1))
                    result = result.Where(delegation => delegations.Contains(delegation.DelegationId) && delegation.IsActive == true);

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedMessagesResult> GetMessagesAsync(int pageSize, int page, string query = null)
        {
            try
            {
                PagedMessagesResult result = null;
                var queryres = _context.Messages.OrderBy(x => x.Key);


                var messagesCount = queryres.Count();
                var totalPages = (int)Math.Ceiling((double)messagesCount / pageSize);
                var currentPage = page < totalPages ? page : totalPages > 0 ? totalPages : 1;

                var reqcycles = queryres
                    .OrderBy(r => r.Key)
                    .Skip(pageSize * (currentPage - 1))
                    .Take(pageSize)
                    .ToList();

                result = new PagedMessagesResult
                {
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Messages = reqcycles
                };

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<IEnumerable<Message>> GetMessageById(int messageId)
        {
            var message = _context.Messages.Where(cs => cs.MessageId.Equals(messageId))
                                                        .ToList();


            return message;
        }

        public async Task<int> SaveDescription(List<Message> message)
        {
            try
            {
                _context.Messages.AddOrUpdate(message[0]);

                return await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        #endregion
    }
}