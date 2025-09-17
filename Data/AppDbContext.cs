using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_workshops.Models;
using Microsoft.EntityFrameworkCore;

namespace api_workshops.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<WorkshopColaboradores> WorkshopColaboradores { get; set; }
    }
}