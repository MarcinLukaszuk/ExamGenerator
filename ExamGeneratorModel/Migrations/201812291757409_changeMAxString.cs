namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeMAxString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "QuestionText", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "QuestionText", c => c.String(maxLength: 500));
        }
    }
}
