#region

using System.Data.Entity;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}