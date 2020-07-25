using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace logger
{
    class logger
    {

        public bool writeFile ( string fnev, int sev, string msg)
        {
            string sever;
            bool fileExist;
            if (File.Exists(fnev))
            {
                fileExist = true;
            }else
            {
                fileExist = false;
            }
            switch (sev)
            {
                case 1:
                    sever = " EMER-1-";
                    break;
                case 2:
                    sever = " CRIT-2-";
                    break;
                case 3:
                    sever = " ERRO-3-";
                    break;
                case 4:
                    sever = " WARN-4-";
                    break;
                case 5:
                    sever = " INFO-5-";
                    break;
                case 6:
                    sever = " LOG--6-";
                    break;
                default:
                    sever = " DEBU-7-";
                    break;
            }
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(fnev, fileExist);
                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + sever + msg);
                file.Close();
                return true;
            }
            catch (Exception e)
            {
                string err_msg = e.ToString();
                if ( err_msg.Contains("denied") )
                {
                    MessageBox.Show("The file write is access denied !!!!! ");
                }
                //MessageBox.Show("An error occurred: " + e.ToString());
                return false;
            }
        }
    }
}
