using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideIt.DB
{
    class DbConnectionStr
    {
        /// <summary>
        /// database connection string
        /// </summary>
        public static string conect()
        {
            return (@"Data Source = SAIRAHBATOOL-PC\SQLEXPRESS ; Initial Catalog = HideitDB ; Integrated Security= true");
        }
    }
}

