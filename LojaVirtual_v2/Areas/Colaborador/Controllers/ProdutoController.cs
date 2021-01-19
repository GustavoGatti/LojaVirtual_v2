using LojaVirtual_v2.Libraries.Arquivo;
using LojaVirtual_v2.Libraries.Filtro;
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
    [ColaboradorAutorizacao]
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoria;
        private IImagemRepository _imagemRepository;

        public ProdutoController(IProdutoRepository produtoRepository, ICategoriaRepository categoria, IImagemRepository imagemRepository)
        {
            this._produtoRepository = produtoRepository;
            this._categoria = categoria;
            this._imagemRepository = imagemRepository;
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
                //Caminho temp para mover a imagem para o caminho definitivo
                this._produtoRepository.Cadastrar(produto);
               
               List<Imagem> CaminhoDefinitivo =  GerenciadorArquivo
                    .MoverImagensProduto(new List<string>(Request.Form["imagem"]), produto.Id);

                // Salvar o caminho definitivo no banco de dados
                this._imagemRepository.CadastrarImagens(CaminhoDefinitivo, produto.Id);

                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                
                ViewBag.Categorias = this._categoria.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.ID.ToString()));
                produto.Imagems = new List<string>(Request.Form["imagem"]).Where(x => x.Trim().Length > 0).Select(x => new Imagem() { Caminho = x }).ToList();
                return View(produto);
            }
           
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
                //Caminho temp para mover a imagem para o caminho definitivo
                this._produtoRepository.Atualizar(produto);

                // Alterar Mover imagens, precisa alterar somente as imagens q estao na pasta temp

                List<Imagem> CaminhoDefinitivo = GerenciadorArquivo
                     .MoverImagensProduto(new List<string>(Request.Form["imagem"]), produto.Id);

                this._imagemRepository.ExcluirImagemDoProduto(produto.Id);

                // Salvar o caminho definitivo no banco de dados
                this._imagemRepository.CadastrarImagens(CaminhoDefinitivo, produto.Id);

                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            }
            else
            {

                ViewBag.Categorias = this._categoria.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.ID.ToString()));
                produto.Imagems = new List<string>(Request.Form["imagem"]).Where(x => x.Trim().Length > 0).Select(x => new Imagem() { Caminho = x }).ToList();
                return View(produto);
            }
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            //TODO - ler o produto, Deletar Imagens da Pasta, Deletar Imagens do Banco, Deletar o Produto 
            Produto produto = this._produtoRepository.ObterProduto(id);
            GerenciadorArquivo.ExcluirImagensProduto(produto.Imagems.ToList());
            this._imagemRepository.ExcluirImagemDoProduto(id);
            this._produtoRepository.Excluir(id);
            TempData["MSG_S"] = Mensagem.MSG_S003;
            return RedirectToAction(nameof(Index));
        }

    }
}
