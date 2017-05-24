namespace SklepWWW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DaneUzytkownika_KodPocztowy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DaneUzytkownika_KodPocztowy");
        }
    }
}
