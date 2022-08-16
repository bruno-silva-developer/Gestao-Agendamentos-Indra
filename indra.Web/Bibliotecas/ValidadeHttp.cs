using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Bibliotecas
{
    public class ValidadeHttp : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // EXECUTADO ANTES DE PASSAR PELO CONTROLADOR
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                context.Result = new ContentResult() { Content = "Acesso Negado!" };
            }
            else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServidor = context.HttpContext.Request.Host.Host;

                if (hostReferer != hostServidor)
                {
                    context.Result = new ContentResult() { Content = "Acesso Negado!" };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // EXECUTADO APOS PASSAR PELO CONTROLADOR
        }
    }
}
