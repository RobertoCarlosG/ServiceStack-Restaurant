using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace TechnicalTest.Migrations;

public class Migration1001 : MigrationBase
{
    public class Restaurant
    {
        [AutoIncrement]
        public int Id { get; set; }
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
