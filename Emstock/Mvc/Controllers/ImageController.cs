using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Models;

namespace Mvc.Controllers
{
    public class ImageController : Controller
    {
        private readonly EmstockContext _context;

        public ImageController(EmstockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);

            if (image == null) return NotFound();
            return View(image);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageMimeType,Thumbnail,ImageData")] Image image)
        {
            if (!ModelState.IsValid) return View(image);

            _context.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);

            if (image == null) return NotFound();
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ImageMimeType,Thumbnail,ImageData")] Image image)
        {
            if (id != image.Id) return NotFound();
            if (!ModelState.IsValid) return View(image);
            try
            {
                _context.Update(image);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(image.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);

            if (image == null) return NotFound();
            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var image = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(long id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
