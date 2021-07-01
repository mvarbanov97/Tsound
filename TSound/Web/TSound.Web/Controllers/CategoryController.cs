using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models.ViewModels.Category;

namespace TSound.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService genreService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService genreService, IMapper mapper)
        {
            this.genreService = genreService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All()
        {
            var genres = await this.genreService.GetAllCategoriesAsync();

            var genreLightViewModel = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(genres);

            var model = new CollectionCategoriesFullViewModel
            {
                Collection = genreLightViewModel
            };

            return this.View(model);
        }
    }
}
