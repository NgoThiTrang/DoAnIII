namespace DoAn.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreatedDate = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Avatar = c.String(),
                        Address = c.String(),
                        LoginEndIP = c.String(),
                        LoginCount = c.Int(),
                        LastLoginDate = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ApplicationGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(maxLength: 250),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.DataPackage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(nullable: false),
                        TimePackage = c.DateTime(nullable: false),
                        PH = c.Double(nullable: false),
                        Salt = c.Double(nullable: false),
                        Oxy = c.Double(nullable: false),
                        Temp = c.Double(nullable: false),
                        H2S = c.Double(nullable: false),
                        NH3 = c.Double(nullable: false),
                        NH4Min = c.Double(nullable: false),
                        NH4Max = c.Double(nullable: false),
                        NO2Min = c.Double(nullable: false),
                        NO2Max = c.Double(nullable: false),
                        SulfideMin = c.Double(nullable: false),
                        SulfideMax = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Imei = c.String(nullable: false, maxLength: 30),
                        WarningPhoneNumber = c.String(maxLength: 20, unicode: false),
                        WarningMail = c.String(maxLength: 30, unicode: false),
                        CreatedDate = c.DateTime(),
                        DistrictId = c.Int(nullable: false),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.District", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
            CreateTable(
                "dbo.District",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(maxLength: 20, unicode: false),
                        CreatedDate = c.DateTime(),
                        isActive = c.Boolean(),
                        ProvinceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Province", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(maxLength: 20, unicode: false),
                        CreatedDate = c.DateTime(),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExceptionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ControllerName = c.String(),
                        StackTrace = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SendSms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarningNewId = c.Int(nullable: false),
                        TimeReceive = c.DateTime(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        isSent = c.Boolean(nullable: false),
                        Content = c.String(),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.WarningNew", t => t.WarningNewId, cascadeDelete: true)
                .Index(t => t.WarningNewId)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.WarningNew",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeReceive = c.DateTime(nullable: false),
                        TimeSent = c.DateTime(nullable: false),
                        WarningProfileId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        WarningContent = c.String(),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WarningProfile", t => t.WarningProfileId, cascadeDelete: true)
                .Index(t => t.WarningProfileId);
            
            CreateTable(
                "dbo.WarningProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Up_Thres = c.Double(nullable: false),
                        Low_Thres = c.Double(nullable: false),
                        ProcessTimeOut = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        WarningContent = c.String(nullable: false, maxLength: 200),
                        PropertiesName = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.SendSms", "WarningNewId", "dbo.WarningNew");
            DropForeignKey("dbo.WarningNew", "WarningProfileId", "dbo.WarningProfile");
            DropForeignKey("dbo.WarningProfile", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.SendSms", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.DataPackage", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.Device", "DistrictId", "dbo.District");
            DropForeignKey("dbo.District", "ProvinceId", "dbo.Province");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ActivityLog", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.WarningProfile", new[] { "UserId" });
            DropIndex("dbo.WarningNew", new[] { "WarningProfileId" });
            DropIndex("dbo.SendSms", new[] { "DeviceId" });
            DropIndex("dbo.SendSms", new[] { "WarningNewId" });
            DropIndex("dbo.District", new[] { "ProvinceId" });
            DropIndex("dbo.Device", new[] { "DistrictId" });
            DropIndex("dbo.DataPackage", new[] { "DeviceId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ActivityLog", new[] { "UserId" });
            DropTable("dbo.WarningProfile");
            DropTable("dbo.WarningNew");
            DropTable("dbo.SendSms");
            DropTable("dbo.ExceptionLog");
            DropTable("dbo.Province");
            DropTable("dbo.District");
            DropTable("dbo.Device");
            DropTable("dbo.DataPackage");
            DropTable("dbo.ApplicationUserGroups");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationRoleGroups");
            DropTable("dbo.ApplicationGroups");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ActivityLog");
        }
    }
}
