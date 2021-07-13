namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Confirmed", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Confirmed");
        }
    }
}
