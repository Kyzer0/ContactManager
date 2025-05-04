using Microsoft.AspNetCore.Mvc;

namespace CrudOperationFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
