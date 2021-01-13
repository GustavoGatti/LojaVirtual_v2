using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class CategoriaController : Controller
    {

        private ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            this._categoriaRepository = categoriaRepository;
        }

        public IActionResult Index(int? pagina)
        {
            /*Paginacao*/
            var categorias = this._categoriaRepository.ObterTodasCategorias(pagina);

            return View(categorias);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a => new SelectListItem(a.Nome, a.ID.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Categoria categoria)
        {
            //TODO - Implementar
            if (ModelState.IsValid)
            {
                this._categoriaRepository.Cadastrar(categoria);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a => new SelectListItem(a.Nome, a.ID.ToString()));
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int Id)
        {
            var categoria =_categoriaRepository.ObterCategoria(Id);
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Where(a => a.ID != Id).Select(a => new SelectListItem(a.Nome, a.ID.ToString()));
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Categoria categoria, int Id)
        {
            if (ModelState.IsValid)
            {
                this._categoriaRepository.Atualizar(categoria);
                TempData["MSG_S"] = "Registro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Where(a => a.ID != Id)
                .Select(a => new SelectListItem(a.Nome, a.ID.ToString()));
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int Id)
        {
            this._categoriaRepository.Excluir(Id);
            TempData["MSG_S"] = "Registro excluido com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
