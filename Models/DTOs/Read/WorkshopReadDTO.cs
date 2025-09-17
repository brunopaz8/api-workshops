using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_workshops.Models.DTOs.Read;

namespace api_workshops.Models.DTOs
{
    public class WorkshopReadDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; }
        public List<ColaboradorReadDTOSimples> Colaboradores { get; set; }
    }
}