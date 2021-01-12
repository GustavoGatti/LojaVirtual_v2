﻿using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories.Contracts
{
    public interface IColaboradorRepository
    {
        void Cadastrar(Colaborador colaborador);

        void Atualizar(Colaborador colaborador);
        void Excluir(int Id);
        Colaborador Login(string Email, string Senha);
        List<Colaborador> ObterTodosColaborador();
        Colaborador ObterColaborador(int Id);
    }
}

