using LojaVirtual_v2.Database;
using LojaVirtual_v2.Libraries.Email;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.ViewModels;
using LojaVirtual_v2.Repositories;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _repository;
        private INewletterRepository _newsletterRepository;
        private LoginCliente _loginCliente;
        private GerenciarEmail _gerenciarEmail;
        private IProdutoRepository _produtoRepository;
        public HomeController(IProdutoRepository produtoRepository, IClienteRepository repository, INewletterRepository newsletterRepository, LoginCliente loginCliente, GerenciarEmail gerenciarEmail)
        {
            this._repository = repository;
            this._newsletterRepository = newsletterRepository;
            this._loginCliente = loginCliente;
            this._gerenciarEmail = gerenciarEmail;
            this._produtoRepository = produtoRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {

            if (ModelState.IsValid)
            {

                this._newsletterRepository.Cadastrar(newsletter);

                TempData["MSG_S"] = "E-mail cadastrado! Agora você vai receber promoções especiais no seu e-mail! Fique atento as novidades!";                
                return RedirectToAction(nameof(Index));
            }
            else
            {
               
                return View();
            }
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            try
            {

                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var ListaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid = Validator.TryValidateObject(contato, contexto, ListaMensagens, true);

                if (isValid)
                {
                    this._gerenciarEmail.EnviarContatoPorEmail(contato);
                    ViewData["MSG_S"] = "Mensagem de contato enviado com sucesso!";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in ListaMensagens)
                    {
                        sb.Append(texto.ErrorMessage + "<br/>");
                    }
                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTATO"] = contato;
                }
            }
            catch (Exception)
            {
                ViewData["MSG_E"] = "Opps! Tivemos um erro, tente novamente mais tarde";
                //TODO - Implementar Log
            }
         
            return View("Contato");
        }

        [HttpGet]
        public IActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Cliente cliente)
        {
            Cliente clienteDB = this._repository.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                //Fazer Consulta no Banco de dados Email e Senha
                //Armazenar essa informação na sessão(Cliente)

                this._loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(Painel)));
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
        public IActionResult CadastroCliente([FromForm]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                this._repository.Cadastrar(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                return RedirectToAction(nameof(CadastroCliente));
            }
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }

        public IActionResult Categoria()
        {
            return View();
        }
    }
}
