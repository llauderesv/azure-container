using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureContainer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AzureContainer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string _message;
        private readonly string _configMessage;
        private readonly ProductDbContext _context;

        public HomeController(
            ILogger<HomeController> logger, 
            IConfiguration config, 
            ProductDbContext context)
        {
            _logger = logger;
            _message = $"Essential Docker ({config["HOSTNAME"]})";
            _configMessage = $"Essential Docker ({config["CONFIG_MESSAGE"]})";

            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Message = _message;
            ViewBag.ConfigMessage = _configMessage;

            var products = await _context.Products.ToListAsync();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Category, Price")] Product product)
        {
            if (!ModelState.IsValid) { return View(product); }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception){ throw; }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) 
        {
            if (id == null) { return NotFound(); }

            var product = await _context.Products.FindAsync(id);
            if (product == null) { return NotFound(); }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Category, Price")] Product product)
        {
            if (id != product.Id) { return NotFound(); }
            if (!ModelState.IsValid) { return View(product); }

            try 
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                var findProduct = await _context.Products.FindAsync(id);
                if (findProduct == null) { return NotFound(); }
                else { throw; }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null) { return NotFound(); }

            var product = await _context.Products.FindAsync(id);
            if (product == null) { return NotFound(); }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null){ return NotFound(); }

            var movie = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (movie == null) { return NotFound(); }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
