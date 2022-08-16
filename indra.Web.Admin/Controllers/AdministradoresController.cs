using indra.Infra.Data;
using indra.Models;
using indra.Web.Admin.Bibliotecas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Admin.Controllers
{
    public class AdministradoresController : Controller
    {
        private readonly AgendamentoDb _context;

        public AdministradoresController(AgendamentoDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var admins = _context.PessoasFisicas.Where(e => e.Tipo == Models.Enums.eTipo.Administrador).OrderBy(e => e.Id).ToList();
            return View(admins);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(PessoaFisica admin)
        {
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == admin.Email).Count() > 0)
                {
                    throw new Exception("Já existe um admin criado com o email " + admin.Email);
                }
                else
                {
                    admin.DtCriacao = DateTime.Now;
                    admin.DtAlteracao = DateTime.Now;
                    admin.Tipo = Models.Enums.eTipo.Administrador;
                    admin.Ativo = true;
                    _context.PessoasFisicas.Add(admin);
                    _context.SaveChanges();
                    TempData["S_ADM_C"] = "Administrador " + admin.Nome + " criado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_ADM_C"] = e.Message;
                return View(admin);
            }
        }

        public IActionResult Editar(int id)
        {
            var admin = _context.PessoasFisicas.Find(id);
            if (admin == null)
            {
                return NotFound();
            }
            else
            {
                return View(admin);
            }
        }

        [HttpPost]
        public IActionResult Editar(PessoaFisica admin)
        {
            ViewBag.error = null;
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == admin.Email).Count() > 0)
                {
                    throw new Exception("Já existe um profissional criado com o email " + admin.Email);
                }
                else
                {
                    admin.DtAlteracao = DateTime.Now;
                    _context.Update(admin);
                    _context.SaveChanges();
                    TempData["S_ADM_E"] = "Administrador " + admin.Nome + " editado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_ADM_E"] = e.Message;
                return View(admin);
            }
        }

        public IActionResult Desativar(int id)
        {
            var admin = _context.PessoasFisicas.Find(id);
            return View(admin);
        }

        public IActionResult Ativar(int id)
        {
            ViewBag.error = null;
            try
            {
                var admin = _context.PessoasFisicas.Find(id);


                if (admin == null)
                {
                    throw new Exception("Não é possível ativar pois ele não foi encontrado");
                }
                else
                {
                    admin.DtAlteracao = DateTime.Now;
                    admin.Ativo = true;
                    _context.PessoasFisicas.Update(admin);
                    _context.SaveChanges();
                    TempData["S_ADM_A"] = "Administrador " + admin.Nome + " ativado ";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                var admin = _context.PessoasFisicas.Find(id);
                TempData["E_ADM_A"] = "Erro ao ativar admin " + admin.Nome;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarDesativacao(int id)
        {
            try
            {
                var admin = _context.PessoasFisicas.Find(id);
                admin.DtAlteracao = DateTime.Now;
                admin.Ativo = false;
                _context.PessoasFisicas.Update(admin);
                _context.SaveChanges();
                return Json(new { success = true, message = $"{admin.Nome} desativado." });
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = $"{e.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetListaDeAdministradores(string termoDeBusca)
        {
            try
            {
                var normalizado = string.IsNullOrEmpty(termoDeBusca) ? "" : termoDeBusca.ToUpper();
                var admins = _context.PessoasFisicas.Where(c => c.Tipo == Models.Enums.eTipo.Administrador && c.Nome.ToUpper().Contains(normalizado)
                || c.Email.Contains(normalizado)
                || c.Id.ToString().Contains(normalizado)).ToList();

                var html = this.RenderizarHtmlParaString("_TabelaAdministradoresPartial", admins);
                return Json(new { success = true, html = html });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Falha ao buscar administradores." });
            }
        }
    }
}
