namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSupplierAndManufacturerModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DobavljačiDb", "Pib", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.ProizvođačDb", "Pib", c => c.String(nullable: false, maxLength: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProizvođačDb", "Pib", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.DobavljačiDb", "Pib", c => c.String(nullable: false, maxLength: 8));
        }
    }
}
