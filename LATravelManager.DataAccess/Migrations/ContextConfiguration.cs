#region Assembly MySql.Data.Entity.EF6, Version=6.10.8.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d

// C:\Users\achil\source\repos\LATravelManager_v2\packages\MySql.Data.Entity.6.10.8\lib\net452\MySql.Data.Entity.EF6.dll

#endregion Assembly MySql.Data.Entity.EF6, Version=6.10.8.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d

using MySql.Data.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace LATravelManager.DataAccess.Migrations
{
    public class ContextConfiguration : MySqlEFConfiguration
    {
        public ContextConfiguration() : base()
        {
            var path = Path.GetDirectoryName(GetType().Assembly.Location);
            SetModelStore(new DefaultDbModelStore(path));
        }
    }
}