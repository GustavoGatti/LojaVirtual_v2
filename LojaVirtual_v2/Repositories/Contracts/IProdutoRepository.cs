using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories.Contracts
{
    public interface IProdutoRepository
    {
        void Cadastrar(Produto produto);
        void Atualizar(Produto produto);
        void Excluir(int id);
        Produto ObterProduto(int id);
        IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa, string ordenacao, IEnumerable<Categoria> categorias);
        IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa);
    }
}
