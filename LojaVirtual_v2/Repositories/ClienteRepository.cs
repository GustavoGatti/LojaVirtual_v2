using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public LojaVirtualContext _banco;
        private IConfiguration _conf;
        public ClienteRepository(LojaVirtualContext banco, IConfiguration conf)
        {
            this._conf = conf;
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

        public IPagedList<Cliente> ObterTodosClientes(int? pagina, string pesquisa)
        {
            var bancoCliente = _banco.cliente.AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                //ñ vazio
                bancoCliente = bancoCliente.Where(x => x.Nome.Contains(pesquisa.Trim()) || x.Email.Contains(pesquisa.Trim()));
            }
            int NumeroPagina = pagina ?? 1;
            return bancoCliente.ToPagedList<Cliente>(NumeroPagina, this._conf.GetValue<int>("RegistroPorPagina"));
            
        }
    }
}
