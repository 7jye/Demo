using IRepository;
using Microsoft.Extensions.Options;
using Student.Domain.Entities;
using Student.IService;
using System.Collections.Generic;
using ThisInfrastruture.DataStructure;

namespace Student.Service
{
    public class HomeService : BaseService, IHomeService 
    {
        #region 字段
        private readonly IHomeRepository _studentInfoOrderRepository;
        //private readonly CustomSettings _customSettings;
        #endregion
        public HomeService(IHomeRepository studentInfoOrderRepository
           )//IOptions<CustomSettings> customSettingsOption
        {
            _studentInfoOrderRepository = studentInfoOrderRepository;
            //_customSettings = customSettingsOption.Value;
        }

        public List<StudentInfo> GetAllStudentInfos(int pageIndex, int pageSize)
        {
            List<StudentInfo> studentAll = _studentInfoOrderRepository.GetAllStudentInfos(int pageIndex, int pageSize);
            return studentAll;

        }

    }
}
