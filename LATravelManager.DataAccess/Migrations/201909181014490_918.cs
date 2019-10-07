namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _918 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CheckIn = c.DateTime(nullable: false, precision: 0),
                        CheckOut = c.DateTime(nullable: false, precision: 0),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Description = c.String(maxLength: 200, unicode: false),
                        ExpenseBaseCategory = c.Int(nullable: false),
                        GroupExpenseCategory = c.Int(nullable: false),
                        IncomeBaseCategory = c.Int(nullable: false),
                        StandardExpenseCategory = c.Int(nullable: false),
                        TaxExpenseCategory = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        ModifiedDate = c.DateTime(precision: 0),
                        Booking_Id = c.Int(),
                        PaymentTo_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id)
                .ForeignKey("dbo.Users", t => t.PaymentTo_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Booking_Id)
                .Index(t => t.PaymentTo_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Transactions", "PaymentTo_Id", "dbo.Users");
            DropForeignKey("dbo.Transactions", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.Transactions", new[] { "User_Id" });
            DropIndex("dbo.Transactions", new[] { "PaymentTo_Id" });
            DropIndex("dbo.Transactions", new[] { "Booking_Id" });
            DropTable("dbo.Transactions");
        }
    }
}
