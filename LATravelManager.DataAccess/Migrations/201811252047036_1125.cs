namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1125 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Tel", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddColumn("dbo.Partners", "Note", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Partners", "Email", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Partners", "Email");
            DropColumn("dbo.Partners", "Note");
            DropColumn("dbo.Partners", "Tel");
        }
    }
}