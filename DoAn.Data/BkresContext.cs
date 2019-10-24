using DoAn.Data.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DoAn.Data
{
    public class BkresContext : IdentityDbContext<ApplicationUser>
    {
        public BkresContext() : base("name=BkresContext")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<WarningNew> WarningNews { get; set; }
        public DbSet<WarningProfile> WarningProfiles { get; set; }
        public DbSet<DataPackage> DataPackages { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<SendSms> GuiBanTinSMSes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
        public static BkresContext Create()
        {
            return new BkresContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
          //  builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}