namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _717 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Checked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Checked");
        }
    }
}
