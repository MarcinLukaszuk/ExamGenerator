namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addValidatedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCoreStudentGroups", "IsValidated", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExamCoreStudentGroups", "IsValidated");
        }
    }
}
