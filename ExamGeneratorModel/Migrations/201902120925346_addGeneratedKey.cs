namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGeneratedKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedExams", "ExamCoreStudentGroupID", c => c.Int(nullable: false));
            CreateIndex("dbo.GeneratedExams", "ExamCoreStudentGroupID");
            AddForeignKey("dbo.GeneratedExams", "ExamCoreStudentGroupID", "dbo.ExamCoreStudentGroups", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeneratedExams", "ExamCoreStudentGroupID", "dbo.ExamCoreStudentGroups");
            DropIndex("dbo.GeneratedExams", new[] { "ExamCoreStudentGroupID" });
            DropColumn("dbo.GeneratedExams", "ExamCoreStudentGroupID");
        }
    }
}
