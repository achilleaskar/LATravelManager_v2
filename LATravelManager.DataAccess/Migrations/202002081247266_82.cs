namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _82 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifStatus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OkDate = c.DateTime(nullable: false, precision: 0),
                    IsOk = c.Boolean(nullable: false),
                    OkByUser_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OkByUser_Id)
                .Index(t => t.OkByUser_Id);

            AddColumn("dbo.Services", "NotifStatus_Id", c => c.Int());
            AddColumn("dbo.Options", "NotifStatus_Id", c => c.Int());
            CreateIndex("dbo.Services", "NotifStatus_Id");
            CreateIndex("dbo.Options", "NotifStatus_Id");
            AddForeignKey("dbo.Services", "NotifStatus_Id", "dbo.NotifStatus", "Id");
            AddForeignKey("dbo.Options", "NotifStatus_Id", "dbo.NotifStatus", "Id");
            DropColumn("dbo.Services", "NotifOk");
            DropColumn("dbo.Options", "Paid");
        }

        public override void Down()
        {
            AddColumn("dbo.Options", "Paid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "NotifOk", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Options", "NotifStatus_Id", "dbo.NotifStatus");
            DropForeignKey("dbo.Services", "NotifStatus_Id", "dbo.NotifStatus");
            DropForeignKey("dbo.NotifStatus", "OkByUser_Id", "dbo.Users");
            DropIndex("dbo.Options", new[] { "NotifStatus_Id" });
            DropIndex("dbo.NotifStatus", new[] { "OkByUser_Id" });
            DropIndex("dbo.Services", new[] { "NotifStatus_Id" });
            DropColumn("dbo.Options", "NotifStatus_Id");
            DropColumn("dbo.Services", "NotifStatus_Id");
            DropTable("dbo.NotifStatus");
        }
    }
}