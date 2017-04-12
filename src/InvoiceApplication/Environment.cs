using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class Environment
    {
        private static Environment instance;
        private static String _rootPath;

        private Environment() { }

        public static Environment Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Environment();
                }
                return instance;
            }
        }

        public static string rootPath
        {
            set
            {
                _rootPath = value;
            }
            get
            {
                return _rootPath;
            }
        }
    }
}