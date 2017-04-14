using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace InvoiceApplication.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<Product>> GetProducts()
        {
            List<Product> productList = await _context.Products.Include(s => s.InvoiceItems).ToListAsync();
            return productList;
        }

        private async Task<Product> GetProduct(int? id)
        {
            Product product = null;

            try
            {
                product = await _context.Products.SingleOrDefaultAsync(s => s.ProductID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return product;
        }

        private async Task CreateProduct(Product product, string price)
        {
            product.Price = decimal.Parse(price);

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UpdateProduct(Product product, string price)
        {
            product.Price = decimal.Parse(price);

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteProduct(int id)
        {
            Product product = await GetProduct(id);

            try
            {
                _context.InvoiceItems.RemoveRange(_context.InvoiceItems.Where(s => s.ProductID == product.ProductID).ToList());
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: Product
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            //SORTING OPTIONS PRODUCT LIST
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            var products = await GetProducts();
            var query = from product in products
                        select product;

            //SEARCH OPTION PRODUCT LIST
            if (!String.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.Name.Contains(searchQuery)
                                    || s.Price.ToString().Contains(searchQuery));
            }

            switch (sortOrder)
            {
                //WHEN NO SORT
                case "begin_desc":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                //NAME
                case "Name":
                    query = query.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                //PRICE
                case "Price":
                    query = query.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(s => s.Price);
                    break;
                //DEFAUlT
                default:
                    query = query.OrderBy(s => s.Name);
                    break;
            }

            return View(query);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Description,Name,TaxPercentage")] Product product, string price)
        {
            if (ModelState.IsValid)
            {
                await CreateProduct(product, price);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);
            ViewBag.Price = String.Format("{0:N2}", product.Price);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Description,Name,TaxPercentage")] Product product, string price)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await UpdateProduct(product, price);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await DeleteProduct(id);
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
