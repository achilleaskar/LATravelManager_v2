namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Excursion_Id", c => c.Int());
            AddColumn("dbo.Transactions", "PersonalBooking_Id", c => c.Int());
            AddColumn("dbo.Transactions", "ThirdPartyBooking_Id", c => c.Int());
            CreateIndex("dbo.Transactions", "Excursion_Id");
            CreateIndex("dbo.Transactions", "PersonalBooking_Id");
            CreateIndex("dbo.Transactions", "ThirdPartyBooking_Id");
            AddForeignKey("dbo.Transactions", "Excursion_Id", "dbo.Excursions", "Id");
            AddForeignKey("dbo.Transactions", "PersonalBooking_Id", "dbo.Personal_Booking", "Id");
            AddForeignKey("dbo.Transactions", "ThirdPartyBooking_Id", "dbo.ThirdParty_Booking", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "ThirdPartyBooking_Id", "dbo.ThirdParty_Booking");
            DropForeignKey("dbo.Transactions", "PersonalBooking_Id", "dbo.Personal_Booking");
            DropForeignKey("dbo.Transactions", "Excursion_Id", "dbo.Excursions");
            DropIndex("dbo.Transactions", new[] { "ThirdPartyBooking_Id" });
            DropIndex("dbo.Transactions", new[] { "PersonalBooking_Id" });
            DropIndex("dbo.Transactions", new[] { "Excursion_Id" });
            DropColumn("dbo.Transactions", "ThirdPartyBooking_Id");
            DropColumn("dbo.Transactions", "PersonalBooking_Id");
            DropColumn("dbo.Transactions", "Excursion_Id");
        }
    }
}