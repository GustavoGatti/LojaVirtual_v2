using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories
{
    public class EnderecoEntregaRepository : IEnderecoEntregaRepository
    {
        private LojaVirtualContext _banco;

        public EnderecoEntregaRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        public void Atualizar(EnderecoEntrega endereco)
        {
            this._banco.EnderecosEntrega.Update(endereco);
            this._banco.SaveChanges();
        }

        public void Cadastrar(EnderecoEntrega endereco)
        {
            this._banco.EnderecosEntrega.Add(endereco);
            this._banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            EnderecoEntrega endereco = ObterEnderecoEntrega(Id);
            this._banco.Remove(endereco);
            this._banco.SaveChanges();
        }

        public EnderecoEntrega ObterEnderecoEntrega(int Id)
        {
            return this._banco.EnderecosEntrega.Find(Id);
        }

        public IList<EnderecoEntrega> ObterTodosEnderecoEntregaCliente(int ClienteId)
        {
            return this._banco.EnderecosEntrega.Where(x => x.ClienteId == ClienteId).ToList();
        }
    }
}
