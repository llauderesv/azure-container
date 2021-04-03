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
using AzureContainer.Exceptions;
using AzureContainer.Interfaces;

namespace AzureContainer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string _message;
        private readonly string _configMessage;
        private readonly IProductRepository _productRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration config,
            IProductRepository productRepository)
        {
            _logger = logger;
            _message = $"Essential Docker ({config["HOSTNAME"]})";
            _configMessage = $"Essential Docker ({config["CONFIG_MESSAGE"]})";

            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Message = _message;
            ViewBag.ConfigMessage = _configMessage;

            return View(await _productRepository.Products.ToListAsync());
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
                await _productRepository.Insert(product);
            }
            catch (Exception) { throw; }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product =
                await _productRepository.Products
                        .Where(p => p.Id == id)
                        .FirstOrDefaultAsync();

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
                await _productRepository.Update(product);
            }
            catch (RepositoryException ex)
            {
                var findProduct = await _productRepository.Products
                        .Where(p => p.Id == id)
                        .FirstOrDefaultAsync();
                if (findProduct == null) { return NotFound(); }
                else { throw; }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.Products
                            .Where(p => p.Id == id)
                            .FirstOrDefaultAsync();
            if (product == null) { return NotFound(); }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.Products
                        .Where(p => p.Id == id)
                        .FirstOrDefaultAsync();
            if (product == null) { return NotFound(); }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (RepositoryException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult PageNotFound()
        {
            return View("~/Views/Shared/PageNotFound.cshtml");
        }

    }
}
