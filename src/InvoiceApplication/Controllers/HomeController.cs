using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Data;
using InvoiceApplication.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string email)
        {
            if (email != "")
            {
                GetInvoices(email);
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private void GetInvoices(string email)
        {
            decimal total = 0;
            List<Invoice> invoiceList = null;
            int paid = 0;
            int unpaid = 0;

            try
            {
                invoiceList = _context.Invoices.Include(inv => inv.Debtor).Where(inv => inv.Debtor.Email == email && inv.Paid == false && inv.Type != "Concept").ToList();
                paid = _context.Invoices.Include(inv => inv.Debtor).Where(inv => inv.Debtor.Email == email && inv.Paid == true).ToList().Count;
                unpaid = _context.Invoices.Include(inv => inv.Debtor).Where(inv => inv.Debtor.Email == email && inv.Paid == false).ToList().Count;

                ViewBag.paid = paid;
                ViewBag.unpaid = unpaid;
                ViewBag.invoiceList = invoiceList;

                total = calculateTotal(invoiceList);
                ViewBag.total = String.Format("{0:N2}", total);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private decimal calculateTotal(List<Invoice> invoices)
        {
            decimal total = 0;

            foreach (var invoice in invoices)
            {
                total += invoice.Total;
            }

            return total;
        }


    }
}
