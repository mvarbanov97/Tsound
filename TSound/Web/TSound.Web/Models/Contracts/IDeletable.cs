using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.Contracts
{
    public interface IDeletable
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string NameController { get; set; }

        bool IsToBeDeletedByAdmin { get; set; }

        int CurrentPage { get; set; }
    }
}
