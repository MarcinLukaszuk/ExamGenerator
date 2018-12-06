namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGeneratedExamToAP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnswerPositions", "GeneratedExamID", c => c.Int(nullable: false));
            CreateIndex("dbo.AnswerPositions", "GeneratedExamID");
            AddForeignKey("dbo.AnswerPositions", "GeneratedExamID", "dbo.GeneratedExams", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnswerPositions", "GeneratedExamID", "dbo.GeneratedExams");
            DropIndex("dbo.AnswerPositions", new[] { "GeneratedExamID" });
            DropColumn("dbo.AnswerPositions", "GeneratedExamID");
        }
    }
}
