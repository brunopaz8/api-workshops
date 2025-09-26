using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_workshops.Models.DTOs
{
    public class WorkshopCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public DateTime DataRealizacao { get; set; }
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
        public List<int> ColaboradorId { get; set; } = new();
    }
}