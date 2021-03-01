namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoginDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EncryptedPassword = c.String(maxLength: 200, unicode: false),
                        Link = c.String(maxLength: 200, unicode: false),
                        ModifiedDate = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 30, unicode: false),
                        Username = c.String(maxLength: 30, unicode: false),
                        LastEditedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.LastEditedBy_Id)
                .Index(t => t.LastEditedBy_Id);
            
            AddColumn("dbo.ChangeInBookings", "ChangeType", c => c.Int(nullable: false));
            AddColumn("dbo.ChangeInBookings", "LoginData_Id", c => c.Int());
            CreateIndex("dbo.ChangeInBookings", "LoginData_Id");
            AddForeignKey("dbo.ChangeInBookings", "LoginData_Id", "dbo.LoginDatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginDatas", "LastEditedBy_Id", "dbo.Users");
            DropForeignKey("dbo.ChangeInBookings", "LoginData_Id", "dbo.LoginDatas");
            DropIndex("dbo.LoginDatas", new[] { "LastEditedBy_Id" });
            DropIndex("dbo.ChangeInBookings", new[] { "LoginData_Id" });
            DropColumn("dbo.ChangeInBookings", "LoginData_Id");
            DropColumn("dbo.ChangeInBookings", "ChangeType");
            DropTable("dbo.LoginDatas");
        }
    }
}
