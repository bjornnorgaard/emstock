using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Enums;
using Mvc.ViewModels;
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
            var types = await _context.Types
                .Include(x => x.CategoryTypes)
                .ThenInclude(t => t.Category).ToListAsync();

            return View(types);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var type = await _context.Types
                .Include(t => t.CategoryTypes)
                .ThenInclude(t => t.Category)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (type == null) return NotFound();

            return View(type);
        }

        public IActionResult Create()
        {
            var model = new TypeViewModel();

            var categories = _context.Categories.ToList();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeViewModel typeViewModel)
        {
            if (!ModelState.IsValid) return View(typeViewModel);

            var categories = _context.Categories
                .Where(l => typeViewModel.SelectedCategories.Contains(l.Id.ToString()));

            typeViewModel.Type.CategoryTypes = new List<CategoryType>();
            foreach (var category in categories)
            {
                typeViewModel.Type.CategoryTypes.Add(new CategoryType { CategoryId = category.Id, Type = typeViewModel.Type });
            }

            _context.Add(typeViewModel.Type);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)return NotFound();
            var model = new TypeViewModel();

            var categories = _context.Categories.ToListAsync();
            var type = _context.Types.Include(x => x.CategoryTypes)
                .SingleOrDefaultAsync(m => m.Id == id);

            Task.WaitAll(categories, type);

            model.Categories = categories.Result.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Type = type.Result;
            model.SelectedCategories = type.Result.CategoryTypes.Select(x => x.CategoryId.ToString()).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id)
        {
            var editType = CreateTypeFromRequestBody(Request.Form, id);
            if (!ModelState.IsValid) return View(new TypeViewModel {Type = editType});
            try
            {
                _context.Update(editType);
                UpdateManyToManyRelationship(Request.Form, id);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExists(editType.Id))return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private void UpdateManyToManyRelationship(IFormCollection requestForm, long id)
        {
            var categoryTypes = _context.CategoryType.Where(x => x.TypeId == id);
            if (!requestForm.TryGetValue("SelectedCategories", out var categoryIds)) return;

            _context.CategoryType.RemoveRange(categoryTypes);
            _context.SaveChanges();

            var categoryList = new List<string>(categoryIds.ToString().Split(','))
                .Select(x => new CategoryType {CategoryId = long.Parse(x), TypeId = id});

            _context.CategoryType.AddRange(categoryList);
        }


        private static Type CreateTypeFromRequestBody(IFormCollection requestForm, long id)
        {
            var editType = new Type {Id = id};

            if (requestForm.TryGetValue("Type.Name", out var name))editType.Name = name;
            if (requestForm.TryGetValue("Type.Info", out var info))editType.Info = info;
            if (requestForm.TryGetValue("Type.Location", out var location))editType.Location = location;
            if (requestForm.TryGetValue("Type.Datasheet", out var dataSheet))editType.Datasheet = dataSheet;
            if (requestForm.TryGetValue("Type.ImageUrl", out var imageUrl))editType.ImageUrl = imageUrl;
            if (requestForm.TryGetValue("Type.Manufacturer", out var manufacturer))editType.Manufacturer = manufacturer;
            if (requestForm.TryGetValue("Type.WikiLink", out var wikiLink))editType.WikiLink = wikiLink;
            if (requestForm.TryGetValue("Type.AdminComment", out var adminComment))editType.AdminComment = adminComment;
            if (requestForm.TryGetValue("Type.Status", out var status))
            {
                if (Enum.TryParse(status, out ComponentTypeStatus statusEnum))editType.Status = statusEnum;
            }

            return editType;
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)return NotFound();
            var type = await _context.Types.SingleOrDefaultAsync(m => m.Id == id);
            if (type == null)return NotFound();
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
