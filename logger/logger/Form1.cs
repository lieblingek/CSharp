using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; //process.module
using System.Text.RegularExpressions; //regexp
using logger;
using System.IO;
//using System.Globalization;
//using System.Text.RegularExpressions;

namespace logger
{


    public partial class logWindow : Form
    {
        public string[] Status = new string[4] { "System OK", "", "", "" };
        public logWindow()
        {
            InitializeComponent();
            Sever_combo.SelectedIndex = 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool valid;
            int poz;
            if ( fileNev.Text == "" ){
                MessageBox.Show("No filenev");
                valid = false;
            }else
            {
                poz = fileNev.Text.Length;
                poz -= 4;
                if ( poz < 1 || fileNev.Text.Substring(poz,4) != ".txt")
                {
                    MessageBox.Show("file extension not txt");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            }
            if ( valid && logText.Text == "")
            {
                MessageBox.Show("No log message");
                valid = false;
            }else
            {
                logger logg = new logger();
                Status[3] = Status[2];
                Status[2] = Status[1];
                Status[1] = Status[0];
                Status[0] = DateTime.Now.ToString("hh:mm:ss.fff") + "Write file successfull";
                bool res = logg.writeFile(fileNev.Text, (Sever_combo.SelectedIndex + 1), logText.Text);
                if ( res)
                {
                    Status[0] = DateTime.Now.ToString("hh:mm:ss.fff") + " Write file successfull";
                }
                else
                {
                    Status[0] = DateTime.Now.ToString("hh:mm:ss.fff") + " Write file not successfull";
                }
                foreach (ProcessModule processModule in Process.GetCurrentProcess().Modules)
                {
                    string regexp = @"CRYPT32";
                    Regex regRes = new Regex(regexp, RegexOptions.IgnoreCase);
                    Match m = regRes.Match(processModule.FileName);
                    if (m.Success)
                    {
                        logg.writeFile(fileNev.Text, 7, "Filename    " + processModule.FileName);
                        logg.writeFile(fileNev.Text, 7, "Base address" + processModule.BaseAddress.ToString());
                        logg.writeFile(fileNev.Text, 7, "Entry point " + processModule.EntryPointAddress.ToString());
                        logg.writeFile(fileNev.Text, 7, "Module Size " + processModule.ModuleMemorySize.ToString());
                        logg.writeFile(fileNev.Text, 7, "Modul name  " + processModule.ModuleName.ToString());
                        logg.writeFile(fileNev.Text, 7, "Module Size " + processModule.ToString());
                        memProgram mem = new memProgram();
                        mem.Memread();
                    }
                }
                labelStatus.Text = Status[0] + Environment.NewLine + Status[1] + Environment.NewLine + Status[2] + Environment.NewLine + Status[3];
            }
        }
    }
}
