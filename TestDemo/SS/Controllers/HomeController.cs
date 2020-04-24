using Microsoft.AspNetCore.Mvc;
using Student.IService;

namespace SS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _indexService;
        public HomeController(IHomeService indexService) {
            _indexService = indexService;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAllStudentInfos(int pageIndex,int pageSize)
        {
            var list = _indexService.GetAllStudentInfos(pageIndex, pageSize);
            var total = list.Count;
            return Json( list ,total);
        }

    }
}