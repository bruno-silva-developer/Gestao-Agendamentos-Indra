using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Models
{
    public class Servico
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome do Serviço é requerido")]
        [Display(Name = "Nome do Serviço")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Descrição é requerida")]
        [Display(Name = "Descrição")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Valor é requerido")]
        [Display(Name = "Valor")]
        //[DataType(DataType.Currency)]
        public decimal Valor { get; set; }
    }
}
