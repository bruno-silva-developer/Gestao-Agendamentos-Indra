using indra.Infra.Data;
using indra.Models;
using indra.Web.Bibliotecas;
using indra.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AgendamentoDb _context;
        private readonly LoginPessoaFisica _login;

        public HomeController(AgendamentoDb context, LoginPessoaFisica login)
        {
            _context = context;
            _login = login;
        }

        // Index serve para apresentar os dados na tela e retorna a View de Index feita no ASP
        public IActionResult TelaPrincipal()
        {
            var user = _login.GetUsuario();
            var agendamentos = _context.Agendamentos.Where(e => e.SituacaoAgendamentoId == 1 && e.ClienteId == user.PessoaFisicaId).Include(e => e.Profissional).Include(e => e.Cliente).Include(e => e.Servico).Include(e => e.SituacaoAgendamento).OrderBy(e => e.Horario);
            if (agendamentos == null)
            {
                TempData["SEM_AGENDAMENTO_AGUARDANDO"] = "Não existe nenhum agendamento aguardando";
                return View(agendamentos.ToList());
            }
            return View(agendamentos.ToList());
        }

        public IActionResult Painel()
        {
            var user = _login.GetUsuario();
            var agendamentos = _context.Agendamentos.Where(e => e.SituacaoAgendamentoId == 1 && e.ProfissionalId == user.PessoaFisicaId).Include(e => e.Profissional).Include(e => e.Cliente).Include(e => e.Servico).Include(e => e.SituacaoAgendamento).OrderBy(e => e.Horario);
            if (agendamentos == null)
            {
                TempData["SEM_AGENDAMENTO_AGUARDANDO"] = "Não existe nenhum agendamento aguardando";
                return View(agendamentos.ToList());
            }
            return View(agendamentos.ToList());
        }

        public IActionResult Perfil()
        {
            var user = _login.GetUsuario();
            var dadosPerfil = _context.PessoasFisicas.Find(user.PessoaFisicaId);
            ViewBag.Tipo = dadosPerfil.Tipo;
            ViewBag.Nome = dadosPerfil.Nome;
            ViewBag.Email = dadosPerfil.Email;
            ViewBag.DtNascimento = dadosPerfil.DtNascimento.ToString("dd/MM/yyyy");
            ViewBag.Sexo = dadosPerfil.Sexo;
            ViewBag.Cpf = dadosPerfil.Cpf;
            ViewBag.Celular = dadosPerfil.Celular;
            return View();
        }

        public IActionResult AtualizarPerfil()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AtualizarPerfil(PessoaFisica pessoa)
        {
            try
            {
                //Aqui será feito também a atualização do login e senha do usuário caso o email e senha seja mudado
                Usuario user = _context.Usuarios.Where(e => e.PessoaFisicaId == pessoa.Id).FirstOrDefault();
                user.Id = user.Id;
                user.Login = pessoa.Email;
                user.Senha = pessoa.Senha;
                user.PessoaFisicaId = pessoa.Id;
                user.PessoaFisica = pessoa;
                pessoa.DtAlteracao = DateTime.Now;
                _context.Usuarios.Update(user);
                _context.PessoasFisicas.Update(pessoa);
                _context.SaveChanges();
                Usuario usuario = _context.Usuarios.Where(p => p.PessoaFisicaId == pessoa.Id).Include(e => e.PessoaFisica).FirstOrDefault();
                _login.Login(usuario);
                return Json(new { success = true, message = $"Perfil atualizado com sucesso." });
            }
            catch (Exception e)
            {
                return Json(new { erro = true, message = $"{e.Message}" });
            }
        }
    }
}
