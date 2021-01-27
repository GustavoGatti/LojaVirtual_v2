using LojaVirtual_v2.Libraries.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _httpContext;
        private IConfiguration _configuration;

        public Cookie(IHttpContextAccessor httpContext, IConfiguration configuration)
        {
            this._httpContext = httpContext;
            this._configuration = configuration;
        }

        public void Cadastrar(string Key, string Valor)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(7);
            Options.IsEssential = true;

            //Criptografar

            var ValorCrypt = StringCipher.Encrypt(Valor, this._configuration.GetValue<string>("KeyCrypt"));

            _httpContext.HttpContext.Response.Cookies.Append(Key, ValorCrypt, Options);
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

        public string Consultar(string Key, bool Cript = true)
        {
            var valor = this._httpContext.HttpContext.Request.Cookies[Key];
            if (Cript)
            {
                valor = StringCipher.Decrypt(valor, this._configuration.GetValue<string>("KeyCrypt"));
            }
            
            return valor;
                
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
