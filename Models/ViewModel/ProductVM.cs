﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Awake_Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? AppTypes { get; set; }
    }
}
