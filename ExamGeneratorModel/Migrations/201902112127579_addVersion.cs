namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCoreStudentGroups", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExamCoreStudentGroups", "Version");
        }
    }
}
