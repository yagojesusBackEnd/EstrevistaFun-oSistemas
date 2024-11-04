namespace FI.AtividadeEntrevista.DML
{
	/// <summary>
	/// Classe de cliente que representa o registo na tabela Beneficiários do Banco de Dados
	/// </summary>
	public class Beneficiario
	{
        public long Id { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public long IdCliente { get; set; }
    }
}
