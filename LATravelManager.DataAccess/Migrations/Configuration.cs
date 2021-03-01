namespace LATravelManager.DataAccess.Migrations
{
    using MySql.Data.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;

    internal sealed class Configuration : DbMigrationsConfiguration<MainDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new SqlGenerator());
        }

        protected override void Seed(MainDatabase context)
        {
            // This method will be called after migrating to the latest version.

            // You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating
            // duplicate seed data.
        }
    }

    class SqlGenerator : MySqlMigrationSqlGenerator
    {
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            IEnumerable<MigrationStatement> res = base.Generate(migrationOperations, providerManifestToken);
            foreach (MigrationStatement ms in res)
            {
                ms.Sql = ms.Sql.Replace("dbo.", "");
            }
            return res;
        }
    }
}