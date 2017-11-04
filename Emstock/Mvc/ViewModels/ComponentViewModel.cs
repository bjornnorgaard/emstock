using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace Mvc.ViewModels
{
    public class ComponentViewModel
    {
        public Component Component { get; set; }
        public List<SelectListItem> Types { get; set; }
        public string TypeId { get; set; }
    }
}