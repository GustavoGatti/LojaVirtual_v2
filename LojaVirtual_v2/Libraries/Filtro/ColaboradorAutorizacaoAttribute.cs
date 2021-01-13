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
    public class ColaboradorAutorizacaoAttribute: Attribute, IAuthorizationFilter
    {
        LoginColaborador _loginColaborador;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginColaborador = (LoginColaborador)context.HttpContext.RequestServices.GetService(typeof(LoginColaborador));
            Colaborador colaborador = this._loginColaborador.GetCliente();
            if (colaborador == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }

        }
    }
}
