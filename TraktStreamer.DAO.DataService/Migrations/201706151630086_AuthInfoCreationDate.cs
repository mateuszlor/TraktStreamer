namespace TraktStreamer.DAO.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthInfoCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthorizationInfoes", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthorizationInfoes", "CreationDate");
        }
    }
}
