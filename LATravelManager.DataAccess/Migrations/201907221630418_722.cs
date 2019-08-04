namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _722 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChangeInBookings", "Description", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChangeInBookings", "Description", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
