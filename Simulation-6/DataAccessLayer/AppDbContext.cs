using Microsoft.EntityFrameworkCore;
using Simulation_6.Models;
using Simulation_6.ViewModel.WorkerVIewModels;

namespace Simulation_6.DataAccessLayer
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Worker> Workers { get; set; }
    }
}
