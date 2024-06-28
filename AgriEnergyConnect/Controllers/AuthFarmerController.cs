using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class AuthFarmerController : Controller
    {
        private readonly AgriEnergyConnectContext _context;
        private readonly PasswordService _passwordService;
        private readonly ILogger<AuthFarmerController> _logger;

        public AuthFarmerController(AgriEnergyConnectContext context, PasswordService passwordService, ILogger<AuthFarmerController> logger)
        {
            _context = context;
            _passwordService = passwordService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult RegisterFarmer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Farmers.AnyAsync(f => f.Name.Equals(farmer.Name)))
                {
                    ViewBag.Error = "A farmer with the same name already exists.";
                    return View(farmer);
                }

                // Hash the password
                byte[] salt = _passwordService.GenerateSalt();
                string passwordWithSalt = farmer.Password + Convert.ToBase64String(salt);
                farmer.PasswordHash = _passwordService.HashPassword(passwordWithSalt);

                // Optionally clear the plain text password
                farmer.Password = null;

                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                ViewBag.Message = $"Farmer {farmer.Name} added successfully.";
                return RedirectToAction("Index", "EmployeeDashboard");
            }

            return View(farmer);
        }
    }
}

public class PasswordService
{
    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }

    public string HashPassword(string passwordWithSalt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(bytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
