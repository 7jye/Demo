using Student.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student.Service
{
    public class IndexService : BaseService, IIndexService
    {
        public string test1(int id)
        {
            string test1 = $"Service_test,id为：{id}";
            return test1;

        }

    }
}
