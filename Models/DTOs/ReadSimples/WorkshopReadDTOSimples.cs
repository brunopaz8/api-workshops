using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs.Read
{
    public class WorkshopReadDTOSimples
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; }
    }
}