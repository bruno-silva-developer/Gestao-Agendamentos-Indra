using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Service.Interface
{
    public interface IEmailSender
    {
        Task EnviaEmailAsync(string email, string assunto, string mensagem);
    }
}
