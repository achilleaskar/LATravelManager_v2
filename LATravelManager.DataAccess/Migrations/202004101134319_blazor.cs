namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blazor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisabledInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisableDate = c.DateTime(precision: 0),
                        CancelReason = c.String(maxLength: 200, unicode: false),
                        DisabledBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DisabledBy_Id)
                .Index(t => t.DisabledBy_Id);
            
            CreateTable(
                "dbo.PartnerInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Commision = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartnerEmail = c.String(maxLength: 60, unicode: false),
                        ProformaSent = c.Boolean(nullable: false, storeType: "bit"),
                        Partner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .Index(t => t.Partner_Id);
            
            AddColumn("dbo.Bookings", "DisabledInfo_Id", c => c.Int());
            AddColumn("dbo.Bookings", "PartnerInfo_Id", c => c.Int());
            AddColumn("dbo.Personal_Booking", "DisabledInfo_Id", c => c.Int());
            AddColumn("dbo.Personal_Booking", "PartnerInfo_Id", c => c.Int());
            AddColumn("dbo.ThirdParty_Booking", "DisabledInfo_Id", c => c.Int());
            AddColumn("dbo.ThirdParty_Booking", "PartnerInfo_Id", c => c.Int());
            AlterColumn("dbo.Rooms", "Note", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Buses", "Comment", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.CustomerOptionals", "Note", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Bookings", "Comment", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("dbo.Partners", "Emails", c => c.String(maxLength: 400, unicode: false));
            AlterColumn("dbo.Services", "CompanyInfo", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Services", "From", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Services", "To", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.StartingPlaces", "Details", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.Bookings", "DisabledInfo_Id");
            CreateIndex("dbo.Bookings", "PartnerInfo_Id");
            CreateIndex("dbo.Personal_Booking", "DisabledInfo_Id");
            CreateIndex("dbo.Personal_Booking", "PartnerInfo_Id");
            CreateIndex("dbo.ThirdParty_Booking", "DisabledInfo_Id");
            CreateIndex("dbo.ThirdParty_Booking", "PartnerInfo_Id");
            AddForeignKey("dbo.Bookings", "DisabledInfo_Id", "dbo.DisabledInfoes", "Id");
            AddForeignKey("dbo.Bookings", "PartnerInfo_Id", "dbo.PartnerInfoes", "Id");
            AddForeignKey("dbo.Personal_Booking", "DisabledInfo_Id", "dbo.DisabledInfoes", "Id");
            AddForeignKey("dbo.Personal_Booking", "PartnerInfo_Id", "dbo.PartnerInfoes", "Id");
            AddForeignKey("dbo.ThirdParty_Booking", "DisabledInfo_Id", "dbo.DisabledInfoes", "Id");
            AddForeignKey("dbo.ThirdParty_Booking", "PartnerInfo_Id", "dbo.PartnerInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThirdParty_Booking", "PartnerInfo_Id", "dbo.PartnerInfoes");
            DropForeignKey("dbo.ThirdParty_Booking", "DisabledInfo_Id", "dbo.DisabledInfoes");
            DropForeignKey("dbo.Personal_Booking", "PartnerInfo_Id", "dbo.PartnerInfoes");
            DropForeignKey("dbo.Personal_Booking", "DisabledInfo_Id", "dbo.DisabledInfoes");
            DropForeignKey("dbo.Bookings", "PartnerInfo_Id", "dbo.PartnerInfoes");
            DropForeignKey("dbo.PartnerInfoes", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.Bookings", "DisabledInfo_Id", "dbo.DisabledInfoes");
            DropForeignKey("dbo.DisabledInfoes", "DisabledBy_Id", "dbo.Users");
            DropIndex("dbo.ThirdParty_Booking", new[] { "PartnerInfo_Id" });
            DropIndex("dbo.ThirdParty_Booking", new[] { "DisabledInfo_Id" });
            DropIndex("dbo.Personal_Booking", new[] { "PartnerInfo_Id" });
            DropIndex("dbo.Personal_Booking", new[] { "DisabledInfo_Id" });
            DropIndex("dbo.PartnerInfoes", new[] { "Partner_Id" });
            DropIndex("dbo.DisabledInfoes", new[] { "DisabledBy_Id" });
            DropIndex("dbo.Bookings", new[] { "PartnerInfo_Id" });
            DropIndex("dbo.Bookings", new[] { "DisabledInfo_Id" });
            AlterColumn("dbo.StartingPlaces", "Details", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Services", "To", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Services", "From", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Services", "CompanyInfo", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Partners", "Emails", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Bookings", "Comment", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.CustomerOptionals", "Note", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Buses", "Comment", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Rooms", "Note", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.ThirdParty_Booking", "PartnerInfo_Id");
            DropColumn("dbo.ThirdParty_Booking", "DisabledInfo_Id");
            DropColumn("dbo.Personal_Booking", "PartnerInfo_Id");
            DropColumn("dbo.Personal_Booking", "DisabledInfo_Id");
            DropColumn("dbo.Bookings", "PartnerInfo_Id");
            DropColumn("dbo.Bookings", "DisabledInfo_Id");
            DropTable("dbo.PartnerInfoes");
            DropTable("dbo.DisabledInfoes");
        }
    }
}
