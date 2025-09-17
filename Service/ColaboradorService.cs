using api_workshops.Data;
using api_workshops.Interface;
using api_workshops.Models;
using api_workshops.Models.DTOs;
using api_workshops.Models.DTOs.Read;
using Microsoft.EntityFrameworkCore;

namespace api_workshops.Service
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly AppDbContext _Context;

        public ColaboradorService(AppDbContext appDbContext)
        {
            _Context = appDbContext;
        }

        public async Task<ColaboradorReadDTO> Create(ColaboradorCreateDTO dto)
        {
            var colaborador = new Colaborador
            {
                Nome = dto.Nome
            };

            _Context.Colaboradores.Add(colaborador);

            if (dto.WorkshopId != null && dto.WorkshopId.Any())
            {
                var workshopsValidos = await _Context.Workshops
                    .Where(w => dto.WorkshopId.Contains(w.Id))
                    .ToListAsync();

                if (workshopsValidos == null || workshopsValidos.Count == 0)
                {
                    throw new InvalidOperationException();
                }

                foreach (var workshop in workshopsValidos)
                {
                    var relacao = new WorkshopColaboradores
                    {
                        WorkshopId = workshop.Id,
                        ColaboradorId = colaborador.Id,
                        Presente = false
                    };
                    _Context.WorkshopColaboradores.Add(relacao);
                }
            }

            await _Context.SaveChangesAsync();

            var colaboradorComWorkshops = await _Context.Colaboradores
                .Where(c => c.Id == colaborador.Id)
                .Include(c => c.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Workshop)
                .FirstOrDefaultAsync();

            var colaboradorDTO = new ColaboradorReadDTO
            {
                Id = colaboradorComWorkshops.Id,
                Nome = colaboradorComWorkshops.Nome,
                Workshops = colaboradorComWorkshops.WorkshopColaboradores
                    .Select(wc => new WorkshopReadDTOSimples
                    {
                        Id = wc.Workshop.Id,
                        Nome = wc.Workshop.Nome,
                        DataRealizacao = wc.Workshop.DataRealizacao,
                        Descricao = wc.Workshop.Descricao
                    }).ToList()
            };

            return colaboradorDTO;
        }

        public async Task<ColaboradorReadDTO> GetById(int id)
        {
            var Colaborador = await _Context.Colaboradores
                .Where(c => c.Id == id)
                .Include(c => c.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Workshop)
                .FirstOrDefaultAsync();

            if (Colaborador == null)
            {
                throw new KeyNotFoundException();
            }

            return new ColaboradorReadDTO
            {
                Id = Colaborador.Id,
                Nome = Colaborador.Nome,
                Workshops = Colaborador.WorkshopColaboradores
                .Select(wc => new WorkshopReadDTOSimples
                {
                    Id = wc.Workshop.Id,
                    Nome = wc.Workshop.Nome,
                    DataRealizacao = wc.Workshop.DataRealizacao,
                    Descricao = wc.Workshop.Descricao

                }).ToList()
            };
        }

        public async Task<List<ColaboradorReadDTO>> GetAll()
        {
            var colaboradores = await _Context.Colaboradores
                .Include(c => c.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Workshop)
                .ToListAsync();

            if (colaboradores == null || colaboradores.Count == 0)
            {
                throw new KeyNotFoundException();
            }

            return colaboradores.Select(c => new ColaboradorReadDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Workshops = c.WorkshopColaboradores
                    .Select(wc => new WorkshopReadDTOSimples
                    {
                        Id = wc.Workshop.Id,
                        Nome = wc.Workshop.Nome,
                        DataRealizacao = wc.Workshop.DataRealizacao,
                        Descricao = wc.Workshop.Descricao
                    }).ToList()
            }).ToList();
        }

        public async Task<List<ColaboradorReadDTO>> GetByName(string name)
        {
            var colaboradores = await _Context.Colaboradores
                .Where(c => c.Nome.Contains(name))
                .Include(c => c.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Workshop)
                .ToListAsync();

            if (colaboradores == null || colaboradores.Count == 0)
            {
                throw new KeyNotFoundException();
            }

            return colaboradores.Select(c => new ColaboradorReadDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Workshops = c.WorkshopColaboradores
                    .Select(wc => new WorkshopReadDTOSimples
                    {
                        Id = wc.Workshop.Id,
                        Nome = wc.Workshop.Nome,
                        DataRealizacao = wc.Workshop.DataRealizacao,
                        Descricao = wc.Workshop.Descricao
                    }).ToList()
            }).ToList();
        }

        public async Task Update(int id, ColaboradorCreateDTO dto)
        {
            var colaborador = await _Context.Colaboradores.FindAsync(id);

            if (colaborador == null)
            {
                throw new KeyNotFoundException();
            }

            colaborador.Nome = dto.Nome;

            var relacoesExistentes = _Context.WorkshopColaboradores.Where(wc => wc.ColaboradorId == id);

            _Context.WorkshopColaboradores.RemoveRange(relacoesExistentes);

            var workshopsValidos = await _Context.Workshops
                .Where(w => dto.WorkshopId.Contains(w.Id))
                .ToListAsync();

            if (workshopsValidos == null || workshopsValidos.Count == 0)
            {
                throw new ArgumentException();
            }

            foreach (var workshop in workshopsValidos)
            {
                var relacao = new WorkshopColaboradores
                {
                    WorkshopId = workshop.Id,
                    ColaboradorId = colaborador.Id
                };
                _Context.WorkshopColaboradores.Add(relacao);
            }

            await _Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var colaborador = await _Context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                throw new KeyNotFoundException();
            }

            var relacoes = _Context.WorkshopColaboradores.Where(wc => wc.ColaboradorId == id);
            _Context.WorkshopColaboradores.RemoveRange(relacoes);

            _Context.Colaboradores.Remove(colaborador);
            await _Context.SaveChangesAsync();
        }
        
    }
}
