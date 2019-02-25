namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "ExcursionDate_Id", c => c.Int());
            AlterColumn("dbo.Customers", "DOB", c => c.DateTime(precision: 0));
            CreateIndex("dbo.Bookings", "ExcursionDate_Id");
            AddForeignKey("dbo.Bookings", "ExcursionDate_Id", "dbo.ExcursionDates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "ExcursionDate_Id", "dbo.ExcursionDates");
            DropIndex("dbo.Bookings", new[] { "ExcursionDate_Id" });
            AlterColumn("dbo.Customers", "DOB", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.Bookings", "ExcursionDate_Id");
        }
    }
}
