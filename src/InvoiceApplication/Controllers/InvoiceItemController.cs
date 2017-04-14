using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;
using InvoiceApplication.Services;
using System;
using System.Collections.Generic;

namespace InvoiceApplication.Controllers
{
    public class InvoiceItemController : Controller
    {
        private ApplicationDbContext _context;

        public InvoiceItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<InvoiceItem>> GetItems()
        {
            List<InvoiceItem> itemList = await _context.InvoiceItems.ToListAsync();
            return itemList;
        }

        private async Task<InvoiceItem> GetItem(int? id)
        {
            InvoiceItem item = null;

            try
            {
                item = await _context.InvoiceItems.SingleOrDefaultAsync(s => s.ItemID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return item;
        }

        private async Task CreateItem(InvoiceItem item)
        {
            try
            {
                _context.InvoiceItems.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UpdateItem(InvoiceItem item)
        {
            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteItem(int id)
        {
            InvoiceItem item = await GetItem(id);

            try
            {
                _context.InvoiceItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: InvoiceItem
        public async Task<IActionResult> Index()
        {
            List<InvoiceItem> itemList = await GetItems();
            return View(itemList);
        }

        // GET: InvoiceItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await GetItem(id);

            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // GET: InvoiceItem/Create
        public IActionResult Create()
        {
            ViewData["InvoiceNumber"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            return View();
        }

        // POST: InvoiceItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,Amount,InvoiceNumber,ProductID")] InvoiceItem invoiceItem)
        {
            if (ModelState.IsValid)
            {
                await CreateItem(invoiceItem);
                return RedirectToAction("Index");
            }

            ViewData["InvoiceNumber"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", invoiceItem.InvoiceNumber);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", invoiceItem.ProductID);
            return View(invoiceItem);
        }

        // GET: InvoiceItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await GetItem(id);

            if (invoiceItem == null)
            {
                return NotFound();
            }

            ViewData["InvoiceNumber"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", invoiceItem.InvoiceNumber);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", invoiceItem.ProductID);
            return View(invoiceItem);
        }

        // POST: InvoiceItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,Amount,InvoiceNumber,ProductID")] InvoiceItem invoiceItem)
        {
            if (id != invoiceItem.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await UpdateItem(invoiceItem);
                return RedirectToAction("Index");
            }

            ViewData["InvoiceNumber"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", invoiceItem.InvoiceNumber);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", invoiceItem.ProductID);
            return View(invoiceItem);
        }

        // GET: InvoiceItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await GetItem(id);

            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // POST: InvoiceItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await DeleteItem(id);
            return RedirectToAction("Index");
        }

        private bool InvoiceItemExists(int id)
        {
            return _context.InvoiceItems.Any(e => e.ItemID == id);
        }
    }
}
