using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using indra.Infra.Data;
using indra.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indra.Web.Admin.Relatorios;
using Microsoft.AspNetCore.Hosting;
using indra.Models.Enums;

namespace indra.Web.Admin.Controllers
{
    public class RelatoriosController : Controller
    {
        public List<Agendamentos> listaFiltrada;
        private readonly AgendamentoDb _context;
        private readonly IWebHostEnvironment _oHostEnvironment;

        public RelatoriosController(AgendamentoDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Agendamentos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agendamentos(int? param, DateTime dataInicio, DateTime dataFim, int situacao)
        {
            List<Agendamentos> agendamentos = _context.Agendamentos
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
                    e.SituacaoAgendamentoId == 3).ToList();
            }


            RelatorioAgendamentos report = new RelatorioAgendamentos(_oHostEnvironment);
            return File(report.PrepareReport(listaFiltrada, situacao), "application/pdf");
        }

        [HttpGet]
        public IActionResult Profissionais()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Profissionais(int? param)
        {
            List<PessoaFisica> profissionais = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional).OrderBy(e => e.Id).ToList();
            RelatorioProfissionais report = new RelatorioProfissionais(_oHostEnvironment);
            return File(report.PrepareReport(profissionais), "application/pdf");
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

        [HttpGet]
        public IActionResult Servicos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Servicos(int? param)
        {
            List<ProfissionalServico> servicos = _context.ProfissionalServicos.OrderBy(e => e.Id).Include(e => e.Servico).Include(e => e.Profissional).ToList();
            RelatorioServicos report = new RelatorioServicos(_oHostEnvironment);
            return File(report.PrepareReport(servicos), "application/pdf");
        }
    }
}
