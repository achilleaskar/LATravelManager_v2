namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "ExcursionExpenseCategory", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Transactions", "ExcursionExpenseCategory");
        }
    }
}