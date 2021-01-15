using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private LoginColaborador _loginColaborador;
        private IColaboradorRepository _colaboradorRepository;

        public HomeController(LoginColaborador loginColaborador, IColaboradorRepository colaboradorRepository)
        {
            _loginColaborador = loginColaborador;
            _colaboradorRepository = colaboradorRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.Colaborador colaborador)
        {
            Models.Colaborador colaboradorDB = this._colaboradorRepository.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB != null)
            {
                //Fazer Consulta no Banco de dados Email e Senha
                //Armazenar essa informação na sessão(Cliente)

                this._loginColaborador.Login(colaboradorDB);
                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuario não encontrado, verifique o email e senha digitado!";
                return View();
            }
        }

        [ColaboradorAutorizacao]
        public IActionResult Painel()
        {
            return View();
        }

        public IActionResult RecuperarSenha()
        {
            return View();
        }

        public IActionResult CadastrarNovaSenha()
        {
            return View();
        }

        [ColaboradorAutorizacao]
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction(nameof(Login));
        }
    }
}
