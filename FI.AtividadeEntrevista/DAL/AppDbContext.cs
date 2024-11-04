using FI.AtividadeEntrevista.DML;
using System.Data.Entity;

namespace FI.AtividadeEntrevista.DAL
{
	public class AppDbContext : DbContext
	{
		//Chamando a string de conexão com o banco
		public AppDbContext() : base("BancoDeDados")
		{
		}

		public DbSet<Cliente> Clientes { get; set; }
	}
}
