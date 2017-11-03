using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.ViewModels;

namespace Mvc.Controllers
{
    public class ComponentController : Controller
    {
        private readonly EmstockContext _context;

        public ComponentController(EmstockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Components.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var component = await _context.Components.SingleOrDefaultAsync(m => m.Id == id);
            if (component == null) return NotFound();
            return View(component);
        }

        public IActionResult Create()
        {
            var model = new ComponentViewModel();
            var types = _context.Types.ToList();
            model.Types = types.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentViewModel componentViewModel)
        {
            if (!ModelState.IsValid) return View(componentViewModel);

            var type = _context.Types.FirstOrDefault(x => x.Id == int.Parse(componentViewModel.TypeString));
            componentViewModel.Component.Type = type;
            componentViewModel.Component.TypeId = (int)type.Id;
            _context.Add(componentViewModel.Component);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var component = await _context.Components.SingleOrDefaultAsync(m => m.Id == id);
            var model = new ComponentViewModel();
            var types = _context.Types.ToList();
            model.TypeString = component.TypeId.ToString();
            model.Types = types.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Component = component;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Component,TypeString")] ComponentViewModel component)
        {
            if (id != component.Component.Id) return NotFound();
            if (!ModelState.IsValid) return View(component);

            try
            {
                var type = _context.Types.FirstOrDefault(x => x.Id == int.Parse(component.TypeString));
                component.Component.Type = type;
                component.Component.Id = (int)type.Id;

                _context.Update(component);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(component.Component.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var component = await _context.Components.SingleOrDefaultAsync(m => m.Id == id);
            if (component == null) return NotFound();

            return View(component);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var component = await _context.Components.SingleOrDefaultAsync(m => m.Id == id);
            _context.Components.Remove(component);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentExists(long id)
        {
            return _context.Components.Any(e => e.Id == id);
        }
    }
}
