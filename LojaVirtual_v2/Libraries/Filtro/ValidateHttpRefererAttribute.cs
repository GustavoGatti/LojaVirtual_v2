﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Filtro
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Executa antes de passar pelo controlador
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                context.Result = new ContentResult() { Content = "Acesso negado!"};
            }
            else
            {
                Uri uri = new Uri(referer);
                string hostReferer = uri.Host;
                string hostServidor = context.HttpContext.Request.Host.Host;
                if(hostReferer != hostServidor)
                {
                    context.Result = new ContentResult() { Content = "Acesso negado"};
                }

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Executa apos passar pelo controlador
        }
    }
}
