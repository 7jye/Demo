using Student.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRepository
{
   public interface IHomeRepository : IBaseRepository<StudentInfo>
    {

        #region Customized
        List<StudentInfo> GetAllStudentInfos(int pageIndex, int pageSize);

        #endregion
    }
}
