using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class ProdutoController : Controller
    {
        private ICategoriaRepository _categoriaRepository;
        private IProdutoRepository _produtoRepository;
     

        public ProdutoController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        [Route("/Produto/Categoria/{slug}")]
        public IActionResult ListagemCategoria(string slug)
        {
           
            return View(this._categoriaRepository.ObterTodasCategorias(slug));
        }



        /*------------------------------------------------*/
        [HttpGet]
        public ActionResult Visualizar(int id)
        {
            Produto produto = this._produtoRepository.ObterProduto(id);
   
            return View(produto);
        }
    }
}
