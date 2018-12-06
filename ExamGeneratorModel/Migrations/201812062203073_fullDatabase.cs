namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fullDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GeneratedExams", "StudentID", "dbo.Students");
            DropIndex("dbo.GeneratedExams", new[] { "StudentID" });
            AddColumn("dbo.GeneratedExams", "StudentGroupStudentID", c => c.Int(nullable: false));
            CreateIndex("dbo.GeneratedExams", "StudentGroupStudentID");
            AddForeignKey("dbo.GeneratedExams", "StudentGroupStudentID", "dbo.StudentGroupStudents", "Id", cascadeDelete: true);
            DropColumn("dbo.GeneratedExams", "StudentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneratedExams", "StudentID", c => c.Int(nullable: false));
            DropForeignKey("dbo.GeneratedExams", "StudentGroupStudentID", "dbo.StudentGroupStudents");
            DropIndex("dbo.GeneratedExams", new[] { "StudentGroupStudentID" });
            DropColumn("dbo.GeneratedExams", "StudentGroupStudentID");
            CreateIndex("dbo.GeneratedExams", "StudentID");
            AddForeignKey("dbo.GeneratedExams", "StudentID", "dbo.Students", "Id", cascadeDelete: true);
        }
    }
}
