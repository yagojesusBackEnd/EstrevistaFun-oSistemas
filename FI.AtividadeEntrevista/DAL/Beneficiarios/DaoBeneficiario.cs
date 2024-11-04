using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
	/// <summary>
	/// Classe de acesso a dados de Beneficiário
	/// </summary>
	internal class DaoBeneficiario : AcessoDados
	{
		/// <summary>
		/// Inclui um novo beneficiário
		/// </summary>
		/// <param name="beneficiario">Objeto de beneficiário</param>
		internal long Incluir(DML.Beneficiario beneficiario)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
			parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
			parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiario.IdCliente));

			DataSet ds = base.Consultar("FI_SP_IncBeneficiarioV2", parametros);
			long ret = 0;
			if (ds.Tables[0].Rows.Count > 0)
				long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
			return ret;
		}

		/// <summary>
		/// Consulta beneficiário por ID
		/// </summary>
		/// <param name="Id">ID do beneficiário</param>
		internal DML.Beneficiario Consultar(long Id)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

			DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
			List<DML.Beneficiario> beneficiarios = Converter(ds);

			return beneficiarios.FirstOrDefault();
		}

		public bool VerificarExistencia(string CPF, long? idAtual = null)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
			{
				new System.Data.SqlClient.SqlParameter("CPF", CPF)
			};

			if (idAtual.HasValue)
			{
				parametros.Add(new System.Data.SqlClient.SqlParameter("IdAtual", idAtual.Value));
			}

			DataSet ds = base.Consultar("FI_SP_VerificaCPFBeneficiario", parametros);

			// Retorna se encontrou mais de um CPF correspondente (excluindo o idAtual)
			return ds.Tables[0].Rows.Count > 0;
		}


		internal List<Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm));
			parametros.Add(new System.Data.SqlClient.SqlParameter("quantidade", quantidade));
			parametros.Add(new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao));
			parametros.Add(new System.Data.SqlClient.SqlParameter("crescente", crescente));

			DataSet ds = base.Consultar("FI_SP_PesqBeneficiario", parametros);
			List<DML.Beneficiario> beneficiarios = Converter(ds);

			int iQtd = 0;

			if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

			qtd = iQtd;

			return beneficiarios;
		}

		internal List<Beneficiario> PesquisaById(long idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
			{
				new System.Data.SqlClient.SqlParameter("idCliente", idCliente),
				new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm),
				new System.Data.SqlClient.SqlParameter("quantidade", quantidade),
				new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao),
				new System.Data.SqlClient.SqlParameter("crescente", crescente)
			};

			DataSet ds = base.Consultar("FI_SP_PesqBeneficiarioPorCliente", parametros);
			List<DML.Beneficiario> beneficiarios = Converter(ds);

			int iQtd = 0;

			if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

			qtd = iQtd;

			return beneficiarios;
		}
		/// <summary>
		/// Lista todos os beneficiários
		/// </summary>
		internal List<DML.Beneficiario> Listar()
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

			DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
			List<DML.Beneficiario> beneficiarios = Converter(ds);

			return beneficiarios;
		}

		/// <summary>
		/// Altera um beneficiário existente
		/// </summary>
		/// <param name="beneficiario">Objeto de beneficiário</param>
		internal void Alterar(DML.Beneficiario beneficiario)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
			parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
			parametros.Add(new System.Data.SqlClient.SqlParameter("Id", beneficiario.Id));
			parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiario.IdCliente));

			base.Executar("FI_SP_AltBeneficiario", parametros);
		}

		/// <summary>
		/// Exclui um beneficiário
		/// </summary>
		/// <param name="Id">ID do beneficiário</param>
		internal void Excluir(long Id)
		{
			List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

			parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

			base.Executar("FI_SP_DelBeneficiario", parametros);
		}

		private List<DML.Beneficiario> Converter(DataSet ds)
		{
			List<DML.Beneficiario> lista = new List<DML.Beneficiario>();
			if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					DML.Beneficiario beneficiario = new DML.Beneficiario();
					beneficiario.Id = row.Field<long>("Id");
					beneficiario.Nome = row.Field<string>("Nome");
					beneficiario.CPF = row.Field<string>("CPF");
					beneficiario.IdCliente = row.Field<long>("IDCLIENTE");
					lista.Add(beneficiario);
				}
			}

			return lista;
		}
	}
}
