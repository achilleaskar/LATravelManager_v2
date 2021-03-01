namespace LATravelManager.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mystery : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Partners", name: "CompanyInfo_Id", newName: "CompanyInfoId");
            //RenameIndex(table: "dbo.Partners", name: "IX_CompanyInfo_Id", newName: "IX_CompanyInfoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Partners", name: "IX_CompanyInfoId", newName: "IX_CompanyInfo_Id");
            RenameColumn(table: "dbo.Partners", name: "CompanyInfoId", newName: "CompanyInfo_Id");
        }
    }
}
