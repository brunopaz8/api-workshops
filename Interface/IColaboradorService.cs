using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_workshops.Models;
using api_workshops.Models.DTOs;
using api_workshops.Models.DTOs.Read;

namespace api_workshops.Interface
{
    public interface IColaboradorService
    {
        public Task<ColaboradorReadDTO> Create(ColaboradorCreateDTO dto);
        public Task<ColaboradorReadDTO> GetById(int id);
        public Task<List<ColaboradorReadDTO>> GetAll();
        public Task<List<ColaboradorReadDTO>> GetByName(string name);
        public Task Update(int id, ColaboradorCreateDTO dto);
        public Task Delete(int id);
    }
}