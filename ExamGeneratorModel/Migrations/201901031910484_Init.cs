namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TextAnswer = c.String(maxLength: 500),
                        QuestionID = c.Int(nullable: false),
                        IfCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: false)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExamCoreID = c.Int(nullable: false),
                        QuestionText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamCores", t => t.ExamCoreID, cascadeDelete: false)
                .Index(t => t.ExamCoreID);
            
            CreateTable(
                "dbo.ExamCores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnswerPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerID = c.Int(nullable: false),
                        GeneratedExamID = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Width = c.Single(nullable: false),
                        Height = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerID, cascadeDelete: false)
                .ForeignKey("dbo.GeneratedExams", t => t.GeneratedExamID, cascadeDelete: false)
                .Index(t => t.AnswerID)
                .Index(t => t.GeneratedExamID);
            
            CreateTable(
                "dbo.GeneratedExams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentGroupStudentID = c.Int(nullable: false),
                        ExamCoreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamCores", t => t.ExamCoreID, cascadeDelete: false)
                .ForeignKey("dbo.StudentGroupStudents", t => t.StudentGroupStudentID, cascadeDelete: false)
                .Index(t => t.StudentGroupStudentID)
                .Index(t => t.ExamCoreID);
            
            CreateTable(
                "dbo.StudentGroupStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        StudentGroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: false)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroupID, cascadeDelete: false)
                .Index(t => t.StudentID)
                .Index(t => t.StudentGroupID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SurName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExamCoreStudentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentGroupID = c.Int(nullable: false),
                        ExamCoreID = c.Int(nullable: false),
                        IsGenerated = c.Boolean(),
                        IsValidated = c.Boolean(),
                        ZIPArchiveName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamCores", t => t.ExamCoreID, cascadeDelete: false)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroupID, cascadeDelete: false)
                .Index(t => t.StudentGroupID)
                .Index(t => t.ExamCoreID);
            
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
            DropForeignKey("dbo.GeneratedExamQuestions", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.GeneratedExamQuestions", "GeneratedExamID", "dbo.GeneratedExams");
            DropForeignKey("dbo.ExamCoreStudentGroups", "StudentGroupID", "dbo.StudentGroups");
            DropForeignKey("dbo.ExamCoreStudentGroups", "ExamCoreID", "dbo.ExamCores");
            DropForeignKey("dbo.AnswerPositions", "GeneratedExamID", "dbo.GeneratedExams");
            DropForeignKey("dbo.GeneratedExams", "StudentGroupStudentID", "dbo.StudentGroupStudents");
            DropForeignKey("dbo.StudentGroupStudents", "StudentGroupID", "dbo.StudentGroups");
            DropForeignKey("dbo.StudentGroupStudents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.GeneratedExams", "ExamCoreID", "dbo.ExamCores");
            DropForeignKey("dbo.AnswerPositions", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.Answers", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "ExamCoreID", "dbo.ExamCores");
            DropIndex("dbo.Results", new[] { "StudentID" });
            DropIndex("dbo.Results", new[] { "GeneratedExamID" });
            DropIndex("dbo.GeneratedExamQuestions", new[] { "GeneratedExamID" });
            DropIndex("dbo.GeneratedExamQuestions", new[] { "QuestionID" });
            DropIndex("dbo.ExamCoreStudentGroups", new[] { "ExamCoreID" });
            DropIndex("dbo.ExamCoreStudentGroups", new[] { "StudentGroupID" });
            DropIndex("dbo.StudentGroupStudents", new[] { "StudentGroupID" });
            DropIndex("dbo.StudentGroupStudents", new[] { "StudentID" });
            DropIndex("dbo.GeneratedExams", new[] { "ExamCoreID" });
            DropIndex("dbo.GeneratedExams", new[] { "StudentGroupStudentID" });
            DropIndex("dbo.AnswerPositions", new[] { "GeneratedExamID" });
            DropIndex("dbo.AnswerPositions", new[] { "AnswerID" });
            DropIndex("dbo.Questions", new[] { "ExamCoreID" });
            DropIndex("dbo.Answers", new[] { "QuestionID" });
            DropTable("dbo.Results");
            DropTable("dbo.GeneratedExamQuestions");
            DropTable("dbo.ExamCoreStudentGroups");
            DropTable("dbo.StudentGroups");
            DropTable("dbo.Students");
            DropTable("dbo.StudentGroupStudents");
            DropTable("dbo.GeneratedExams");
            DropTable("dbo.AnswerPositions");
            DropTable("dbo.ExamCores");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
