namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddProductModel : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ManufacturerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SupplierId");
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.Products", "ManufacturerId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "ManufacturerId", "dbo.Manufacturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
        }
    }
}
