using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using indra.Infra.Data;
using indra.Models;
using indra.Models.Enums;
using indra.Web.Admin.Bibliotecas;
using indra.Web.Admin.Bibliotecas.Criptografia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace indra.Web.Admin.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AgendamentoDb _context;

        public ClientesController(AgendamentoDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var clientes = _context.PessoasFisicas.Where(e => e.Tipo == eTipo.Cliente).OrderBy(e => e.Id).ToList();
            return View(clientes);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(PessoaFisica cliente)
        {
            ViewBag.error = null;
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == cliente.Email).Count() > 0)
                {
                    throw new Exception("Já existe um cliente com o email " + cliente.Email + " criado");
                }
                else if (_context.PessoasFisicas.Where(e => e.Cpf == cliente.Cpf).Count() > 0)
                {
                    throw new Exception("Já existe um cliente com o CPF " + cliente.Cpf + " criado");
                }
                else
                {
                    var usuario = new Usuario();
                    usuario.Login = cliente.Email;
                    usuario.Senha = cliente.Senha;
                    cliente.DtCriacao = DateTime.Now;
                    cliente.DtAlteracao = DateTime.Now;
                    cliente.Tipo = eTipo.Cliente;
                    cliente.Ativo = true;
                    usuario.PessoaFisica = cliente;
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    TempData["S_CLIE_C"] = "Cliente " + cliente.Nome + " criado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_CLIE_C"] = e.Message;
                return View(cliente);
            }
        }

        public IActionResult Editar(int id)
        {
            var cliente = _context.PessoasFisicas.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                return View(cliente);
            }
        }

        [HttpPost]
        public IActionResult Editar(PessoaFisica cliente)
        {
            try
            {
                if (_context.PessoasFisicas.Where(e => e.Email == cliente.Email).Count() > 0)
                {
                    throw new Exception("Já existe um profissional criado com o email " + cliente.Email);
                }
                else
                {
                    cliente.DtAlteracao = DateTime.Now;
                    _context.PessoasFisicas.Update(cliente);
                    _context.SaveChanges();
                    TempData["S_CLIE_E"] = "Cliente " + cliente.Nome + " editado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["E_CLIE_E"] = e.Message;
                return View(cliente);
            }
        }

        public IActionResult Ativar(int id)
        {

            try
            {
                var cliente = _context.PessoasFisicas.Find(id);

                if (cliente == null)
                {
                    throw new Exception("Não é possível ativar pois ele não foi encontrado");
                }
                else
                {
                    cliente.DtAlteracao = DateTime.Now;
                    cliente.Ativo = true;
                    _context.PessoasFisicas.Update(cliente);
                    _context.SaveChanges();
                    return Json(new { success = true, message = $"{cliente.Nome} ativado." });
                }
            }
            catch (Exception e)
            {

                return Json(new { error = true, message = $"{e.Message}" });
            }
        }


        public IActionResult Desativar(int id)
        {
            var cliente = _context.PessoasFisicas.Where(e => e.Id == id).FirstOrDefault();
            return View(cliente);
        }

        [HttpPost]
        public IActionResult ConfirmarDesativacao(int id)
        {
            ViewBag.error = null;
            try
            {
                var cliente = _context.PessoasFisicas.Where(e => e.Id == id).FirstOrDefault();

                var agendamentoExistenteCliente = _context.Agendamentos.Where(e => e.ClienteId == id && e.SituacaoAgendamentoId == 1);
                if (agendamentoExistenteCliente.Count() > 0)
                {
                    throw new Exception("Não é possível desativar esse cliente pois existem agendamentos para ele.");
                }
                else
                {
                    cliente.DtAlteracao = DateTime.Now;
                    cliente.Ativo = false;
                    _context.PessoasFisicas.Update(cliente);
                    _context.SaveChanges();
                    return Json(new { success = true, message = $"{cliente.Nome} desativado." });
                }
            }
            catch (Exception e)
            {

                return Json(new { error = true, message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult GetListaDeClientes(string termoDeBusca)
        {
            try
            {
                var normalizado = string.IsNullOrEmpty(termoDeBusca) ? "" : termoDeBusca.ToUpper();
                var clientes = _context.PessoasFisicas.Where(c => c.Tipo == eTipo.Cliente &&
                (c.Nome.ToUpper().Contains(normalizado)
                || c.Celular.Contains(normalizado)
                || c.Email.Contains(normalizado)
                || c.Id.ToString().Contains(normalizado))).ToList();

                var html = this.RenderizarHtmlParaString("_TabelasClientesPartial", clientes);
                return Json(new { success = true, html = html });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Falha ao buscar clientes." });
            }
        }
    }
}