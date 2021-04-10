namespace BOOKSTORE.INFRASTRUCTURE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAt_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "UpdateAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "UpdateAt", c => c.DateTime(nullable: false));
        }
    }
}
