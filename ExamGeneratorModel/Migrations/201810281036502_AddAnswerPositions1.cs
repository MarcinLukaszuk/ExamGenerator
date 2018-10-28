namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswerPositions1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnswerPositions", "X", c => c.Single(nullable: false));
            AlterColumn("dbo.AnswerPositions", "Y", c => c.Single(nullable: false));
            AlterColumn("dbo.AnswerPositions", "Width", c => c.Single(nullable: false));
            AlterColumn("dbo.AnswerPositions", "Height", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AnswerPositions", "Height", c => c.Int(nullable: false));
            AlterColumn("dbo.AnswerPositions", "Width", c => c.Int(nullable: false));
            AlterColumn("dbo.AnswerPositions", "Y", c => c.Int(nullable: false));
            AlterColumn("dbo.AnswerPositions", "X", c => c.Int(nullable: false));
        }
    }
}
