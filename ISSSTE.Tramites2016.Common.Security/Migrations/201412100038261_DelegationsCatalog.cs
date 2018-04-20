namespace ISSSTE.Tramites2016.Common.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelegationsCatalog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "admin.IsssteDelegations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        State = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("admin.IsssteDelegations");
        }
    }
}
