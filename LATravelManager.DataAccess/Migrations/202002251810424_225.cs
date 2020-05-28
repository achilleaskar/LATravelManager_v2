namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _225 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personal_Booking", "Group", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Personal_Booking", "Group");
        }
    }
}
