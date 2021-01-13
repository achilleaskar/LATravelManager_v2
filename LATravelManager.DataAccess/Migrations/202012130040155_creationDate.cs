namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creationDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Companies", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
