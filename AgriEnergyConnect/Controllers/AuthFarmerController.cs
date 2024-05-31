using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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

                string password = _passwordService.GenerateRandomPassword(12, true);
                byte[] salt = _passwordService.GenerateSalt();
                string passwordWithSalt = password + Convert.ToBase64String(salt);
                string hashedPassword = _passwordService.HashPassword(passwordWithSalt);
                farmer.PasswordHash = hashedPassword;

                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                ViewBag.Message = $"Farmer {farmer.Name} added successfully. Auto-generated password (masked): {_passwordService.MaskPassword(password)}";
                return RedirectToAction("Index", "Home");
            }

            return View(farmer);
        }
    }
}

public class PasswordService
{
    private readonly string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public string GenerateRandomPassword(int length, bool includeSpecialChars = false)
    {
        StringBuilder stringBuilder = new StringBuilder();
        Random random = new Random();

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(validChars[random.Next(validChars.Length)]);
        }

        return stringBuilder.ToString();
    }

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

    public string MaskPassword(string password)
    {
        return password.Length > 2 ? password[0] + new string('*', password.Length - 2) + password[password.Length - 1] : password;
    }
}
