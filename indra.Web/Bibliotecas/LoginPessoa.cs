using indra.Models;
using indra.Web.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Bibliotecas
{
    public class LoginPessoaFisica
    {
        private string Key = "Login.PessoaFisica";
        private Sessao _sessao;

        public LoginPessoaFisica(Sessao sessao) 
        {
            _sessao = sessao;
        }

        public void Login(Usuario usuario)
        {
            //ARMAZENAR SESSAO
            //SERIALIZAR
            string userJsonString = JsonConvert.SerializeObject(usuario);
            _sessao.Cadastrar(Key, userJsonString);
        }

        public Usuario GetUsuario()
        {

            if (_sessao.Existe(Key))
            {
                //DESSERIALIZAR
                string usuarioJsonString = _sessao.Consultar(Key);

                return JsonConvert.DeserializeObject<Usuario>(usuarioJsonString);
            }
            return null;
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
