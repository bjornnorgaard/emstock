using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
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
            var types = await _context.Types.Include(x => x.CategoryTypes)
                .ThenInclude(t => t.Category).ToListAsync();

            return View(types);
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
            var model = new TypeViewModel();

            var categories = _context.Categories.ToList();
            model.Categories = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeViewModel typeViewModel)
        {
            if (ModelState.IsValid)
            {
                var categories = _context.Categories.Where(l => typeViewModel.SelectedCategories.Contains(l.Id.ToString()));
                
                typeViewModel.Type.CategoryTypes = new List<CategoryType>();
                foreach (var category in categories)
                {
                    typeViewModel.Type.CategoryTypes.Add(new CategoryType{CategoryId = category.Id, Type = typeViewModel.Type});
                }

                _context.Add(typeViewModel.Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeViewModel);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new TypeViewModel();

            var categories = _context.Categories.ToListAsync();
            var type = _context.Types.Include(x => x.CategoryTypes)
                .SingleOrDefaultAsync(m => m.Id == id);

            Task.WaitAll(categories, type);

            model.Categories = categories.Result.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Type = type.Result;
            model.SelectedCategories = type.Result.CategoryTypes.Select(x => x.CategoryId.ToString()).ToList();

            if (type == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id)
        {
            var editType = CreateTypeFromRequestBody(Request.Form, id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editType);
                    UpdateManyToManyRelationship(Request.Form, id);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(editType.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(new TypeViewModel{Type = editType});
        }

        private void UpdateManyToManyRelationship(IFormCollection requestForm, long id)
        {
            //Remove old relations
            IQueryable<CategoryType> categoryTypes = _context.CategoryType.Where(x => x.TypeId == id);
            
            StringValues categoryIds;
            if (requestForm.TryGetValue("SelectedCategories", out categoryIds))
            {
                // Remove old category types
                _context.CategoryType.RemoveRange(categoryTypes);

                _context.SaveChanges();

                var categoryList = new List<string>(categoryIds.ToString().Split(','))
                    .Select(x => new CategoryType{CategoryId = long.Parse(x), TypeId = id});

                _context.CategoryType.AddRange(categoryList);
            }
        }


        private Type CreateTypeFromRequestBody(IFormCollection requestForm, long id)
        {
            Type editType = new Type();
            editType.Id = id;

            StringValues name;
            if (requestForm.TryGetValue("Type.Name", out name))
                editType.Name = name;
            StringValues info;
            if (requestForm.TryGetValue("Type.Info", out info))
                editType.Info = info;
            StringValues location;
            if (requestForm.TryGetValue("Type.Location", out location))
                editType.Location = location;
            StringValues status;
            if (requestForm.TryGetValue("Type.Status", out status))
                if (Enum.TryParse(status, out ComponentTypeStatus statusEnum))
                    editType.Status = statusEnum;
            StringValues dataSheet;
            if (requestForm.TryGetValue("Type.Datasheet", out dataSheet))
                editType.Datasheet = dataSheet;
            StringValues imageUrl;
            if (requestForm.TryGetValue("Type.ImageUrl", out imageUrl))
                editType.ImageUrl = imageUrl;
            StringValues manufacturer;
            if (requestForm.TryGetValue("Type.Manufacturer", out manufacturer))
                editType.Manufacturer = manufacturer;
            StringValues wikiLink;
            if (requestForm.TryGetValue("Type.WikiLink", out wikiLink))
                editType.WikiLink = wikiLink;
            StringValues adminComment;
            if (requestForm.TryGetValue("Type.AdminComment", out adminComment))
                editType.AdminComment = adminComment;

            return editType;
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
