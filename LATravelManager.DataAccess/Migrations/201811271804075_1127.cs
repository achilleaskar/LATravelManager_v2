namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1127 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "RoomType_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rooms", "RoomType_Id");
            AddForeignKey("dbo.Rooms", "RoomType_Id", "dbo.RoomTypes", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "RoomType_Id", "dbo.RoomTypes");
            DropIndex("dbo.Rooms", new[] { "RoomType_Id" });
            AlterColumn("dbo.Rooms", "RoomType_Id", c => c.Int());
            CreateIndex("dbo.Rooms", "RoomType_Id");
            AddForeignKey("dbo.Rooms", "RoomType_Id", "dbo.RoomTypes", "Id");
        }
    }
}