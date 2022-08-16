using indra.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Infra.Data
{
    public class AgendamentoDb : DbContext
    {
        public AgendamentoDb()
        {
        }
        public AgendamentoDb(DbContextOptions<AgendamentoDb> options) : base(options)
        {
        }
        public DbSet<Agendamentos> Agendamentos { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<SituacaoAgendamento> SituacaoAgendamentos { get; internal set; }
        public DbSet<ProfissionalServico> ProfissionalServicos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
