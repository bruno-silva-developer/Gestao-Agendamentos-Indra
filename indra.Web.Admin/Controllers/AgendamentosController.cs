using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indra.Infra.Data;
using indra.Models;
using indra.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace indra.Web.Admin.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly AgendamentoDb _context;

        public AgendamentosController(AgendamentoDb context)
        {
            _context = context;
        }

        // Index serve para apresentar os dados na tela e retorna a View de Index feita no ASP
        public IActionResult Index()
        {
            var agendamentos = _context.Agendamentos.Where(e => e.SituacaoAgendamentoId == 1).Include(e => e.Profissional).Include(e => e.Cliente).Include(e => e.Servico).Include(e => e.SituacaoAgendamento).OrderBy(e => e.Horario);
            if (agendamentos == null)
            {
                TempData["SEM_AGENDAMENTO_AGUARDANDO"] = "Não existe nenhum agendamento aguardando";
                return View(agendamentos.ToList());
            }
            return View(agendamentos.ToList());
        }

        // Retorna a tela de Criação de Agendamento, trazendo dados de Profissionais, Clientes e Serviços
        public IActionResult Criar()
        {
            ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
            ViewBag.Cliente = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Cliente), "Id", "Nome");
            ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        // Requisita um método de HTTP para postar dados para inserção
        [HttpPost]
        public IActionResult Criar(Agendamentos agendamento)
        {
            ViewBag.error = null;
            try
            {
                var horaVerificacao = agendamento.Horario.Hour;
                var minutoVerificacao = agendamento.Horario.Minute;

                var qtdAgendamentosMarcados = _context.Agendamentos.Where(e =>
                    e.ProfissionalId == agendamento.ProfissionalId &&
                    e.Horario.Hour == horaVerificacao &&
                    e.Horario.Minute == minutoVerificacao);

                var clientePersistido = _context.PessoasFisicas.FirstOrDefault(c => c.Id == agendamento.ClienteId);
                if (clientePersistido == null)
                    throw new Exception("Cliente não encontrado, por favor recarregue a página e tente novamente");

                agendamento.Cliente = clientePersistido;

                if (qtdAgendamentosMarcados.Count() >= 10)
                {
                    throw new Exception("Não é possível realizar mais de 10 agendamentos por dia para o profissional.");
                }

                var clienteJaAgendado = _context.Agendamentos.Where(e =>
                    e.ClienteId == agendamento.ClienteId &&
                    e.Horario.Hour == horaVerificacao &&
                    e.Horario.Minute == minutoVerificacao);

                if (clienteJaAgendado.Count() > 0)
                {
                    throw new Exception("Já existe um agendamento para esse cliente nessa horário.");
                }
                else
                {
                    _context.Agendamentos.Add(agendamento);
                    _context.SaveChanges();
                    TempData["S_AGENDA_C"] = "Agendamento para " + agendamento.Cliente.Nome + " criado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
                ViewBag.Cliente = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Cliente), "Id", "Nome");
                ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
                ViewBag.error = e.Message;
                TempData["E_AGENDA_C"] = "Erro ao fazer agendamento";
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetDadosServico(int id)
        {
            try
            {
                var servico = _context.Servicos.Find(id);
                if (servico == null)
                {
                    throw new Exception("Serviço não encontrado");
                }
                var profissionais = _context.ProfissionalServicos.Where(e => e.ServicoId == id).Include(e => e.Profissional).ToList();
                if (!profissionais.Any())
                {
                    throw new Exception("Profissional não encontrado");
                }

                return Json(new { success = true, valor = servico.Valor, profissionais = profissionais.Select(c => new { Id = c.Profissional.Id, Nome = c.Profissional.Nome }) });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Conclui o agendamento mudando o ID da Situação do Agendamento para Concluído
        public IActionResult Concluir(int id)
        {
            try
            {
                var agendamento = _context.Agendamentos.FirstOrDefault(c => c.Id == id);
                var horario = _context.Agendamentos.Where(e => e.Id == id).Include(e => e.Profissional).Include(e => e.Cliente).Include(e => e.Servico).Include(e => e.SituacaoAgendamento);
                if (agendamento == null)
                {
                    throw new Exception("Não foi possível concluir o agendamento, por favor recarregue a página e tente novamente.");
                }

                agendamento.SituacaoAgendamentoId = 2;
                _context.Update(agendamento);
                _context.SaveChanges();
                return Json(new { success = true, message = $"O agendamento foi concluido." });
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = $"{e.Message}" });
            }
        }

        // Cancela o agendamento mudando o ID da Situação do Agendamento para Cancelado
        public IActionResult Cancelar(int id)
        {
            try
            {
                var agendamento = _context.Agendamentos.Find(id);
                if (agendamento == null)
                {
                    throw new Exception("Não foi possível cancelar o agendamento, favor recarregue a página e tente novamente.");
                }
                agendamento.SituacaoAgendamentoId = 3;
                _context.Agendamentos.Update(agendamento);
                _context.SaveChanges();
                return Json(new { success = true, message = $"O agendamento foi cancelado." });
            }
            catch (Exception e)
            {
                return Json(new { erro = true, message = $"{e.Message}" });
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCancelamento(int id)
        {

            try
            {
                var agendamento = _context.Agendamentos.Find(id);

                if (agendamento == null)
                {
                    throw new Exception("Não foi possível cancelar o agendamento, favor recarregue a página e tente novamente.");
                }
                agendamento.SituacaoAgendamentoId = 3;
                _context.Agendamentos.Update(agendamento);
                _context.SaveChanges();
                return Json(new { success = true, message = $"O agendamento do cliente {agendamento.Cliente.Nome} foi cancelado." });
            }
            catch (Exception e)
            {
                return Json(new { erro = true, message = $"{e.Message}" });
            }
        }
    }
}