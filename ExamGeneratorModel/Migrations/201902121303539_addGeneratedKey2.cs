namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGeneratedKey2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GeneratedExams", "ExamCoreID", "dbo.ExamCores");
            DropIndex("dbo.GeneratedExams", new[] { "ExamCoreID" });
            DropColumn("dbo.GeneratedExams", "ExamCoreID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneratedExams", "ExamCoreID", c => c.Int(nullable: false));
            CreateIndex("dbo.GeneratedExams", "ExamCoreID");
            AddForeignKey("dbo.GeneratedExams", "ExamCoreID", "dbo.ExamCores", "Id", cascadeDelete: true);
        }
    }
}
