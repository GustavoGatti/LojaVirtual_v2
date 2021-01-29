using LojaVirtual_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.CarrinhoCompra
{
    public class CookieFrete
    {
        private string Key = "Carrinho.ValorFrete";
        private Cookie.Cookie _cookie;
        /*
       * CRUD - Cadastrar, Read, Update, Delete
       * Adicionar Item, remover Item, Alterar Quantidade
       */

        public CookieFrete(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }


        public void Cadastrar(Frete item)
        {
            List<Frete> Lista;
            if (this._cookie.Existe(Key))
            {
                //Ler - Adicionar o item no carrinho Existente
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(X => X.CEP == item.CEP);

                if (ItemLocalizado == null)
                {
                    Lista.Add(item);
                    //Salvar lista
                }
                else
                {
                    ItemLocalizado.CodCarrinho = item.CodCarrinho;
                    ItemLocalizado.ListaValores = item.ListaValores;
                    //Adicionar mais um na qtd do item
                }

            }
            else
            {
                Lista = new List<Frete>();
                Lista.Add(item);
                //Criar o cookie com o item no Carrinho

            }
            //Salvar
            Salvar(Lista);
        }

        private void Salvar(List<Frete> lista)
        {
            string valor = JsonConvert.SerializeObject(lista);
            this._cookie.Cadastrar(Key, valor);
        }

        public void Remover(Frete item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(X => X.CEP == item.CEP);

            if (ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                //=Salvar lista
                Salvar(Lista);
            }
        }

        public void Atualizar(Frete item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(X => X.CEP == item.CEP);

            if (ItemLocalizado != null)
            {

                ItemLocalizado.CodCarrinho = item.CodCarrinho;
                ItemLocalizado.ListaValores = item.ListaValores;
                Salvar(Lista);
            }
        }

        public List<Frete> Consultar()
        {
            if (this._cookie.Existe(Key))
            {
                string valor = this._cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Frete>>(valor);
            }
            else
            {
                return new List<Frete>();
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
