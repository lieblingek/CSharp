using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace Converter_1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public struct szamla
		{
			public string szamlaszam;
			public string datum;
			public float netto;
			public float afa;
			public float brutto;
		}

		public string parsFile1(string textIn) {
			szamla[,] main = new szamla[50,50];
			string retstr = "Add text" + textIn;
			// file ból tábala, [1] a fő tábla
			string[] strarr = textIn.Split(new[] { "table" }, StringSplitOptions.None);
			// táblából különböző áfakulcsok
			string[] strarr2 = strarr[1].Split(new[] { "</tr>" }, StringSplitOptions.None);

			treeView1.BeginUpdate();

			treeView1.Nodes.Add("Cég 1");
			treeView1.Nodes[0].Nodes.Add("Számla 1");
			treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 1");
			treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 2");
			treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 3");
			treeView1.Nodes[0].Nodes.Add("Számla 2");
			treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 4");
			treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 5");
			treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 6");
			treeView1.Nodes.Add("Data 1");
			treeView1.Nodes.Add("Data 2");
			treeView1.Nodes.Add("Data 3");
			treeView1.Nodes.Add("Data 4");
			List<string> line = new List<string>();
			foreach (string st in strarr2)
			{
				//string strRegex = @"(041A93\"></font>)";
				string strRegex = @"(>\+</font>)";
				Regex re = new Regex(strRegex);
				if (re.IsMatch(st))
				{
					line.Clear();
					string[] strarr3 = st.Split(new[] { "</font>" }, StringSplitOptions.None);
					// st2 -  0 -  - üres
					// st2 -0 1 - 2 - dátum
					// st2 -1 2 - 4 - Bizonylatszám
					// st2 -2 3 - 6 - Cégnév
					// st2 -3 4 - 8 - Adószám
					// st2 -4 5 - 10 - +
					// st2 -5 6 - 12 - Áfa alap
					// st2 -6 7 - 14 - Áfa %
					// st2 -7 8 - 16 - Áfa összeg
					// st2 -8 9 - 18 - Bruttó
					// string [,,] adatb; cégnév+adószám   Biozonylatszám+Dátum   ÁfaAlap+Áfa%+Áfa+Bruttó
					foreach (string st2 in strarr3)
					{
						string[] strarr4 = st2.Split(new[] { "041A93\">" }, StringSplitOptions.None);
						if (strarr4.Length > 1)
						{
							line.Add(strarr4[1]);
						}
					}
					string[] adatb = line.ToArray();
					treeView1.Nodes[3].Nodes.Add("++++++++++++ new 3 +++++++++++++");
					string tests = "Adat0: " + adatb[0] + " Adat1: " + adatb[1] + " Adat2: " + adatb[2] + " Adat3: " + adatb[3] + " Adat4: " + adatb[4] + " Adat5: " + adatb[5];
					tests += " Adat6: " + adatb[6] + " Adat7: " + adatb[7] + " Adat8: " + adatb[8];
					treeView1.Nodes[3].Nodes.Add(tests);
				}
			}
			treeView1.EndUpdate();

			//string retstr = "Almakorte";
			//string retstr2 = strarr.Reverse.;
			return strarr2[0];


		}
		private void load_Click(object sender, EventArgs e)
		{
			Stream file1_stream;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Számlák file megnyitása";
			ofd.Filter = "HTML file|*.htm|All file|*.*";
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
			{
				if ((file1_stream = ofd.OpenFile()) != null) {
					string strFileName = ofd.FileName;
					string file1text = File.ReadAllText(strFileName, Encoding.Default);
					// Comment 1
					richTextBox1.Text = parsFile1(file1text);
					//richTextBox1.Text = parsFile1(strFileName);
				}
			}
		}
	}
}
