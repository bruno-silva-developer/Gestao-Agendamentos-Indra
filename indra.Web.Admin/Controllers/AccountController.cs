using indra.Infra.Data;
using indra.Models;
using indra.Models.Enums;
using indra.Web.Admin.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgendamentoDb _context;
        private LoginAdmin _login;

        public AccountController(AgendamentoDb context, LoginAdmin login)
        {
            _context = context;
            _login = login;
        }

        public IActionResult Login()
        {
            var logado = _login.GetAdmin() != null;
            if (logado)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Senha)
        {
            try
            {
                Usuario admin = _context.Usuarios.Where(p => p.Login == Email && p.Senha == Senha).FirstOrDefault();
                if (admin != null)
                {
                    var pessoa = _context.PessoasFisicas.Find(admin.PessoaFisicaId);
                    if (pessoa.Ativo == true)
                    {
                        if (pessoa.Tipo == eTipo.Administrador)
                        {
                            _login.Login(admin);
                            return RedirectToAction("Dashboard", "Home");
                        }
                        else
                        {
                            throw new Exception("Esse usuário não é administrador");
                        }
                    }
                    else
                    {
                        throw new Exception("Esse usuário está inativo");
                    }
                }
                else
                {
                    throw new Exception("Email ou senha inválidos");
                }
            }
            catch (Exception e)
            {
                TempData["E_LOGIN"] = e.Message;
                return View();
            }
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(PessoaFisica admin)
        {
            ViewBag.error = null;
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == admin.Email).Count() > 0)
                {
                    throw new Exception("Já existe um admin criado com esse Email");
                }
                else if (_context.PessoasFisicas.Where(e => e.Email == admin.Email).Count() > 0)
                {
                    throw new Exception("Já existe um admin criado com esse CPF");
                }
                else
                {
                    var usuario = new Usuario();
                    usuario.Login = admin.Email;
                    usuario.Senha = admin.Senha;
                    admin.DtCriacao = DateTime.Now;
                    admin.DtAlteracao = DateTime.Now;
                    admin.Tipo = eTipo.Administrador;
                    admin.Ativo = true;
                    usuario.PessoaFisica = admin;

                    //ATRIBUIÇÃO DE DADOS DE PESSOA EM USUÁRIO
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    TempData["S_REGISTER"] = "Administrador " + admin.Nome + " criado.";
                    return RedirectToAction("Login", "Account");

                }
            }
            catch (Exception e)
            {
                TempData["E_REGISTER"] = e.Message;
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult RecuperarConta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarConta(string Cpf)
        {
            try
            {
                PessoaFisica admin = _context.PessoasFisicas.Where(p => p.Cpf == Cpf).FirstOrDefault();
                if (admin != null)
                {
                    Usuario user = _context.Usuarios.Where(e => e.PessoaFisicaId == admin.Id).FirstOrDefault();
                    _login.Login(user);
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    throw new Exception("Usuário não existe, tente novamente");
                }
            }
            catch (Exception e)
            {
                TempData["E_RECUPERAR"] = e.Message;
                return RedirectToAction("Login", "Account");
            }
        }

        [ValidadeHttp]
        public IActionResult Logout()
        {
            _login.Logout();
            return RedirectToAction("Login", "Account");
        }
    }
}
