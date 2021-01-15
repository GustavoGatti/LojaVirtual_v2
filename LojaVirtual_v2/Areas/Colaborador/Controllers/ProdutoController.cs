using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoria;

        public ProdutoController(IProdutoRepository produtoRepository, ICategoriaRepository categoria)
        {
            this._produtoRepository = produtoRepository;
            this._categoria = categoria;
        }

        public IActionResult Index(int? pagina, string pesquisa)
        {
            var produtos = this._produtoRepository.ObterTodosProdutos(pagina, pesquisa);
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = this._categoria.ObterTodasCategorias().Select(x => new SelectListItem(x .Nome, x.ID.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto produto)
        {
            if (ModelState.IsValid)
            {
                this._produtoRepository.Cadastrar(produto);
                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            ViewBag.Categorias = this._categoria.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.ID.ToString()));
            Produto produto = this._produtoRepository.ObterProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult Atualizar(Produto produto, int id)
        {
            if (ModelState.IsValid)
            {
                this._produtoRepository.Atualizar(produto);
                TempData["MSG_S"] = Mensagem.MSG_S002;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = this._categoria.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.ID.ToString()));
            return View(produto);
        }
    }
}
