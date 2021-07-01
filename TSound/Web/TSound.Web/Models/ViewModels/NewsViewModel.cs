using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels
{
    public class NewsViewModel
    {
        public string Title { get; set; }

        public string Publisher { get; set; }

        public string Content { get; set; }

        public string Link { get; set; }

        public string ImageUrl { get; set; }
    }
}
