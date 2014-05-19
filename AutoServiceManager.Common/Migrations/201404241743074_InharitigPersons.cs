namespace AutoServiceManager.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InharitigPersons : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.People", name: "Type", newName: "Discriminator");
            AlterColumn("dbo.People", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Discriminator", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.People", name: "Discriminator", newName: "Type");
        }
    }
}
