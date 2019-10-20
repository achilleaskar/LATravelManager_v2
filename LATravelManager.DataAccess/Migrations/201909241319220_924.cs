namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _924 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hotels", "Email", c => c.String(maxLength: 30, unicode: false));
            AddColumn("dbo.Personal_Booking", "Reciept", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "PassportExpiration", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Customers", "PassportPublish", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Services", "StopArrive", c => c.Time(precision: 0));
            AddColumn("dbo.Services", "StopLeave", c => c.Time(precision: 0));
            DropColumn("dbo.Services", "WaitingTime");
        }

        public override void Down()
        {
            AddColumn("dbo.Services", "WaitingTime", c => c.DateTime(precision: 0));
            DropColumn("dbo.Services", "StopLeave");
            DropColumn("dbo.Services", "StopArrive");
            DropColumn("dbo.Customers", "PassportPublish");
            DropColumn("dbo.Customers", "PassportExpiration");
            DropColumn("dbo.Personal_Booking", "Reciept");
            DropColumn("dbo.Hotels", "Email");
        }
    }
}