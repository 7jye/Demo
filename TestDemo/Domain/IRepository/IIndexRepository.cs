using Student.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepository
{
   public interface IIndexRepository:IBaseRepository<StudentInfo>
    {
        string test1(int id);
    }
}
