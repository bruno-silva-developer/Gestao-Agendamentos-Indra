using indra.Infra.Data;
using indra.Infra.Extensions;
using indra.Models;
using indra.Models.Enums;
using indra.Web.Bibliotecas;
using indra.Web.Models;
using indra.Web.Service.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace indra.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgendamentoDb _context;
        private readonly IEmailSender _emailSender;
        private LoginPessoaFisica _login;
        public AccountController(AgendamentoDb context, LoginPessoaFisica login, IEmailSender emailSender, IHostingEnvironment env)
        {
            _context = context;
            _login = login;
            _emailSender = emailSender;
        }

        public IActionResult LoginUser()
        {
            var logado = _login.GetUsuario() != null;

            if (logado)
            {
                if (_login.GetUsuario().PessoaFisica.Tipo == eTipo.Cliente)
                {
                    return RedirectToAction("TelaPrincipal", "Home");
                }
                else
                {
                    return RedirectToAction("Painel", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(string Email, string Senha)
        {
            try
            {
                Usuario usuario = _context.Usuarios.Where(p => p.Login == Email && p.Senha == Senha).Include(e => e.PessoaFisica).FirstOrDefault();
                if (usuario != null)
                {
                    var pessoa = _context.PessoasFisicas.Find(usuario.PessoaFisicaId);

                    if (pessoa.Ativo == true)
                    {
                        usuario.PessoaFisica = pessoa;
                        _login.Login(usuario);
                        if (usuario.PessoaFisica.Tipo == eTipo.Cliente)
                        {
                            return RedirectToAction("TelaPrincipal", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Painel", "Home");
                        }
                    }
                    else
                    {
                        throw new Exception("Usuário está inativo");
                    }
                }
                else
                {
                    throw new Exception("Email ou senha inválidos, tente novamente.");
                }
            }
            catch (Exception e)
            {
                TempData["E_LOGIN"] = e.Message;
                return View();
            }
        }

        public IActionResult RegistrarCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCliente(PessoaFisica pessoa)
        {

            ViewBag.error = null;
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == pessoa.Email).Count() > 0)
                {
                    throw new Exception("Já existe um cliente criado com esse Email");
                }
                else if (_context.PessoasFisicas.Where(e => e.Cpf == pessoa.Cpf).Count() > 0)
                {
                    throw new Exception("Já existe uma cliente criado com esse Cpf");
                }
                else
                {
                    var usuario = new Usuario();
                    usuario.Login = pessoa.Email;
                    usuario.Senha = pessoa.Senha;
                    pessoa.DtCriacao = DateTime.Now;
                    pessoa.DtAlteracao = DateTime.Now;
                    pessoa.Ativo = true;
                    pessoa.Tipo = eTipo.Cliente;
                    usuario.PessoaFisica = pessoa;

                    //ATRIBUIÇÃO DE DADOS DE PESSOA EM USUÁRIO
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();

                    TempData["S_REGISTER"] = "Cliente " + pessoa.Nome + " criado.";
                    return RedirectToAction("LoginUser", "Account");

                }
            }
            catch (Exception e)
            {
                TempData["E_REGISTER"] = e.Message;
                return RedirectToAction("LoginUser", "Account");
            }
        }

        public IActionResult RegistrarProfissional()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarProfissional(PessoaFisica pessoa)
        {
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == pessoa.Email).Count() > 0)
                {
                    throw new Exception("Já existe um profissional criado com esse Email");
                }
                else if (_context.PessoasFisicas.Where(e => e.Cpf == pessoa.Cpf).Count() > 0)
                {
                    throw new Exception("Já existe uma profissional criado com esse Cpf");
                }
                else
                {
                    var usuario = new Usuario();
                    usuario.Login = pessoa.Email;
                    usuario.Senha = pessoa.Senha;
                    pessoa.DtCriacao = DateTime.Now;
                    pessoa.DtAlteracao = DateTime.Now;
                    pessoa.Ativo = true;
                    pessoa.Tipo = eTipo.Profissional;
                    usuario.PessoaFisica = pessoa;

                    //ATRIBUIÇÃO DE DADOS DE PESSOA EM USUÁRIO
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    TempData["S_REGISTER"] = "Profissional " + pessoa.Nome + " criado.";
                    return RedirectToAction("LoginUser", "Account");

                }
            }
            catch (Exception e)
            {
                TempData["E_REGISTER"] = e.Message;
                return RedirectToAction("LoginUser", "Account");
            }
        }

        public IActionResult RecuperarConta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarConta(string Cpf)
        {
            PessoaFisica pessoa = _context.PessoasFisicas.Where(p => p.Cpf == Cpf).FirstOrDefault();
            if (pessoa != null)
            {
                Usuario user = _context.Usuarios.Where(e => e.PessoaFisicaId == pessoa.Id).FirstOrDefault();
                _login.Login(user);
                return RedirectToAction("Painel", "Home");
            }

            TempData["E_RECUPERAR"] = "Usuário não existe, tente novamente.";

            return RedirectToAction("LoginUser", "Account");
        }

        [AllowAnonymous]
        public ActionResult EnviarEmailProfissional()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarEmailProfissional(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var protocolo = HttpContext.Request.Scheme;
                    var dominio = HttpContext.Request.Host.Host;
                    var porta = HttpContext.Request.Host.Port;
                    var callbackUrl = string.Format("{0}://{1}:{2}{3}", protocolo, dominio, porta, Url.Action("RegistrarProfissional", "Account"));
                    EnvioEmail(model.Email, "Registro de Profissional", $"Registre-se como profissional clicando em <a href=\"{callbackUrl}\">AQUI</a>");
                    TempData["S_EMAIL"] = "Email Enviado";
                    return View();
                }
                catch (Exception)
                {
                    TempData["E_EMAIL"] = "Email não enviado";
                    return View();
                }
            }
            return View(model);
        }

        public void EnvioEmail(string email, string assunto, string mensagem)
        {
            try
            {
                //email destino, assunto e mensagem
                _emailSender.EnviaEmailAsync(email, assunto, mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Autorizacao]
        [ValidadeHttp]
        public IActionResult Logout()
        {
            _login.Logout();
            return RedirectToAction("LoginUser", "Account");
        }


    }
}
