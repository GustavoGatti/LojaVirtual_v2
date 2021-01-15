using LojaVirtual_v2.Libraries.Email;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Libraries.Texto;
using LojaVirtual_v2.Models.Constants;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstante.Gerente)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        private GerenciarEmail _gerenciarEmail;

        public ColaboradorController(IColaboradorRepository colaboradorRepository, GerenciarEmail gerenciarEmail)
        {
            this._gerenciarEmail = gerenciarEmail;
            this._colaboradorRepository = colaboradorRepository;
        }

        public IActionResult Index(int? pagina)
        {
            IPagedList<Models.Colaborador> colaboradores =  this._colaboradorRepository.ObterTodosColaboradores(pagina);
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Models.Colaborador colaborador)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                colaborador.Tipo = ColaboradorTipoConstante.Comum;
                colaborador.Senha = KeyGenerator.GetUniqueKey(8);
                this._colaboradorRepository.Cadastrar(colaborador);
                this._gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);
                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult GerarSenha(int Id)
        {
            
            Models.Colaborador colaborador = this._colaboradorRepository.ObterColaborador(Id);
            colaborador.Senha = KeyGenerator.GetUniqueKey(8);
            this._colaboradorRepository.AtualizarSenha(colaborador);

            this._gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);
            TempData["MSG_S"] = Mensagem.MSG_S004;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Atualizar(int Id)
        {
            Models.Colaborador colaborador = this._colaboradorRepository.ObterColaborador(Id);
            return View(colaborador);
        }


        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador, int Id)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                //colaborador.Tipo = "C";
                this._colaboradorRepository.Atualizar(colaborador);
                TempData["MSG_S"] = Mensagem.MSG_S002;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            this._colaboradorRepository.Excluir(id);
            TempData["MSG_S"] = Mensagem.MSG_S003;
            return RedirectToAction(nameof(Index));
        }
    }
}
