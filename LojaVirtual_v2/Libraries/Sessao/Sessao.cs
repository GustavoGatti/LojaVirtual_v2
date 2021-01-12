using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Sessao
{
    public class Sessao
    {
        private IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
        }

        public void Cadastrar(string Key, string Valor)
        {
            this._httpContext.HttpContext.Session.SetString(Key, Valor);
        }

        public void Atualizar(string Key, string Valor)
        {
            if (Existe(Key))
            {
                this._httpContext.HttpContext.Session.Remove(Key);
            }
            this._httpContext.HttpContext.Session.SetString(Key, Valor);
        }

        public void Remover(string Key)
        {
            this._httpContext.HttpContext.Session.Remove(Key);
        }

        public string Consultar(string Key)
        {
            return this._httpContext.HttpContext.Session.GetString(Key);
        }

        public bool Existe(string Key)
        {
            if (this._httpContext.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }
            return true;
        }

        public void RemoverTodos()
        {
            this._httpContext.HttpContext.Session.Clear();
        }
    }
}
