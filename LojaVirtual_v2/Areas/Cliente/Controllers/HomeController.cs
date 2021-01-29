using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Repositories;
using LojaVirtual_v2.Repositories.Contracts;

namespace LojaVirtual_v2.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private LoginCliente _loginCliente;
        private IClienteRepository _repository;
        private IEnderecoEntregaRepository _endereco;

        public HomeController(LoginCliente loginCliente, IClienteRepository repository, IEnderecoEntregaRepository endereco)
        {
            _loginCliente = loginCliente;
            _repository = repository;
            _endereco = endereco;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            Models.Cliente clienteDB = this._repository.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                //Fazer Consulta no Banco de dados Email e Senha
                //Armazenar essa informação na sessão(Cliente)

                this._loginCliente.Login(clienteDB);
                if(returnUrl == null)
                {
                    return new RedirectResult(Url.Action(nameof(Painel)));
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
              
            }
            else
            {
                ViewData["MSG_E"] = "Usuario não encontrado, verifique o email e senha digitado!";
                return View();
            }

        }

        [HttpGet]
        [ClienteAutorizacao]
        public IActionResult Painel()
        {
            return new ContentResult() { Content = "Este é o painel do cliente" };
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                this._repository.Cadastrar(cliente);
                this._loginCliente.Login(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                if(returnUrl == null)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }

        public IActionResult Categoria()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CadastroEnderecoEntrega()
        {
            //TODO melhorar o html do campo Nome
            //TODO - remover do JS a opção de carregar o cep quando ele esta no cookie para esta tela
            return View();
        }

        [HttpPost]
        public IActionResult CadastroEnderecoEntrega([FromForm]EnderecoEntrega enderecoentrega, string returnUrl = null)
        {

            if(ModelState.IsValid)
            {
                enderecoentrega.ClienteId = this._loginCliente.GetCliente().Id;
                this._endereco.Cadastrar(enderecoentrega);
                if(returnUrl == null)
                {
                    //TODO - Listagem de endereco
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }

            return View();
        }
    }
}
