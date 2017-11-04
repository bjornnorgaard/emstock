using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace Mvc.ViewModels
{
    public class TypeViewModel
    {
        public Type Type { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<string> SelectedCategories { get; set; }
    }
}

