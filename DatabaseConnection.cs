using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biometrics_Automation_System
{
    internal class DatabaseConnection
    {
        public string GetConnection()
        {
            //string con = @"Data Source=10.244.5.244;Initial Catalog=MYHR-TKDC_v2_roselle;Persist Security Info=True;User ID=sa;Password=essbu2013";
            string con = @"Data Source=10.244.5.244;Initial Catalog=MYHR-TKDC;User ID=sa;Password=essbu2013";
            return con;
        }
    }
}
