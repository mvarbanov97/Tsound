﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TSound.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider);
    }
}
