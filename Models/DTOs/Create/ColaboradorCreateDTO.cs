using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs
{
    public class ColaboradorCreateDTO
    {
        public string Nome { get; set; }
        public List<int> WorkshopId { get; set; }
    }
}