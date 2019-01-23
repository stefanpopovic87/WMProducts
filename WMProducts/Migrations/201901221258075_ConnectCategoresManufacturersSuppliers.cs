namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectCategoresManufacturersSuppliers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Manufacturers", t => t.ManufacturerId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.CategoryId)
                .Index(t => t.ManufacturerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Stores", "ManufacturerId", "dbo.Manufacturers");
            DropForeignKey("dbo.Stores", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Stores", new[] { "ManufacturerId" });
            DropIndex("dbo.Stores", new[] { "CategoryId" });
            DropIndex("dbo.Stores", new[] { "SupplierId" });
            DropTable("dbo.Stores");
        }
    }
}
