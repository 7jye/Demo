using IRepository;
using Microsoft.EntityFrameworkCore;
using Student.Domain;
using Student.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
   public partial class HomeRepository : RepositoryBase<StudentInfo, DemoDbContext>, IHomeRepository
    {
        public HomeRepository(DemoDbContext dbContext)
           : base(dbContext)
        {

        }

        public List<StudentInfo> GetAllStudentInfos(int pageIndex, int pageSize) {
            int total = base.Table.Count();
            List<StudentInfo> studentAll = base.Table.OrderByDescending(o => o.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return studentAll;
        
        }

    }
}
