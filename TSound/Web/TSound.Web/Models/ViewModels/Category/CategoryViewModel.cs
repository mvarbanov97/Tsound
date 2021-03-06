﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Category
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }
    }
}
