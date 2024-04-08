namespace EF6_QueryTaker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Query_Category_Comment_user_from : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Queries", "status_id", "dbo.QueryStatus");
            DropIndex("dbo.Queries", new[] { "status_id" });
            RenameColumn(table: "dbo.Queries", name: "user_id", newName: "customer_id");
            RenameIndex(table: "dbo.Queries", name: "IX_user_id", newName: "IX_customer_id");
            CreateTable(
                "dbo.QueryCategories",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Comments", "user_from_id", c => c.String(maxLength: 128));
            AddColumn("dbo.Queries", "category_id", c => c.Long());
            AlterColumn("dbo.Queries", "status_id", c => c.Long());
            CreateIndex("dbo.Comments", "user_from_id");
            CreateIndex("dbo.Queries", "status_id");
            CreateIndex("dbo.Queries", "category_id");
            AddForeignKey("dbo.Queries", "category_id", "dbo.QueryCategories", "id");
            AddForeignKey("dbo.Comments", "user_from_id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Queries", "status_id", "dbo.QueryStatus", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queries", "status_id", "dbo.QueryStatus");
            DropForeignKey("dbo.Comments", "user_from_id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Queries", "category_id", "dbo.QueryCategories");
            DropIndex("dbo.Queries", new[] { "category_id" });
            DropIndex("dbo.Queries", new[] { "status_id" });
            DropIndex("dbo.Comments", new[] { "user_from_id" });
            AlterColumn("dbo.Queries", "status_id", c => c.Long(nullable: false));
            DropColumn("dbo.Queries", "category_id");
            DropColumn("dbo.Comments", "user_from_id");
            DropTable("dbo.QueryCategories");
            RenameIndex(table: "dbo.Queries", name: "IX_customer_id", newName: "IX_user_id");
            RenameColumn(table: "dbo.Queries", name: "customer_id", newName: "user_id");
            CreateIndex("dbo.Queries", "status_id");
            AddForeignKey("dbo.Queries", "status_id", "dbo.QueryStatus", "id", cascadeDelete: true);
        }
    }
}
