namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10202 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leaders", "Tel", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leaders", "Tel", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
