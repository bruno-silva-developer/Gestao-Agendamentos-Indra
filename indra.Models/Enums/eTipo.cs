using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Models.Enums
{
    public enum eTipo
    {

        [Description("Profissional")]
        Profissional,
        [Description("Cliente")]
        Cliente,
        [Description("Administrador")]
        Administrador,
    }
}
