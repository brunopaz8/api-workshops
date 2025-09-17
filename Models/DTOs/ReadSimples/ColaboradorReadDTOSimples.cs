using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs.Read
{
    public class ColaboradorReadDTOSimples
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Presente { get; set; }
    }
}