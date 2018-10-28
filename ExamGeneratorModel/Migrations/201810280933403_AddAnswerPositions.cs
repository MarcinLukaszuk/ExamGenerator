namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswerPositions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerID = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerID, cascadeDelete: true)
                .Index(t => t.AnswerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnswerPositions", "AnswerID", "dbo.Answers");
            DropIndex("dbo.AnswerPositions", new[] { "AnswerID" });
            DropTable("dbo.AnswerPositions");
        }
    }
}
