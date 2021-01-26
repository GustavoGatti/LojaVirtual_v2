using AutoMapper;
using LojaVirtual_v2.Controllers.Base;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.Constants;
using LojaVirtual_v2.Models.ProdutoAgregador;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class CarrinhoCompraController : BaseController
    {

        public CarrinhoCompraController(CookieCarrinhoCompra carrinhoCompra,
            IProdutoRepository produtoRepository, IMapper mapper,
            WSCorreiosCalcularFrete wSCorreios, CalcularPacote calcular,
            CookieValorPrazoFrete cookieValorPrazoFrete): base(carrinhoCompra, produtoRepository, mapper, wSCorreios, calcular, cookieValorPrazoFrete)
        {

        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

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

        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try
            {
                var produto = CarregarProdutoDB();
                List<Pacote> pacotes = this.CalcularPacote.CalcularPacotesDeProdutos(produto);

                ValorPrazoFrete valorPAC = await this._wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstante.PAC, pacotes);
                ValorPrazoFrete valorSEDEX = await this._wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstante.SEDEX, pacotes);
                ValorPrazoFrete valorSEDEX10 = await this._wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstante.SEDEX10, pacotes);

                List<ValorPrazoFrete> lista = new List<ValorPrazoFrete>();
                if (valorPAC != null)
                {
                    lista.Add(valorPAC);
                }
                if (valorSEDEX != null)
                {
                    lista.Add(valorSEDEX);
                }
                if (valorSEDEX10 != null)
                {
                    lista.Add(valorSEDEX10);
                }


                this._cookieValorPrazo.Cadastrar(lista);
                return Ok(lista);
            }
            catch (Exception e)
            {
                this._cookieValorPrazo.Remover();
                return BadRequest(e);
                
            }
        }
    }
}
