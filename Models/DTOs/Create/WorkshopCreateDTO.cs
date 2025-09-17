using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs
{
    public class WorkshopCreateDTO
    {
        public string Nome { get; set; }
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; }
        public List<int> ColaboradorId { get; set; }
    }
}