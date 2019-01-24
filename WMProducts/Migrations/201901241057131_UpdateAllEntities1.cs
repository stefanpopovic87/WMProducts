namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAllEntities1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Dobavljač", newName: "Dobavljači");
            RenameTable(name: "dbo.Kategorijas", newName: "Kategorije");
            RenameTable(name: "dbo.Proizvods", newName: "Proizvodi");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Proizvodi", newName: "Proizvods");
            RenameTable(name: "dbo.Kategorije", newName: "Kategorijas");
            RenameTable(name: "dbo.Dobavljači", newName: "Dobavljač");
        }
    }
}
