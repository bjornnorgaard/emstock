using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Type = Models.Type;

namespace Mvc.Controllers
{
    public class TypeController : Controller
    {
        private readonly EmstockContext _context;

        public TypeController(EmstockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Types.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types
                .Include(t => t.CategoryTypes)
                .ThenInclude(t => t.Category)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info,Location,Status,Datasheet,ImageUrl,Manufacturer,WikiLink,AdminComment")] Type type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types.SingleOrDefaultAsync(m => m.Id == id);

            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Info,Location,Status,Datasheet,ImageUrl,Manufacturer,WikiLink,AdminComment")] Type type)
        {
            if (id != type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(type.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = await _context.Types
                .SingleOrDefaultAsync(m => m.Id == id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var type = await _context.Types.SingleOrDefaultAsync(m => m.Id == id);
            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(long id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
    }
}
