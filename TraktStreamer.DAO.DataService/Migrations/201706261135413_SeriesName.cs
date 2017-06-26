namespace TraktStreamer.DAO.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Series", "Name");
        }
    }
}
