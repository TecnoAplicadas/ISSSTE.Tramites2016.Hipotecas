namespace ISSSTE.Tramites2016.Common.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "admin.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "admin.IsssteProcedurePermissions",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ProcedureId = c.String(nullable: false, maxLength: 128),
                        CanRead = c.Boolean(nullable: false),
                        CanCreate = c.Boolean(nullable: false),
                        CanEdit = c.Boolean(nullable: false),
                        CanCancel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ProcedureId })
                .ForeignKey("admin.IsssteProcedures", t => t.ProcedureId, cascadeDelete: true)
                .ForeignKey("admin.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ProcedureId);
            
            CreateTable(
                "admin.IsssteProcedures",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(nullable: false),
                        DirectionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("admin.IsssteDirections", t => t.DirectionId)
                .Index(t => t.DirectionId);
            
            CreateTable(
                "admin.IsssteDirections",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "admin.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("admin.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("admin.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "admin.McsClients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "admin.McsRefreshToken",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 128),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("admin.McsClients", t => t.ClientId, cascadeDelete: true)
                .Index(t => new { t.Subject, t.ClientId }, unique: true, name: "NCIX_McsRefreshToken_SubjectId_ClienId");
            
            CreateTable(
                "admin.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        DelegationId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "admin.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("admin.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "admin.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("admin.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("admin.AspNetUserRoles", "UserId", "admin.AspNetUsers");
            DropForeignKey("admin.AspNetUserLogins", "UserId", "admin.AspNetUsers");
            DropForeignKey("admin.AspNetUserClaims", "UserId", "admin.AspNetUsers");
            DropForeignKey("admin.AspNetUserRoles", "RoleId", "admin.AspNetRoles");
            DropForeignKey("admin.McsRefreshToken", "ClientId", "admin.McsClients");
            DropForeignKey("admin.IsssteProcedurePermissions", "RoleId", "admin.AspNetRoles");
            DropForeignKey("admin.IsssteProcedurePermissions", "ProcedureId", "admin.IsssteProcedures");
            DropForeignKey("admin.IsssteProcedures", "DirectionId", "admin.IsssteDirections");
            DropIndex("admin.AspNetUserLogins", new[] { "UserId" });
            DropIndex("admin.AspNetUserClaims", new[] { "UserId" });
            DropIndex("admin.AspNetUsers", "UserNameIndex");
            DropIndex("admin.McsRefreshToken", "NCIX_McsRefreshToken_SubjectId_ClienId");
            DropIndex("admin.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("admin.AspNetUserRoles", new[] { "UserId" });
            DropIndex("admin.IsssteProcedures", new[] { "DirectionId" });
            DropIndex("admin.IsssteProcedurePermissions", new[] { "ProcedureId" });
            DropIndex("admin.IsssteProcedurePermissions", new[] { "RoleId" });
            DropIndex("admin.AspNetRoles", "RoleNameIndex");
            DropTable("admin.AspNetUserLogins");
            DropTable("admin.AspNetUserClaims");
            DropTable("admin.AspNetUsers");
            DropTable("admin.McsRefreshToken");
            DropTable("admin.McsClients");
            DropTable("admin.AspNetUserRoles");
            DropTable("admin.IsssteDirections");
            DropTable("admin.IsssteProcedures");
            DropTable("admin.IsssteProcedurePermissions");
            DropTable("admin.AspNetRoles");
        }
    }
}
