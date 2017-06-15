namespace TraktStreamer.DAO.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorizationInfoes",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        AccessToken = c.String(),
                        RefreshToken = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TraktSlug = c.String(),
                        ToDownload = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Series");
            DropTable("dbo.AuthorizationInfoes");
        }
    }
}
