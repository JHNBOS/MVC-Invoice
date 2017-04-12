using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public interface IMySettingsService
    {
        string GetEmail();
        string GetPassword();
        string GetSMTP();
        int GetPort();
        string GetName();
        string GetWebsite();
        string GetPhone();
        string GetAddress();
        string GetPostalCode();
        string GetCity();
        string GetLogo();
        bool UseLogo();
        string GetTaxNumber();
        string GetCompanyNumber();

        void SetEmail(string value);
        void SetPassword(string value);
        void SetSMTP(string value);
        void SetPort(int value);
        void SetName(string value);
        void SetWebsite(string value);

        void SetPhone(string value);
        void SetAddress(string value);
        void SetPostalCode(string value);
        void SetCity(string value);
        void SetLogo(string value);
        void SetUseLogo(bool value);
        void SetTaxNumber(string value);
        void SetCompanyNumber(string value);
    }
}
