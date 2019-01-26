namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _11201 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "Hotel_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rooms", "Hotel_Id");
            AddForeignKey("dbo.Rooms", "Hotel_Id", "dbo.Hotels", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "Hotel_Id", "dbo.Hotels");
            DropIndex("dbo.Rooms", new[] { "Hotel_Id" });
            AlterColumn("dbo.Rooms", "Hotel_Id", c => c.Int());
            CreateIndex("dbo.Rooms", "Hotel_Id");
            AddForeignKey("dbo.Rooms", "Hotel_Id", "dbo.Hotels", "Id");
        }
    }
}