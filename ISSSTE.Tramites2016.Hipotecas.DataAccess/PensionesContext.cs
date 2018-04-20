#region

using System.Data.Entity;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    /// <summary>
    ///     Contexto de la BD de Hipotecas el cual implementa IUnitOfWork
    /// </summary>
    public class HipotecasContext : DbContext, IUnitOfWork
    {
        public HipotecasContext()
            : base("name=DataConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<HipotecasContext>(null);
        }

        public virtual Database Database { get; }
        public virtual DbSet<Appoinment> Appoinments { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<Delegation> Delegations { get; set; }
        public virtual DbSet<Entitle> Entitles { get; set; }
        public virtual DbSet<Holyday> Holydays { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Opinion> Opinions { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestStatu> RequestStatus { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<SpecialDay> SpecialDays { get; set; }
        public virtual DbSet<SpecialDaysSchedule> SpecialDaysSchedules { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StatusNextStatu> StatusNextStatus { get; set; }
        public virtual DbSet<StatusRelatedStatu> StatusRelatedStatus { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        //public virtual DbSet<Validation> Validations { get; set; }
        public virtual DbSet<Weekday> Weekdays { get; set; }
        public virtual DbSet<OpinionMessage> OpinionMessages { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }
        public virtual DbSet<RelationshipDocument> RelationshipDocuments { get; set; }
        public virtual DbSet<RelationshipTitle> RelationshipTitles { get; set; }
        public virtual DbSet<TimeContribution> TimeContributions { get; set; }

        public DbSet<Relationship> Relationships { get; set; }

        public DbSet<RelationshipsTitlesKey> RelationshipsTitlesKeys { get; set; }
        public virtual DbSet<MaximumSalary> MaximumSalaries { get; set; }
        //public virtual DbSet<MaximumSalary> MaximumSalary { get; set; }

        public void SetEntityAsPartialyUpdated<T>(T entity) where T : class
        {
            Extensions.SetEntityAsPartialyUpdated(this, entity);
        }

        public async Task<int> SaveChangesHandlingOptimisticConcurrencyAsync<T>(Extensions.ResolveConcurrency<T> concurrencyResolution)
            where T : class
        {
            return await Extensions.SaveChangesHandlingOptimisticConcurrencyAsync(this, concurrencyResolution);
        }

        public async Task<int> SaveChangesHandlingOptimisticConcurrencyDatabaseWinsAsync<T>()
            where T : class
        {
            return await Extensions.SaveChangesHandlingOptimisticConcurrencyDatabaseWinsAsync<T>(this);
        }

        public async Task<int> SaveChangesHandlingOptimisticConcurrencyClientWinsAsync<T>()
            where T : class
        {
            return await Extensions.SaveChangesHandlingOptimisticConcurrencyClientWinsAsync<T>(this);
        }
    }
}