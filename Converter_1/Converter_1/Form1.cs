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
using System.Xml;
using static System.Xml.XmlDocument;

namespace Converter_1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public struct Alap_adatok
		{
			public string Nyotat_azon;
			public string Cegnev;
			public string Adoszam;
			public string ido_tol;
			public string ido_ig;
			public string verzio;
		}
		public struct Ceg
		{
			public string cegnev;
			public int kov_ceg;
			public string adoszam;
			public int ceg_szama;
			public int van_meg;
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
			public float netto;
			public float afa;
			public double brutto_kerek;
			public double netto_kerek;
			public double afa_kerek;
			public int kov_szamla;
			public int kijelolve;
		}
		public int max_ceg_szam = 100;
		public int max_tetel_szam = 100;
		public int max_szama_szam = 30;
		public Ceg[] ceglista = new Ceg[100];
		public Szamla_ossz[,] szamla_osszegzo = new Szamla_ossz[100, 30];
		public Alap_adatok alap_Adatok = new Alap_adatok();
		public int[,] Selector = new int[100, 30];
		public int max_checkbox;
		public XmlDocument xDoc = new XmlDocument();
		public string Xml_file_nev;
		public string datum_ev;

		public string ParsFile1(string textIn)
		{
			Szamla_tetelek[,] szamla_tetel = new Szamla_tetelek[max_ceg_szam, max_tetel_szam];
			int kov_ceg_sorsz = 0;
			int ceg_most;
			// file ból tábala, [1] a fő tábla
			string[] strarr = textIn.Split(new[] { "table" }, StringSplitOptions.None);
			// táblából különböző áfakulcsok
			string strRegex2 = @"intervallum:\s*(\d+)\s";
			Regex re2 = new Regex(strRegex2);
			Match m = re2.Match(strarr[0]);
			if (m.Success)
			{
				datum_ev = m.Groups[1].ToString();
			}
			string[] strarr2 = strarr[1].Split(new[] { "</tr>" }, StringSplitOptions.None);


			treeView1.BeginUpdate();

			treeView1.Nodes.Add("File 1 adatok");
			treeView1.Nodes.Add("File 1 adatok 2");
			List<string> line = new List<string>();
			foreach (string st in strarr2)
			{
			    //string strRegex = @"(041A93\"></font>)";
			    //intervallum: 2020
				string strRegex = @"(>\+</font>)";
				Regex re = new Regex(strRegex);
				if (re.IsMatch(st))
				{
					line.Clear();
					string[] strarr3 = st.Split(new[] { "</font>" }, StringSplitOptions.None);
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
					adatb[0] = datum_ev + adatb[0];
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
						szamla_osszegzo[i, 0].netto = szamla_tetel[i, k].netto;
						szamla_osszegzo[i, 0].datum = szamla_tetel[i, k].datum;
						szamla_osszegzo[i, 0].afa = szamla_tetel[i, k].afa;
						szamla_osszegzo[i, 0].kov_szamla = 0;
						szamla_osszegzo[i, 0].kijelolve = 0;
					}
					else
					{
						for (int j = 0; j < max_szama_szam; j++)
						{
							if (szamla_osszegzo[i, j].bizonylatszam == szamla_tetel[i, k].bizonylatszam)
							{
								szamla_osszegzo[i, j].netto += szamla_tetel[i, k].netto;
								szamla_osszegzo[i, j].brutto += szamla_tetel[i, k].brutto;
								szamla_osszegzo[i, j].afa += szamla_tetel[i, k].afa;
								j = max_szama_szam;
							}
							else if (szamla_osszegzo[i, j].kov_szamla == 0)
							{
								szamla_osszegzo[i, j + 1].bizonylatszam = szamla_tetel[i, k].bizonylatszam;
								szamla_osszegzo[i, j + 1].brutto = szamla_tetel[i, k].brutto;
								szamla_osszegzo[i, j + 1].netto = szamla_tetel[i, k].netto;
								szamla_osszegzo[i, j + 1].datum = szamla_tetel[i, k].datum;
								szamla_osszegzo[i, j + 1].afa = szamla_tetel[i, k].afa;
								szamla_osszegzo[i, j + 1].kov_szamla = 0;
								szamla_osszegzo[i, j + 1].kijelolve = 0;
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
				ceglista[i].ceg_szama = checkedListBox1.Items.Count;
				Selector[i, 0] = checkedListBox1.Items.Count;
				int j = 1;
				checkedListBox1.Items.Add(ceglista[i].cegnev + " Adószám: " + ceglista[i].adoszam);
				treeView1.Nodes[1].Nodes.Add(ceglista[i].cegnev + " + " + ceglista[i].adoszam + " + " + ceglista[i].kov_ceg + " ind: " + i);
				for (int k = 0; k < max_tetel_szam; k++)
				{
					szamla_osszegzo[i, k].brutto_kerek = Math.Round(szamla_osszegzo[i, k].brutto / 1000);
					szamla_osszegzo[i, k].netto_kerek = Math.Round(szamla_osszegzo[i, k].netto / 1000);
					szamla_osszegzo[i, k].afa_kerek = Math.Round(szamla_osszegzo[i, k].afa / 1000);
					treeView1.Nodes[1].Nodes[i].Nodes.Add("Biz: " + szamla_osszegzo[i, k].bizonylatszam + " date: " + szamla_osszegzo[i, k].datum + " afa: " + szamla_osszegzo[i, k].afa + " brutto: " + szamla_osszegzo[i, k].brutto + " brutto kerek: " + szamla_osszegzo[i, k].brutto_kerek + " afa kerek: " + szamla_osszegzo[i, k].afa_kerek);
					Selector[i, j] = checkedListBox1.Items.Count;
					j++;
					checkedListBox1.Items.Add("     Bizonylatsz.:" + szamla_osszegzo[i, k].bizonylatszam + " Dátum: " + szamla_osszegzo[i, k].datum + " Brutto: " + szamla_osszegzo[i, k].brutto_kerek + " Áfa: " + szamla_osszegzo[i, k].afa_kerek);
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
				if ((ofd.OpenFile()) != null)
				{
					string strFileName = ofd.FileName;
					string file1text = File.ReadAllText(strFileName, Encoding.Default);
					// Comment 1
					ParsFile1(file1text);
					//richTextBox1.Text = ParsFile1(file1text);
				}
			}
		}

		public string ParsFile2()
		{
			xDoc.Load(Xml_file_nev);
			richTextBox1.Text = "XML-ből kiolvasott adatok:\n";
			alap_Adatok.Nyotat_azon = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;
			richTextBox1.Text += "Fő nyomtatvány azonosító: " + alap_Adatok.Nyotat_azon + "\n";
			alap_Adatok.verzio = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText;
			alap_Adatok.Cegnev = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText;
			richTextBox1.Text += "Cégnév: " + alap_Adatok.Cegnev + "\n";
			alap_Adatok.Adoszam = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText;
			richTextBox1.Text += "Adószám: " + alap_Adatok.Adoszam + "\n";
			alap_Adatok.ido_tol = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText;
			richTextBox1.Text += "Ídőszak kezdetete: " + alap_Adatok.ido_tol + "\n";
			alap_Adatok.ido_ig = xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[3].ChildNodes[1].InnerText;
			richTextBox1.Text += "Ídőszak vége: " + alap_Adatok.ido_ig + "\n\n";
			if (xDoc.ChildNodes[1].ChildNodes[2] != null)
			{
				richTextBox1.Text += "Alnyomtatvány: " + xDoc.ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText + "\n";
				richTextBox1.Text += "Partnercég: " + xDoc.ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText + "\n";
				richTextBox1.Text += "Partner adószám: " + xDoc.ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[1].InnerText + "\n\n";

			}
			for (int i = 3; i < 55; i++)
			{
				if (xDoc.ChildNodes[1].ChildNodes[i] != null)
				{
					richTextBox1.Text += "Alnyomtatvány: " + xDoc.ChildNodes[1].ChildNodes[i].ChildNodes[0].ChildNodes[0].InnerText + "\n";
					richTextBox1.Text += "Partnercég: " + xDoc.ChildNodes[1].ChildNodes[i].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText + "\n";
					richTextBox1.Text += "Partner adószám: " + xDoc.ChildNodes[1].ChildNodes[i].ChildNodes[0].ChildNodes[3].ChildNodes[1].InnerText + "\n\n";
				}
			}
			if (ceglista[0].kov_ceg == 1)
			{ 
			    checkedListBox1.Enabled = true;
			    button5.Enabled = true;
			    button4.Enabled = true;
			    button3.Enabled = true;
			    button2.Enabled = true;
			}
			string ret = "alma";
			return ret;
		}

		private void Load_file2_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd2 = new OpenFileDialog();
			ofd2.Title = "ÁNYK file megnyitása";
			ofd2.Filter = "XML file|*.xml|All file|*.*";
			if (ofd2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if ((ofd2.OpenFile()) != null)
				{
					Xml_file_nev = ofd2.FileName;
					ParsFile2();
				}
			}
		}

		private void Timer1_Tick(object sender, EventArgs e)
		{
			if (checkedListBox1.Enabled == true)
			{
				for (int i = 0; i < max_ceg_szam; i++)
				{
					if (checkedListBox1.GetItemCheckState(ceglista[i].ceg_szama) == CheckState.Checked)
					{
						for (int k = 1; k < 100; k++)
						{
							checkedListBox1.SetItemCheckState(Selector[i, k], CheckState.Checked);
							if (Selector[i, k + 1] == 0)
							{
								k = 100;
							}
						}
					}
					if (ceglista[i].kov_ceg == 0)
					{
						i = max_ceg_szam;
					}
				}
			}
		}

		private void Button2_Click(object sender, EventArgs e)
		{
			// Minden kijelölést megszüntet
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
			}
		}

		private void Button3_Click(object sender, EventArgs e)
		{
			// Mindent kijelöl
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				checkedListBox1.SetItemCheckState(i, CheckState.Checked);
			}
		}

        private void Button5_Click(object sender, EventArgs e)
        {
			int last_index = 1;
			int ossz_kijelolt_ceg_szama = 0;
			int[] kijelolt_ceg_lista = new int[max_ceg_szam];
			string xmlns = "http://www.apeh.hu/abev/nyomtatvanyok/2005/01";
			if (xDoc.ChildNodes[1].ChildNodes[2] != null)
			{
				last_index = 2;
				for (int i = 3; i < 55; i++)
			    {
			    	if (xDoc.ChildNodes[1].ChildNodes[i] != null)
			    	{
						last_index = i;
					}
			    }
			}
			for (int i = 0; i < max_ceg_szam; i++)
			{
				int ceg_kijelolve = 0;
				for (int k = 1; k < 100; k++)
				{
					if (checkedListBox1.GetItemCheckState(Selector[i, k]) == CheckState.Checked)
					{
						ceg_kijelolve = 1;
						szamla_osszegzo[i, k-1].kijelolve = 1;
					}
					if (Selector[i, k + 1] == 0)
					{
						k = 100;
					}
				}
				if (ceg_kijelolve == 1)
				{
					kijelolt_ceg_lista[ossz_kijelolt_ceg_szama] = i;
				    ossz_kijelolt_ceg_szama++;
				}
				if (ceglista[i].kov_ceg == 0)
				{
					i = max_ceg_szam;
				}
			}
			for (int i = 0; i < max_ceg_szam; i++)
			{
				//richTextBox1.Text += "Kijelölt cég: " + ceglista[kijelolt_ceg_lista[i]].cegnev + "\n\n";
				XmlElement rootnode_ny = xDoc.CreateElement("nyomtatvany", xmlns);
				xDoc.ChildNodes[1].AppendChild(rootnode_ny);
				XmlElement rootnode_0 = xDoc.CreateElement("nyomtatvanyinformacio", xmlns);
				rootnode_ny.AppendChild(rootnode_0);
				XmlElement rootnode_0_0 = xDoc.CreateElement("nyomtatvanyazonosito", xmlns);
				rootnode_0_0.InnerText = "2065M";
				rootnode_0.AppendChild(rootnode_0_0);
				XmlElement rootnode_0_1 = xDoc.CreateElement("nyomtatvanyverzio", xmlns);
				rootnode_0_1.InnerText = alap_Adatok.verzio;
				rootnode_0.AppendChild(rootnode_0_1);
				XmlElement rootnode_0_2 = xDoc.CreateElement("adozo", xmlns);
				rootnode_0.AppendChild(rootnode_0_2);
				XmlElement rootnode_0_2_0 = xDoc.CreateElement("nev", xmlns);
				rootnode_0_2_0.InnerText = alap_Adatok.Cegnev;
				rootnode_0_2.AppendChild(rootnode_0_2_0);
				XmlElement rootnode_0_2_1 = xDoc.CreateElement("adoszam", xmlns);
				rootnode_0_2_1.InnerText = alap_Adatok.Adoszam;
				rootnode_0_2.AppendChild(rootnode_0_2_1);
				XmlElement rootnode_0_3 = xDoc.CreateElement("albizonylatazonositas", xmlns);
				rootnode_0.AppendChild(rootnode_0_3);
				XmlElement rootnode_0_3_0 = xDoc.CreateElement("megnevezes", xmlns);
				rootnode_0_3_0.InnerText = ceglista[kijelolt_ceg_lista[i]].cegnev;
				rootnode_0_3.AppendChild(rootnode_0_3_0);
				XmlElement rootnode_0_3_1 = xDoc.CreateElement("azonosito", xmlns);
				rootnode_0_3_1.InnerText = ceglista[kijelolt_ceg_lista[i]].adoszam;
				rootnode_0_3.AppendChild(rootnode_0_3_1);
				XmlElement rootnode_0_4 = xDoc.CreateElement("idoszak", xmlns);
				rootnode_0.AppendChild(rootnode_0_4);
				XmlElement rootnode_0_4_0 = xDoc.CreateElement("tol", xmlns);
				rootnode_0_4_0.InnerText = alap_Adatok.ido_tol;
				rootnode_0_4.AppendChild(rootnode_0_4_0);
				XmlElement rootnode_0_4_1 = xDoc.CreateElement("ig", xmlns);
				rootnode_0_4_1.InnerText = alap_Adatok.ido_ig;
				rootnode_0_4.AppendChild(rootnode_0_4_1);
				int tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[51].InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
				xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[51].InnerText = tmp.ToString();
				tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[59].InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
				xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[59].InnerText = tmp.ToString();
				XmlElement rootnode_1 = xDoc.CreateElement("mezok", xmlns);
				rootnode_ny.AppendChild(rootnode_1);
				XmlElement rootnode_1_0 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_0.SetAttribute("eazon", "0A0001C001A");
				rootnode_1_0.InnerText = alap_Adatok.Adoszam;
				rootnode_1.AppendChild(rootnode_1_0);
				XmlElement rootnode_1_1 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_1.SetAttribute("eazon", "0A0001C005A");
				rootnode_1_1.InnerText = alap_Adatok.Cegnev;
				rootnode_1.AppendChild(rootnode_1_1);
				XmlElement rootnode_1_2 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_2.SetAttribute("eazon", "0A0001C006A");
				rootnode_1_2.InnerText = ceglista[kijelolt_ceg_lista[i]].adoszam;
				rootnode_1.AppendChild(rootnode_1_2);
				XmlElement rootnode_1_3 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_3.SetAttribute("eazon", "0A0001C008A");
				rootnode_1_3.InnerText = ceglista[kijelolt_ceg_lista[i]].cegnev;
				rootnode_1.AppendChild(rootnode_1_3);
				XmlElement rootnode_1_4 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_4.SetAttribute("eazon", "0A0001D001A");
				rootnode_1_4.InnerText = alap_Adatok.ido_tol;
				rootnode_1.AppendChild(rootnode_1_4);
				XmlElement rootnode_1_5 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_5.SetAttribute("eazon", "0A0001D002A");
				rootnode_1_5.InnerText = alap_Adatok.ido_ig;
				rootnode_1.AppendChild(rootnode_1_5);
				XmlElement rootnode_1_6 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_6.SetAttribute("eazon", "0A0001E0004BA");
				rootnode_1_6.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_6);
				XmlElement rootnode_1_7 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_7.SetAttribute("eazon", "0A0001E0004CA");
				rootnode_1_7.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_7);
				XmlElement rootnode_1_8 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_8.SetAttribute("eazon", "0A0001E0004DA");
				rootnode_1_8.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_8);
				XmlElement rootnode_1_9 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_9.SetAttribute("eazon", "0A0001E0007BA");
				rootnode_1_9.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_9);
				XmlElement rootnode_1_10 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_10.SetAttribute("eazon", "0A0001E0007CA");
				rootnode_1_10.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_10);
				XmlElement rootnode_1_11 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_11.SetAttribute("eazon", "0A0001E0007DA");
				rootnode_1_11.InnerText = "0";
				rootnode_1.AppendChild(rootnode_1_11);
				XmlElement rootnode_1_12 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_12.SetAttribute("eazon", "0B0001B001A");
				rootnode_1_12.InnerText = "1";
				rootnode_1.AppendChild(rootnode_1_12);
				XmlElement rootnode_1_13 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_13.SetAttribute("eazon", "0B0001B002A");
				rootnode_1_13.InnerText = alap_Adatok.Adoszam;
				rootnode_1.AppendChild(rootnode_1_13);
				XmlElement rootnode_1_14 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_14.SetAttribute("eazon", "0B0001B004A");
				rootnode_1_14.InnerText = alap_Adatok.ido_tol;
				rootnode_1.AppendChild(rootnode_1_14);
				XmlElement rootnode_1_15 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_15.SetAttribute("eazon", "0B0001B005A");
				rootnode_1_15.InnerText = alap_Adatok.ido_ig;
				rootnode_1.AppendChild(rootnode_1_15);
				XmlElement rootnode_1_16 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_16.SetAttribute("eazon", "0B0001B006A");
				rootnode_1_16.InnerText = alap_Adatok.Cegnev;
				rootnode_1.AppendChild(rootnode_1_16);
				XmlElement rootnode_1_17 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_17.SetAttribute("eazon", "0B0001B007A");
				rootnode_1_17.InnerText = ceglista[kijelolt_ceg_lista[i]].adoszam;
				rootnode_1.AppendChild(rootnode_1_17);
				XmlElement rootnode_1_18 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_18.SetAttribute("eazon", "0B0001B009A");
				rootnode_1_18.InnerText = ceglista[kijelolt_ceg_lista[i]].cegnev;
				rootnode_1.AppendChild(rootnode_1_18);
				int row = 1;
				for (int k = 0; k < max_szama_szam; k++)
				{
					if (szamla_osszegzo[kijelolt_ceg_lista[i], k].kijelolve == 1)
					{
						XmlElement rootnode_1_19 = xDoc.CreateElement("mezo", xmlns);
						rootnode_1_19.SetAttribute("eazon", "0B0001C000" + row + "AA");
						rootnode_1_19.InnerText = szamla_osszegzo[kijelolt_ceg_lista[i], k].bizonylatszam;
						rootnode_1.AppendChild(rootnode_1_19);
						XmlElement rootnode_1_20 = xDoc.CreateElement("mezo", xmlns);
						rootnode_1_20.SetAttribute("eazon", "0B0001C000" + row + "BA");
						rootnode_1_20.InnerText = szamla_osszegzo[kijelolt_ceg_lista[i], k].datum;
						rootnode_1.AppendChild(rootnode_1_20);
						XmlElement rootnode_1_21 = xDoc.CreateElement("mezo", xmlns);
						rootnode_1_21.SetAttribute("eazon", "0B0001C000" + row + "CA");
						rootnode_1_21.InnerText = szamla_osszegzo[kijelolt_ceg_lista[i], k].netto_kerek.ToString();
						rootnode_1.AppendChild(rootnode_1_21);
						XmlElement rootnode_1_22 = xDoc.CreateElement("mezo", xmlns);
						rootnode_1_22.SetAttribute("eazon", "0B0001C000" + row + "DA");
						rootnode_1_22.InnerText = szamla_osszegzo[kijelolt_ceg_lista[i], k].afa_kerek.ToString();
						rootnode_1.AppendChild(rootnode_1_22);
						tmp = int.Parse(rootnode_1_6.InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
						rootnode_1_6.InnerText = tmp.ToString();
						tmp = int.Parse(rootnode_1_9.InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
						rootnode_1_9.InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[52].InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[52].InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[60].InnerText, CultureInfo.InvariantCulture.NumberFormat) + 1;
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[60].InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[53].InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].netto_kerek);
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[53].InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[61].InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].netto_kerek);
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[61].InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[54].InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].afa_kerek);
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[54].InnerText = tmp.ToString();
						tmp = int.Parse(xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[62].InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].afa_kerek);
						xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[62].InnerText = tmp.ToString();
						tmp = int.Parse(rootnode_1_7.InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].netto_kerek);
						rootnode_1_7.InnerText = tmp.ToString();
						tmp = int.Parse(rootnode_1_10.InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].netto_kerek);
						rootnode_1_10.InnerText = tmp.ToString();
						tmp = int.Parse(rootnode_1_8.InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].afa_kerek);
						rootnode_1_8.InnerText = tmp.ToString();
						tmp = int.Parse(rootnode_1_11.InnerText, CultureInfo.InvariantCulture.NumberFormat) + Convert.ToInt32(szamla_osszegzo[kijelolt_ceg_lista[i], k].afa_kerek);
						rootnode_1_11.InnerText = tmp.ToString();
						row++;
					}
					if (szamla_osszegzo[kijelolt_ceg_lista[i], k].kov_szamla == 0)
					{
						k = max_tetel_szam;
					}
				}
				XmlElement rootnode_1_23 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_23.SetAttribute("eazon", "0B0001C0037CA");
				rootnode_1_23.InnerText = rootnode_1_7.InnerText;
				rootnode_1.AppendChild(rootnode_1_23);
				XmlElement rootnode_1_24 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_24.SetAttribute("eazon", "0B0001C0037DA");
				rootnode_1_24.InnerText = rootnode_1_8.InnerText;
				rootnode_1.AppendChild(rootnode_1_24);

				XmlElement rootnode_1_25 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_25.SetAttribute("eazon", "0C0001B001A");
				rootnode_1_25.InnerText = "1";
				rootnode_1.AppendChild(rootnode_1_25);
				XmlElement rootnode_1_26 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_26.SetAttribute("eazon", "0C0001B002A");
				rootnode_1_26.InnerText = alap_Adatok.Adoszam;
				rootnode_1.AppendChild(rootnode_1_26);
				XmlElement rootnode_1_27 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_27.SetAttribute("eazon", "0C0001B004A");
				rootnode_1_27.InnerText = alap_Adatok.ido_tol;
				rootnode_1.AppendChild(rootnode_1_27);
				XmlElement rootnode_1_28 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_28.SetAttribute("eazon", "0C0001B005A");
				rootnode_1_28.InnerText = alap_Adatok.ido_ig;
				rootnode_1.AppendChild(rootnode_1_28);
				XmlElement rootnode_1_29 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_29.SetAttribute("eazon", "0C0001B006A");
				rootnode_1_29.InnerText = alap_Adatok.Cegnev;
				rootnode_1.AppendChild(rootnode_1_29);
				XmlElement rootnode_1_30 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_30.SetAttribute("eazon", "0C0001B007A");
				rootnode_1_30.InnerText = ceglista[kijelolt_ceg_lista[i]].adoszam;
				rootnode_1.AppendChild(rootnode_1_30);
				XmlElement rootnode_1_31 = xDoc.CreateElement("mezo", xmlns);
				rootnode_1_31.SetAttribute("eazon", "0C0001B009A");
				rootnode_1_31.InnerText = ceglista[kijelolt_ceg_lista[i]].cegnev;
				rootnode_1.AppendChild(rootnode_1_31);



				if (kijelolt_ceg_lista[i + 1] == 0)
				{
					i = max_ceg_szam;
				}
			}
			string strRegex3 = @"(.*)\.xml";
			Regex re3 = new Regex(strRegex3);
			Match m2 = re3.Match(Xml_file_nev);
			string new_file_name = m2.Groups[1].ToString();
			new_file_name += "_new.xml";
			xDoc.Save(new_file_name);
		}

        private void button4_Click(object sender, EventArgs e)
        {
			// Magyart kijelöl
			string strRegex4 = @"\d{8}-?\d-?\d{2}";
			Regex re4 = new Regex(strRegex4);
			
			for (int i = 0; i < max_ceg_szam; i++)
			{
				Match m4 = re4.Match(ceglista[i].adoszam);
				if (m4.Success)
				{
					checkedListBox1.SetItemCheckState(ceglista[i].ceg_szama, CheckState.Checked);
					//richTextBox1.Text = "Selected: " + ceglista[i].adoszam + "\n";
				}
				if (ceglista[i].kov_ceg == 0)
				{
					i = max_ceg_szam;
				}
			}
		}
	}
}
