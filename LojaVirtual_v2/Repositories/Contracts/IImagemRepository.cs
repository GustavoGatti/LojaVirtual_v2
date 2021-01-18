using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories.Contracts
{
    public interface IImagemRepository
    {
        void Cadastrar(Imagem imagem);
        void Excluir(int id);
        void ExcluirImagemDoProduto(int ProdutoId);

        void CadastrarImagens(List<Imagem> CaminhoImagens, int id);
    }
}
