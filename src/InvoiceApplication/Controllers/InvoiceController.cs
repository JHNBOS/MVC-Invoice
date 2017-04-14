using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;
using InvoiceApplication.Services;

namespace InvoiceApplication.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext _context;
        private ISettingsService _appService;

        public InvoiceController(ApplicationDbContext context, ISettingsService settingsService)
        {
            _context = context;
            _appService = settingsService;
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<Invoice>> GetInvoices()
        {
            List<Invoice> invoiceList = await _context.Invoices.Include(s => s.Debtor)
                                .Include(s => s.InvoiceItems)
                                .ThenInclude(s => s.Product).ToListAsync();
            return invoiceList;
        }

        private async Task<Invoice> GetInvoice(int? id)
        {
            Invoice invoice = null;

            try
            {
                invoice = await _context.Invoices.Include(s => s.Debtor)
                                    .Include(s => s.InvoiceItems)
                                    .ThenInclude(s => s.Product)
                                    .SingleOrDefaultAsync(s => s.InvoiceNumber == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return invoice;
        }

        private async Task<List<InvoiceItem>> GetInvoiceItems(int? id)
        {
            List<InvoiceItem> itemList = null;

            try
            {
                itemList = await _context.InvoiceItems.Include(d => d.Product)
                                        .Where(s => s.InvoiceNumber == id)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return itemList;
        }

        private async Task CreateInvoice(Invoice invoice, string pids, string amounts, string total)
        {
            string[] pidArray = null;
            string[] amountArray = null;
            List<InvoiceItem> items = new List<InvoiceItem>();

            try
            {
                invoice.Total = decimal.Parse(total);

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                
                if (pids.Contains(','))
                {
                    pidArray = pids.Split(',');
                }

                if (amounts.Contains(','))
                {
                    amountArray = amounts.Split(',');
                }

                if (pidArray != null)
                {
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
                }
                else
                {
                    InvoiceItem item = new InvoiceItem();
                    item.Amount = int.Parse(amounts);
                    item.InvoiceNumber = invoice.InvoiceNumber;
                    item.ProductID = int.Parse(pids.Split('_')[0]);

                    _context.InvoiceItems.Add(item);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UpdateInvoice(Invoice invoice, string pid, string amount, string total)
        {
            bool isEmpty = false;
            string[] pidArray = null;
            string[] amountArray = null;
            List<InvoiceItem> items = new List<InvoiceItem>();

            try
            { 
                invoice.Total = decimal.Parse(total);

                _context.Update(invoice);
                _context.InvoiceItems.RemoveRange(_context.InvoiceItems.Where(s => s.InvoiceNumber == invoice.InvoiceNumber));
                _context.SaveChanges();
                
                if (pid == "" || pid == null)
                {
                    isEmpty = true;
                }

                if (isEmpty == false)
                {
                    if (pid.Length > 1)
                    {
                        pidArray = pid.Split(',');
                    }

                    if (amount.Length > 1)
                    {
                        amountArray = amount.Split(',');
                    }

                    if (pidArray != null)
                    {
                        for (int i = 0; i < pidArray.Length; i++)
                        {
                            int _pid = int.Parse(pidArray[i]);
                            int _amount = int.Parse(amountArray[i]);

                            InvoiceItem item = new InvoiceItem();
                            item.Amount = _amount;
                            item.InvoiceNumber = invoice.InvoiceNumber;
                            item.ProductID = _pid;

                            items.Add(item);
                        }

                        _context.InvoiceItems.AddRange(items);
                    }

                    if (pidArray == null && (pid != "" || pid != null))
                    {
                        InvoiceItem item = new InvoiceItem();
                        item.Amount = int.Parse(amount);
                        item.InvoiceNumber = invoice.InvoiceNumber;
                        item.ProductID = int.Parse(pid.Split('_')[0]);

                        _context.InvoiceItems.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteInvoice(int id)
        {
            Invoice invoice = await GetInvoice(id);
            List<InvoiceItem> itemList = await GetInvoiceItems(id);

            try
            {
                _context.InvoiceItems.RemoveRange(itemList);
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: Invoice
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            //SORTING OPTIONS INVOICE LIST
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";
            ViewBag.NumberSortParm = sortOrder == "Number" ? "number_desc" : "Number";
            ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";
            ViewBag.DebtorSortParm = sortOrder == "Debtor" ? "debtor_desc" : "Debtor";

            var invoices = await GetInvoices();
            var query = from invoice in invoices
                        select invoice;

            //SEARCH OPTION INVOICE LIST
            if (!String.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.Debtor.FullName.Contains(searchQuery)
                                    || s.InvoiceNumber.ToString().Contains(searchQuery)
                                    || s.Total.ToString().Contains(searchQuery));
            }

            switch (sortOrder)
            {
                //WHEN NO SORT
                case "begin_desc":
                    query = query.OrderBy(s => s.InvoiceNumber);
                    break;
                //INVOICE NUMBER
                case "Number":
                    query = query.OrderBy(s => s.InvoiceNumber);
                    break;
                case "number_desc":
                    query = query.OrderByDescending(s => s.InvoiceNumber);
                    break;
                //DEBTOR
                case "Debtor":
                    query = query.OrderBy(s => s.Debtor.FullName);
                    break;
                case "debtor_desc":
                    query = query.OrderByDescending(s => s.Debtor.FullName);
                    break;
                //TOTAL
                case "Total":
                    query = query.OrderBy(s => s.Total);
                    break;
                case "total_desc":
                    query = query.OrderByDescending(s => s.Total);
                    break;
                //DEFAUlT
                default:
                    query = query.OrderBy(s => s.InvoiceNumber);
                    break;
            }

            return View(query);
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await GetInvoice(id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceNumber,CreatedOn,DebtorID,ExpirationDate,Type")] Invoice invoice, string total, string pids, string amounts)
        {
            if (ModelState.IsValid)
            {
                await CreateInvoice(invoice, pids, amounts, total);

                //SEND MAIL TO DEBTOR NOTIFYING ABOUT INVOICE
                if (invoice.Type == "Final")
                {
                    Debtor debtor = _context.Debtors.Single(s => s.DebtorID == invoice.DebtorID);
                    AuthMessageSender email = new AuthMessageSender(_appService);
                    await email.SendInvoiceEmailAsync(debtor.Email);
                }

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

            var products = _context.Products
                 .Select(s => new SelectListItem
                 {
                     Value = s.ProductID.ToString() + "_" + s.Price.ToString(),
                     Text = s.Name
                 });

            var invoice = await GetInvoice(id);
            var items = await GetInvoiceItems(id);

            if (invoice == null)
            {
                return NotFound();
            }

            var p = _context.Products;
            string[] pids = new string[p.Count()];

            int cnt = 0;
            foreach (var pid in p)
            {
                string _id = pid.ProductID + "_" + pid.Price;
                pids[cnt] = _id;
                cnt++;
            }

            ViewBag.PIDs = pids;
            ViewBag.Amounts = items.Select(s => s.Amount).ToArray();
            ViewBag.Names = items.Select(s => s.Product.Name).ToArray();
            ViewBag.Total = String.Format("{0:N2}", invoice.Total);

            ViewBag.Products = new SelectList(products, "Value", "Text");
            ViewData["DebtorID"] = new SelectList(_context.Debtors, "DebtorID", "FullName", invoice.DebtorID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceNumber,CreatedOn,DebtorID,ExpirationDate,Type")] Invoice invoice, string total, string pid, string amount)
        {
            if (id != invoice.InvoiceNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Invoice invoiceBeforeUpdate = _context.Invoices.Single(s => s.InvoiceNumber == invoice.InvoiceNumber);
                await UpdateInvoice(invoice, pid, amount, total);

                //SEND MAIL TO DEBTOR NOTIFYING ABOUT INVOICE
                if (invoice.Type == "Final" && invoiceBeforeUpdate.Type != "Final")
                {
                    Debtor debtor = _context.Debtors.Single(s => s.DebtorID == invoice.DebtorID);
                    AuthMessageSender email = new AuthMessageSender(_appService);
                    await email.SendInvoiceEmailAsync(debtor.Email);
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

            var invoice = await GetInvoice(id);

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
            await DeleteInvoice(id);
            return RedirectToAction("Index");
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceNumber == id);
        }
    }
}
