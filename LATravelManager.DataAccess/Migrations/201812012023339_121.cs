namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _121 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Hotel_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "Hotel_Id");
            AddForeignKey("dbo.Reservations", "Hotel_Id", "dbo.Hotels", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Hotel_Id", "dbo.Hotels");
            DropIndex("dbo.Reservations", new[] { "Hotel_Id" });
            DropColumn("dbo.Reservations", "Hotel_Id");
        }
    }
}