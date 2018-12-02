namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Exams", newName: "ExamCores");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ExamCores", newName: "Exams");
        }
    }
}
