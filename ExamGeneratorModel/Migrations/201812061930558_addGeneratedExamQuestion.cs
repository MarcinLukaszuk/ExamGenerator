namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGeneratedExamQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneratedExamQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        GeneratedExamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GeneratedExams", t => t.GeneratedExamID, cascadeDelete: false)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: false)
                .Index(t => t.QuestionID)
                .Index(t => t.GeneratedExamID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeneratedExamQuestions", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.GeneratedExamQuestions", "GeneratedExamID", "dbo.GeneratedExams");
            DropIndex("dbo.GeneratedExamQuestions", new[] { "GeneratedExamID" });
            DropIndex("dbo.GeneratedExamQuestions", new[] { "QuestionID" });
            DropTable("dbo.GeneratedExamQuestions");
        }
    }
}
