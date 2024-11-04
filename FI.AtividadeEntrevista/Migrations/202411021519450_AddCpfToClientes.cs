namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCpfToClientes : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Clientes", "CPF", c => c.String(maxLength: 14));
		}
        
        public override void Down()
        {
            DropTable("dbo.Clientes");
        }
    }
}
