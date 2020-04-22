using IRepository;
using Student.Domain.Entities;
using Student.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student.Service
{
    public class IndexService : BaseService, IIndexService 
    {
        #region 字段
        private readonly IIndexRepository<StudentInfo> _studentInfoOrderRepository;
        #endregion


        public string test1(int id)
        {
            string test1 = $"Service_test,id为：{id}";
            return test1;

        }

    }
}
