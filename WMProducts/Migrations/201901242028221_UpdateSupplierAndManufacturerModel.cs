namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSupplierAndManufacturerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DobavljačiDb", "Pib", c => c.String(nullable: false, maxLength: 8));
            AddColumn("dbo.DobavljačiDb", "Adresa", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.ProizvođačDb", "Pib", c => c.String(nullable: false, maxLength: 8));
            AddColumn("dbo.ProizvođačDb", "Adresa", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProizvođačDb", "Adresa");
            DropColumn("dbo.ProizvođačDb", "Pib");
            DropColumn("dbo.DobavljačiDb", "Adresa");
            DropColumn("dbo.DobavljačiDb", "Pib");
        }
    }
}
