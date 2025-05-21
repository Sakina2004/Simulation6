using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Simulation_6.DataAccessLayer;
using Simulation_6.Models;
using Simulation_6.ViewModel.PositionViewModels;
using Simulation_6.ViewModel.WorkerVIewModels;
using System.Reflection;

namespace Simulation_6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkerController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var worker = await _context.Workers.Select(x => new WorkerGetVm
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Description = x.Description,
                ImagePath = x.ImagePath,
                PositionId = x.PositionId,
            }).ToListAsync();
            return View(worker);
        }
        private async Task ViewBags()
        {
            var positions = await _context.Positions.Select(x => new PositionGetvm
            {
                Id = x.Id,
                fullname = x.Fullname,
            }).ToListAsync();
            ViewBag.Positions = positions;
        }
        public async Task<IActionResult>Create()
        {
            await ViewBags();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(WorkerCreateVm vm)
        {
            await ViewBags();

            if (!ModelState.IsValid)
                return View(vm);
            if(vm.Image.Length*1024*1024>2)
            {
                ModelState.AddModelError("image", "Seklin olcusu 2kb-i kece bilmez!");
            }
            if(!vm.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("image", "Datani sekil formatinda daxiol edin! ");
            }
            string filename = Guid.NewGuid().ToString() + vm.Image.FileName;
            string path = Path.Combine("wwwroot", "images", filename);
            using FileStream stream = new(path, FileMode.OpenOrCreate);
            await vm.Image.CopyToAsync(stream);
                Worker worker = new()
                {
                    Fullname = vm.Fullname,
                    Description = vm.Description,
                    ImagePath =filename,
                    PositionId = vm.PositionId,
                };
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Delete(int? id)
        {
            if (!id.HasValue || id < 1)
                return BadRequest();
            var entity = await _context.Workers.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (entity == 0)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }
      public async Task<IActionResult>Update(int? id)
        {
            await ViewBags();
            if (!id.HasValue||id<1)
                return BadRequest();
            var workers = await _context.Workers.Where(x => x.Id == id).Select(x => new WorkerUpdateVm()
            {
                Fullname = x.Fullname,
                Description=x.Description,
           }).FirstOrDefaultAsync();
            if (workers is null)
                return NotFound();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int?id,WorkerUpdateVm vm)
        {
            await ViewBags();
            if (!id.HasValue || id < 1)
                return BadRequest();
            var entity = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                return BadRequest();
            if(vm.Image is not null )
            {
                if(!vm.ImagePath.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("image", "Datanin sekil formasinda olduguna diqqet edekkk!");
                    return View(vm);
                }
                if(vm.ImagePath.Length*1024*1024>2 )
                {
                    ModelState.AddModelError("image", "Sekilin olcusu 2 kb-i asmamalidir!");
                    return View(vm);
                }
                string filename = Guid.NewGuid().ToString() + vm.ImagePath.FileName;
                string path = Path.Combine("wwwroot", "images", filename);
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                await vm.ImagePath.CopyToAsync(fs);
                entity.ImagePath = filename;
            }
                entity.Description = vm.Description;
                entity.Fullname = vm.Fullname;
                entity.PositionId = vm.PositionId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

    }
}