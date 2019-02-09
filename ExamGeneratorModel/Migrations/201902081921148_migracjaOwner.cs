namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracjaOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Owner");
        }
    }
}
