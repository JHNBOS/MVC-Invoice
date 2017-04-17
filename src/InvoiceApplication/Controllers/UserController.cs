using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace InvoiceApplication.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private ISettingsService _settings;
        private IHostingEnvironment _env;

        public UserController(ApplicationDbContext context, ISettingsService settingsAccessor, IHostingEnvironment env)
        {
            _context = context;
            _settings = settingsAccessor;
            _env = env;
        }

        /*----------------------------------------------------------------------*/
        //DATABASE ACTION METHODS

        private async Task<List<User>> GetUsers()
        {
            List<User> userList = await _context.User.ToListAsync();
            return userList;
        }

        private async Task<User> GetUser(int? id)
        {
            User user = null;

            try
            {
                user = await _context.User.SingleOrDefaultAsync(s => s.ID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return user;
        }

        private async Task CreateUser(User user)
        {
            user.AccountType = "Client";

            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task UpdateUser(User user)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task DeleteUser(int id)
        {
            User user = await GetUser(id);

            try
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*----------------------------------------------------------------------*/
        //CONTROLLER ACTIONS

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await GetUsers());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetUser(id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountType,Address,City,Country,Email,FirstName,LastName,Password,PostalCode")] User user)
        {
            if (ModelState.IsValid)
            {
                await CreateUser(user);
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

            var user = await GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
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
                await UpdateUser(user);

                User currentUser = SessionHelper.Get<User>(this.HttpContext.Session, "User");
                return RedirectToAction("Index", "Home", new { email = currentUser.Email });
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

            var user = await GetUser(id);

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
            await DeleteUser(id);
            return RedirectToAction("Login", "User", new { area = "" });
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

        //GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: User/Login
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
                Debug.WriteLine(ex);
            }

            if (login != null)
            {
                SessionHelper.Set(this.HttpContext.Session, "User", login);
                return RedirectToAction("Index", "Home", new { email = login.Email });
            }

            return View(login);
        }

        //GET: User/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login", "User", new { area = "" });
        }

        //GET: User/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //POST: User/ForgotPassword
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

                return RedirectToAction("Login", "User", new { area = "" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return View(user);
            }
        }

        //GET: User/Settings
        public ActionResult Settings()
        {
            AppSettings current = new AppSettings();

            current.Email = _settings.GetEmail();
            current.Password = _settings.GetPassword();
            current.SMTP = _settings.GetSMTP();
            current.Port = _settings.GetPort();
            current.Name = _settings.GetName();
            current.Website = _settings.GetWebsite();
            current.Phone = _settings.GetPhone();
            current.Address = _settings.GetAddress();
            current.City = _settings.GetCity();
            current.PostalCode = _settings.GetPostalCode();
            current.Country = _settings.GetCountry();
            current.CompanyNumber = _settings.GetCompanyNumber();
            current.TaxNumber = _settings.GetTaxNumber();
            current.Logo = _settings.GetLogo();
            current.UseLogo = _settings.UseLogo();

            return View(current);
        }

        //POST: User/Settings
        [HttpPost]
        public async Task<ActionResult> Settings(AppSettings AppSettings, IFormFile file)
        {
            if (AppSettings != null)
            {
                try
                {
                    if (file != null)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "images");

                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(uploads, file.FileName);
                            _settings.SetLogo(file.FileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                //Update Settings
                _settings.SetEmail(AppSettings.Email);
                _settings.SetPassword(AppSettings.Password);
                _settings.SetSMTP(AppSettings.SMTP);
                _settings.SetPort(AppSettings.Port);
                _settings.SetName(AppSettings.Name);
                _settings.SetWebsite(AppSettings.Website);
                _settings.SetPhone(AppSettings.Phone);
                _settings.SetAddress(AppSettings.Address);
                _settings.SetPostalCode(AppSettings.PostalCode);
                _settings.SetCity(AppSettings.City);
                _settings.SetCountry(AppSettings.Country);
                _settings.SetCompanyNumber(AppSettings.CompanyNumber);
                _settings.SetTaxNumber(AppSettings.TaxNumber);
                _settings.SetUseLogo(AppSettings.UseLogo);

                User currentUser = SessionHelper.Get<User>(this.HttpContext.Session, "User");
                return RedirectToAction("Index", "Home", new { email = currentUser.Email });
            }

            return View();
        }




    }
}
