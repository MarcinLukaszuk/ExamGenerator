namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "Exam_Id" });
            RenameColumn(table: "dbo.Questions", name: "Exam_Id", newName: "ExamID");
            AlterColumn("dbo.Questions", "ExamID", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "ExamID");
            AddForeignKey("dbo.Questions", "ExamID", "dbo.Exams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "ExamID", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "ExamID" });
            AlterColumn("dbo.Questions", "ExamID", c => c.Int());
            RenameColumn(table: "dbo.Questions", name: "ExamID", newName: "Exam_Id");
            CreateIndex("dbo.Questions", "Exam_Id");
            AddForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams", "Id");
        }
    }
}
