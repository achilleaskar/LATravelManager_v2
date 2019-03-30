namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _2522 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Airline_Id", c => c.Int());
            CreateIndex("dbo.Services", "Airline_Id");
            AddForeignKey("dbo.Services", "Airline_Id", "dbo.Airlines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "Airline_Id", "dbo.Airlines");
            DropIndex("dbo.Services", new[] { "Airline_Id" });
            DropColumn("dbo.Services", "Airline_Id");
        }
    }
}
