using AutoMapper;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.ProdutoAgregador;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;
        private IMapper _mapper;
        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper)
        {
            this._mapper = mapper;
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemsNoCarrinho =  this._carrinhoCompra.Consultar();
            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemsNoCarrinho)
            {
                //AutoMapper

                Produto produto = this._produtoRepository.ObterProduto(item.Id);
                ProdutoItem produtoItem = this._mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;
                produtoItemCompleto.Add(produtoItem);
            }

            return View(produtoItemCompleto);
        }

        //Item ID = ID do produto
        public IActionResult AdicionarItem(int id)
        {
            Produto produto = this._produtoRepository.ObterProduto(id);
            
            if(produto == null)
            {
                //TODO - Mensagem de erro
                return View("NaoExisteItem");
            }
            else
            {

                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = 1 };
                this._carrinhoCompra.Cadastrar(item);
                return RedirectToAction(nameof(Index));
            }
     }
        public IActionResult AlterarItem(int id, int quantidade)
        {

            Produto produto = this._produtoRepository.ObterProduto(id);
            //Validar se existe essa quantidade no estoque
            if (quantidade < 1)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E007});   
            }else if (quantidade > produto.Quantidade)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E008 });
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
                this._carrinhoCompra.Atualizar(item);
                return Ok(new { mensagem = Mensagem.MSG_S005 });
            }
        }
        public IActionResult RemoverItem(int id)
        {
            this._carrinhoCompra.Remover(new ProdutoItem() {Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
