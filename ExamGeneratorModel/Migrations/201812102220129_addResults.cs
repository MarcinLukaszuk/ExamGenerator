namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GeneratedExamID = c.Int(nullable: false),
                        StudentID = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GeneratedExams", t => t.GeneratedExamID, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: false)
                .Index(t => t.GeneratedExamID)
                .Index(t => t.StudentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Results", "GeneratedExamID", "dbo.GeneratedExams");
            DropIndex("dbo.Results", new[] { "StudentID" });
            DropIndex("dbo.Results", new[] { "GeneratedExamID" });
            DropTable("dbo.Results");
        }
    }
}
