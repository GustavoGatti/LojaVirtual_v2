using LojaVirtual_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Login
{
    public class LoginColaborador
    {

        private Sessao.Sessao _Sessao;
        private string Key = "Login.Colaborador";
        public LoginColaborador(Sessao.Sessao sessao)
        {
            this._Sessao = sessao;
        }

        public void Login(Colaborador colaborador)
        {
            //Serializar
            string colaboradorJsonString = JsonConvert.SerializeObject(colaborador);

            this._Sessao.Cadastrar(Key, colaboradorJsonString);
        }

        public Colaborador GetCliente()
        {
            //Deserializar
            if (this._Sessao.Existe(Key))
            {
                string colaboradorJsonString = this._Sessao.Consultar(Key);

                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJsonString);
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
