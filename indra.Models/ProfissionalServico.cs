using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace indra.Models
{
    public class ProfissionalServico
    {
        public int Id { get; set; }

        [ForeignKey("idProfissional")]
        public int ProfissionalId { get; set; }
        public virtual PessoaFisica Profissional { get; set; }

        [ForeignKey("idServico")]
        public int ServicoId { get; set; }
        public virtual Servico Servico { get; set; }

    }
}
