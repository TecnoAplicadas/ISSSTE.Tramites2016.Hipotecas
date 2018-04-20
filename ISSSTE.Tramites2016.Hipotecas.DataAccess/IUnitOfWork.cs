#region

using System.Data.Entity;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Common.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    /// <summary>
    ///     Interface para UnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDbContext
    {
        #region DbSets

        Database Database { get; }

        /// <summary>
        ///     DbSet que representa las citas
        /// </summary>
        DbSet<Appoinment> Appoinments { get; set; }


        /// <summary>
        ///     DbSet que representa las configuraciones del sistema
        /// </summary>
        DbSet<Configuration> Configurations { get; set; }

        /// <summary>
        ///     DbSet que representa los deudos
        /// </summary>
        DbSet<Debtor> Debtors { get; set; }

        /// <summary>
        ///     Dbset que representa las delegaciones
        /// </summary>
        DbSet<Delegation> Delegations { get; set; }

        /// <summary>
        ///     Dbset que representa los derechohabientes
        /// </summary>
        DbSet<Entitle> Entitles { get; set; }

        /// <summary>
        ///     Dbset que representa los dias feriados
        /// </summary>
        DbSet<Holyday> Holydays { get; set; }

        /// <summary>
        ///     DbSet que representa el catalogod de mensajes configurables
        /// </summary>
        DbSet<Message> Messages { get; set; }

        /// <summary>
        ///     DbSet que representa los diagnosticos de las solicitudes
        /// </summary>
        DbSet<Opinion> Opinions { get; set; }

        /// <summary>
        ///     DbSet que representa las solicitudes del sietema
        /// </summary>
        DbSet<Request> Requests { get; set; }

        /// <summary>
        ///     DbSet que representa los status de las solicitudes
        /// </summary>
        DbSet<RequestStatu> RequestStatus { get; set; }

        /// <summary>
        ///     DbSet que representa los requerimientos
        /// </summary>
        DbSet<Requirement> Requirements { get; set; }

        /// <summary>
        ///     DbSet que representa los roles
        /// </summary>
        DbSet<Role> Roles { get; set; }

        /// <summary>
        ///     DbSet que representa la configuracion de citas
        /// </summary>
        DbSet<Schedule> Schedules { get; set; }

        /// <summary>
        ///     DbSet que representa los dias especiales
        /// </summary>
        DbSet<SpecialDay> SpecialDays { get; set; }

        DbSet<DocumentTypes> DocumentTypes { get; set; }

        /// <summary>
        ///     DbSet que representa las citass en los dias especiales
        /// </summary>
        DbSet<SpecialDaysSchedule> SpecialDaysSchedules { get; set; }

        /// <summary>
        ///     DbSet que representa el catalago de estatus para las citas
        /// </summary>
        DbSet<Status> Status { get; set; }

        /// <summary>
        ///     DbSet que representa la relacion de estatus y sus sigueintes estatus
        /// </summary>
        DbSet<StatusNextStatu> StatusNextStatus { get; set; }

        /// <summary>
        ///     DbSet que representa la relacion de status con otros roles
        /// </summary>
        DbSet<StatusRelatedStatu> StatusRelatedStatus { get; set; }

        DbSet<sysdiagram> sysdiagrams { get; set; }

        /// <summary>
        ///     DbSet que contiene las validacione sde datos de las solicitudes
        /// </summary>
        //DbSet<Validation> Validations { get; set; }


        DbSet<TimeContribution> TimeContributions { get; set; }

        /// <summary>
        ///     DbSet Catalogo de dias de la semana
        /// </summary>
        DbSet<Weekday> Weekdays { get; set; }


        /// <summary>
        ///     DbSet que contiene los mensajes para los diagnosticos
        /// </summary>
        DbSet<OpinionMessage> OpinionMessages { get; set; }

        /*Db Sets que contiene la administracion de usuarios*/
        DbSet<AspNetRole> AspNetRoles { get; set; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<AspNetUserRole> AspNetUserRoles { get; set; }


        DbSet<RelationshipDocument> RelationshipDocuments { get; set; }
        DbSet<Relationship> Relationships { get; set; }
        DbSet<RelationshipTitle> RelationshipTitles { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<RelationshipsTitlesKey> RelationshipsTitlesKeys { get; set; }

        #endregion
    }
}