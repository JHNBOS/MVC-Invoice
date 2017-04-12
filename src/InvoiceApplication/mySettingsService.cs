using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class mySettingsService : IMySettingsService
    {
        private readonly mySettings mySettings;

        public mySettingsService(IOptions<mySettings> _mySettings)
        {
            mySettings = _mySettings.Value;
        }

        /*--------------------------------------------------------------------*/

        /* BEGIN OF GETTERS */
        public string GetEmail()
        {
            return mySettings.Email;
        }

        public string GetPassword()
        {
            return mySettings.Password;
        }

        public string GetName()
        {
            return mySettings.Name;
        }

        public string GetSMTP()
        {
            return mySettings.SMTP;
        }

        public int GetPort()
        {
            return mySettings.Port;
        }

        public string GetWebsite()
        {
            return mySettings.Website;
        }

        public string GetPhone()
        {
            return mySettings.Phone;
        }

        public string GetLogo()
        {
            return mySettings.Logo;
        }

        public bool UseLogo()
        {
            return mySettings.UseLogo;
        }

        public string GetAddress()
        {
            return mySettings.Address;
        }

        public string GetPostalCode()
        {
            return mySettings.PostalCode;
        }

        public string GetCity()
        {
            return mySettings.City;
        }

        public string GetTaxNumber()
        {
            return mySettings.TaxNumber;
        }

        public string GetCompanyNumber()
        {
            return mySettings.CompanyNumber;
        }

        /* END OF GETTERS */

        /*--------------------------------------------------------------------*/

        /* BEGIN OF SETTERS */
        public void SetEmail(string value)
        {
            mySettings.Email = value;
        }

        public void SetPassword(string value)
        {
            mySettings.Password = value;
        }

        public void SetName(string value)
        {
            mySettings.Name = value;
        }

        public void SetSMTP(string value)
        {
            mySettings.SMTP = value;
        }

        public void SetPort(int value)
        {
            mySettings.Port = value;
        }

        public void SetWebsite(string value)
        {
            mySettings.Website = value;
        }

        public void SetPhone(string value)
        {
            mySettings.Phone = value;
        }

        public void SetLogo(string value)
        {
            mySettings.Logo = value;
        }

        public void SetUseLogo(bool value)
        {
            mySettings.UseLogo = value;
        }

        public void SetAddress(string value)
        {
            mySettings.Address = value;
        }

        public void SetPostalCode(string value)
        {
            mySettings.PostalCode = value;
        }

        public void SetCity(string value)
        {
            mySettings.City = value;
        }

        public void SetTaxNumber(string value)
        {
            mySettings.TaxNumber = value;
        }

        public void SetCompanyNumber(string value)
        {
            mySettings.CompanyNumber = value;
        }

        /* END OF SETTERS */
        /*--------------------------------------------------------------------*/

    }
}
