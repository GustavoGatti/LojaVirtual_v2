using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories.Contracts
{
    public interface IEnderecoEntregaRepository
    {
        void Cadastrar(EnderecoEntrega endereco);
        void Atualizar(EnderecoEntrega endereco);
        void Excluir(int Id);
        EnderecoEntrega ObterEnderecoEntrega(int Id);
        IList<EnderecoEntrega> ObterTodosEnderecoEntregaCliente(int ClienteId);

    }
}
