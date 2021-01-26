using LojaVirtual_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.CarrinhoCompra
{
    public class CookieValorPrazoFrete
    {
        private string Key = "Carrinho.ValorPrazoFrete";
        private Cookie.Cookie _cookie;

        /*
       * CRUD - Cadastrar, Read, Update, Delete
       * Adicionar Item, remover Item, Alterar Quantidade
       */
        public CookieValorPrazoFrete(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Cadastrar(List<ValorPrazoFrete> lista)
        {
            var jsonstring = JsonConvert.SerializeObject(lista);
            _cookie.Cadastrar(Key, jsonstring);
        }

        public List<ValorPrazoFrete> Consultar()
        {
            if (this._cookie.Existe(Key))
            {
                string valor = this._cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<ValorPrazoFrete>>(valor);
            }
            else
            {
                return null;
            }
        }

        public void Remover()
        {
            this._cookie.Remover(Key);
        }
    }
}
