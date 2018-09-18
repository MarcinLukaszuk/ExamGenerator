namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(maxLength: 500),
                        Exam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.Exam_Id)
                .Index(t => t.Exam_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "Exam_Id" });
            DropTable("dbo.Questions");
            DropTable("dbo.Exams");
        }
    }
}
