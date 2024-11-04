using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
	/// <summary>
	/// Classe de Modelo de Beneficiário
	/// </summary>
	public class BeneficiarioModel
	{
		public long Id { get; set; }

		/// <summary>
		/// CPF do Beneficiário
		/// </summary>
		[Required]
		[MaxLength(14)]
		[RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite um CPF válido")]
		public string CPF { get; set; }

		/// <summary>
		/// Nome do Beneficiário
		/// </summary>
		[Required]
		public string Nome { get; set; }

		/// <summary>
		/// ID do Cliente ao qual o Beneficiário está associado
		/// </summary>
		public long IdCliente { get; set; }
	}
}
