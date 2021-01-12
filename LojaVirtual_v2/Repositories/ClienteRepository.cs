using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public LojaVirtualContext _banco;
        public ClienteRepository(LojaVirtualContext banco)
        {
            this._banco = banco;
        }


        public void Atualizar(Cliente cliente)
        {
            this._banco.cliente.Update(cliente);
            this._banco.SaveChanges();
        }

        public void Cadastrar(Cliente cliente)
        {
            this._banco.cliente.Add(cliente);
            this._banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Cliente cliente = ObterCliente(Id);
            _banco.Remove(cliente);
            this._banco.SaveChanges();
        }

        public Cliente Login(string Email, string Senha)
        {
            Cliente cliente = this._banco.cliente.Where(m => m.Email == Email && m.Senha == Senha).FirstOrDefault();
            return cliente;
        }

        public Cliente ObterCliente(int Id)
        {
            return (this._banco.cliente.Find(Id));
        }

        public List<Cliente> ObterTodosClientes()
        {

            return (this._banco.cliente.ToList());
        }
    }
}
