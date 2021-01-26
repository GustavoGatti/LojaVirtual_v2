using LojaVirtual_v2.Models.ProdutoAgregador;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.CarrinhoCompra
{
    public class CookieCarrinhoCompra
    {
        private string Key = "Carrinho.Comprar";
        private Cookie.Cookie _cookie;
        /*
       * CRUD - Cadastrar, Read, Update, Delete
       * Adicionar Item, remover Item, Alterar Quantidade
       */

        public CookieCarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

      
        public void Cadastrar(ProdutoItem item)
        {
            List<ProdutoItem> Lista;
            if (this._cookie.Existe(Key))
            {
                //Ler - Adicionar o item no carrinho Existente
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(X => X.Id == item.Id);

                if (ItemLocalizado == null)
                {
                    Lista.Add(item);
                    //Salvar lista
                }
                else
                {
                    ItemLocalizado.QuantidadeProdutoCarrinho = ItemLocalizado.QuantidadeProdutoCarrinho + 1;
                    //Adicionar mais um na qtd do item
                }
            
            }
            else
            {
                Lista = new List<ProdutoItem>();
                Lista.Add(item);
                //Criar o cookie com o item no Carrinho
                
            }
            //Salvar
            Salvar(Lista);
        }

        private void Salvar(List<ProdutoItem> lista)
        {
            string valor = JsonConvert.SerializeObject(lista);
            this._cookie.Cadastrar(Key, valor);
        }

        public void Remover(ProdutoItem item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(X => X.Id == item.Id);

            if(ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                //=Salvar lista
                Salvar(Lista);
            }
        }

        public void Atualizar(ProdutoItem item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(X => X.Id == item.Id);

            if(ItemLocalizado != null)
            {
                ItemLocalizado.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;
                Salvar(Lista);
            }
        }

        public List<ProdutoItem> Consultar()
        {
            if (this._cookie.Existe(Key))
            {
                string valor = this._cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<ProdutoItem>>(valor);
            }
            else
            {
                return new List<ProdutoItem>();
            }
        }

        public bool Existe(string Key)
        {
            if (this._cookie.Existe(Key))
            {
                return false;
            }
            return true;
        }
        public void RemoverTodos()
        {
            this._cookie.Remover(Key);
        }
    }
}
