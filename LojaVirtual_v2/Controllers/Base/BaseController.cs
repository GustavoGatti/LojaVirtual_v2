using AutoMapper;
using LojaVirtual_v2.Libraries.CarrinhoCompra;
using LojaVirtual_v2.Libraries.Gerenciador.Frete;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Libraries.Seguranca;
using LojaVirtual_v2.Models.ProdutoAgregador;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers.Base
{
    public class BaseController : Controller
    {

        protected CookieCarrinhoCompra _carrinhoCompra;
        protected CookieFrete _cookieFrete;
        protected IProdutoRepository _produtoRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _wsCorreios;
        protected CalcularPacote calcularPacote;
        protected IEnderecoEntregaRepository _enderecoEntrega;
        protected LoginCliente _login;

        public BaseController(CookieCarrinhoCompra carrinhoCompra,
            IProdutoRepository produtoRepository, IMapper mapper,
            WSCorreiosCalcularFrete wSCorreios, CalcularPacote calcular,
            CookieFrete cookieValorPrazoFrete, IEnderecoEntregaRepository enderecoEntrega
            , LoginCliente login)
        {
            this._mapper = mapper;
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            this._wsCorreios = wSCorreios;
            this.CalcularPacote = calcular;
            this._cookieFrete = cookieValorPrazoFrete;
            this._enderecoEntrega = enderecoEntrega;
            this._login = login;
        }

        protected CalcularPacote CalcularPacote { get => calcularPacote; set => calcularPacote = value; }


        protected List<ProdutoItem> CarregarProdutoDB()
        {
            List<ProdutoItem> produtoItemsNoCarrinho = this._carrinhoCompra.Consultar();
            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemsNoCarrinho)
            {
                //AutoMapper

                Produto produto = this._produtoRepository.ObterProduto(item.Id);
                ProdutoItem produtoItem = this._mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;
                produtoItemCompleto.Add(produtoItem);
            }

            return produtoItemCompleto;
        }
    
        protected string GerarHash(object objeto)
        {
            return StringMD5.MD5Hash(JsonConvert.SerializeObject(objeto));
        }

    }

}
