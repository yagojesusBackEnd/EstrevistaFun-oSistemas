using System;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
	public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {

			// Verifica se o CPF já existe no banco

			if (VerificarExistencia(cliente.CPF))
			{
				throw new Exception("O CPF já está cadastrado.");
			}

			DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

		/// <summary>
		/// Altera um cliente
		/// </summary>
		/// <param name="cliente">Objeto de cliente</param>
		public void Alterar(DML.Cliente cliente)
		{
			var clienteExistente = Consultar(cliente.Id);

			if (clienteExistente == null)
			{
				throw new Exception("Cliente não encontrado.");
			}

			if (clienteExistente.CPF != cliente.CPF && VerificarExistencia(cliente.CPF))
			{
				throw new Exception("Erro: CPF já está cadastrado");
			}

			DAL.DaoCliente cli = new DAL.DaoCliente();
			cli.Alterar(cliente);
		}


		/// <summary>
		/// Consulta o cliente pelo id
		/// </summary>
		/// <param name="id">id do cliente</param>
		/// <returns></returns>
		public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }
		/// <summary>
		/// VerificaExistencia
		/// </summary>
		/// <param name="CPF"></param>
		/// <returns></returns>
		public bool VerificarExistencia(string CPF)
		{
			if (!ValidarCPF(CPF))
				throw new ArgumentException("CPF inválido.");

			// Formata o CPF para salvar no banco
			CPF = FormatarCPFParaSalvar(CPF);

			DAL.DaoCliente cli = new DAL.DaoCliente();
			return cli.VerificarExistencia(CPF);
		}

		/// <summary>
		/// ValidarCPF
		/// </summary>
		/// <param name="CPF"></param>
		/// <returns></returns>
		public bool ValidarCPF(string CPF)
		{
			// Remove caracteres não numéricos
			var reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
			CPF = reg.Replace(CPF, string.Empty);

			#region Verificação CPF

			if (CPF.Length != 11)
				return false;

			bool igual = true;
			for (int i = 1; i < 11 && igual; i++)
				if (CPF[i] != CPF[0])
					igual = false;

			if (igual || CPF == "12345678909")
				return false;

			int[] numeros = new int[11];
			for (int i = 0; i < 11; i++)
				numeros[i] = int.Parse(CPF[i].ToString());

			int soma = 0;
			for (int i = 0; i < 9; i++)
				soma += (10 - i) * numeros[i];

			int resultado = soma % 11;
			if (resultado == 1 || resultado == 0)
			{
				if (numeros[9] != 0)
					return false;
			}
			else if (numeros[9] != 11 - resultado)
				return false;

			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += (11 - i) * numeros[i];

			resultado = soma % 11;
			if (resultado == 1 || resultado == 0)
			{
				if (numeros[10] != 0)
					return false;
			}
			else if (numeros[10] != 11 - resultado)
				return false;

			return true;

			#endregion
		}

		private string FormatarCPFParaSalvar(string cpf)
		{
			// Remove caracteres não numéricos para garantir que o CPF tenha apenas números
			var reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
			cpf = reg.Replace(cpf, string.Empty);

			// Converte para UInt64 e formata com pontos e traço
			return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
		}
	}
}
