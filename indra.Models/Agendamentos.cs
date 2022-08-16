using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace indra.Models
{
    public class Agendamentos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProfissionalId { get; set; }
        public virtual PessoaFisica Profissional { get; set; }
        [Required]
        public int ServicoId { get; set; }
        public virtual Servico Servico { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public virtual PessoaFisica Cliente { get; set; }
        [Required]
        public int SituacaoAgendamentoId { get; set; }
        public virtual SituacaoAgendamento SituacaoAgendamento { get; set; }
        [Required]
        public DateTime Horario { get; set; }
    }
}