using AutoMapper;
using LojaVirtual_v2.Controllers.Base;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Cookie;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
using LojaVirtual_v2.Libraries.Gerenciador.Pagamento.PagarMe;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Libraries.Texto;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.ProdutoAgregador;
using LojaVirtual_v2.Models.ViewModels.Pagamento;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;

        public PagamentoController(CookieCarrinhoCompra carrinhoCompra,
        IProdutoRepository produtoRepository, IMapper mapper,
        WSCorreiosCalcularFrete wSCorreios, CalcularPacote calcular,
        CookieFrete cookieValorPrazoFrete, Cookie cookie, IEnderecoEntregaRepository enderecoEntrega,
        LoginCliente loginCliente, GerenciarPagarMe gerenciarPagarMe)
        : base(carrinhoCompra, produtoRepository, mapper, wSCorreios, calcular, cookieValorPrazoFrete, enderecoEntrega, loginCliente)
        {
            this._cookie = cookie;
            this._gerenciarPagarMe = gerenciarPagarMe;
        }

        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);

            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntrega = ObterEndereco();
                var carrinhoHash = GerarHash(this._carrinhoCompra.Consultar());
              
                int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));
                List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                var frete = ObterFrete(cep.ToString());
                var total = ObterValorTotalCompra(frete, produtoItemCompleto);
                var parcelamento = this._gerenciarPagarMe.CalcularPagamentoParcelado(total);


                ViewBag.Frete = frete;
                ViewBag.Parcelamento = parcelamento.Select( x => new SelectListItem(
                        String.Format("{0} x {1} {2} - TOTAL: {3}", x.Numero, x.ValorPorParcela.ToString("C"), (x.Juros)? "c/ juros": "s/ juros", x.Valor.ToString("C")),
                        x.Numero.ToString()  
                    )
                ).ToList();
                ViewBag.Produtos = produtoItemCompleto;



                return View(nameof(Index));

            }
            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
        }

        [HttpPost]
        [ClienteAutorizacao]
        public IActionResult Index([FromForm]IndexViewModel indexViewModel)
        {
            if (ModelState.IsValid)
            {
                //TODO - Integrar os dados, Salvar os dados e redirecionar para a tela de pedido concluido
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete(enderecoEntrega.CEP.ToString());
                List<ProdutoItem> produtos = CarregarProdutoDB();

                var parcela =  this._gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(frete, produtos)).Where(x => x.Numero == indexViewModel.parcelamento.Numero).FirstOrDefault();

                try
                {
                    dynamic pagarmeReposta = this._gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.cartaoCredito, parcela, enderecoEntrega, frete, produtos);
                    return new ContentResult() { Content = "Sucesso!" + pagarmeReposta.TransactionId};
                }
                catch (PagarMeException e)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    if (e.Error.Errors.Count() > 0)
                    {
                        sb.Append("Erro no pagamento: ");
                        foreach (var erro in e.Error.Errors)
                        {
                            sb.Append("-" + erro.Message + "<br/>");
                        }
                    }
                    TempData["MSG_E"] = sb.ToString();
                    return Index();
                }     
            }
            else
            {
                return Index();
            }
        }

        private EnderecoEntrega ObterEndereco()
        {
            EnderecoEntrega enderecoEntrega = null;
            var EnderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

            if (EnderecoEntregaId == 0)
            {
                Cliente cliente = this._login.GetCliente();

                enderecoEntrega = new EnderecoEntrega();
                enderecoEntrega.Nome = "Endereço do cliente";
                enderecoEntrega.Id = 0;
                enderecoEntrega.CEP = cliente.CEP;
                enderecoEntrega.Estado = cliente.Estado;
                enderecoEntrega.Cidade = cliente.Cidade;
                enderecoEntrega.Bairro = cliente.Bairro;
                enderecoEntrega.Endereco = cliente.Endereco;
                enderecoEntrega.Complemento = cliente.Complemento;
                enderecoEntrega.Numero = cliente.Numero;
            }
            else
            {
                enderecoEntrega = this._enderecoEntrega.ObterEnderecoEntrega(EnderecoEntregaId);
            }
            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete(string cepDestino)
        {
            var tipoFreteSelecionadoPeloUsuario = this._cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(this._carrinhoCompra.Consultar());

            int cep = int.Parse(Mascara.Remover(cepDestino));

            var frete = this._cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();
            if (frete != null)
            {
                return frete.ListaValores.Where(x => x.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private decimal ObterValorTotalCompra(ValorPrazoFrete frete, List<ProdutoItem> produtos)
        {
            decimal total = Convert.ToDecimal(frete.Valor);

            foreach (var produto in produtos)
            {
                total += produto.Valor;
            }

            return total;
        }
    }
}
