using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using InvoiceApplication.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace InvoiceApplication.Controllers
{
    public class DebtorController : Controller
    {
        private ApplicationDbContext _context;
        private IMySettingsService mySettingsService;

        public DebtorController(ApplicationDbContext context, IMySettingsService settingsService)
        {
            _context = context;
            mySettingsService = settingsService;
        }

        // GET: Debtor
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";

            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "firstname_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "lastname_desc" : "LastName";
            ViewBag.CityNameSortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            var debtors = _context.Debtors;
            var query = from debtor in debtors
                        select debtor;

            if (!String.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.FirstName.Contains(searchQuery)
                                       || s.LastName.Contains(searchQuery)
                                       || s.Email.Contains(searchQuery)
                                       || s.City.Contains(searchQuery));
            }

            switch (sortOrder)
            {
                //WHEN NO SORT
                case "begin_desc":
                    query = query.OrderByDescending(s => s.LastName);
                    break;
                //FIRST NAME
                case "FirstName":
                    query = query.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    query = query.OrderByDescending(s => s.FirstName);
                    break;
                //LAST NAME
                case "LastName":
                    query = query.OrderBy(s => s.LastName);
                    break;
                case "lastname_desc":
                    query = query.OrderByDescending(s => s.LastName);
                    break;
                //EMAIL
                case "Email":
                    query = query.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    query = query.OrderByDescending(s => s.Email);
                    break;
                //CITY
                case "City":
                    query = query.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    query = query.OrderByDescending(s => s.City);
                    break;
                //DEFAUlT
                default:
                    query = query.OrderBy(s => s.FirstName);
                    break;
            }

            return View(await query.ToListAsync());
        }

        // GET: Debtor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtor = await _context.Debtors.SingleOrDefaultAsync(m => m.DebtorID == id);
            if (debtor == null)
            {
                return NotFound();
            }

            return View(debtor);
        }

        // GET: Debtor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Debtor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DebtorID,Address,BankAccount,City,Country,Email,FirstName,LastName,Phone,PostalCode")] Debtor debtor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Debtors.Add(debtor);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                try
                {
                    User user = new User();
                    user.AccountType = "Client";
                    user.Address = debtor.Address;
                    user.PostalCode = debtor.PostalCode;
                    user.City = debtor.City;
                    user.Country = debtor.Country;
                    user.FirstName = debtor.FirstName;
                    user.LastName = debtor.LastName;
                    user.Email = debtor.Email;
                    user.Password = debtor.FirstName + "_" + DateTime.Now.ToString("ddMMHH");

                    _context.User.Add(user);
                    await _context.SaveChangesAsync();

                    AuthMessageSender email = new AuthMessageSender(mySettingsService);
                    await email.SendUserEmailAsync(user.Email, user.Password);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return RedirectToAction("Index");
            }
            return View(debtor);
        }

        // GET: Debtor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtor = await _context.Debtors.SingleOrDefaultAsync(m => m.DebtorID == id);
            if (debtor == null)
            {
                return NotFound();
            }
            return View(debtor);
        }

        // POST: Debtor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DebtorID,Address,BankAccount,City,Country,Email,FirstName,LastName,Phone,PostalCode")] Debtor debtor)
        {
            if (id != debtor.DebtorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debtor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebtorExists(debtor.DebtorID))
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
            return View(debtor);
        }

        // GET: Debtor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtor = await _context.Debtors.SingleOrDefaultAsync(m => m.DebtorID == id);
            if (debtor == null)
            {
                return NotFound();
            }

            return View(debtor);
        }

        // POST: Debtor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var debtor = await _context.Debtors.SingleOrDefaultAsync(m => m.DebtorID == id);
            List<Invoice> invoices = _context.Invoices.Where(i => i.DebtorID == debtor.DebtorID).ToList();

            //REMOVE ALL INVOICE ITEMS
            foreach (var invoice in invoices)
            {
                _context.InvoiceItems.RemoveRange(_context.InvoiceItems.Where(i => i.InvoiceNumber == invoice.InvoiceNumber));
                await _context.SaveChangesAsync();
            }

            //REMOVE ALL INVOICES OF DEBTOR
            _context.Invoices.RemoveRange(invoices);
            await _context.SaveChangesAsync();

            //REMOVE USER ACCOUNT OF DEBTOR
            _context.User.Remove(_context.User.Single(u => u.Email == debtor.Email));
            await _context.SaveChangesAsync();

            //REMOVE DEBTOR
            _context.Debtors.Remove(debtor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool DebtorExists(int id)
        {
            return _context.Debtors.Any(e => e.DebtorID == id);
        }
    }
}
