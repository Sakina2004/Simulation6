namespace Simulation_6.ViewModel.WorkerVIewModels
{
    public class WorkerUpdateVm
    {
        public  int Id { get; set; }
        public string Fullname { get; set; }
        public IFormFile ImagePath { get; set; }
        public string Description { get; set; }
        public int PositionId { get; set; }
        public string Image { get; set; }

    }
}
