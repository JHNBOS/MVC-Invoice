using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class SettingsService : ISettingsService
    {
        private AppSettings _appSettings;

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
            if (value != "" || value != null || value != _appSettings.Email)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Email = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetPassword(string value)
        {
            if (value != "" || value != null || value != _appSettings.Password)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Password = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetName(string value)
        {
            if (value != "" || value != null || value != _appSettings.Name)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Name = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetSMTP(string value)
        {
            if (value != "" || value != null || value != _appSettings.SMTP)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.SMTP = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetPort(int value)
        {
            if (value != _appSettings.Port)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Port = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetWebsite(string value)
        {
            if (value != "" || value != null || value != _appSettings.Website)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Website = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetPhone(string value)
        {
            if (value != "" || value != null || value != _appSettings.Phone)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Phone = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetLogo(string value)
        {
            if (value != "" || value != null || value != _appSettings.Logo)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Logo = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetUseLogo(bool value)
        {
            if (value != _appSettings.UseLogo)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.UseLogo = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetAddress(string value)
        {
            if (value != "" || value != null || value != _appSettings.Address)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.Address = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetPostalCode(string value)
        {
            if (value != "" || value != null || value != _appSettings.PostalCode)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.PostalCode = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetCity(string value)
        {
            if (value != "" || value != null || value != _appSettings.City)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.City = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetTaxNumber(string value)
        {
            if (value != "" || value != null || value != _appSettings.TaxNumber)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.TaxNumber = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        public void SetCompanyNumber(string value)
        {
            if (value != "" || value != null || value != _appSettings.CompanyNumber)
            {
                string json = "";

                //Get content of json file
                using (StreamReader files = System.IO.File.OpenText(@"settings.json"))
                {
                    json = files.ReadToEnd();
                }

                //Change value
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);
                obj.Data.CompanyNumber = value;

                //Write new value to json file
                var newJson = JsonConvert.SerializeObject(obj);
                StreamWriter writer = new StreamWriter(@"settings.json", false);
                writer.Write(newJson);
                writer.Close();
            }
        }

        /* END OF SETTERS */
        /*--------------------------------------------------------------------*/

    }
}
