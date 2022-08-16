using indra.Models;
using indra.Models.Enums;
using indra.Infra.Data;
using indra.Web.Admin.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Services.ClientNotification;
using indra.Models.DTO;

namespace indra.Web.Admin.Controllers
{

    public class HomeController : Controller
    {
        private readonly AgendamentoDb _context;
        private readonly LoginAdmin _login;

        public HomeController(AgendamentoDb context, LoginAdmin login)
        {
            _context = context;
            _login = login;
        }
        public IActionResult Perfil()
        {
            var user = _login.GetAdmin();
            var dadosPerfil = _context.PessoasFisicas.Find(user.Id);
            ViewBag.Nome = dadosPerfil.Nome;
            ViewBag.Email = dadosPerfil.Email;
            ViewBag.Sexo = dadosPerfil.Sexo;
            ViewBag.DtNascimento = dadosPerfil.DtNascimento;
            ViewBag.Cpf = dadosPerfil.Cpf;
            ViewBag.Celular = dadosPerfil.Celular;
            ViewBag.Tipo = dadosPerfil.Tipo.ToString();
            return View();
        }

        public IActionResult AtualizarPerfil()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AtualizarPerfil(PessoaFisica admin)
        {
            try
            {
                admin.DtAlteracao = DateTime.Now;
                _context.PessoasFisicas.Update(admin);
                _context.SaveChanges();
                PessoaFisica adm = _context.PessoasFisicas.Find(admin.Id);
                Usuario user = _context.Usuarios.Where(e => e.PessoaFisicaId == adm.Id).FirstOrDefault();
                _login.Login(user);
                return Json(new { success = true, message = $"Perfil atualizado com sucesso." });
            }
            catch (Exception e)
            {
                return Json(new { erro = true, message = $"{e.Message}" });
            }
        }

        public IActionResult Dashboard()
        {
            var dashDTO = new DashboardDTO();
            dashDTO.QtdProfissionais = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional && e.Ativo == true).Count();
            dashDTO.QtdClientes = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Cliente && e.Ativo == true).Count();
            dashDTO.QtdServicos = _context.Servicos.Count();
            dashDTO.QtdAgendamentos = _context.Agendamentos.Where(e => e.SituacaoAgendamentoId == 1).Count();
            dashDTO.QtdAdministradores = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Administrador && e.Ativo == true).Count();

            return View(dashDTO);
        }
    }
}
