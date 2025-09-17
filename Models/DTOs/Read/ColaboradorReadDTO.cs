using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_workshops.Models.DTOs.Read;

namespace api_workshops.Models.DTOs
{
    public class ColaboradorReadDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<WorkshopReadDTOSimples> Workshops { get; set; }
    }
}