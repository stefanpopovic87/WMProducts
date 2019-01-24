namespace WMProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAllEntities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Products", "ManufacturerId", "dbo.Manufacturers");
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "ManufacturerId" });
            CreateTable(
                "dbo.Dobavljač",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kategorijas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proizvođač",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proizvods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 50),
                        Opis = c.String(maxLength: 255),
                        Cena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DobavljačId = c.Int(nullable: false),
                        KategorijaId = c.Int(nullable: false),
                        ProizvođačId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dobavljač", t => t.DobavljačId, cascadeDelete: true)
                .ForeignKey("dbo.Kategorijas", t => t.KategorijaId, cascadeDelete: true)
                .ForeignKey("dbo.Proizvođač", t => t.ProizvođačId, cascadeDelete: true)
                .Index(t => t.DobavljačId)
                .Index(t => t.KategorijaId)
                .Index(t => t.ProizvođačId);
            
            DropTable("dbo.Categories");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Products");
            DropTable("dbo.Suppliers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 255),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SupplierId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Proizvods", "ProizvođačId", "dbo.Proizvođač");
            DropForeignKey("dbo.Proizvods", "KategorijaId", "dbo.Kategorijas");
            DropForeignKey("dbo.Proizvods", "DobavljačId", "dbo.Dobavljač");
            DropIndex("dbo.Proizvods", new[] { "ProizvođačId" });
            DropIndex("dbo.Proizvods", new[] { "KategorijaId" });
            DropIndex("dbo.Proizvods", new[] { "DobavljačId" });
            DropTable("dbo.Proizvods");
            DropTable("dbo.Proizvođač");
            DropTable("dbo.Kategorijas");
            DropTable("dbo.Dobavljač");
            CreateIndex("dbo.Products", "ManufacturerId");
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "ManufacturerId", "dbo.Manufacturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
