using indra.Infra.Data;
using indra.Models;
using indra.Web.Relatorios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indra.Web.Bibliotecas;

namespace indra.Web.Controllers
{
    public class HistoricoController : Controller
    {
        public List<Agendamentos> listaFiltrada;
        private readonly AgendamentoDb _context;
        private readonly LoginPessoaFisica _login;
        private readonly IWebHostEnvironment _oHostEnvironment;

        public HistoricoController(AgendamentoDb context, LoginPessoaFisica login)
        {
            _context = context;
            _login = login;
        }

        public IActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cliente(int? param, DateTime dataInicio, DateTime dataFim, int situacao)
        {
            var pessoa = _login.GetUsuario();
            List<Agendamentos> agendamentos = _context.Agendamentos.Where(e => e.ClienteId == pessoa.PessoaFisicaId)
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

            HistoricoCliente report = new HistoricoCliente(_oHostEnvironment);
            return File(report.PrepareReport(listaFiltrada, situacao), "application/pdf");
        }
    }
}
