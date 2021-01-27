using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Filtro
{

    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));
            Cliente cliente = this._loginCliente.GetCliente();
            if (cliente == null)
            {
                //TODO - implementar pagina html bonita para acesso negado
                context.Result = new ContentResult() { Content = "Acesso negado" };
            }
 
        }
    }
}
