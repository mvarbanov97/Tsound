using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.User
{
    public class UsersCollectionViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
