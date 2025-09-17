using api_workshops.Data;
using api_workshops.Interface;
using api_workshops.Models;
using api_workshops.Models.DTOs;
using api_workshops.Models.DTOs.Read;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_workshops.Service
{
    public class WorkshopService : IWorkshopService
    {
        private readonly AppDbContext _Context;

        public WorkshopService(AppDbContext appDbContext)
        {
            _Context = appDbContext;
        }

        public async Task<WorkshopReadDTO> Create(WorkshopCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentNullException();
            
            if(dto.DataRealizacao < DateTime.Now)
                throw new ArgumentException();

            var workshopExistente = await _Context.Workshops
                .FirstOrDefaultAsync(w => w.Nome.ToLower() == dto.Nome.ToLower());

            if (workshopExistente != null)
                throw new InvalidOperationException();

            var workshop = new Workshop
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DataRealizacao = dto.DataRealizacao
            };

            _Context.Workshops.Add(workshop);

            if (dto.ColaboradorId != null && dto.ColaboradorId.Any())
            {
                var colaboradoresValidos = await _Context.Colaboradores
                    .Where(c => dto.ColaboradorId.Contains(c.Id))
                    .ToListAsync();
                
                if (colaboradoresValidos == null || colaboradoresValidos.Count == 0)
                {
                    throw new InvalidDataException();
                }
                
                foreach (var colaborador in colaboradoresValidos)
                {
                    var relacao = new WorkshopColaboradores
                    {
                        Workshop = workshop,
                        Colaborador = colaborador,
                        Presente = false
                    };
                    _Context.WorkshopColaboradores.Add(relacao);
                }
            }

            await _Context.SaveChangesAsync();

            var workshopCarregado = await _Context.Workshops
                .Where(w => w.Id == workshop.Id)
                .Include(w => w.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Colaborador)
                .FirstOrDefaultAsync();

            return new WorkshopReadDTO
            {
                Id = workshopCarregado.Id,
                Nome = workshopCarregado.Nome,
                Descricao = workshopCarregado.Descricao,
                DataRealizacao = workshopCarregado.DataRealizacao,
                Colaboradores = workshopCarregado.WorkshopColaboradores
                    .Select(wc => new ColaboradorReadDTOSimples
                    {
                        Id = wc.Colaborador.Id,
                        Nome = wc.Colaborador.Nome
                    })
                    .ToList()
            };
        }

        public async Task<bool> AtualizarPresencaAsync(int workshopId, int colaboradorId, bool presente)
        {
            var participacao = await _Context.WorkshopColaboradores
                .FirstOrDefaultAsync(x => x.WorkshopId == workshopId && x.ColaboradorId == colaboradorId);

            if (participacao == null)
            {
                throw new Exception(); 
            }

            participacao.Presente = presente;
            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<WorkshopReadDTO> GetById(int id)
        {
            var workshop = await _Context.Workshops
                .Where(w => w.Id == id)
                .Include(w => w.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Colaborador)
                .FirstOrDefaultAsync();

            if (workshop == null)
                throw new KeyNotFoundException();

            return new WorkshopReadDTO
            {
                Id = workshop.Id,
                Nome = workshop.Nome,
                Descricao = workshop.Descricao,
                DataRealizacao = workshop.DataRealizacao,
                Colaboradores = workshop.WorkshopColaboradores
                    .Select(wc => new ColaboradorReadDTOSimples
                    {
                        Id = wc.Colaborador.Id,
                        Nome = wc.Colaborador.Nome,
                        Presente = wc.Presente
                    })
                    .ToList()
            };
        }

        public async Task<List<WorkshopReadDTO>> GetAll()
        {
            var workshops = await _Context.Workshops
                .Include(w => w.WorkshopColaboradores)
                    .ThenInclude(wc => wc.Colaborador)
                .ToListAsync();

            if (workshops == null || !workshops.Any()) {
                throw new KeyNotFoundException();
            }

            return workshops.Select(workshop => new WorkshopReadDTO
            {
                Id = workshop.Id,
                Nome = workshop.Nome,
                Descricao = workshop.Descricao,
                DataRealizacao = workshop.DataRealizacao,
                Colaboradores = workshop.WorkshopColaboradores
                    .Select(wc => new ColaboradorReadDTOSimples
                    {
                        Id = wc.Colaborador.Id,
                        Nome = wc.Colaborador.Nome,
                        Presente = wc.Presente
                    })
                    .ToList()
            }).ToList();
        }

        public async Task<List<WorkshopReadDTO>> GetByName(string nome)
        {
            var workshops = await _Context.Workshops
                .Where(w => w.Nome.ToLower().Contains(nome.ToLower()))
                .Select(w => new WorkshopReadDTO
                {
                    Id = w.Id,
                    Nome = w.Nome,
                    Descricao = w.Descricao,
                    DataRealizacao = w.DataRealizacao,
                    Colaboradores = w.WorkshopColaboradores
                        .Select(wc => new ColaboradorReadDTOSimples
                        {
                            Id = wc.Colaborador.Id,
                            Nome = wc.Colaborador.Nome,
                            Presente = wc.Presente
                        })
                        .ToList()
                })
                .ToListAsync();
            if (workshops == null || !workshops.Any())
            {
                throw new KeyNotFoundException();
            }
            return workshops;
        }
        
        public async Task Update(int id, WorkshopCreateDTO dto)
        {
            var workshop = await _Context.Workshops.FindAsync(id);
            if (workshop == null)
                throw new KeyNotFoundException();

            workshop.Nome = dto.Nome;
            workshop.Descricao = dto.Descricao;
            workshop.DataRealizacao = dto.DataRealizacao;

            var colaboradoresExistentes = _Context.WorkshopColaboradores.Where(wc => wc.WorkshopId == id);

            _Context.WorkshopColaboradores.RemoveRange(colaboradoresExistentes);

            if (dto.ColaboradorId != null && dto.ColaboradorId.Any())
            {
                var colaboradoresValidosIds = await _Context.Colaboradores
                    .Where(c => dto.ColaboradorId.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();

                if (colaboradoresValidosIds == null || colaboradoresValidosIds.Count == 0)
                {
                    throw new ArgumentException();
                }

                foreach (var colaboradorId in colaboradoresValidosIds)
                {
                    var relacao = new WorkshopColaboradores
                    {
                        WorkshopId = workshop.Id,
                        ColaboradorId = colaboradorId
                    };
                    _Context.WorkshopColaboradores.Add(relacao);
                }
            }
            await _Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var workshop = await _Context.Workshops.FindAsync(id);
            if (workshop == null)
                throw new KeyNotFoundException("Workshop não encontrado.");

            _Context.Workshops.Remove(workshop);
            await _Context.SaveChangesAsync();
        }

    }
}
