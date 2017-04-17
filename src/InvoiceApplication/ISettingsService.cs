using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public interface ISettingsService
    {
        // Getter methods

        string GetEmail();
        string GetPassword();
        string GetSMTP();
        int GetPort();

        string GetWebsite();
        string GetPhone();

        string GetAddress();
        string GetPostalCode();
        string GetCity();
        string GetCountry();

        string GetLogo();
        bool UseLogo();

        string GetName();
        string GetTaxNumber();
        string GetCompanyNumber();

        // Setter methods

        void SetEmail(string value);
        void SetPassword(string value);
        void SetSMTP(string value);
        void SetPort(int value);

        void SetWebsite(string value);
        void SetPhone(string value);

        void SetAddress(string value);
        void SetPostalCode(string value);
        void SetCity(string value);
        void SetCountry(string value);

        void SetLogo(string value);
        void SetUseLogo(bool value);

        void SetName(string value);
        void SetTaxNumber(string value);
        void SetCompanyNumber(string value);
    }
}
