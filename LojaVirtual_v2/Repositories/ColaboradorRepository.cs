﻿using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.Constants;
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
    public class ColaboradorRepository : IColaboradorRepository
    {
        private LojaVirtualContext _banco;
        private IConfiguration _conf;

        public ColaboradorRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            this._conf = configuration;
            this._banco = banco;
        }

        public void Atualizar(Colaborador colaborador)
        {
            this._banco.Colaboradores.Update(colaborador);
            this._banco.Entry(colaborador).Property(a => a.Senha).IsModified = false;
            this._banco.SaveChanges();
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            this._banco.Colaboradores.Update(colaborador);
            this._banco.Entry(colaborador).Property(a => a.Nome).IsModified = false;
            this._banco.Entry(colaborador).Property(a => a.Email).IsModified = false;
            this._banco.Entry(colaborador).Property(a => a.Tipo).IsModified = false;
            this._banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            this._banco.Colaboradores.Add(colaborador);
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
            return this._banco.Colaboradores.Where(a => a.Tipo != ColaboradorTipoConstante.Gerente).ToList();
        }

        public Colaborador ObterColaborador(int Id)
        {
            return this._banco.Colaboradores.Find(Id);
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return this._banco.Colaboradores.Where(a => a.Tipo != ColaboradorTipoConstante.Gerente)
                .ToPagedList<Colaborador>(NumeroPagina, this._conf.GetValue<int>("RegistroPorPagina"));
        }

        public List<Colaborador> ObterColaboradorPorEmail(string Email)
        {
            return this._banco.Colaboradores.Where(x => x.Email == Email).AsNoTracking().ToList();
        }
    }
}
