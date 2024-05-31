using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly AgriEnergyConnectContext _context;

        public EmployeeDashboardController(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterFarmer()
        {
            return View();
        }

        public async Task<IActionResult> Search(string searchTerm, DateTime? startDate, DateTime? endDate)
        {
            // Initialize an empty IQueryable for products
            var products = _context.Products.AsQueryable();

            // Filter based on search term 
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.Category.Contains(searchTerm) ||
                    p.Farmer.Name.Contains(searchTerm));
            }

            // Filter based on date range (using nullable date comparison)
            if (startDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate <= endDate.Value);
            }

            // Load filtered products as a list for view
            var productList = await products.ToListAsync();

            // Return filtered products to the view
            return View(productList);
        }


    }

}
