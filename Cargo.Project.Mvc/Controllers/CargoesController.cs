using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Cargo.Project.Mvc.Controllers
{
    public class CargoesController : Controller
    {
        private readonly IConfiguration _configuration;

        public CargoesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
