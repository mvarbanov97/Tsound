using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Areas.Administration.Controllers
{
    public class HomeController : AdminController
    {
        public async Task<IActionResult> Welcome()
        {
            return await Task.Run(() => View());
        }
    }
}
