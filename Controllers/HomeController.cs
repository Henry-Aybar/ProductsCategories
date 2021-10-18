using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsCategories.Models;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController( MyContext context)
        {
            _context = context;
        }
        
        
        //================
        // Products Page
        //================
        public IActionResult Index()
        {
            ViewBag.AllProducts = _context.Products.ToList();
            return View("ProductsPage");
        }

        [HttpPost("product/new")]
        public IActionResult CreateProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                _context.Add(newProduct);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("Index");
        }

        //================
        // Categories Page
        //================
        [HttpGet("categories")]
        public IActionResult CategoriesPage()
        {
            ViewBag.AllCategories = _context.Categories.ToList();
            return View();
        }

        [HttpPost("category/new")]
        public IActionResult CreateCategory(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                _context.Add(newCategory);
                _context.SaveChanges();

                return RedirectToAction("CategoriesPage");
            }
            return View("CategoriesPage");
        }

        //================================
        // Product Display & add Catagory
        //================================
        [HttpGet("/product/{id}")]
        public IActionResult DisplayProducts(int id)
        {
            ViewBag.AllCategories = _context.Categories.ToList();

            ViewBag.DisplayProduct = _context.Products
                .Include(pro => pro.ProductCategory)
                .ThenInclude(procat => procat.Category)
                .FirstOrDefault(pro => pro.ProductId == id);

            return View();
        }

        [HttpPost("product/addCategory")]
        public IActionResult AddCategoryToProduct(Association ass)
        {
            _context.Add(ass);
            _context.SaveChanges();

            return RedirectToAction ("DisplayProducts", new {id = ass.ProductId});
        }

        //================================
        // Category Display & add Products
        //================================
        [HttpGet("/category/{id}")]
        public IActionResult DisplayCategory(int id)
        {
            ViewBag.AllProducts = _context.Products.ToList();

            ViewBag.DisplayCategories = _context.Categories
                .Include(pro => pro.ProductCategory)
                .ThenInclude(procat => procat.Product)
                .FirstOrDefault(gory => gory.CategoryId == id);

            return View();
        }

        [HttpPost("product/addCategory")]
        public IActionResult AddProductToCagory(Association ass)
        {
            _context.Add(ass);
            _context.SaveChanges();

            return RedirectToAction ("DisplayCategories", new {id = ass.CategoryId});
        }
        


        //=============================
        //=============================
        //=============================
        //=======Existig Template======

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
