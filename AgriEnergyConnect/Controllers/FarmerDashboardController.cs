using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AgriEnergyConnect.Controllers
{
   
    public class FarmerDashboardController : Controller
    {
        private readonly AgriEnergyConnectContext _context;

        public FarmerDashboardController(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userID == null || userRole != "Farmer")
            {
                return RedirectToAction("Index", "Login");
            }

            var products = await _context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
