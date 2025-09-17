using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models
{
    public class Workshop
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; }
        public ICollection<WorkshopColaboradores> WorkshopColaboradores { get; set; }
    }
}