using LojaVirtual_v2.Libraries.Lang;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Filtro
{
    public class ValidateCookiePagamentoController : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var _cookie = (Cookie.Cookie)context.HttpContext.RequestServices.GetService(typeof(Cookie.Cookie));
            var tipoFrete = _cookie.Consultar("Carrinho.TipoFrete", false);
            var valorFrete = _cookie.Consultar("Carrinho.ValorFrete", true);
            var carinhoCompra = _cookie.Consultar("Carrinho.Comprar", true);

            if (carinhoCompra == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Mensagem.MSG_E010;
                context.Result = new RedirectToActionResult("Index", "CarrinhoCompra", null);
            }

            if (tipoFrete == null || valorFrete == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Mensagem.MSG_E009;
                context.Result = new RedirectToActionResult("EnderecoEntrega", "CarrinhoCompra", null);
            }
        }
    }
}
