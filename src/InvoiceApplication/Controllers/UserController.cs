using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;

namespace InvoiceApplication.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountType,Address,City,Country,Email,FirstName,LastName,Password,PostalCode")] User user)
        {
            if (ModelState.IsValid)
            {
                user.AccountType = "Client";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User", new { area = "" });
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "User", new { area = "" });
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            User login = null;

            try
            {
                login = _context.User.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Oh shit...");
                Debug.WriteLine(ex);
            }

            if (login != null)
            {
                Debug.WriteLine("User exists!");
                HttpContext.Session.Set("User", (User)login);

                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(login);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login", "User", new { area = "" });
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email, string password)
        {
            User user = null;

            try
            {
                user = _context.User.SingleOrDefault(m => m.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.Password = password;
                _context.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Login", "User", new { area=""});
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return View(user);
            }
        }






    }
}
