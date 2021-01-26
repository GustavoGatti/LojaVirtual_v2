using AutoMapper;
using LojaVirtual_v2.Controllers.Base;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
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
        public PagamentoController(CookieCarrinhoCompra carrinhoCompra,
       IProdutoRepository produtoRepository, IMapper mapper,
       WSCorreiosCalcularFrete wSCorreios, CalcularPacote calcular,
       CookieValorPrazoFrete cookieValorPrazoFrete) : base(carrinhoCompra, produtoRepository, mapper, wSCorreios, calcular, cookieValorPrazoFrete)
        {

        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
            return View(produtoItemCompleto);
        }

        
    }
}
