namespace TraktStreamer.DAO.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesLimits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "HasCustomLimits", c => c.Boolean(nullable: false));
            AddColumn("dbo.Series", "Limit1080", c => c.Long());
            AddColumn("dbo.Series", "Limit720", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Series", "Limit720");
            DropColumn("dbo.Series", "Limit1080");
            DropColumn("dbo.Series", "HasCustomLimits");
        }
    }
}
