using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indra.Models;
using indra.Infra.Data;
using indra.Web.Admin.Bibliotecas.Criptografia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using indra.Models.Enums;
using indra.Web.Admin.Bibliotecas;
using indra.Infra.Extensions;

namespace indra.Web.Admin.Controllers
{
    public class ProfissionaisController : Controller
    {
        private readonly AgendamentoDb _context;

        public ProfissionaisController(AgendamentoDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var profissionais = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional).OrderBy(e => e.Id).ToList();
            return View(profissionais);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(PessoaFisica profissional)
        {
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == profissional.Email).Count() > 0)
                {
                    throw new Exception("Já existe um profissional criado com o email " + profissional.Email);
                }
                else
                {
                    var usuario = new Usuario();
                    usuario.Login = profissional.Email;
                    usuario.Senha = profissional.Senha;
                    profissional.DtCriacao = DateTime.Now;
                    profissional.DtAlteracao = DateTime.Now;
                    profissional.Tipo = eTipo.Profissional;
                    profissional.Ativo = true;
                    usuario.PessoaFisica = profissional;
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    TempData["S_PROF_C"] = "Profissional " + profissional.Nome + " criado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                TempData["E_PROF_C"] = "Erro ao criar profissional " + profissional.Nome;
                return View(profissional);
            }
        }

        public IActionResult Editar(int id)
        {
            var profissional = _context.PessoasFisicas.Find(id);
            if (profissional == null)
            {
                return NotFound();
            }
            else
            {
                return View(profissional);
            }
        }

        [HttpPost]
        public IActionResult Editar(PessoaFisica profissional)
        {
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == profissional.Email).Count() > 0)
                {
                    throw new Exception("Já existe um profissional criado com o email " + profissional.Email);
                }
                else
                {
                    profissional.DtAlteracao = DateTime.Now;
                    _context.PessoasFisicas.Update(profissional);
                    _context.SaveChanges();
                    TempData["S_PROF_E"] = "Profissional " + profissional.Nome + " editado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_PROF_E"] = e.Message;
                return View(profissional);
            }
        }

        public IActionResult Desativar(int id)
        {
            var profissional = _context.PessoasFisicas.Find(id);
            return View(profissional);
        }

        public IActionResult Ativar(int id)
        {
            ViewBag.error = null;
            try
            {
                var profissional = _context.PessoasFisicas.Find(id);


                if (profissional == null)
                {
                    throw new Exception("Não é possível ativar pois ele não foi encontrado");
                }
                else
                {
                    profissional.DtAlteracao = DateTime.Now;
                    profissional.Ativo = true;
                    _context.PessoasFisicas.Update(profissional);
                    _context.SaveChanges();
                    TempData["S_PROF_A"] = " Profissional " + profissional.Nome + " ativado ";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                var profissional = _context.PessoasFisicas.Find(id);
                TempData["E_PROF_A"] = "Erro ao ativar profissional " + profissional.Nome;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarDesativacao(int id)
        {
            try
            {
                var profissional = _context.PessoasFisicas.Find(id);

                var agendamentoExistenteProfissional = _context.Agendamentos
                    .Where(e => e.ProfissionalId == id && e.SituacaoAgendamentoId == 1);
                if (agendamentoExistenteProfissional.Count() > 0)
                {
                    throw new Exception("Não é possível desativar esse profissional pois existem agendamentos para o mesmo.");
                }
                else
                {
                    profissional.DtAlteracao = DateTime.Now;
                    profissional.Ativo = false;
                    _context.PessoasFisicas.Update(profissional);
                    _context.SaveChanges();
                    return Json(new { success = true, message = $"{profissional.Nome} desativado." });
                }
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = $"{e.Message}" });
            }
        }

        public IActionResult AtribuirServico()
        {
            ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
            ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult AtribuirServico(ProfissionalServico atribuicao)
        {
            ViewBag.error = null;
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
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["E_ATRI_SER"] = "Profissional já atribuido ao serviço";
                    throw new Exception("Profissional já atribuido ao serviço");
                }
            }
            catch (Exception e)
            {
                ViewBag.Profissional = new SelectList(_context.PessoasFisicas.Where(e => e.Tipo == eTipo.Profissional), "Id", "Nome");
                ViewBag.Servico = new SelectList(_context.Servicos, "Id", "Nome");
                ViewBag.error = e.Message;
                TempData["E_ATRI_SER"] = "Profissional já associado ao Serviço";
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetListaDeProfissionais(string termoDeBusca)
        {
            try
            {
                var normalizado = termoDeBusca.IsNullEmptyOrWhitespace() ? "" : termoDeBusca.ToUpper();
                var profissionais = _context.PessoasFisicas.Where(c => c.Tipo == eTipo.Profissional &&
                (c.Nome.ToUpper().Contains(normalizado)
                || c.Celular.Contains(normalizado)
                || c.Email.Contains(normalizado)
                || c.Id.ToString().Contains(normalizado))).ToList();

                var html = this.RenderizarHtmlParaString("_TabelaProfissionaisPartial", profissionais);
                return Json(new { success = true, html = html });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Falha ao buscar profissionais." });
            }
        }
    }
}