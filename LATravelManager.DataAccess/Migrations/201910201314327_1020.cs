namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1020 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "ReturningPlace", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Vehicles", "Driver", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("dbo.Vehicles", "Name", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("dbo.Vehicles", "Plate", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.Vehicles", "DriverTel", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "DriverTel", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Vehicles", "Plate", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.Vehicles", "Name", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Vehicles", "Driver", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Customers", "ReturningPlace", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
