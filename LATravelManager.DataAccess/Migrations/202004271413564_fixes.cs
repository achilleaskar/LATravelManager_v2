namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixes : DbMigration
    {
        public override void Up()
        {
          
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StartingPlaces", "Name", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "StartingPlace", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Customers", "ReturningPlace", c => c.String(maxLength: 30, unicode: false));
            DropColumn("dbo.Excursions", "SecondDepartMinDiff");
        }
    }
}
