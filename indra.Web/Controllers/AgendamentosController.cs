using indra.Infra.Data;
using indra.Models;
using indra.Models.Enums;
using indra.Web.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly AgendamentoDb _context;
        private readonly LoginPessoaFisica _login;
        public AgendamentosController(AgendamentoDb context, LoginPessoaFisica login)
        {
            _context = context;
            _login = login;
        }
        public IActionResult Criar()
        {
            ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
            ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Agendamentos agendamento)
        {
            try
            {
                var horaVerificacao = agendamento.Horario.Hour;
                var minutoVerificacao = agendamento.Horario.Minute;
                var diaVerificacao = agendamento.Horario.Day;
                var mesVerificacao = agendamento.Horario.Month;
                var anoVerificacao = agendamento.Horario.Year;

                var qtdAgendamentosMarcados = _context.Agendamentos.Where(e =>
                    e.ProfissionalId == agendamento.ProfissionalId &&
                    e.Horario.Hour == horaVerificacao &&
                    e.Horario.Minute == minutoVerificacao &&
                    e.Horario.Day == diaVerificacao &&
                    e.Horario.Month == mesVerificacao &&
                    e.Horario.Year == anoVerificacao &&
                    e.SituacaoAgendamentoId == 1);

                if (qtdAgendamentosMarcados.Count() >= 10)
                {
                    throw new Exception("Não é possível realizar mais de 10 agendamentos por dia para o profissional " + agendamento.Profissional.Nome);
                }

                var clienteJaAgendado = _context.Agendamentos.Where(e =>
                    e.ClienteId == agendamento.ClienteId &&
                    e.Horario.Hour == horaVerificacao &&
                    e.Horario.Minute == minutoVerificacao &&
                    e.Horario.Day == diaVerificacao &&
                    e.Horario.Month == mesVerificacao &&
                    e.Horario.Year == anoVerificacao &&
                    e.SituacaoAgendamentoId == 1);

                if (clienteJaAgendado.Count() > 0)
                {
                    throw new Exception("Já existe um agendamento nesse horário.");
                }
                else
                {
                    //agendamento.ClienteId = _login.GetUsuario().PessoaFisicaId;
                    _context.Agendamentos.Add(agendamento);
                    _context.SaveChanges();
                    return Json(new { success = true, message = $"Agendamento marcado com sucesso." });
                }
            }
            catch (Exception e)
            {
                ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
                ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
                return Json(new { erro = true, message = $"{e.Message}" });
            }
        }

        public IActionResult Editar(int id)
        {
            var agendamento = _context.Agendamentos.Find(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Profissional = _context.PessoasFisicas.Where(e => e.Id == agendamento.ProfissionalId && e.Tipo == eTipo.Profissional).FirstOrDefault();
                ViewBag.Servico = _context.Servicos.Where(e => e.Id == agendamento.ServicoId).FirstOrDefault();
                return View(agendamento);
            }
        }

        [HttpPost]
        public IActionResult Editar(Agendamentos agendamento)
        {
            try
            {
                var horaVerificacao = agendamento.Horario.Hour;
                var minutoVerificacao = agendamento.Horario.Minute;

                var clienteJaAgendado = _context.Agendamentos.Where(e =>
                    e.ClienteId == agendamento.ClienteId &&
                    e.Horario.Hour == horaVerificacao &&
                    e.Horario.Minute == minutoVerificacao &&
                    e.SituacaoAgendamentoId == 1);

                if (clienteJaAgendado.Count() > 0)
                {
                    throw new Exception("Já existe um agendamento nesse horário.");
                }
                else
                {
                    agendamento.ClienteId = _login.GetUsuario().PessoaFisicaId;
                    _context.Update(agendamento);
                    _context.SaveChanges();
                    TempData["S_AGENDA_E"] = "Agendamento editado";
                    return RedirectToAction("TelaPrincipal","Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.Profissional = _context.PessoasFisicas.Where(e => e.Id == agendamento.ProfissionalId && e.Tipo == eTipo.Profissional).FirstOrDefault();
                ViewBag.Servico = _context.Servicos.Where(e => e.Id == agendamento.ServicoId).FirstOrDefault();
                TempData["S_AGENDA_E"] = e.Message;
                return View(agendamento);
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
                }//seu problema resolvido, zuera que era só isso 

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
