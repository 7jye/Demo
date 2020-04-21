using Microsoft.AspNetCore.Mvc;
using Student.IService;

namespace SS.Controllers
{
    public class IndexController : Controller
    {
        private readonly IIndexService _indexService;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int id)
        {
            var test1 = _indexService.test1(id);
            return View(id);
        }

    }
}