using IRepository;
using Microsoft.EntityFrameworkCore;
using Student.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class RepositoryBase<T, TDBContext> : DefaultRepositoryImpl<T, TDBContext>, IBaseRepository<T>
        where T : class
        where TDBContext : DbContext
    {
        public RepositoryBase(TDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
   
