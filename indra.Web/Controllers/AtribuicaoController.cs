using indra.Infra.Data;
using indra.Infra.Extensions;
using indra.Models;
using indra.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Controllers
{
    public class AtribuicaoController : Controller
    {
        private readonly AgendamentoDb _context;

        public AtribuicaoController(AgendamentoDb context)
        {
            _context = context;
        }
        public IActionResult Servico()
        {
            ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Servico(ProfissionalServico atribuicao)
        {
            try
            {
                var profissionaisServico = _context.ProfissionalServicos
                    .Where(e => e.ServicoId == atribuicao.ServicoId && e.ProfissionalId == atribuicao.ProfissionalId)
                    .Include(e => e.Profissional)
                    .Include(e => e.Servico).ToList();

                if (!profissionaisServico.ContemElementos())
                {
                    _context.ProfissionalServicos.Add(atribuicao);
                    _context.SaveChanges();
                    TempData["S_ATRI_SER"] = "Serviço atribuido ao profissional com sucesso";
                    return RedirectToAction("Perfil","Home");

                }
                else
                {
                    throw new Exception("Profissional já atribuido ao serviço");
                }
            }
            catch (Exception e)
            {
                ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
                TempData["E_ATRI_SER"] = e.Message;
                return View();
            }
        }
    }
}
