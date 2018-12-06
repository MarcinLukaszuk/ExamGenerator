namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGeneratedExam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneratedExams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ExamCoreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamCores", t => t.ExamCoreID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.ExamCoreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeneratedExams", "StudentID", "dbo.Students");
            DropForeignKey("dbo.GeneratedExams", "ExamCoreID", "dbo.ExamCores");
            DropIndex("dbo.GeneratedExams", new[] { "ExamCoreID" });
            DropIndex("dbo.GeneratedExams", new[] { "StudentID" });
            DropTable("dbo.GeneratedExams");
        }
    }
}
