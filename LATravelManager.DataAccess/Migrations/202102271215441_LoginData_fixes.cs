namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginData_fixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoginDatas", "Link", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.LoginDatas", "Name", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.LoginDatas", "Username", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoginDatas", "Username", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LoginDatas", "Name", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LoginDatas", "Link", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
