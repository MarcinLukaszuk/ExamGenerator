namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentGroupStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        StudentGroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.StudentGroups", t => t.StudentGroupID, cascadeDelete: true)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentGroupStudents", "StudentGroupID", "dbo.StudentGroups");
            DropForeignKey("dbo.StudentGroupStudents", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentGroupStudents", new[] { "StudentGroupID" });
            DropIndex("dbo.StudentGroupStudents", new[] { "StudentID" });
            DropTable("dbo.Students");
            DropTable("dbo.StudentGroupStudents");
            DropTable("dbo.StudentGroups");
        }
    }
}
