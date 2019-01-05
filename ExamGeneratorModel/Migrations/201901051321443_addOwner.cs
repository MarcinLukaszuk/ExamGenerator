namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCores", "Owner", c => c.String());
            AddColumn("dbo.StudentGroups", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentGroups", "Owner");
            DropColumn("dbo.ExamCores", "Owner");
        }
    }
}
