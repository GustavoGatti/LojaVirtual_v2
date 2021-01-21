using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.ProdutoAgregador;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {

        private LojaVirtualContext _banco;
        private IConfiguration _conf;

        public ProdutoRepository(LojaVirtualContext banco, IConfiguration conf)
        {
            _banco = banco;
            _conf = conf;
        }

        public void Atualizar(Produto produto)
        {
            this._banco.Update(produto);
            this._banco.SaveChanges();
        }

        public void Cadastrar(Produto produto)
        {
            this._banco.Add(produto);
            this._banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            Produto produto = this.ObterProduto(id);
            this._banco.Remove(produto);
            this._banco.SaveChanges();
        }

        public Produto ObterProduto(int id)
        {
            return this._banco.Produtos.Include(x => x.Imagems).OrderBy(x => x.Nome).Where(x => x.Id == id).FirstOrDefault();
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa)
        {

            return ObterTodosProdutos(pagina, pesquisa, "A", null);
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa, string ordenacao, IEnumerable<Categoria> categorias)
        {
           

            int RegistroPorPagina = this._conf.GetValue<int>("RegistroPorPagina");
            int NumeroPagina = pagina ?? 1;
            var bancoProduto = _banco.Produtos.AsQueryable();
            if (!string.IsNullOrEmpty(pesquisa))
            {

                bancoProduto = bancoProduto.Where(x => x.Nome.Contains(pesquisa.Trim()));
            }
            if(ordenacao == "A")
            {
                bancoProduto = bancoProduto.OrderBy(a => a.Nome);
            }
            if (ordenacao == "MA")
            {
                bancoProduto = bancoProduto.OrderByDescending(a => a.Valor);
            }
            if (ordenacao == "ME")
            {
                bancoProduto = bancoProduto.OrderBy(a => a.Valor);
            }

            if(categorias != null && categorias.Count() > 0)
            {

                //1 - Informatica, 5 - Teclado, 
                //SQL: where categoria in (1,5,....), sql da parte de baixo
                bancoProduto = bancoProduto.Where(x => categorias.Select(y => y.ID).Contains(x.CategoriaId));
            }

            return bancoProduto.Include(x => x.Imagems).ToPagedList<Produto>(NumeroPagina, RegistroPorPagina);
        }


        
    }
}
