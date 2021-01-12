using LojaVirtual_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Login
{
    public class LoginCliente
    {
        private Sessao.Sessao _Sessao;
        private string Key = "Login.Cliente";
        public LoginCliente(Sessao.Sessao sessao)
        {
            this._Sessao = sessao;
        }

        public void Login(Cliente cliente)
        {
            //Serializar
            string clienteJsonString = JsonConvert.SerializeObject(cliente);

            this._Sessao.Cadastrar(Key, clienteJsonString);
        }

        public Cliente GetCliente()
        {
            //Deserializar
            if (this._Sessao.Existe(Key))
            {
                string clienteJsonString = this._Sessao.Consultar(Key);

                return JsonConvert.DeserializeObject<Cliente>(clienteJsonString);
            }
            else
            {
                return null;
            }
            
        }

        public void Logout()
        {
            this._Sessao.RemoverTodos();
        }
    }
}
