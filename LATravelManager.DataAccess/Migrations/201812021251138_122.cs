namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _122 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Level", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Users", "Level");
        }
    }
}