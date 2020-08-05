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
using System.Globalization;

namespace Converter_1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public struct ceg
		{
			public string cegnev;
			public int kov_ceg;
			public string adoszam;
		}
		public struct szamla_tetelek
		{
			public string bizonylatszam;
			public string datum;
			public float netto;
			public string afa_szaz;
			public float afa;
			public float brutto;
			public string cegnev;
			public int kov_szamla;
		}
		public struct szamla_ossz
		{
			public string bizonylatszam;
			public string datum;
			public float brutto;
		}

		public string parsFile1(string textIn) {
			ceg[] ceglista = new ceg[50];
			szamla_tetelek[,] szamla_tetel = new szamla_tetelek[50,70];
			szamla_ossz[,] szamla_osszegzo = new szamla_ossz[50,20];
			int kov_ceg_sorsz = 0;
			int ceg_most;
			int kov_tetel_sorszam = 0;
			string retstr = "Add text" + textIn;
			// file ból tábala, [1] a fő tábla
			string[] strarr = textIn.Split(new[] { "table" }, StringSplitOptions.None);
			// táblából különböző áfakulcsok
			string[] strarr2 = strarr[1].Split(new[] { "</tr>" }, StringSplitOptions.None);

			treeView1.BeginUpdate();

			treeView1.Nodes.Add("File 1 adatok");
			//treeView1.Nodes[0].Nodes.Add("Számla 1");
			//treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 1");
			//treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 2");
			//treeView1.Nodes[0].Nodes[0].Nodes.Add("Sor 3");
			//treeView1.Nodes[0].Nodes.Add("Számla 2");
			//treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 4");
			//treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 5");
			//treeView1.Nodes[0].Nodes[1].Nodes.Add("Sor 6");
			//treeView1.Nodes.Add("Data 1");
			//treeView1.Nodes.Add("Data 2");
			//treeView1.Nodes.Add("Data 3");
			//treeView1.Nodes.Add("Data 4");
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
					string tests = "Adat0: " + adatb[0] + " Adat1: " + adatb[1] + " Adat2: " + adatb[2] + " Adat3: " + adatb[3] + " Adat4: " + adatb[4] + " Adat5: " + adatb[5];
					tests += " Adat6: " + adatb[6] + " Adat7: " + adatb[7] + " Adat8: " + adatb[8];
					if (kov_ceg_sorsz == 0)
					{
						ceglista[0].cegnev = adatb[2];
						ceglista[0].adoszam = adatb[3];
						ceglista[0].kov_ceg = 0;
						szamla_tetel[0, 0].bizonylatszam = adatb[1];
						szamla_tetel[0, 0].datum = adatb[0];
						szamla_tetel[0, 0].netto = float.Parse(adatb[5], CultureInfo.InvariantCulture.NumberFormat);
						szamla_tetel[0, 0].afa_szaz = adatb[6];
						szamla_tetel[0, 0].afa = float.Parse(adatb[7], CultureInfo.InvariantCulture.NumberFormat);
						szamla_tetel[0, 0].brutto = float.Parse(adatb[8], CultureInfo.InvariantCulture.NumberFormat);
						szamla_tetel[0, 0].cegnev = adatb[2];
						szamla_tetel[0, 0].kov_szamla = 0;
						kov_ceg_sorsz++;
					}
					else 
					{
						ceg_most = 51;
						for (int i = 0; i < kov_ceg_sorsz + 2; i++) 
						{
							if (ceglista[i].cegnev == adatb[2]) 
							{
								ceg_most = i;
								i = 51;
							}
						}
						if (ceg_most > 50)
						{
							ceglista[kov_ceg_sorsz].cegnev = adatb[2];
							ceglista[kov_ceg_sorsz].adoszam = adatb[3];
							ceglista[kov_ceg_sorsz].kov_ceg = 0;
							ceglista[kov_ceg_sorsz - 1].kov_ceg = 1;
							szamla_tetel[kov_ceg_sorsz, 0].bizonylatszam = adatb[1];
							szamla_tetel[kov_ceg_sorsz, 0].datum = adatb[0];
							szamla_tetel[kov_ceg_sorsz, 0].netto = float.Parse(adatb[5], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].afa_szaz = adatb[6];
							szamla_tetel[kov_ceg_sorsz, 0].afa = float.Parse(adatb[7], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].brutto = float.Parse(adatb[8], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].cegnev = adatb[2];
							szamla_tetel[kov_ceg_sorsz, 0].kov_szamla = 0;
							kov_ceg_sorsz++;
						}
						else
						{
							kov_tetel_sorszam = 0;
							for (int i = 0; i < 50; i++) 
							{
								if (szamla_tetel[ceg_most, i].kov_szamla == 0) 
								{
									kov_tetel_sorszam = i + 1;
									i = 51;
								}
							}
							szamla_tetel[ceg_most, kov_tetel_sorszam].bizonylatszam = adatb[1];
							szamla_tetel[ceg_most, kov_tetel_sorszam].datum = adatb[0];
							szamla_tetel[ceg_most, kov_tetel_sorszam].netto = float.Parse(adatb[5], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].afa_szaz = adatb[6];
							szamla_tetel[ceg_most, kov_tetel_sorszam].afa = float.Parse(adatb[7], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].brutto = float.Parse(adatb[8], CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].cegnev = adatb[2];
							szamla_tetel[ceg_most, kov_tetel_sorszam].kov_szamla = 0;
							szamla_tetel[ceg_most, kov_tetel_sorszam - 1].kov_szamla = 1;
						}
					}
				}
			}
			for (int i = 0; i < 51; i++) 
			{
				treeView1.Nodes[0].Nodes.Add(ceglista[i].cegnev + " + " + ceglista[i].adoszam + " + " + ceglista[i].kov_ceg + " ind: " + i);
				for (int k = 0; k < 71; k++)
				{
					treeView1.Nodes[0].Nodes[i].Nodes.Add("Biz: " + szamla_tetel[i, k].bizonylatszam + " date: " + szamla_tetel[i, k].datum + " netto: " + szamla_tetel[i, k].netto + " i: " + i + " k: " + k);
					if (szamla_tetel[i, k].kov_szamla == 0)
					{
						k = 72;
					}
				}
				if (ceglista[i].kov_ceg == 0)
				{
					i = 52;
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
