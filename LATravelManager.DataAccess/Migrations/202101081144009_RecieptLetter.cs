namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecieptLetter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reciepts", "Printed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reciepts", "Printed", c => c.Boolean(nullable: false, storeType: "bit"));
        }
    }
}
