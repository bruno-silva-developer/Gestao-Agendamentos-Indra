using indra.Models;
using indra.Web.Admin.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Admin.Bibliotecas
{
    public class LoginAdmin
    {
        private string Key = "Login.PessoaFisica";
        private Sessao _sessao;

        public LoginAdmin(Sessao sessao) 
        {
            _sessao = sessao;
        }

        public void Login(Usuario admin)
        {
            //ARMAZENAR SESSAO
            //SERIALIZAR
            string adminJsonString = JsonConvert.SerializeObject(admin);
            _sessao.Cadastrar(Key, adminJsonString);
        }

        public Usuario GetAdmin()
        {

            if (_sessao.Existe(Key))
            {
                //DESSERIALIZAR
                string adminJsonString = _sessao.Consultar(Key);

                return JsonConvert.DeserializeObject<Usuario>(adminJsonString);
            }
            return null;
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
