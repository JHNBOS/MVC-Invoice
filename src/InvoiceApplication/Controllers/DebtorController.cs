using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using InvoiceApplication.Services;
using System.Collections.Generic;

namespace InvoiceApplication.Controllers
{
    public class DebtorController : Controller
    {
        private ApplicationDbContext _context;
        private AppSettings _settings;

        public DebtorController(ApplicationDbContext context)
        {
            _context = context;
            _settings = _context.Settings.Single(s => s.ID == 1);
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<Debtor>> GetDebtors()
        {
            List<Debtor> debtorList = await _context.Debtors.ToListAsync();
            return debtorList;
        }

        private async Task<Debtor> GetDebtor(int? id)
        {
            Debtor debtor = null;

            try
            {
                debtor = await _context.Debtors.SingleOrDefaultAsync(s => s.DebtorID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return debtor;
        }

        private async Task CreateDebtor(Debtor debtor)
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
        }

        private async Task CreateLogin(Debtor debtor)
        {
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

                AuthMessageSender email = new AuthMessageSender();
                await email.SendUserEmailAsync(user.Email, user.Password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UpdateDebtor(Debtor debtor)
        {
            try
            {
                _context.Update(debtor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteDebtor(int id)
        {
            Debtor debtor = await GetDebtor(id);
            List<Invoice> invoices = null;

            try
            {
                invoices = _context.Invoices.Where(s => s.DebtorID == debtor.DebtorID).ToList();

                //REMOVE ALL INVOICE ITEMS
                foreach (var invoice in invoices)
                {
                    _context.InvoiceItems.RemoveRange(_context.InvoiceItems.Where(s => s.InvoiceNumber == invoice.InvoiceNumber));
                    await _context.SaveChangesAsync();
                }

                _context.Invoices.RemoveRange(invoices);
                _context.User.Remove(_context.User.Single(s => s.Email == debtor.Email));
                _context.Debtors.Remove(debtor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: Debtor
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            //SORTING OPTIONS DEBTOR LIST
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "firstname_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "lastname_desc" : "LastName";
            ViewBag.CityNameSortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            var debtors = await GetDebtors();
            var query = from debtor in debtors
                        select debtor;

            //SEARCH OPTION PRODUCT LIST
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

            return View(query);
        }

        // GET: Debtor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtor = await GetDebtor(id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DebtorID,Address,BankAccount,City,Country,Email,FirstName,LastName,Phone,PostalCode")] Debtor debtor)
        {
            if (ModelState.IsValid)
            {
                await CreateDebtor(debtor);
                await CreateLogin(debtor);
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

            var debtor = await GetDebtor(id);

            if (debtor == null)
            {
                return NotFound();
            }

            return View(debtor);
        }

        // POST: Debtor/Edit/5
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
                await UpdateDebtor(debtor);
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

            var debtor = await GetDebtor(id);

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
            await DeleteDebtor(id);
            return RedirectToAction("Index");
        }

        private bool DebtorExists(int id)
        {
            return _context.Debtors.Any(e => e.DebtorID == id);
        }

    }
}
