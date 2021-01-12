using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private LojaVirtualContext _banco;

        public ColaboradorRepository(LojaVirtualContext banco)
        {
            this._banco = banco;
        }

        public void Atualizar(Colaborador colaborador)
        {
            this._banco.Colaboradores.Add(colaborador);
            this._banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            this._banco.Colaboradores.Update(colaborador);
            this._banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Colaborador colaborador = ObterColaborador(Id);
            this._banco.Colaboradores.Remove(colaborador);
            this._banco.SaveChanges();
        }

        public Colaborador Login(string Email, string Senha)
        {
            Colaborador colaborador = this._banco.Colaboradores.Where(m => m.Email == Email && m.Senha == Senha).FirstOrDefault();
            return colaborador;
        }

        public List<Colaborador> ObterTodosColaborador()
        {
            return this._banco.Colaboradores.ToList();
        }

        public Colaborador ObterColaborador(int Id)
        {
            return this._banco.Colaboradores.Find(Id);
        }
    }
}
