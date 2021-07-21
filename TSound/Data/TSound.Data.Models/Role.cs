using Microsoft.AspNetCore.Identity;
using System;

namespace TSound.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
            : this(null)
        {
        }

        public Role(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid();
        }
    }
}
