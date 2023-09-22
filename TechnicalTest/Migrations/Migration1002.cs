using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System.Diagnostics.CodeAnalysis;

namespace TechnicalTest.Migrations;

public class Migration1002 : MigrationBase
{
    public class Restaurant
    {
        [PrimaryKey]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Type { get; set; }
    }

    public override void Up()
    {
        Db.CreateTable<Restaurant>();
    }

    public override void Down()
    {
        Db.DropTable<Restaurant>();
    }
}
