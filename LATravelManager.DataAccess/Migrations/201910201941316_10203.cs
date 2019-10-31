namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10203 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buses", "Comment", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Buses", "Comment");
        }
    }
}
