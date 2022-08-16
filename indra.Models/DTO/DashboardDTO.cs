using System;
using System.Collections.Generic;
using System.Text;

namespace indra.Models.DTO
{
    public class DashboardDTO
    {
        public DashboardDTO()
        {

        }

        public int QtdProfissionais { get; set; }
        public int QtdServicos { get; set; }
        public int QtdAgendamentos { get; set; }
        public int QtdClientes { get; set; }
        public int QtdAdministradores { get; set; }
    }
}
