namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stores", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Stores", "Manufacturer_Id", "dbo.Manufacturers");
            DropForeignKey("dbo.Stores", "Supplier_Id", "dbo.Suppliers");
            DropIndex("dbo.Stores", new[] { "Category_Id" });
            DropIndex("dbo.Stores", new[] { "Manufacturer_Id" });
            DropIndex("dbo.Stores", new[] { "Supplier_Id" });
            DropTable("dbo.Stores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category_Id = c.Int(),
                        Manufacturer_Id = c.Int(),
                        Supplier_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Stores", "Supplier_Id");
            CreateIndex("dbo.Stores", "Manufacturer_Id");
            CreateIndex("dbo.Stores", "Category_Id");
            AddForeignKey("dbo.Stores", "Supplier_Id", "dbo.Suppliers", "Id");
            AddForeignKey("dbo.Stores", "Manufacturer_Id", "dbo.Manufacturers", "Id");
            AddForeignKey("dbo.Stores", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
