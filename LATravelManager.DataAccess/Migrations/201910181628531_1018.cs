namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1018 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Driver = c.String(maxLength: 25, unicode: false),
                        Name = c.String(maxLength: 25, unicode: false),
                        SeatsPassengers = c.Int(nullable: false),
                        Plate = c.String(maxLength: 10, unicode: false),
                        DoorSeat = c.Int(nullable: false),
                        DriverTel = c.String(maxLength: 15, unicode: false),
                        SeatsFront = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Buses", "OneWay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Buses", "TimeGo", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Buses", "TimeReturn", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Buses", "Leader_Id", c => c.Int());
            AddColumn("dbo.Buses", "Vehicle_Id", c => c.Int());
            CreateIndex("dbo.Buses", "Leader_Id");
            CreateIndex("dbo.Buses", "Vehicle_Id");
            AddForeignKey("dbo.Buses", "Leader_Id", "dbo.Leaders", "Id");
            AddForeignKey("dbo.Buses", "Vehicle_Id", "dbo.Vehicles", "Id");
            DropColumn("dbo.Buses", "Name");
            DropColumn("dbo.Buses", "Tel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Buses", "Tel", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Buses", "Name", c => c.String(maxLength: 20, unicode: false));
            DropForeignKey("dbo.Buses", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Buses", "Leader_Id", "dbo.Leaders");
            DropIndex("dbo.Buses", new[] { "Vehicle_Id" });
            DropIndex("dbo.Buses", new[] { "Leader_Id" });
            DropColumn("dbo.Buses", "Vehicle_Id");
            DropColumn("dbo.Buses", "Leader_Id");
            DropColumn("dbo.Buses", "TimeReturn");
            DropColumn("dbo.Buses", "TimeGo");
            DropColumn("dbo.Buses", "OneWay");
            DropTable("dbo.Vehicles");
        }
    }
}
