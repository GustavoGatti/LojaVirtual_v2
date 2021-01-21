using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _httpContext;

        public Cookie(IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
        }

        public void Cadastrar(string Key, string Valor)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(7);
            _httpContext.HttpContext.Response.Cookies.Append(Key, Valor, Options);
        }

        public void Atualizar(string Key, string Valor)
        {
            if (Existe(Key))
            {
                Remover(Key);
            }
            Cadastrar(Key, Valor);
        }

        public void Remover(string Key)
        {
            this._httpContext.HttpContext.Response.Cookies.Delete(Key);
        }

        public string Consultar(string Key)
        {
            return this._httpContext.HttpContext.Request.Cookies[Key];
        }

        public bool Existe(string Key)
        {
            if (this._httpContext.HttpContext.Request.Cookies[Key] == null)
            {
                return false;
            }
            return true;
        }

        public void RemoverTodos()
        {
            var ListaCookie = this._httpContext.HttpContext.Request.Cookies.ToList();
            foreach (var cookie in ListaCookie)
            {
                Remover(cookie.Key);
            }
        }

    }
}
