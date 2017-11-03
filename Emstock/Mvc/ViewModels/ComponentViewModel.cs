using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Type = Models.Type;

namespace Mvc.ViewModels
{
    public class ComponentViewModel
    {
        private Component _component = new Component();

        public List<SelectListItem> Types { get; set; }

        public string TypeString { get; set; }

        public Component Component
        {
            get { return _component; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _component = value;
            }
        }
    }
}