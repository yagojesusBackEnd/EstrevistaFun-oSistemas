using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
	public class BeneficiarioController : Controller
	{
		
		[HttpPost]
		public JsonResult Incluir(BeneficiarioModel model)
		{
			BoBeneficiario bo = new BoBeneficiario();

			if (!ModelState.IsValid)
			{
				// Coleta os erros de validação e retorna no JSON com status 400
				var erros = ModelState.Values
							 .SelectMany(v => v.Errors)
							 .Select(e => e.ErrorMessage)
							 .ToList();

				Response.StatusCode = 400;
				return Json(new { success = false, message = "Erros de validação", errors = erros });
			}
			else
			{
				try
				{
					// Inclui o beneficiário
					model.Id = bo.Incluir(new Beneficiario()
					{
						Nome = model.Nome,
						CPF = model.CPF,
						IdCliente = model.IdCliente
					});

					return Json(new { success = true, message = "Cadastro efetuado com sucesso", id = model.Id });
				}
				catch (Exception ex)
				{
					// Retorna a mensagem de erro no JSON com status 400
					return Json(new { success = false, message = ex.Message });
				}
			}
		}

		[HttpPost]
		public JsonResult Alterar(BeneficiarioModel model)
		{
			BoBeneficiario bo = new BoBeneficiario();

			if (!this.ModelState.IsValid)
			{
				List<string> erros = (from item in ModelState.Values
									  from error in item.Errors
									  select error.ErrorMessage).ToList();

				Response.StatusCode = 400;
				return Json(string.Join(Environment.NewLine, erros));
			}
			else
			{
				try
				{
					bo.Alterar(new Beneficiario()
					{
						Id = model.Id,
						Nome = model.Nome,
						CPF = model.CPF,
						IdCliente = model.IdCliente
					});

					return Json("Cadastro alterado com sucesso");
				}
				catch (Exception ex)
				{
					Response.StatusCode = 400;
					return Json(ex.Message);
				}
			}
		}


		[HttpGet]
		public ActionResult Alterar(long id)
		{
			BoBeneficiario bo = new BoBeneficiario();
			Beneficiario beneficiario = bo.Consultar(id);
			Models.BeneficiarioModel model = null;

			if (beneficiario != null)
			{
				model = new BeneficiarioModel()
				{
					Id = beneficiario.Id,
					Nome = beneficiario.Nome,
					CPF = beneficiario.CPF,
					IdCliente = beneficiario.IdCliente
				};


			}

			return View(model);
		}

		[HttpPost]
		public JsonResult BeneficiarioList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
		{
			try
			{
				int qtd = 0;
				string campo = string.Empty;
				string crescente = string.Empty;
				string[] array = jtSorting.Split(' ');

				if (array.Length > 0)
					campo = array[0];

				if (array.Length > 1)
					crescente = array[1];

				List<Beneficiario> beneficiarios = new BoBeneficiario().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

				//Return result to jTable
				return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd });
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message });
			}
		}

		public JsonResult BeneficiarioListById(long? idCliente, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
		{
			if (idCliente == null || idCliente <= 0)
			{
				return Json(new { Result = "ERROR", Message = "Cliente ID inválido." }, JsonRequestBehavior.AllowGet);
			}

			try
			{
				int qtd = 0;
				string campo = string.Empty;
				string crescente = string.Empty;
				string[] array = jtSorting?.Split(' ');

				if (array != null && array.Length > 0)
					campo = array[0];

				if (array != null && array.Length > 1)
					crescente = array[1];

				List<Beneficiario> beneficiarios = new BoBeneficiario().PesquisaById(idCliente.Value, jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

				return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

	}
}