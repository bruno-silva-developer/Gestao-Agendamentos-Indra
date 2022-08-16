using indra.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace indra.Models
{
    public class PessoaFisica
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Nome é obrigatório")]
        [Display(Name = "Nome" )]
        public string Nome { get; set; } //aqui voce define o nome como obrigatorio, porem no banco tem cliente com nome null, por isso da erro, revisa o crud de inserção de cliente
        //[Required(ErrorMessage = "Data de Nascimento é obrigatória")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DtNascimento { get; set; }
        //[Required(ErrorMessage = "Sexo é obrigatório")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        //[Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Senha", ErrorMessage = "Senhas não conferem")]
        public string ConfirmacaoSenha { get; set; }
        //[Required(ErrorMessage ="Cpf é obrigatório")]
        [Display(Name = "Cpf")]
        public string Cpf { get; set; }
        //[Required(ErrorMessage = "Celular é obrigatório")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }
        public eTipo Tipo { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}