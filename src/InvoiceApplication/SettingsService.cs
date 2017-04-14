using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class SettingsService : ISettingsService
    {
        private readonly AppSettings _appSettings;

        public SettingsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /*--------------------------------------------------------------------*/

        /* BEGIN OF GETTERS */
        public string GetEmail()
        {
            return _appSettings.Email;
        }

        public string GetPassword()
        {
            return _appSettings.Password;
        }

        public string GetName()
        {
            return _appSettings.Name;
        }

        public string GetSMTP()
        {
            return _appSettings.SMTP;
        }

        public int GetPort()
        {
            return _appSettings.Port;
        }

        public string GetWebsite()
        {
            return _appSettings.Website;
        }

        public string GetPhone()
        {
            return _appSettings.Phone;
        }

        public string GetLogo()
        {
            return _appSettings.Logo;
        }

        public bool UseLogo()
        {
            return _appSettings.UseLogo;
        }

        public string GetAddress()
        {
            return _appSettings.Address;
        }

        public string GetPostalCode()
        {
            return _appSettings.PostalCode;
        }

        public string GetCity()
        {
            return _appSettings.City;
        }

        public string GetTaxNumber()
        {
            return _appSettings.TaxNumber;
        }

        public string GetCompanyNumber()
        {
            return _appSettings.CompanyNumber;
        }

        /* END OF GETTERS */

        /*--------------------------------------------------------------------*/

        /* BEGIN OF SETTERS */
        public void SetEmail(string value)
        {
            _appSettings.Email = value;
        }

        public void SetPassword(string value)
        {
            _appSettings.Password = value;
        }

        public void SetName(string value)
        {
            _appSettings.Name = value;
        }

        public void SetSMTP(string value)
        {
            _appSettings.SMTP = value;
        }

        public void SetPort(int value)
        {
            _appSettings.Port = value;
        }

        public void SetWebsite(string value)
        {
            _appSettings.Website = value;
        }

        public void SetPhone(string value)
        {
            _appSettings.Phone = value;
        }

        public void SetLogo(string value)
        {
            _appSettings.Logo = value;
        }

        public void SetUseLogo(bool value)
        {
            _appSettings.UseLogo = value;
        }

        public void SetAddress(string value)
        {
            _appSettings.Address = value;
        }

        public void SetPostalCode(string value)
        {
            _appSettings.PostalCode = value;
        }

        public void SetCity(string value)
        {
            _appSettings.City = value;
        }

        public void SetTaxNumber(string value)
        {
            _appSettings.TaxNumber = value;
        }

        public void SetCompanyNumber(string value)
        {
            _appSettings.CompanyNumber = value;
        }

        /* END OF SETTERS */
        /*--------------------------------------------------------------------*/

    }
}
