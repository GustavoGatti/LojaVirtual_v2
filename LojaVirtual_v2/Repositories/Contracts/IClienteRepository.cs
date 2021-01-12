﻿using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories
{
    public interface IClienteRepository
    {
        Cliente Login(string Email, string Senha);
        
        //CRUD
        void Cadastrar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Excluir(int Id);
        Cliente ObterCliente(int Id);
        List<Cliente> ObterTodosClientes();
    }
}
