using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductController : Controller
    {
        private readonly AgriEnergyConnectContext _context;

        public ProductController(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            int farmerId = (int)HttpContext.Session.GetInt32("UserID");
            var products = await _context.Products
                                         .Where(p => p.FarmerID == farmerId)
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
                int farmerId = (int)HttpContext.Session.GetInt32("UserID");
                product.FarmerID = farmerId;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            return View(product);
        }
    }
}
