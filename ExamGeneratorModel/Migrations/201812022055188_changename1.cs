namespace ExamGeneratorModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Questions", name: "ExamID", newName: "ExamCoreID");
            RenameIndex(table: "dbo.Questions", name: "IX_ExamID", newName: "IX_ExamCoreID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Questions", name: "IX_ExamCoreID", newName: "IX_ExamID");
            RenameColumn(table: "dbo.Questions", name: "ExamCoreID", newName: "ExamID");
        }
    }
}
