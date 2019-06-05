namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class da4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "To", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Services", "From", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Services", "TimeGo", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Services", "CompanyInfo", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Services", "TimeReturn", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Services", "Duration", c => c.Int());
            AddColumn("dbo.Services", "Option", c => c.DateTime(precision: 0));
            AddColumn("dbo.Services", "City_Id", c => c.Int());
            AddColumn("dbo.Services", "Hotel_Id", c => c.Int());
            AddColumn("dbo.Services", "RoomType_Id", c => c.Int());
            AlterColumn("dbo.Services", "NetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Services", "Allerretour", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Services", "City_Id");
            CreateIndex("dbo.Services", "Hotel_Id");
            CreateIndex("dbo.Services", "RoomType_Id");
            AddForeignKey("dbo.Services", "City_Id", "dbo.Cities", "Id");
            AddForeignKey("dbo.Services", "Hotel_Id", "dbo.Hotels", "Id");
            AddForeignKey("dbo.Services", "RoomType_Id", "dbo.RoomTypes", "Id");
            DropColumn("dbo.Services", "FlightGo");
            DropColumn("dbo.Services", "FlightReturn");
            DropColumn("dbo.Services", "FlyFrom");
            DropColumn("dbo.Services", "FlyTo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "FlyTo", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Services", "FlyFrom", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Services", "FlightReturn", c => c.DateTime(precision: 0));
            AddColumn("dbo.Services", "FlightGo", c => c.DateTime(precision: 0));
            DropForeignKey("dbo.Services", "RoomType_Id", "dbo.RoomTypes");
            DropForeignKey("dbo.Services", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.Services", "City_Id", "dbo.Cities");
            DropIndex("dbo.Services", new[] { "RoomType_Id" });
            DropIndex("dbo.Services", new[] { "Hotel_Id" });
            DropIndex("dbo.Services", new[] { "City_Id" });
            AlterColumn("dbo.Services", "Allerretour", c => c.Boolean());
            AlterColumn("dbo.Services", "NetPrice", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Services", "RoomType_Id");
            DropColumn("dbo.Services", "Hotel_Id");
            DropColumn("dbo.Services", "City_Id");
            DropColumn("dbo.Services", "Option");
            DropColumn("dbo.Services", "Duration");
            DropColumn("dbo.Services", "TimeReturn");
            DropColumn("dbo.Services", "CompanyInfo");
            DropColumn("dbo.Services", "TimeGo");
            DropColumn("dbo.Services", "From");
            DropColumn("dbo.Services", "To");
        }
    }
}
