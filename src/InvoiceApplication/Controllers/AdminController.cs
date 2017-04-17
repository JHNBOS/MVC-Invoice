using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;

namespace InvoiceApplication.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<User>> GetAdmins()
        {
            List<User> adminList = await _context.User.Where(u => u.AccountType == "Admin").ToListAsync();
            return adminList;
        }

        private async Task<User> GetAdmin(int? id)
        {
            User admin = null;

            try
            {
                admin = await _context.User.SingleOrDefaultAsync(s => s.ID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return admin;
        }

        private async Task CreateAdmin(User admin)
        {
            admin.AccountType = "Admin";

            try
            {
                _context.User.Add(admin);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private async Task UpdateAdmin(User admin)
        {
            try
            {
                _context.Update(admin);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteAdmin(int id)
        {
            User admin = await GetAdmin(id);

            try
            {
                _context.User.Remove(admin);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await GetAdmins());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetAdmin(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountType,Address,City,Country,Email,FirstName,LastName,Password,PostalCode")] User user)
        {
            if (ModelState.IsValid)
            {
                await CreateAdmin(user);
                return RedirectToAction("Login", "User", new { area = "" });
            }

            return View(user);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetAdmin(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccountType,Address,City,Country,Email,FirstName,LastName,Password,PostalCode")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await UpdateAdmin(user);

                User currentUser = SessionHelper.Get<User>(this.HttpContext.Session, "User");
                return RedirectToAction("Index", "Home", new { email = currentUser.Email });
            }
            return View(user);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetAdmin(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await DeleteAdmin(id);
            return RedirectToAction("Login", "User", new { area = "" });
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

    }
}
