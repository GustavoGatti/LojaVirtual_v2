using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
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
            return this._banco.Produtos.Include(x => x.Imagems).Where(x => x.Id == id).FirstOrDefault();
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa)
        {
            var bancoProduto = _banco.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {

                bancoProduto = bancoProduto.Where(x => x.Nome.Contains(pesquisa.Trim()));
            }
            int NumeroPagina = pagina ?? 1;
            return bancoProduto.Include(x => x.Imagems).ToPagedList<Produto>(NumeroPagina, this._conf.GetValue<int>("RegistroPorPagina"));
        }
    }
}
