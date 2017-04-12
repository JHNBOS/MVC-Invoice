using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using InvoiceApplication.Services;

namespace InvoiceApplication.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private mySettings _settings;

        public UserController(ApplicationDbContext context, IOptions<mySettings> settingsAccessor)
        {
            _context = context;
            _settings = settingsAccessor.Value;
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

        public ActionResult Settings()
        {
            mySettings current = new mySettings();

            current.Email = _settings.Email;
            current.Password = _settings.Password;
            current.SMTP = _settings.SMTP;
            current.Port = _settings.Port;
            current.Name = _settings.Name;
            current.Website = _settings.Website;

            return View(current);
        }
        
        [HttpPost]
        public ActionResult Settings(mySettings mySettings)
        {
            if (mySettings != null)
            {
                _settings.Email = mySettings.Email;
                _settings.Password = mySettings.Password;
                _settings.SMTP = mySettings.SMTP;
                _settings.Port = mySettings.Port;
                _settings.Name = mySettings.Name;
                _settings.Website = mySettings.Website;

                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(mySettings);
        }

    }
}
