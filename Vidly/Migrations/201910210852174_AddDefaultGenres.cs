namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultGenres : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [Genres] VALUES('Comedy')");
            Sql("INSERT INTO [Genres] VALUES('Action')");
            Sql("INSERT INTO [Genres] VALUES('Family')");
            Sql("INSERT INTO [Genres] VALUES('Romance')");
        }
        
        public override void Down()
        {
        }
    }
}
