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

        /* END OF SETTERS */
        /*--------------------------------------------------------------------*/

    }
}
