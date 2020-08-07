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

		public struct Ceg
		{
			public string cegnev;
			public int kov_ceg;
			public string adoszam;
		}
		public struct Szamla_tetelek
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
		public struct Szamla_ossz
		{
			public string bizonylatszam;
			public string datum;
			public float brutto;
			public float afa;
			public double brutto_kerek;
			public double afa_kerek;
			public int kov_szamla;
		}

		public string ParsFile1(string textIn) {
			int max_ceg_szam = 100;
			int max_tetel_szam = 100;
			int max_szama_szam = 30;
			Ceg[] ceglista = new Ceg[max_ceg_szam];
			Szamla_tetelek[,] szamla_tetel = new Szamla_tetelek[max_ceg_szam, max_tetel_szam];
			Szamla_ossz[,] szamla_osszegzo = new Szamla_ossz[max_ceg_szam, max_szama_szam];
			int kov_ceg_sorsz = 0;
			int ceg_most;
			// file ból tábala, [1] a fő tábla
			string[] strarr = textIn.Split(new[] { "table" }, StringSplitOptions.None);
			// táblából különböző áfakulcsok
			string[] strarr2 = strarr[1].Split(new[] { "</tr>" }, StringSplitOptions.None);
			

			treeView1.BeginUpdate();

			treeView1.Nodes.Add("File 1 adatok");
			treeView1.Nodes.Add("File 1 adatok 2");
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
					Regex pattern = new Regex(",");
					String test1 = adatb[5].Replace(',', '.');
					String test2 = adatb[7].Replace(',', '.');
					String test3 = adatb[8].Replace(',', '.');
					string tests = "Adat0: " + adatb[0] + " Adat1: " + adatb[1] + " Adat2: " + adatb[2] + " Adat3: " + adatb[3] + " Adat4: " + adatb[4] + " Adat5: " + adatb[5];
					tests += " Adat6: " + adatb[6] + " Adat7: " + adatb[7] + " Adat8: " + adatb[8];
					if (kov_ceg_sorsz == 0)
					{
						ceglista[0].cegnev = adatb[2];
						ceglista[0].adoszam = adatb[3];
						ceglista[0].kov_ceg = 0;
						szamla_tetel[0, 0].bizonylatszam = adatb[1];
						szamla_tetel[0, 0].datum = adatb[0];
						szamla_tetel[0, 0].netto = float.Parse(test1, CultureInfo.InvariantCulture);
						szamla_tetel[0, 0].afa_szaz = adatb[6];
						szamla_tetel[0, 0].afa = float.Parse(test2, CultureInfo.InvariantCulture.NumberFormat);
						szamla_tetel[0, 0].brutto = float.Parse(test3, CultureInfo.InvariantCulture.NumberFormat);
						szamla_tetel[0, 0].cegnev = adatb[2];
						szamla_tetel[0, 0].kov_szamla = 0;
						kov_ceg_sorsz++;
					}
					else 
					{
						ceg_most = max_ceg_szam + 1;
						for (int i = 0; i < kov_ceg_sorsz + 2; i++) 
						{
							if (ceglista[i].cegnev == adatb[2]) 
							{
								ceg_most = i;
								i = max_ceg_szam + 1;
							}
						}
						if (ceg_most > max_ceg_szam)
						{
							ceglista[kov_ceg_sorsz].cegnev = adatb[2];
							ceglista[kov_ceg_sorsz].adoszam = adatb[3];
							ceglista[kov_ceg_sorsz].kov_ceg = 0;
							ceglista[kov_ceg_sorsz - 1].kov_ceg = 1;
							szamla_tetel[kov_ceg_sorsz, 0].bizonylatszam = adatb[1];
							szamla_tetel[kov_ceg_sorsz, 0].datum = adatb[0];
							szamla_tetel[kov_ceg_sorsz, 0].netto = float.Parse(test1, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].afa_szaz = adatb[6];
							szamla_tetel[kov_ceg_sorsz, 0].afa = float.Parse(test2, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].brutto = float.Parse(test3, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[kov_ceg_sorsz, 0].cegnev = adatb[2];
							szamla_tetel[kov_ceg_sorsz, 0].kov_szamla = 0;
							kov_ceg_sorsz++;
						}
						else
						{
							int kov_tetel_sorszam = 0;
							for (int i = 0; i < max_ceg_szam; i++) 
							{
								if (szamla_tetel[ceg_most, i].kov_szamla == 0) 
								{
									kov_tetel_sorszam = i + 1;
									i = max_ceg_szam;
								}
							}
							szamla_tetel[ceg_most, kov_tetel_sorszam].bizonylatszam = adatb[1];
							szamla_tetel[ceg_most, kov_tetel_sorszam].datum = adatb[0];
							szamla_tetel[ceg_most, kov_tetel_sorszam].netto = float.Parse(test1, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].afa_szaz = adatb[6];
							szamla_tetel[ceg_most, kov_tetel_sorszam].afa = float.Parse(test2, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].brutto = float.Parse(test3, CultureInfo.InvariantCulture.NumberFormat);
							szamla_tetel[ceg_most, kov_tetel_sorszam].cegnev = adatb[2];
							szamla_tetel[ceg_most, kov_tetel_sorszam].kov_szamla = 0;
							szamla_tetel[ceg_most, kov_tetel_sorszam - 1].kov_szamla = 1;
						}
					}
				}
			}
			for (int i = 0; i < max_ceg_szam; i++) 
			{
				treeView1.Nodes[0].Nodes.Add(ceglista[i].cegnev + " + " + ceglista[i].adoszam + " + " + ceglista[i].kov_ceg + " ind: " + i);
				for (int k = 0; k < max_tetel_szam; k++)
				{
					treeView1.Nodes[0].Nodes[i].Nodes.Add("Biz: " + szamla_tetel[i, k].bizonylatszam + " date: " + szamla_tetel[i, k].datum + " afa: " + szamla_tetel[i, k].afa + " netto: " + szamla_tetel[i, k].netto + " i: " + i + " k: " + k);
					if (k == 0)
					{
						szamla_osszegzo[i, 0].bizonylatszam = szamla_tetel[i, k].bizonylatszam;
						szamla_osszegzo[i, 0].brutto = szamla_tetel[i, k].brutto;
						szamla_osszegzo[i, 0].datum = szamla_tetel[i, k].datum;
						szamla_osszegzo[i, 0].afa = szamla_tetel[i, k].afa;
						szamla_osszegzo[i, 0].kov_szamla = 0;
					}
					else
					{
						for (int j = 0; j < max_szama_szam; j++)
						{
							if (szamla_osszegzo[i, j].bizonylatszam == szamla_tetel[i, k].bizonylatszam)
							{
								szamla_osszegzo[i, j].brutto += szamla_tetel[i, k].brutto;
								szamla_osszegzo[i, j].afa += szamla_tetel[i, k].afa;
								j = max_szama_szam;
							}
							else if (szamla_osszegzo[i, j].kov_szamla == 0 )
							{
								szamla_osszegzo[i, j + 1].bizonylatszam = szamla_tetel[i, k].bizonylatszam;
								szamla_osszegzo[i, j + 1].brutto = szamla_tetel[i, k].brutto;
								szamla_osszegzo[i, j + 1].datum = szamla_tetel[i, k].datum;
								szamla_osszegzo[i, j + 1].afa = szamla_tetel[i, k].afa;
								szamla_osszegzo[i, j + 1].kov_szamla = 0;
								szamla_osszegzo[i, j].kov_szamla = 1;
								j = max_szama_szam;
							}
						}
					}
					if (szamla_tetel[i, k].kov_szamla == 0)
					{
						k = max_tetel_szam;
					}
				}
				if (ceglista[i].kov_ceg == 0)
				{
					i = max_ceg_szam;
				}
			}
			for (int i = 0; i < max_ceg_szam; i++)
			{
				treeView1.Nodes[1].Nodes.Add(ceglista[i].cegnev + " + " + ceglista[i].adoszam + " + " + ceglista[i].kov_ceg + " ind: " + i);
				for (int k = 0; k < max_tetel_szam; k++)
				{
					szamla_osszegzo[i, k].brutto_kerek = Math.Round(szamla_osszegzo[i, k].brutto / 1000);
					szamla_osszegzo[i, k].afa_kerek = Math.Round(szamla_osszegzo[i, k].afa / 1000);
					treeView1.Nodes[1].Nodes[i].Nodes.Add("Biz: " + szamla_osszegzo[i, k].bizonylatszam + " date: " + szamla_osszegzo[i, k].datum + " afa: " + szamla_osszegzo[i, k].afa + " brutto: " + szamla_osszegzo[i, k].brutto + " brutto kerek: " + szamla_osszegzo[i, k].brutto_kerek + " afa kerek: " + szamla_osszegzo[i, k].afa_kerek);
					if (szamla_osszegzo[i, k].kov_szamla == 0)
					{
						k = max_tetel_szam;
					}
				}
				if (ceglista[i].kov_ceg == 0)
				{
					i = max_ceg_szam;
				}
			}
			treeView1.EndUpdate();
			return strarr2[0];
		}
		private void Load_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Számlák file megnyitása";
			ofd.Filter = "HTML file|*.htm|All file|*.*";
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
			{
				if ((ofd.OpenFile()) != null) {
					string strFileName = ofd.FileName;
					string file1text = File.ReadAllText(strFileName, Encoding.Default);
					// Comment 1
					ParsFile1(file1text);
					//richTextBox1.Text = ParsFile1(file1text);
				}
			}
		}
	}
}
