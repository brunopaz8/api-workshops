using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models
{
    public class WorkshopColaboradores
    {
        public int Id { get; set; }
        
        public int ColaboradorId { get; set; }
        public Colaborador Colaborador { get; set; } = null;

        public int WorkshopId { get; set; }
        public Workshop Workshop { get; set; } = null;

        public bool Presente { get; set; } = false;
    }
}