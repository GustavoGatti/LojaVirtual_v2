using AutoMapper;
using LojaVirtual_v2.Controllers.Base;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Cookie;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Libraries.Texto;
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
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;

        public PagamentoController(CookieCarrinhoCompra carrinhoCompra,
        IProdutoRepository produtoRepository, IMapper mapper,
        WSCorreiosCalcularFrete wSCorreios, CalcularPacote calcular,
        CookieFrete cookieValorPrazoFrete, Cookie cookie, IEnderecoEntregaRepository enderecoEntrega,
        LoginCliente loginCliente)
        : base(carrinhoCompra, produtoRepository, mapper, wSCorreios, calcular, cookieValorPrazoFrete, enderecoEntrega, loginCliente)
        {
            this._cookie = cookie;
        }

        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = this._cookie.Consultar("Carrinho.TipoFrete", false);

            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntrega = ObterEndereco();
                var carrinhoHash = GerarHash(this._carrinhoCompra.Consultar());

                int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));

                ViewBag.Frete = ObterFrete(cep.ToString());
                List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                ViewBag.Produtos = produtoItemCompleto;
                return View(nameof(Index));

            }
            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
        }

        [HttpPost]
        [ClienteAutorizacao]
        public IActionResult Index([FromForm] CartaoCredito cartaoCredito)
        {
            if (ModelState.IsValid)
            {
                //TODO - Integrar os dados, Salvar os dados e redirecionar para a tela de pedido concluido
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete(enderecoEntrega.CEP.ToString());
                List<ProdutoItem> produtos = CarregarProdutoDB();

                return View();
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
                enderecoEntrega.Estado = cliente.Estado;
                enderecoEntrega.Cidade = cliente.Cidade;
                enderecoEntrega.Bairro = cliente.Bairro;
                enderecoEntrega.Endereco = cliente.Endereco;
                enderecoEntrega.Complemento = cliente.Complemento;
                enderecoEntrega.Numero = cliente.Numero;
            }
            else
            {
                var Endereco = this._enderecoEntrega.ObterEnderecoEntrega(EnderecoEntregaId);
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
    }
}
