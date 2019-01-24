namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Dobavljači", newName: "DobavljačiDb");
            RenameTable(name: "dbo.Kategorije", newName: "KategorijeDb");
            RenameTable(name: "dbo.Proizvođač", newName: "ProizvođačDb");
            RenameTable(name: "dbo.Proizvodi", newName: "ProizvodiDb");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProizvodiDb", newName: "Proizvodi");
            RenameTable(name: "dbo.ProizvođačDb", newName: "Proizvođač");
            RenameTable(name: "dbo.KategorijeDb", newName: "Kategorije");
            RenameTable(name: "dbo.DobavljačiDb", newName: "Dobavljači");
        }
    }
}
