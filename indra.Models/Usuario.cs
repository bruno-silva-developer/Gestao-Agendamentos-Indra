using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace indra.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Login é requirido")]
        [Display(Name = "Email")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Senha é requirida")]
        public string Senha { get; set; }
        public int PessoaFisicaId { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}
