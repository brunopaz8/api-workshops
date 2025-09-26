using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs
{
    public class ColaboradorCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        public List<int> WorkshopId { get; set; } = new();
    }
}