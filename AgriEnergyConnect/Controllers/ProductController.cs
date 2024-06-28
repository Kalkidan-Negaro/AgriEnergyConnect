using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductController : Controller
    {
        private readonly AgriEnergyConnectContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(AgriEnergyConnectContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Dashboard()
        {
           
            int farmerId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            // Query products where FarmerID matches the logged-in farmer's ID
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
                // Retrieve FarmerID from session
                int farmerId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("UserID");

                // Assign FarmerID to the new product
                product.FarmerID = farmerId;

                // Add product to context and save changes
                _context.Add(product);
                await _context.SaveChangesAsync();

                // Redirect to the Dashboard action
                return RedirectToAction(nameof(Dashboard));
            }

            return View(product);
        }
    }
}
