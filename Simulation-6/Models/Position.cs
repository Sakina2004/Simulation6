using Simulation_6.Models.Common;

namespace Simulation_6.Models
{
    public class Position:BaseEntity
    {
        public string Fullname { get; set; }
       public IEnumerable<Position> Positions { get; set; }
    }
}
