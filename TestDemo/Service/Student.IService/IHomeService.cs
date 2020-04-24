using Student.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student.IService
{
    public interface IHomeService 
    {
        List<StudentInfo> GetAllStudentInfos(int pageIndex, int pageSize);
    }
}
