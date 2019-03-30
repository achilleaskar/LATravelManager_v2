namespace LATravelManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1932 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Emails", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Emails");
        }
    }
}
