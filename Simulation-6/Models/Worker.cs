using Simulation_6.Models.Common;
using System.Diagnostics.Contracts;

namespace Simulation_6.Models
{
    public class Worker:BaseEntity
    {
        public  string Fullname { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
