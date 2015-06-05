using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace team3
{
    class MainClass
    {
        public static void Main(String[] argv)
        {
            if (File.Exists(DatabaseConnection.DBName))
                File.Delete(DatabaseConnection.DBName);
             DatabaseConnection.Init();
        }
    }
}
