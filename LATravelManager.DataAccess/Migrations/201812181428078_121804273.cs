namespace LATravelManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _121804273 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hotels", "Address", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hotels", "Address");
        }
    }
}
