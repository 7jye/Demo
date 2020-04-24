using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.ViewModels
{
    public class BaseViewModel<T>
        where T : class
    {
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int Total { get; set; }

        public List<T> list { get; set; }
    }
}
