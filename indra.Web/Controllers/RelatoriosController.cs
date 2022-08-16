using indra.Infra.Data;
using indra.Models;
using indra.Models.Enums;
using indra.Web.Bibliotecas;
using indra.Web.Relatorios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Controllers
{
    public class RelatoriosController : Controller
    {
        public List<Agendamentos> listaFiltrada;
        private readonly AgendamentoDb _context;
        private readonly LoginPessoaFisica _login;
        private readonly IWebHostEnvironment _oHostEnvironment;

        public RelatoriosController(AgendamentoDb context, LoginPessoaFisica login)
        {
            _context = context;
            _login = login;
        }

        [HttpGet]
        public IActionResult Agendamentos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agendamentos(int? param, DateTime dataInicio, DateTime dataFim, int situacao)
        {
            var pessoa = _login.GetUsuario();
            List<Agendamentos> agendamentos = _context.Agendamentos.Where(e => e.ProfissionalId == pessoa.PessoaFisicaId)
                .Include(e => e.Profissional)
                .Include(e => e.Cliente)
                .Include(e => e.Servico)
                .Include(e => e.SituacaoAgendamento)
                .OrderBy(e => e.Horario).ToList();

            if (situacao == 0)
            {
                listaFiltrada = agendamentos
                    .Where(e => e.Horario >= dataInicio
                    && e.Horario <= dataFim).ToList();
            }
            else if (situacao == 1)
            {
                listaFiltrada = agendamentos
                    .Where(e => e.Horario >= dataInicio
                    && e.Horario <= dataFim &&
                    e.SituacaoAgendamentoId == 1).ToList();
            }
            else if (situacao == 2)
            {
                listaFiltrada = agendamentos
                    .Where(e => e.Horario >= dataInicio
                    && e.Horario <= dataFim &&
                    e.SituacaoAgendamentoId == 2).ToList();
            }
            else if (situacao == 3)
            {
                listaFiltrada = agendamentos
                    .Where(e => e.Horario >= dataInicio
                    && e.Horario <= dataFim &&
                    e.SituacaoAgendamentoId == 2).ToList();
            }

            RelatorioAgendamentos report = new RelatorioAgendamentos(_oHostEnvironment);
            return File(report.PrepareReport(listaFiltrada, situacao), "application/pdf");
        }

        [HttpGet]
        public IActionResult Clientes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Clientes(int? param)
        {
            List<PessoaFisica> clientes = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Cliente).OrderBy(e => e.Id).ToList();
            RelatorioClientes report = new RelatorioClientes(_oHostEnvironment);
            return File(report.PrepareReport(clientes), "application/pdf");
        }

    }
}
