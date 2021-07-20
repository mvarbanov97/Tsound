using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data;
using TSound.Data.UnitOfWork;

namespace TSound.Services.Tests.Infrastructure
{
    public abstract class BaseTest
    {
        protected readonly TSoundDbContext dbContext;
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;

        public BaseTest()
        {
            this.dbContext = TSoundDbContextFactory.Create();
            this.mapper = AutoMapperFactory.Create();
            this.unitOfWork = new UnitOfWork(dbContext);
        }
    }
}
