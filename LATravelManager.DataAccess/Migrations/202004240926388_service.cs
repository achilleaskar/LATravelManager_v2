namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class service : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExcursionTimes", "Enabled");
            DropColumn("dbo.Hotels", "HotelCategoryEnum");
        }
    }
}
