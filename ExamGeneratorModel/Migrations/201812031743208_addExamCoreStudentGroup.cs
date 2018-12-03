namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExamCoreStudentGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamCoreStudentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentGroupID = c.Int(nullable: false),
                        ExamCoreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamCores", t => t.ExamCoreID, cascadeDelete: true)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroupID, cascadeDelete: true)
                .Index(t => t.StudentGroupID)
                .Index(t => t.ExamCoreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamCoreStudentGroups", "StudentGroupID", "dbo.StudentGroups");
            DropForeignKey("dbo.ExamCoreStudentGroups", "ExamCoreID", "dbo.ExamCores");
            DropIndex("dbo.ExamCoreStudentGroups", new[] { "ExamCoreID" });
            DropIndex("dbo.ExamCoreStudentGroups", new[] { "StudentGroupID" });
            DropTable("dbo.ExamCoreStudentGroups");
        }
    }
}
