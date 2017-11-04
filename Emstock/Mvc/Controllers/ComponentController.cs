using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            return View(await _context.Components.Include(x => x.Type).ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components
                .SingleOrDefaultAsync(m => m.Id == id);
            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        public IActionResult Create()
        {
            var model = new ComponentViewModel();

            var types = _context.Types.ToList();
            model.Types = types.Select(x => new SelectListItem{Value = x.Id.ToString(), Text = x.Name}).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentViewModel componentViewModel)
        {
            if (ModelState.IsValid)
            {
                var type = _context.Types.FirstOrDefault(x => x.Id == int.Parse(componentViewModel.TypeId));
                componentViewModel.Component.Type = type;
                componentViewModel.Component.TypeId = (int)type.Id;

                _context.Add(componentViewModel.Component);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(componentViewModel);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components.SingleOrDefaultAsync(m => m.Id == id);
            var model = new ComponentViewModel();
            var types = _context.Types.ToList();
            model.TypeId = component.TypeId.ToString();
            model.Types = types.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Component = component;

            if (component == null)
            {
                return NotFound();
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id)
        {
            var editComponent = CreateComponentFromRequestBody(Request.Form, id);

            if (ModelState.IsValid)
            {
                try
                {
                    var type = _context.Types.FirstOrDefault(x => x.Id == editComponent.TypeId);
                    editComponent.Type = type;

                    _context.Update(editComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(editComponent.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(new ComponentViewModel{Component = editComponent,
                TypeId = editComponent.TypeId.ToString(),
                Types = _context.Types.Select(x => new SelectListItem{Text = x.Name,
                    Value = x.Id.ToString()}).ToList()});
        }

        private Component CreateComponentFromRequestBody(IFormCollection requestForm, long id)
        {
            Component editComponent = new Component();
            editComponent.Id = id;

            StringValues number;
            if (requestForm.TryGetValue("Component.Number", out number))
                editComponent.Number = int.Parse(number);
            StringValues serialNumber;
            if (requestForm.TryGetValue("Component.SerialNo", out serialNumber))
                editComponent.SerialNo = serialNumber;
            StringValues status;
            if (requestForm.TryGetValue("Component.Status", out status))
                if (Enum.TryParse(status, out ComponentStatus statusEnum))
                    editComponent.Status = statusEnum;
            StringValues adminComment;
            if (requestForm.TryGetValue("Component.AdminComment", out adminComment))
                editComponent.AdminComment = adminComment;
            StringValues userComment;
            if (requestForm.TryGetValue("Component.UserComment", out userComment))
                editComponent.UserComment = userComment;
            StringValues currentLoanInformationId;
            if (requestForm.TryGetValue("Component.CurrentLoanInformationId", out currentLoanInformationId))
                editComponent.CurrentLoanInformationId = StringValues.IsNullOrEmpty(currentLoanInformationId) ? (long?)null : long.Parse(currentLoanInformationId);
            
            
            StringValues TypeId;
            if (requestForm.TryGetValue("TypeId", out TypeId))
                editComponent.TypeId = int.Parse(TypeId);


            return editComponent;
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components
                .SingleOrDefaultAsync(m => m.Id == id);
            if (component == null)
            {
                return NotFound();
            }

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
