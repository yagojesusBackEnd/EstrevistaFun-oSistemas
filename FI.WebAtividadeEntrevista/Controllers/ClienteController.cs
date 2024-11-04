using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
	public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

		public ActionResult IncluirModal()
		{
			return View();
		}

		[HttpPost]
		public JsonResult Incluir(ClienteModel model)
		{
			BoCliente bo = new BoCliente();

			if (!this.ModelState.IsValid)
			{
				// Coleta os erros de validação e retorna no JSON
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
					model.Id = bo.Incluir(new Cliente()
					{
						CEP = model.CEP,
						Cidade = model.Cidade,
						Email = model.Email,
						Estado = model.Estado,
						Logradouro = model.Logradouro,
						Nacionalidade = model.Nacionalidade,
						Nome = model.Nome,
						Sobrenome = model.Sobrenome,
						Telefone = model.Telefone,
						CPF = model.CPF
					});

					return Json("Cadastro efetuado com sucesso");
				}
				catch (Exception ex)
				{
					// Retorna a mensagem de erro no JSON com status 400
					Response.StatusCode = 400;
					return Json(ex.Message);
				}
			}
		}


		[HttpPost]
		public JsonResult Alterar(ClienteModel model)
		{
			BoCliente bo = new BoCliente();

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
					bo.Alterar(new Cliente()
					{
						Id = model.Id,
						CEP = model.CEP,
						Cidade = model.Cidade,
						Email = model.Email,
						Estado = model.Estado,
						Logradouro = model.Logradouro,
						Nacionalidade = model.Nacionalidade,
						Nome = model.Nome,
						Sobrenome = model.Sobrenome,
						Telefone = model.Telefone,
						CPF = model.CPF
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
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
					CPF = cliente.CPF
				};

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
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

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

		[HttpPost]
		public JsonResult SalvarComBeneficiarios(ClienteComBeneficiariosModel model)
		{

			if (model.Cliente == null || model.Beneficiarios == null)
			{
				return Json(new { success = false, message = "Dados incompletos." });
			}

			try
			{
				BoCliente boCliente = new BoCliente();
				BoBeneficiario boBeneficiario = new BoBeneficiario();

				// Converter ClienteModel para DML.Cliente
				var clienteDML = new FI.AtividadeEntrevista.DML.Cliente
				{
					Id = model.Cliente.Id,
					Nome = model.Cliente.Nome,
					Sobrenome = model.Cliente.Sobrenome,
					CEP = model.Cliente.CEP,
					Email = model.Cliente.Email,
					Nacionalidade = model.Cliente.Nacionalidade,
					CPF = model.Cliente.CPF,
					Estado = model.Cliente.Estado,
					Cidade = model.Cliente.Cidade,
					Logradouro = model.Cliente.Logradouro,
					Telefone = model.Cliente.Telefone
				};

				long idCliente;

				// Se o cliente já existir, chamamos Alterar; caso contrário, Incluir
				if (clienteDML.Id > 0)
				{
					boCliente.Alterar(clienteDML);
					idCliente = clienteDML.Id;
				}
				else
				{
					idCliente = boCliente.Incluir(clienteDML);
				}

				// Inclui beneficiários com o idCliente associado
				foreach (var beneficiarioModel in model.Beneficiarios)
				{
					if (!boBeneficiario.VerificarExistencia(beneficiarioModel.CPF))
					{
						boBeneficiario.Incluir(new Beneficiario
						{
							Nome = beneficiarioModel.Nome,
							CPF = beneficiarioModel.CPF,
							IdCliente = idCliente
						});
					}
				}

				return Json(new { success = true, message = "Cliente e beneficiários salvos com sucesso!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

	}
}
