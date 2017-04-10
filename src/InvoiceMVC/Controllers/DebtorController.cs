using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceMVC.Data;
using InvoiceMVC.Models;

namespace InvoiceMVC.Controllers
{
    public class DebtorController : Controller
    {
        private ApplicationDbContext _context;

        public DebtorController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Debtor
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            ViewBag.BeginSortParm = String.IsNullOrEmpty(sortOrder) ? "begin_desc" : "";

            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "firstname_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "lastname_desc" : "LastName";
            ViewBag.CityNameSortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            var debtors = from s in _context.Debtors
                          select s;

            if (!String.IsNullOrEmpty(searchQuery))
            {
                debtors = debtors.Where(s => s.FirstName.Contains(searchQuery)
                                       || s.LastName.Contains(searchQuery)
                                       || s.Email.Contains(searchQuery)
                                       || s.City.Contains(searchQuery)
                );
            }

            switch (sortOrder)
            {
                //WHEN NO SORT
                case "begin_desc":
                    debtors = debtors.OrderByDescending(s => s.LastName);
                    break;
                //FIRST NAME
                case "FirstName":
                    debtors = debtors.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    debtors = debtors.OrderByDescending(s => s.FirstName);
                    break;
                //LAST NAME
                case "LastName":
                    debtors = debtors.OrderBy(s => s.LastName);
                    break;
                case "lastname_desc":
                    debtors = debtors.OrderByDescending(s => s.LastName);
                    break;
                //EMAIL
                case "Email":
                    debtors = debtors.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    debtors = debtors.OrderByDescending(s => s.Email);
                    break;
                //CITY
                case "City":
                    debtors = debtors.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    debtors = debtors.OrderByDescending(s => s.City);
                    break;
                //DEFAUlT
                default:
                    debtors = debtors.OrderBy(s => s.FirstName);
                    break;
            }

            return View(await debtors.ToListAsync());
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
                _context.Debtors.Add(debtor);
                await _context.SaveChangesAsync();
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
