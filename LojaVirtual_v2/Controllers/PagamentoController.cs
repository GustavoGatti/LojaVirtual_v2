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

    [ClienteAutorizacao]
    [ValidateCookiePagamentoController]
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

 
        [HttpGet]
        public IActionResult Index()
        {
            
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
            ValorPrazoFrete frete = ObterFrete();

            ViewBag.Frete = frete;
            ViewBag.Produtos = produtoItemCompleto;
            ViewBag.Parcelamento = CalcularParcelamento(produtoItemCompleto, frete);

            return View(nameof(Index));
            
        }



        [HttpPost]
        public IActionResult Index([FromForm]IndexViewModel indexViewModel)
        {
            if (ModelState.IsValid)
            {
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete();
                List<ProdutoItem> produtos = CarregarProdutoDB();
                Parcelamento parcela = GerarParcela(indexViewModel, produtos);

                try
                {
                    Transaction pagarmeReposta = this._gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.cartaoCredito, parcela, enderecoEntrega, frete, produtos);
                    return new ContentResult() { Content = "Sucesso!" + pagarmeReposta.Id };
                }
                catch (PagarMeException e)
                {
                    TempData["MSG_E"] = MontarMensagemdeErro(e);
                    return Index();
                }
            }
            else
            {
                return Index();
            }
        }

        private string MontarMensagemdeErro(PagarMeException e)
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
           return sb.ToString();
        }

        private Parcelamento GerarParcela(IndexViewModel indexViewModel, List<ProdutoItem> produtos)
        {
            return this._gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(produtos)).Where(x => x.Numero == indexViewModel.parcelamento.Numero).FirstOrDefault();
        }

       
        public IActionResult BoletoBancario()
        {
            EnderecoEntrega enderecoEntrega = ObterEndereco();
            ValorPrazoFrete frete = ObterFrete();
            List<ProdutoItem> produtos = CarregarProdutoDB();
            var valorTotal = ObterValorTotalCompra( produtos);

            try
            {
                Transaction transaction = _gerenciarPagarMe.GerarBoleto(valorTotal);
                //return View("PedidoSucesso");
                return new ContentResult() { Content = "Sucesso!" + transaction.Id };
            }
            catch (PagarMeException e)
            {
                TempData["MSG_E"] = MontarMensagemdeErro(e);
                return RedirectToAction(nameof(Index));
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
        private ValorPrazoFrete ObterFrete()
        {

            var enderecoEntrega = ObterEndereco();
            int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));
            var tipoFreteSelecionadoPeloUsuario = this._cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(this._carrinhoCompra.Consultar());


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

        private List<SelectListItem> CalcularParcelamento(List<ProdutoItem> produtoItemCompleto, ValorPrazoFrete frete)
        {
            var total = ObterValorTotalCompra(produtoItemCompleto);
            var parcelamento = this._gerenciarPagarMe.CalcularPagamentoParcelado(total);



            return parcelamento.Select(x => new SelectListItem(
                    String.Format("{0} x {1} {2} - TOTAL: {3}", x.Numero, x.ValorPorParcela.ToString("C"), (x.Juros) ? "c/ juros" : "s/ juros", x.Valor.ToString("C")),
                    x.Numero.ToString()
                )
            ).ToList();

        }

        private decimal ObterValorTotalCompra(List<ProdutoItem> produtos)
        {
            var frete = ObterFrete();
            decimal total = Convert.ToDecimal(frete.Valor);

            foreach (var produto in produtos)
            {
                total += produto.Valor;
            }

            return total;
        }
    }
}
