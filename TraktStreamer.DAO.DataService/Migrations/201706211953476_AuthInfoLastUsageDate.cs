namespace TraktStreamer.DAO.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthInfoLastUsageDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthorizationInfoes", "LastUsageDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthorizationInfoes", "LastUsageDate");
        }
    }
}
