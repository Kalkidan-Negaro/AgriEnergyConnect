using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AgriEnergyConnect.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;

namespace AgriEnergyConnect.Controllers
{
    public class LoginController : Controller
    {
        private readonly AgriEnergyConnectContext db;
        private readonly ILogger<LoginController> logger;

        public LoginController(AgriEnergyConnectContext context, ILogger<LoginController> logger)
        {
            db = context;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult EmployeeLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = db.Employees
                    .Where(e => e.Name == model.Name && e.PasswordHash == model.Password)
                    .FirstOrDefault();

                if (employee != null)
                {
                    ViewBag.Message = "Login successful!";
                    ViewBag.LoginError = null;
                    HttpContext.Session.SetInt32("UserID", employee.EmployeeID);
                    HttpContext.Session.SetString("UserRole", "Employee");
                    return RedirectToAction("Index", "EmployeeDashboard");
                }
                else
                {
                    logger.LogWarning($"Unsuccessful login attempt for employee: {model.Name}");
                    ViewBag.LoginError = "Incorrect details. Please try again.";
                }
            }

            return View("Index", model);
        }


        [HttpPost]
        public IActionResult FarmerLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find farmer by name 
                Employee farmer = db.Employees.FirstOrDefault(f => f.Name == model.Name);

                if (farmer != null)
                {
                    // Validate password using Employee table with FarmerID 
                    Employee employee = db.Employees.FirstOrDefault(u => u.FarmerID == farmer.FarmerID);

                    if (employee != null)
                    {
                        if (employee.PasswordHash != null && model.Password == employee.PasswordHash) 
                        {
                            ViewBag.Message = "Login successful!";
                            ViewBag.LoginError = null;
                            HttpContext.Session.SetInt32("UserID", employee.EmployeeID); 
                            HttpContext.Session.SetString("UserRole", "Farmer"); // Set user role as "Farmer"
                            return RedirectToAction("Index", "FarmerDashboard");
                        }
                        else
                        {
                            logger.LogWarning($"Unsuccessful login attempt for farmer name: {model.Name}");
                            ViewBag.LoginError = "Incorrect details. Please try again.";
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Employee not found for farmer name: {model.Name}");
                        ViewBag.LoginError = "Incorrect details. Please try again.";
                    }
                }
                else
                {
                    logger.LogWarning($"Farmer with name {model.Name} not found.");
                    ViewBag.LoginError = "Incorrect details. Please try again.";
                }
            }

            return View("Index", model);
        }


    }
}


