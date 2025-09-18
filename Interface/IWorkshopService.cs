using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_workshops.Models.DTOs;

namespace api_workshops.Interface
{
    public interface IWorkshopService
    {
        public Task<WorkshopReadDTO> Create(WorkshopCreateDTO workshopCreateDTO);
        public Task<bool> AtualizarPresencaAsync(int workshopId, int colaboradorId);
        public Task<WorkshopReadDTO> GetById(int id);
        public Task<List<WorkshopReadDTO>> GetAll();
        public Task<List<WorkshopReadDTO>> GetByName(string name);
        public Task Update(int id, WorkshopCreateDTO dto);
        public Task Delete(int id);

    }
}