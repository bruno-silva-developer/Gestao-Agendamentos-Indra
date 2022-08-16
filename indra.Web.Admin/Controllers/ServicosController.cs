using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indra.Models;
using indra.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace indra.Web.Admin.Controllers
{
    public class ServicosController : Controller
    {
        private readonly AgendamentoDb _context;

        public ServicosController(AgendamentoDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var servicos = _context.Servicos.OrderBy(e => e.Id).ToList();
            return View(servicos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Servico servico)
        {
            ViewBag.error = null;
            try
            {
                if (_context.Servicos.Where(e => e.Nome == servico.Nome).Count() > 0)
                {
                    throw new Exception("Serviço " + servico.Nome + " já existe");
                }
                else
                {
                    _context.Servicos.Add(servico);
                    _context.SaveChanges();
                    TempData["S_SER_C"] = "Serviço " + servico.Nome + " criado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_SER_C"] = e.Message;
                return View(servico);
            }
        }


        public IActionResult Editar(int id)
        {
            var servico = _context.Servicos.Find(id);
            if (servico == null)
            {
                return NotFound();
            }
            else
            {
                return View(servico);
            }
        }

        [HttpPost]
        public IActionResult Editar(Servico servico)
        {
            ViewBag.error = null;
            try
            {
                _context.Update(servico);
                _context.SaveChanges();
                TempData["S_SER_E"] = "Serviço " + servico.Nome + " editado";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["E_SER_E"] = e.Message;
                return View(servico);
            }
        }

        public IActionResult Excluir(int id)
        {
            var servico = _context.Servicos.Find(id);
            return View(servico);
        }

        [HttpPost]
        public IActionResult ConfirmarExclusao(int id)
        {
            try
            {
                var servico = _context.Servicos.Find(id);

                var clienteComServico = _context.Agendamentos.Where(e => e.ServicoId == id && e.SituacaoAgendamentoId == 1);
                if (clienteComServico.Count() > 0)
                {
                    throw new Exception("Não é possível excluir esse serviço pois existem clientes que o utilizam.");
                }
                else
                {
                    _context.Servicos.Remove(servico);
                    _context.SaveChanges();
                    return Json(new { excluido = true, mensagem = $"{servico.Nome} excluído." });
                }
            }
            catch (Exception e)
            {
                return Json(new { erro = true, mensagem = e.Message });
            }
        }
    }
}