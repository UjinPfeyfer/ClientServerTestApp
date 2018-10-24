using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClientServerTestApp.Models;
using ClientServerTestApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ClientServerTestApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Home() { Products = _context.Products.ToList(), Categories = _context.Categories.ToList() });
        }

        [HttpPost]
        public IActionResult Index(string searchString)
        {
            IQueryable<Product> products = _context.Products;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }
            return View(new Home() { Products = products.ToList(), Categories = _context.Categories.ToList(), SearchString = searchString });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Category =new SelectList( _context.Categories.ToList<Category>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                ViewBag.Category = new SelectList(_context.Categories.ToList<Category>(), "Id", "Name");
                if (product != null)
                    return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (product != null)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(Home Model)
        {
            foreach (var id in Model.SelectedItems)
            {
                var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Download(Home Model)
        {
            StringBuilder singleRow = new StringBuilder();
            foreach (var id in Model.SelectedItems)
            {
                var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                singleRow.Append(product.ToString());
            };
            byte[] text = Encoding.UTF8.GetBytes(singleRow.ToString());
            var content = new System.IO.MemoryStream(text);
            var contentType = "text/csv";
            var fileName = "data.csv";
            return File(content, contentType, fileName);
            
        }

        public async Task<IActionResult> Upload(IFormFile file)
        {
            StreamReader sr = new StreamReader(file.OpenReadStream());
            string s;
            try
            {
                while ((s = sr.ReadLine()) != null)
                {
                    string[] result = s.Split(',');
                    if (result.Length == 5)
                    {
                        _context.Products.Add(new Product()
                        {
                            Name = result[0],
                            Price = float.Parse(result[1]),
                            Count = int.Parse(result[2]),
                            Description = result[3],
                            CategoryId = int.Parse(result[4])
                        });
                    }
                    else
                    {
                        _context.Products.Add(new Product()
                        {
                            Name = result[0],
                            Price = float.Parse(result[1]),
                            Count = int.Parse(result[2]),
                            CategoryId = int.Parse(result[3])
                        });
                    }
                }
            }
            catch(Exception e)
            { }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}