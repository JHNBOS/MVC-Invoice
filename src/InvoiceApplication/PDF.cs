using InvoiceApplication.Data;
using InvoiceApplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Drawing;

namespace InvoiceApplication
{
    public class PDF
    {
        private ApplicationDbContext _context;
        private AppSettings settings;
        private IHostingEnvironment _env;

        public PDF(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            settings = _context.Settings.Single(s => s.ID == 1);
            _env = env;
        }

        public FileStreamResult CreatePDF(int invoiceNumber, HttpContext context)
        {
            Invoice selected = null;
            List<InvoiceItem> itemList = null;
            List<Product> productList = new List<Product>();
            Debtor Debtor = null;

            try
            {
                selected = _context.Invoices.Single(p => p.InvoiceNumber == invoiceNumber);
                itemList = _context.InvoiceItems.Where(i => i.InvoiceNumber == selected.InvoiceNumber).ToList();
                Debtor = _context.Debtors.Single(d => d.DebtorID == selected.DebtorID);

                foreach (var inv_item in itemList)
                {
                    Product product = _context.Products.Single(p => p.ProductID == inv_item.ProductID);
                    productList.Add(product);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            /*--------------------------------------------------------------------------*/
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            page.Size = PdfSharp.PageSize.A4;

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            /*--------------------------------------------------------------------------*/
            // Create a font
            XFont bigFont = new XFont("Arial", 25, XFontStyle.Bold);
            XFont textFont = new XFont("Arial", 12, XFontStyle.Regular);
            XFont textFontBold = new XFont("Arial", 11, XFontStyle.Bold);
            XFont infoFont = new XFont("Arial", 11, XFontStyle.Regular);
            XFont infoFontSmall = new XFont("Arial", 9, XFontStyle.Regular);
            XFont infoFontLarge = new XFont("Arial", 13, XFontStyle.Bold);

            //Margins
            int leftMargin = 40;
            int rightMargin = Convert.ToInt32(page.Width.Point) - 175;
            int topMargin = 40;
            int bottomMargin = Convert.ToInt32(page.Height.Point) - 40;

            /*--------------------------------------------------------------------------*/
            //Strings and image
            string fname = Debtor.FirstName;
            string lname = Debtor.LastName;
            string dAddress = Debtor.Address;
            string dCity = Debtor.PostalCode + " " + Debtor.City;
            string dCountry = Debtor.Country;

            string logo = settings.Logo;
            bool useLogo = settings.UseLogo;
            string email = settings.Email;
            string company = settings.CompanyName;
            string web = settings.Website;
            string name = "";
            string address = settings.Address;
            string city = settings.PostalCode + " | " + settings.City;
            string country = settings.Country;
            string phone = settings.Phone;
            string btw = settings.CommerceNumber;
            string kvk = settings.CompanyNumber;
            string prefix = settings.Prefix;

            string[] words = fname.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                name += words[i].ElementAt(0);
            }

            name += ". " + lname;

            //Company name or logo
            if (useLogo)
            {
                XImage img = XImage.FromGdiPlusImage(Image.FromFile(_env.WebRootPath + @"\images\" + logo));
                int imgMargin = Convert.ToInt32(page.Width.Point) - img.PixelWidth;
                gfx.DrawImage(img, new XRect(imgMargin - 40, 22, 240, 75));
            }
            else
            {
                tf.DrawString(company, bigFont, XBrushes.Black, new XRect(rightMargin, topMargin, 280, 20), XStringFormats.TopLeft);
            }

            /*--------------------------------------------------------------------------*/
            //Company info rows               
            tf.DrawString("Addr:", infoFont, XBrushes.Black, new XRect(leftMargin + 0, topMargin, 24, 20), XStringFormats.TopLeft);
            tf.DrawString("Tel:", infoFont, XBrushes.Black, new XRect(leftMargin + 7, topMargin + 43, 24, 20), XStringFormats.TopLeft);
            tf.DrawString("Email:", infoFont, XBrushes.Black, new XRect(leftMargin - 4, topMargin + 58, 24, 20), XStringFormats.TopLeft);
            tf.DrawString("Tax Nr:", infoFont, XBrushes.Black, new XRect(leftMargin - 9, topMargin + 73, 40, 20), XStringFormats.TopLeft);
            tf.DrawString("Company:", infoFont, XBrushes.Black, new XRect(leftMargin - 24, topMargin + 88, 24, 20), XStringFormats.TopLeft);
            tf.DrawString("Web:", infoFont, XBrushes.Black, new XRect(leftMargin + 1, topMargin + 103, 24, 20), XStringFormats.TopLeft);

            //Company info
            tf.DrawString(address, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(city, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 13, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(country, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 27, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(phone, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 43, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(email, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 58, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(kvk, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 73, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(btw, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 88, 280, 20), XStringFormats.TopLeft);
            tf.DrawString(web, infoFont, XBrushes.Black, new XRect(leftMargin + 44, topMargin + 103, 280, 20), XStringFormats.TopLeft);

            /*------------------------------------------*/
            //invoice info rows
            tf.DrawString("Invoice Nr:", textFont, XBrushes.Black, new XRect(leftMargin, topMargin + 255, 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Invoice Date:", textFont, XBrushes.Black, new XRect(leftMargin, topMargin + 268, 100, 20), XStringFormats.TopLeft);

            //invoice info
            tf.DrawString(prefix + selected.InvoiceNumber.ToString(), textFont, XBrushes.Black, new XRect(leftMargin + 95, topMargin + 255, 100, 20), XStringFormats.TopLeft);
            tf.DrawString(selected.CreatedOn.ToString("dd-MM-yyyy"), textFont, XBrushes.Black, new XRect(leftMargin + 95, topMargin + 268, 100, 20), XStringFormats.TopLeft);

            /*------------------------------------------*/
            //debtor info rows
            tf.DrawString("Invoice", bigFont, XBrushes.Black, new XRect(leftMargin - 2, topMargin + 150, 80, 35), XStringFormats.TopLeft);

            tf.DrawString(name, textFont, XBrushes.Black, new XRect(rightMargin - 40, topMargin + 150, 160, 20), XStringFormats.TopLeft);
            tf.DrawString(dAddress, textFont, XBrushes.Black, new XRect(rightMargin - 40, topMargin + 165, 200, 20), XStringFormats.TopLeft);
            tf.DrawString(dCity, textFont, XBrushes.Black, new XRect(rightMargin - 40, topMargin + 180, 200, 20), XStringFormats.TopLeft);
            tf.DrawString(dCountry, textFont, XBrushes.Black, new XRect(rightMargin - 40, topMargin + 195, 200, 20), XStringFormats.TopLeft);

            /*------------------------------------------*/
            //Add divider between invoice info and message
            XColor color = new XColor();
            color.R = 0;
            color.G = 0;
            color.B = 0;

            int rightMargin2 = Convert.ToInt32(page.Width.Point) - 40;

            XPen pen = new XPen(color);
            Point a = new Point(leftMargin, topMargin + 290);
            Point b = new Point(rightMargin2, topMargin + 290);

            gfx.DrawLine(pen, a, b);

            /*--------------------------------------------------------------------------*/
            //invoice message
            string message = "Hereby we bring you in charge:";
            tf.DrawString(message, textFont, XBrushes.Black, new XRect(leftMargin, topMargin + 305, rightMargin2, 20), XStringFormats.TopLeft);

            /*--------------------------------------------------------------------------*/
            int yLoc = topMargin + 345;

            //table header
            XPoint th1 = new XPoint(leftMargin, yLoc);
            XPoint th2 = new XPoint(rightMargin2, yLoc + 14);

            XBrush brush = new XSolidBrush(XColor.FromArgb(255, 255, 255));
            XRect thead = new XRect(th1, th2);
            gfx.DrawRectangle(pen, brush, thead);

            //Table dividers
            Point d1 = new Point(leftMargin + 220, yLoc);
            Point d2 = new Point(leftMargin + 220, yLoc + 14);
            gfx.DrawLine(pen, d1, d2);

            Point d3 = new Point(leftMargin + 270, yLoc);
            Point d4 = new Point(leftMargin + 270, yLoc + 14);
            gfx.DrawLine(pen, d3, d4);

            Point d5 = new Point(leftMargin + 360, yLoc);
            Point d6 = new Point(leftMargin + 360, yLoc + 14);
            gfx.DrawLine(pen, d5, d6);

            Point d7 = new Point(rightMargin2 - 96, yLoc);
            Point d8 = new Point(rightMargin2 - 96, yLoc + 14);
            gfx.DrawLine(pen, d7, d8);

            /*--------------------------------------------------------------------------*/
            //Columns
            tf.DrawString("Product", textFontBold, XBrushes.Black, new XRect(leftMargin + 2, yLoc + 1, 188, 15), XStringFormats.TopLeft);
            tf.DrawString("Qnt.", textFontBold, XBrushes.Black, new XRect(leftMargin + 224, yLoc + 1, 25, 15), XStringFormats.TopLeft);
            tf.DrawString("Price", textFontBold, XBrushes.Black, new XRect(leftMargin + 274, yLoc + 1, 60, 15), XStringFormats.TopLeft);
            tf.DrawString("Tax", textFontBold, XBrushes.Black, new XRect(leftMargin + 364, yLoc + 1, 25, 15), XStringFormats.TopLeft);
            tf.DrawString("Total", textFontBold, XBrushes.Black, new XRect(rightMargin2 - 92, yLoc + 1, 60, 15), XStringFormats.TopLeft);

            /*--------------------------------------------------------------------------*/
            int y = topMargin + 359;

            decimal subTotal = 0;
            decimal total = 0;
            decimal btwDecimal = 0;

            for (int i = 0; i < productList.Count; i++)
            {
                Product product = productList[i];
                InvoiceItem pItem = itemList.Where(l => l.ProductID == product.ProductID
                                        && l.InvoiceNumber == selected.InvoiceNumber).FirstOrDefault();

                int tax = 100 + product.TaxPercentage;

                total += (decimal)(product.Price * pItem.Amount);
                subTotal += (total * 100) / tax;
                btwDecimal += (total - subTotal);

                /*--------------------------------------------*/
                //Product column
                XPoint t1 = new XPoint(leftMargin, y);
                XPoint t2 = new XPoint(leftMargin + 220, y + 15);

                XRect tProduct = new XRect(t1, t2);
                gfx.DrawRectangle(pen, brush, tProduct);
                /*--------------------------------------------*/
                //Quantity column
                XPoint t3 = new XPoint(leftMargin + 220, y);
                XPoint t4 = new XPoint(leftMargin + 270, y + 15);

                XRect tQnt = new XRect(t3, t4);
                gfx.DrawRectangle(pen, brush, tQnt);
                /*--------------------------------------------*/
                //Price column
                XPoint t5 = new XPoint(leftMargin + 270, y);
                XPoint t6 = new XPoint(leftMargin + 360, y + 15);

                XRect tPrice = new XRect(t5, t6);
                gfx.DrawRectangle(pen, brush, tPrice);
                /*--------------------------------------------*/
                //BTW column
                XPoint t7 = new XPoint(leftMargin + 360, y);
                XPoint t8 = new XPoint(leftMargin + 419, y + 15);

                XRect tBTW = new XRect(t7, t8);
                gfx.DrawRectangle(pen, brush, tBTW);
                /*--------------------------------------------*/
                //Total column
                XPoint t9 = new XPoint(rightMargin2 - 96, y);
                XPoint t10 = new XPoint(rightMargin2, y + 15);

                XRect tTotal = new XRect(t9, t10);
                gfx.DrawRectangle(pen, brush, tTotal);
                /*--------------------------------------------*/
                //Text
                tf.DrawString(product.Name, infoFont, XBrushes.Black, new XRect(leftMargin + 2, y + 2, 188, 15), XStringFormats.TopLeft);
                tf.DrawString(pItem.Amount.ToString(), infoFont, XBrushes.Black, new XRect(leftMargin + 224, y + 2, 188, 15), XStringFormats.TopLeft);
                tf.DrawString("€ " + string.Format("{0:N2}", product.Price), infoFont, XBrushes.Black, new XRect(leftMargin + 274, y + 2, 188, 15), XStringFormats.TopLeft);
                tf.DrawString(product.TaxPercentage + "%", infoFont, XBrushes.Black, new XRect(leftMargin + 364, y + 2, 188, 15), XStringFormats.TopLeft);
                tf.DrawString("€ " + string.Format("{0:N2}", (product.Price * pItem.Amount)), infoFont, XBrushes.Black, new XRect(rightMargin2 - 92, y + 2, 188, 15), XStringFormats.TopLeft);
                /*--------------------------------------------*/

                y = y + 15;
            }
            /*--------------------------------------------------------------------------*/
            //Text
            tf.DrawString("Subtotal", infoFont, XBrushes.Black, new XRect(rightMargin - 25, bottomMargin - 150, 45, 15), XStringFormats.TopLeft);
            tf.DrawString("Tax", infoFont, XBrushes.Black, new XRect(rightMargin - 25, bottomMargin - 135, 45, 15), XStringFormats.TopLeft);
            tf.DrawString("Total", infoFont, XBrushes.Black, new XRect(rightMargin - 25, bottomMargin - 114, 65, 15), XStringFormats.TopLeft);

            //Divider with plus sign
            Point subA = new Point(rightMargin - 25, bottomMargin - 118);
            Point subB = new Point(rightMargin + 115, bottomMargin - 118);

            gfx.DrawLine(pen, subA, subB);
            tf.DrawString("+", infoFont, XBrushes.Black, new XRect(rightMargin + 117, bottomMargin - 123.5, 5, 5), XStringFormats.TopLeft);

            //Numbers
            tf.DrawString("€" + " " + string.Format("{0:N2}", subTotal), infoFont, XBrushes.Black, new XRect(rightMargin + 25, bottomMargin - 150, 65, 15), XStringFormats.TopLeft);
            tf.DrawString("€" + " " + string.Format("{0:N2}", btwDecimal), infoFont, XBrushes.Black, new XRect(rightMargin + 25, bottomMargin - 135, 65, 15), XStringFormats.TopLeft);
            tf.DrawString("€" + " " + string.Format("{0:N2}", total), infoFont, XBrushes.Black, new XRect(rightMargin + 25, bottomMargin - 114, 65, 15), XStringFormats.TopLeft);
            /*--------------------------------------------------------------------------*/
            //Disclaimer
            int width = (Convert.ToInt32(page.Width.Point) - 40);
            int height = 50;
            tf.DrawString("We kindly request you to transfer the amount due within 30 days, stating the invoice number"
                + "\n" + "Our terms and conditions apply to all services",
                infoFontSmall, XBrushes.Black, new XRect(leftMargin, bottomMargin - 10, width, height),
                XStringFormats.TopLeft);
            /*--------------------------------------------------------------------------*/
            // Save the document...
            string fileName = "invoice_" + invoiceNumber + ".pdf";

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);

            //stream.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = fileName;

            return fileStreamResult;

            //End of method
        }



    }
}