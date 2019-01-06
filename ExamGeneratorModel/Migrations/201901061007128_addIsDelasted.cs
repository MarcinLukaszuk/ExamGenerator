namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsDelasted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCores", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StudentGroups", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentGroups", "IsDeleted");
            DropColumn("dbo.Students", "IsDeleted");
            DropColumn("dbo.ExamCores", "IsDeleted");
        }
    }
}
