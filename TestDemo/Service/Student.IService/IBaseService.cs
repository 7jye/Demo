using System;
using System.Collections.Generic;
using System.Text;

namespace Student.IService
{
    public interface IBaseService
    {
        object ConvertToViewModel(object entity);
        object ConvertToViewModelList(object entityList);
    }
}
