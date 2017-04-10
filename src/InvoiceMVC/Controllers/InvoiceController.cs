using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using InvoiceMVC.Data;
using InvoiceMVC.Models;
using System.Collections;
using System.Linq;

namespace InvoiceMVC.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public IActionResult Index(string sortOrder, string searchQuery)
        {
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";

            ViewBag.NumberSortParm = sortOrder == "Number" ? "number_desc" : "Number";
            ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";
            ViewBag.DebtorSortParm = sortOrder == "Debtor" ? "debtor_desc" : "Debtor";

            List<Invoice> invoices = _context.Invoices
               .Include(i => i.Debtor)
               .Include(i => i.InvoiceItems)
                   .ThenInclude(c => c.Product).ToList();

            switch (sortOrder)
            {
                //WHEN NO SORT
                case "begin_desc":
                    invoices = invoices.OrderBy(x => x.InvoiceNumber).ToList();
                    break;
                //INVOICE NUMBER
                case "Number":
                    invoices = invoices.OrderBy(s => s.InvoiceNumber).ToList();
                    break;
                case "number_desc":
                    invoices = invoices.OrderByDescending(s => s.InvoiceNumber).ToList();
                    break;
                //DEBTOR
                case "Debtor":
                    invoices = invoices.OrderBy(s => s.Total).ToList();
                    break;
                case "debtor_desc":
                    invoices = invoices.OrderByDescending(s => s.Total).ToList();
                    break;
                //DEFAUlT
                default:
                    invoices = invoices.OrderBy(s => s.InvoiceNumber).ToList();
                    break;
            }

            return View(invoices.ToList());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(d => d.Debtor)
                .Include(d => d.InvoiceItems)
                    .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(m => m.InvoiceNumber == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoice/Create
        public IActionResult Create()
        {
            var products = _context.Products
                  .Select(s => new SelectListItem
                  {
                      Value = s.ProductID.ToString() + "_" + s.Price.ToString(),
                      Text = s.Name
                  });

            ViewBag.Products = new SelectList(products, "Value", "Text");
            ViewData["DebtorID"] = new SelectList(_context.Debtors, "DebtorID", "FullName");

            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceNumber,CreatedOn,DebtorID,ExpirationDate")] Invoice invoice, string total, string pids, string amounts)
        {
            if (ModelState.IsValid)
            {
                invoice.Total = decimal.Parse(total);
                _context.Add(invoice);
                await _context.SaveChangesAsync();

                string[] pidArray = pids.Split(',');
                string[] amountArray = amounts.Split(',');

                List<InvoiceItem> items = new List<InvoiceItem>();

                for (int i = 0; i < pidArray.Length; i++)
                {
                    int pid = int.Parse(pidArray[i]);
                    int amount = int.Parse(amountArray[i]);

                    InvoiceItem item = new InvoiceItem();
                    item.Amount = amount;
                    item.InvoiceNumber = invoice.InvoiceNumber;
                    item.ProductID = pid;

                    items.Add(item);
                }

                _context.AddRange(items);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var products = _context.Products
                  .Select(s => new SelectListItem
                  {
                      Value = s.ProductID.ToString() + "_" + s.Price.ToString(),
                      Text = s.Name
                  });

            ViewBag.Products = new SelectList(products, "Value", "Text", pids);
            ViewData["DebtorID"] = new SelectList(_context.Debtors, "DebtorID", "FullName", invoice.DebtorID);
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(d => d.Debtor)
                .Include(d => d.InvoiceItems)
                    .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(m => m.InvoiceNumber == id);

            var items = await _context.InvoiceItems
                .Include(d => d.Product)
                .Where(m => m.InvoiceNumber == id)
                .ToListAsync();

            if (invoice == null)
            {
                return NotFound();
            }


            ViewData["DebtorID"] = new SelectList(_context.Debtors, "DebtorID", "FullName", invoice.DebtorID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceNumber,CreatedOn,DebtorID,ExpirationDate")] Invoice invoice, string total)
        {
            if (id != invoice.InvoiceNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DebtorID"] = new SelectList(_context.Debtors, "DebtorID", "FullName", invoice.DebtorID);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(d => d.Debtor)
                .Include(d => d.InvoiceItems)
                    .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(m => m.InvoiceNumber == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceNumber == id);
            var items = await _context.InvoiceItems.Where(i => i.InvoiceNumber == id).ToListAsync();

            _context.InvoiceItems.RemoveRange(items);
            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceNumber == id);
        }
    }
}
