using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Category
{
    public class CollectionCategoriesFullViewModel
    {
        public IEnumerable<CategoryFullViewModel> Collection { get; set; }
    }
}
