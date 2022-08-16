using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Models
{
    public class EmailModel
    {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
    }
}
