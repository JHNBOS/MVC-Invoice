using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Invoice.Data;
using E_Invoice.Models;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace E_Invoice.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices
                .Include(i => i.Debtor)
                .Include(i => i.Items)
                .ThenInclude(c => c.Product)
                .ToListAsync();

            return View(await applicationDbContext);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            var debtors = _context.Debtors
                  .Select(s => new SelectListItem
                  {
                      Value = s.DebtorID.ToString(),
                      Text = s.FirstName + " " + s.LastName
            });

            var products = _context.Products
                  .Select(s => new SelectListItem
                  {
                      Value = s.ProductID.ToString() + "_"  + s.Price.ToString(),
                      Text = s.Name
                  });

            ViewData["DebtorID"] = new SelectList(debtors, "Value", "Text");
            ViewData["ProductID"] = new SelectList(products, "Value", "Text");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceID,CreatedOn,DebtorID,ExpirationDate")] Invoice invoice, int pid, int amount, string total)
        {
            var debtors = _context.Debtors
                 .Select(s => new SelectListItem
                 {
                     Value = s.DebtorID.ToString(),
                     Text = s.FirstName + " " + s.LastName
                 }
            );

            var products = _context.Products
                  .Select(s => new SelectListItem
                  {
                      Value = s.ProductID.ToString(),
                      Text = s.Name
            });

            InvoiceItem ii = null;

            if (ModelState.IsValid)
            {
                invoice.Total = decimal.Parse(total, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                Thread.Sleep(2500);

                ii = new InvoiceItem();
                ii.InvoiceID = invoice.InvoiceID;
                ii.ProductID = pid;
                ii.Quantity = amount;

                _context.InvoiceItems.Add(ii);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewData["DebtorID"] = new SelectList(debtors, "Value", "Text", invoice.DebtorID);
            ViewData["ProductID"] = new SelectList(products, "Value", "Text", pid);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            var debtors = _context.Debtors
                              .Select(s => new SelectListItem
                              {
                                  Value = s.DebtorID.ToString(),
                                  Text = s.FirstName + " " + s.LastName
                              });

            ViewData["DebtorID"] = new SelectList(debtors, "Value", "Text");
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceID,CreatedOn,DebtorID,ExpirationDate,Total")] Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return NotFound();
            }

            var debtors = _context.Debtors
                 .Select(s => new SelectListItem
                 {
                     Value = s.DebtorID.ToString(),
                     Text = s.FirstName + " " + s.LastName
                 }
            );

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceID))
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

            ViewData["DebtorID"] = new SelectList(debtors, "Value", "Text", invoice.DebtorID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceID == id);
            var invoice_items = await _context.InvoiceItems.Where(n => n.InvoiceID == id).ToListAsync();

            _context.InvoiceItems.RemoveRange(invoice_items);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceID == id);
        }
    }
}
