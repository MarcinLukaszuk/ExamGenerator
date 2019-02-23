namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _base : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExamCores", "Owner", c => c.String(nullable: false));
            AlterColumn("dbo.StudentGroups", "Owner", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Owner", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Owner", c => c.String());
            AlterColumn("dbo.StudentGroups", "Owner", c => c.String());
            AlterColumn("dbo.ExamCores", "Owner", c => c.String());
        }
    }
}
