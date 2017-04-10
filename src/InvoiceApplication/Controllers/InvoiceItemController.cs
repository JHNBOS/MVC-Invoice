using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;

namespace InvoiceApplication.Controllers
{
    public class InvoiceItemController : Controller
    {
        private ApplicationDbContext _context;

        public InvoiceItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvoiceItem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InvoiceItems.Include(i => i.Invoice).Include(i => i.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InvoiceItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.ItemID == id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,Amount,InvoiceNumber,ProductID")] InvoiceItem invoiceItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceItem);
                await _context.SaveChangesAsync();
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

            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.ItemID == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }
            ViewData["InvoiceNumber"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", invoiceItem.InvoiceNumber);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", invoiceItem.ProductID);
            return View(invoiceItem);
        }

        // POST: InvoiceItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _context.Update(invoiceItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceItemExists(invoiceItem.ItemID))
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

            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.ItemID == id);
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
            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.ItemID == id);
            _context.InvoiceItems.Remove(invoiceItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InvoiceItemExists(int id)
        {
            return _context.InvoiceItems.Any(e => e.ItemID == id);
        }
    }
}
