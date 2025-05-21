using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
//using Simulation_6.Models;

namespace Simulation_6.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

       
    }
}
