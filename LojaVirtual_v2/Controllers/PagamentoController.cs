using AutoMapper;
using LojaVirtual_v2.Controllers.Base;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Cookie;
using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
using LojaVirtual_v2.Libraries.Lang;
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
       CookieValorPrazoFrete cookieValorPrazoFrete, Cookie cookie) : base(carrinhoCompra, produtoRepository, mapper, wSCorreios, calcular, cookieValorPrazoFrete)
        {
            this._cookie = cookie;
        }

        [ClienteAutorizacao]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = this._cookie.Consultar("Carrinho.TipoFrete", false);
            var frete = this._cookieValorPrazo.Consultar().Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                if (frete != null)
                {
                    ViewBag.Frete = frete;
                    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                    return View(produtoItemCompleto);
                }
            }

            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction(nameof(Index));

        }
    }
}
