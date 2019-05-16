using Microsoft.AspNetCore.Mvc;

namespace StockManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}