using System;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
	public class BoBeneficiario
	{
		public long Incluir(DML.Beneficiario beneficiario, bool verificarDuplicidade = true)
		{
			if (verificarDuplicidade && VerificarExistencia(beneficiario.CPF))
			{
				throw new Exception("O CPF já está cadastrado.");
			}

			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			return dao.Incluir(beneficiario);
		}

		public void Alterar(DML.Beneficiario beneficiario)
		{
			if (VerificarExistencia(beneficiario.CPF))
			{
				throw new Exception("Erro: CPF já está cadastrado");
			}

			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			dao.Alterar(beneficiario);
		}

		public DML.Beneficiario Consultar(long id)
		{
			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			return dao.Consultar(id);
		}

		public void Excluir(long id)
		{
			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			dao.Excluir(id);
		}

		public List<DML.Beneficiario> Listar()
		{
			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			return dao.Listar();
		}

		public List<DML.Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
		{
			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			return dao.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
		}

		public List<DML.Beneficiario> PesquisaById(long idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
		{
			DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
			return dao.PesquisaById(idCliente, iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
		}

		public bool VerificarExistencia(string CPF, long? idCliente = null)
		{
			if (!ValidarCPF(CPF))
				throw new ArgumentException("CPF inválido.");

			// Formata o CPF para salvar no banco
			CPF = FormatarCPFParaSalvar(CPF);

			DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
			return cli.VerificarExistencia(CPF, idCliente);
		}


		public bool ValidarCPF(string cpf)
		{
			cpf = System.Text.RegularExpressions.Regex.Replace(cpf, @"[^0-9]", string.Empty);

			if (cpf.Length != 11 || new HashSet<char>(cpf).Count == 1 || cpf == "12345678909")
				return false;

			return VerificarDigitosCPF(cpf);
		}

		private bool VerificarDigitosCPF(string cpf)
		{
			int[] numeros = Array.ConvertAll(cpf.ToCharArray(), c => (int)char.GetNumericValue(c));

			int soma = 0;
			for (int i = 0; i < 9; i++)
				soma += (10 - i) * numeros[i];
			int resultado = soma % 11;

			if ((resultado < 2 && numeros[9] != 0) || (resultado >= 2 && numeros[9] != 11 - resultado))
				return false;

			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += (11 - i) * numeros[i];
			resultado = soma % 11;

			return (resultado < 2 && numeros[10] == 0) || (resultado >= 2 && numeros[10] == 11 - resultado);
		}

		private string FormatarCPFParaSalvar(string cpf)
		{
			cpf = System.Text.RegularExpressions.Regex.Replace(cpf, @"[^0-9]", string.Empty);
			return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
		}
	}
}
