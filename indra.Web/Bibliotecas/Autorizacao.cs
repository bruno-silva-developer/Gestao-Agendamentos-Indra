using indra.Models;
using indra.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Bibliotecas
{
    public class Autorizacao : Attribute, IAuthorizationFilter
    {
        private eTipo _tipo;
        public Autorizacao(eTipo tipo = eTipo.Cliente)
        {
            _tipo = tipo;
        }

        LoginPessoaFisica _login;

        // Autoriza se o usuario tem acesso como profissional
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _login = (LoginPessoaFisica)context.HttpContext.RequestServices.GetService(typeof(LoginPessoaFisica));

            Usuario usuario = _login.GetUsuario();

            if (usuario == null)
            {
                context.Result = new RedirectToActionResult("LoginUser", "Account", null);
            }
            else
            {
                if (usuario.PessoaFisica.Tipo == eTipo.Cliente && _tipo == eTipo.Profissional)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
