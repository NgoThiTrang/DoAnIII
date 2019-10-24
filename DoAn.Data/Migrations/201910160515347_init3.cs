namespace DoAn.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WarningProfile", "PropertiesName", c => c.String(maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WarningProfile", "PropertiesName", c => c.String(maxLength: 10, unicode: false));
        }
    }
}
