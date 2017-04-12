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

        void SetEmail(string value);
        void SetPassword(string value);
        void SetSMTP(string value);
        void SetPort(int value);
        void SetName(string value);
        void SetWebsite(string value);
    }
}
