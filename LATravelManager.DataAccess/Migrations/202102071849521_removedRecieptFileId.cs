namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedRecieptFileId : DbMigration
    {
        public override void Up()
        {
           DropColumn("dbo.Reciepts", "RecieptFileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reciepts", "RecieptFileId", c => c.Int());
        }
    }
}
