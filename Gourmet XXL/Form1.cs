using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using System.Data.SqlClient;

using System.Net;
using System.Drawing.Printing;

namespace Gourmet_XXL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Variablen!!!
        ////SqlConnection dataBaseConnection;
        ////SqlDataReader dataBaseReader;
        List<Speise> speisen = new List<Speise>();
        Settings options = new Settings();

        //SuchForm
        SuchForm suche;

        bool internetRecipes = false;

        //Eckbefehlsknöpfe
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Form bewegen
        private Point mouseposition;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseposition = new Point(-e.X, -e.Y);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseposition.X, mouseposition.Y);
                Location = mousePos;
            }
        }

        //Neue Speise
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            updateList();
        }

        //Liste updaten
        public void updateList()
        {
            listBox1.Items.Clear();
            speisen.Clear();

            if (!internetRecipes)
            {
                DirectoryInfo Dinfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\");
                foreach (FileInfo info in Dinfo.GetFiles("*.grec"))
                {
                    speisen.Add(grecDateiAuslesen(info.FullName));
                }

                foreach (Speise s in speisen)
                {
                    listBox1.Items.Add(s.Name);
                }
            }
            else
            {
                List<string> rezepte = FTPClass.getFileList(@"ftp://ftp.lima-city.de/gourmetxxl/", "Hausseite", "tingle25");

                if (rezepte.Count != 0)
                {
                    foreach (string s in rezepte)
                    {
                        if (s.Contains(".grec"))
                        {
                            speisen.Add(grecDateiAuslesenString(FTPClass.DateiAuslesen(@"ftp://ftp.lima-city.de/gourmetxxl/" + s, "Hausseite", "tingle25"), s.Replace("ftp://ftp.lima-city.de/gourmetxxl/", "").Replace(".grec", "")));
                        }
                    }

                    foreach (Speise sp in speisen)
                    {
                        listBox1.Items.Add(sp.Name);
                    }
                }
            }

            textBox1.Text = "";
            label2.Text = "Kein Gericht ausgewählt";
        }

        //"Speise"-List<string> updaten
        public void updateSpeisen()
        {
            listBox1.Items.Clear();
            speisen.Clear();

            if (!internetRecipes)
            {
                DirectoryInfo Dinfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\");
                foreach (FileInfo info in Dinfo.GetFiles("*.grec"))
                {
                    speisen.Add(grecDateiAuslesen(info.FullName));
                }

                for (int i = 0; i < speisen.Count; i++)
                {
                    speisen[i].Name += " ";
                }
            }
            else
            {
                List<string> rezepte = FTPClass.getFileList(@"ftp://ftp.lima-city.de/gourmetxxl/", "Hausseite", "tingle25");

                if (rezepte.Count != 0)
                {
                    foreach (string s in rezepte)
                    {
                        if (s.Contains(".grec"))
                        {
                            speisen.Add(grecDateiAuslesenString(FTPClass.DateiAuslesen(@"ftp://ftp.lima-city.de/gourmetxxl/" + s, "Hausseite", "tingle25"), s.Replace(".grec", "")));
                        }
                    }
                }

                for (int i = 0; i < speisen.Count; i++)
                {
                    speisen[i].Name += " ";
                }
            }

            textBox1.Text = "";
            label2.Text = "Kein Gericht ausgewählt";

            //////DataBase

            ////string conString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "recipes\\Rezepte.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

            ////try
            ////{
            ////    dataBaseConnection = new SqlConnection(conString);
            ////    dataBaseConnection.Open();
            ////}
            ////catch (Exception ex)
            ////{
            ////    MessageBox.Show(ex.Message);
            ////}

            ////SqlCommand com = dataBaseConnection.CreateCommand();
            ////com.CommandText = "SELECT * FROM recipes";
            ////dataBaseReader = com.ExecuteReader();

            ////int aktDatensatz = -1;

            ////while (dataBaseReader.Read())
            ////{
            ////    aktDatensatz++;
            ////    Speise tempSpeise = new Speise();

            ////    tempSpeise.Name = dataBaseReader.GetString(0);
            ////    tempSpeise.Attribute = dataBaseReader.GetString(1);
            ////    tempSpeise.Beschreibung = dataBaseReader.GetString(2);
            ////    tempSpeise.Dauer = dataBaseReader.GetInt32(3).ToString();
            ////    tempSpeise.Schwierigkeit = dataBaseReader.GetInt32(4).ToString();
            ////    tempSpeise.Zutaten = dataBaseReader.GetString(5);
            ////    tempSpeise.Zubereitung = dataBaseReader.GetString(6);

            ////    speisen.Add(tempSpeise);
            ////    listBox1.Items.Add(speisen[aktDatensatz].Name);
            ////}

            //////DB-Ende
        }

        //Form1 load
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\");
            }

            //if (options.hasToConvert)
            //{
            #region Convert

            //    String[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\");

            //    foreach (string fileName in files)
            //    {
            //        try
            //        {
            //            string line;
            //            int wo = 0;

            //            Speise speiseToReturn = new Speise();

            //            // Read the file and display it line by line.
            //            System.IO.StreamReader file = new System.IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + fileName);

            //            speiseToReturn.Name = Path.GetFileNameWithoutExtension(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + fileName);

            //            speiseToReturn.Attribute = "";
            //            speiseToReturn.Beschreibung = "";
            //            speiseToReturn.Zutaten = "";
            //            speiseToReturn.Zubereitung = "";

            //            while ((line = file.ReadLine()) != null)
            //            {
            //                if (line == ";")
            //                {
            //                    wo++;
            //                    continue;
            //                }

            //                if (wo == 0)
            //                {
            //                    speiseToReturn.Attribute += line + "; ";
            //                }

            //                if (wo == 1)
            //                {
            //                    speiseToReturn.Beschreibung += line + "\r\n";
            //                }

            //                if (wo == 2)
            //                {
            //                    speiseToReturn.Dauer = line;
            //                }

            //                if (wo == 3)
            //                {
            //                    speiseToReturn.Schwierigkeit = line;
            //                }

            //                if (wo == 4)
            //                {
            //                    speiseToReturn.Zutaten += line + "\r\n";
            //                }

            //                if (wo == 5)
            //                {
            //                    speiseToReturn.Zubereitung += line + "\r\n";
            //                }
            //            }

            //            file.Close();

            //            speiseToReturn.Beilagen = "";
            //            speiseToReturn.Personen = "";

            //            //Speichern
            //            StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + textBox1.Text + ".grec");

            //            sw.AutoFlush = true;

            //            sw.WriteLine(speiseToReturn.Attribute);

            //            sw.WriteLine(";");

            //            sw.WriteLine(speiseToReturn.Beschreibung);

            //            sw.WriteLine(";");

            //            sw.WriteLine(speiseToReturn.Dauer);

            //            sw.WriteLine(";");

            //            sw.WriteLine(speiseToReturn.Schwierigkeit);

            //            sw.WriteLine(";");

            //            sw.WriteLine(speiseToReturn.Zutaten);

            //            sw.WriteLine(";");
            //            sw.WriteLine(speiseToReturn.Zubereitung);

            //            sw.WriteLine(";");
            //            sw.WriteLine(speiseToReturn.Personen);

            //            sw.WriteLine(";");
            //            sw.WriteLine(speiseToReturn.Beilagen);

            //            sw.Close();
            //            sw.Dispose();
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message, "Kritischer Fehler");
            //        }
            //    }

            #endregion
            //    options.hasToConvert = false;
            //}

            updateList();

            openFileDialog1.Filter = "Rezeptdateien|*.grec";
            saveFileDialog1.Filter = "Rezeptdateien|*.grec";

            comboBox1.SelectedIndex = 0;

             suche = new SuchForm(this);

             fontToDruck = new Font(FontFamily.GenericSansSerif, 13F, FontStyle.Regular);
        }

        //Speise ausgewählt
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (listBox1.SelectedItem != null)
            {
                if (!internetRecipes)
                {
                    Speise speise = grecDateiAuslesen(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + (string)listBox1.SelectedItem + ".grec");
                    ////Speise speise = speisen[listBox1.SelectedIndex];

                    textBox1.Text = "";

                    label2.Text = speise.Name;

                    textBox1.Text += "EIGENSCHAFTEN: " + speise.Attribute + "\r\n\r\n";
                    textBox1.Text += "BESCHREIBUNG: \r\n" + speise.Beschreibung + "\r\n";
                    textBox1.Text += "\r\nDAUER: " + speise.Dauer + " min\r\n";

                    if (speise.Schwierigkeit == "1")
                        textBox1.Text += "SCHWIERIGKEIT: " + "leicht" + "\r\n";

                    if (speise.Schwierigkeit == "2")
                        textBox1.Text += "SCHWIERIGKEIT: " + "mittel" + "\r\n";

                    if (speise.Schwierigkeit == "3")
                        textBox1.Text += "SCHWIERIGKEIT: " + "schwer" + "\r\n";

                    textBox1.Text += "\r\nZUTATEN (für " + speise.Personen + " Person(-en)): \r\n" + speise.Zutaten + "\r\n";
                    textBox1.Text += "ZUBEREITUNG: \r\n" + speise.Zubereitung;
                    textBox1.Text += "\r\n\r\nBEILAGEN: \r\n" + speise.Beilagen;
                }
                else
                {
                    Speise speise = grecDateiAuslesenString(FTPClass.DateiAuslesen(@"ftp://ftp.lima-city.de/gourmetxxl/" + listBox1.SelectedItem + ".grec", "Hausseite", "tingle25"), (string)listBox1.SelectedItem);

                    textBox1.Text = "";

                    label2.Text = speise.Name;

                    textBox1.Text += "EIGENSCHAFTEN: " + speise.Attribute + "\r\n\r\n";
                    textBox1.Text += "BESCHREIBUNG: \r\n" + speise.Beschreibung + "\r\n";
                    textBox1.Text += "\r\nDAUER: " + speise.Dauer + " min\r\n";

                    if (speise.Schwierigkeit == "1")
                        textBox1.Text += "SCHWIERIGKEIT: " + "leicht" + "\r\n";

                    if (speise.Schwierigkeit == "2")
                        textBox1.Text += "SCHWIERIGKEIT: " + "mittel" + "\r\n";

                    if (speise.Schwierigkeit == "3")
                        textBox1.Text += "SCHWIERIGKEIT: " + "schwer" + "\r\n";

                    textBox1.Text += "\r\nZUTATEN (für " + speise.Personen + " Person(-en)): \r\n" + speise.Zutaten + "\r\n";
                    textBox1.Text += "ZUBEREITUNG: \r\n" + speise.Zubereitung;
                    textBox1.Text += "\r\n\r\nBEILAGEN: \r\n" + speise.Beilagen;
                }
            }
            else
                label2.Text = "Kein Gericht ausgewählt";
        }

        //Wie der Name schon sagt...
        public static Speise grecDateiAuslesen(string path)
        {
            try
            {

                string line;
                int wo = 0;

                Speise speiseToReturn = new Speise();

                // Read the file and display it line by line.
                System.IO.StreamReader file = new System.IO.StreamReader(path);

                speiseToReturn.Name = Path.GetFileNameWithoutExtension(path);

                speiseToReturn.Attribute = "";
                speiseToReturn.Beschreibung = "";
                speiseToReturn.Zutaten = "";
                speiseToReturn.Zubereitung = "";

                while ((line = file.ReadLine()) != null)
                {
                    if (line == ";")
                    {
                        wo++;
                        continue;
                    }

                    if (wo == 0)
                    {
                        speiseToReturn.Attribute += line + "; ";
                    }

                    if (wo == 1)
                    {
                        speiseToReturn.Beschreibung += line + "\r\n";
                    }

                    if (wo == 2)
                    {
                        speiseToReturn.Dauer = line;
                    }

                    if (wo == 3)
                    {
                        speiseToReturn.Schwierigkeit = line;
                    }

                    if (wo == 4)
                    {
                        speiseToReturn.Zutaten += line + "\r\n";
                    }

                    if (wo == 5)
                    {
                        speiseToReturn.Zubereitung += line + "\r\n";
                    }
                    if (wo == 6)
                    {
                        speiseToReturn.Personen += line;
                    }
                    if (wo == 7)
                    {
                        speiseToReturn.Beilagen += line + "\r\n";
                    }
                }

                file.Close();

                return speiseToReturn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kritischer Fehler");
                return null;
            }
        }
        //Mit SCHTRINGILEIN
        public static Speise grecDateiAuslesenString(string text, string name)
        {
            try
            {
                int wo = 0;

                Speise speiseToReturn = new Speise();

                speiseToReturn.Name = name;
                speiseToReturn.Attribute = "";
                speiseToReturn.Beschreibung = "";
                speiseToReturn.Zutaten = "";
                speiseToReturn.Zubereitung = "";

                string[] tempS = text.Split(new string[] {"\r\n"}, StringSplitOptions.None);

                for (int i = 0; i < tempS.Length; i++)
                {
                    if (tempS[i] == ";")
                    {
                        wo++;
                        continue;
                    }

                    if (wo == 0)
                    {
                        speiseToReturn.Attribute += tempS[i] + "; ";
                    }

                    if (wo == 1)
                    {
                        speiseToReturn.Beschreibung += tempS[i] + "\r\n";
                    }

                    if (wo == 2)
                    {
                        speiseToReturn.Dauer = tempS[i];
                    }

                    if (wo == 3)
                    {
                        speiseToReturn.Schwierigkeit = tempS[i];
                    }

                    if (wo == 4)
                    {
                        speiseToReturn.Zutaten += tempS[i] + "\r\n";
                    }

                    if (wo == 5)
                    {
                        speiseToReturn.Zubereitung += tempS[i] + "\r\n";
                    }
                    if (wo == 6)
                    {
                        speiseToReturn.Personen += tempS[i];
                    }
                    if (wo == 7)
                    {
                        speiseToReturn.Beilagen += tempS[i] + "\r\n";
                    }
                }

                return speiseToReturn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kritischer Fehler");
                return null;
            }
        }
        
        //Speise löschen-Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (MessageBox.Show("Sind sie sicher, dass sie " + listBox1.SelectedItem + " löschen wollen?", "Frage", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string fileName = (string)listBox1.SelectedItem;

                    if (!internetRecipes)
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + fileName + ".grec");
                    }
                    else
                    {
                        FTPClass.DeleteFtpFileOrFolder(@"ftp://ftp.lima-city.de/gourmetxxl/" + fileName + ".grec", true, "Hausseite", "tingle25");
                    }

                    updateList();
                }
            }

            else
                MessageBox.Show("Nichts ausgewählt!", "Fehler");
        }

        //PicBox markierer
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = Color.Gray;
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = Color.DimGray;
        }

        //update-List Reload Button!
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            updateList();
        }

        //Speise bearbeiten
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Form2 f2 = new Form2(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + (string)listBox1.SelectedItem + ".grec");
                f2.ShowDialog();
                updateList();
            }
            else
                MessageBox.Show("Bitte eine Speise auwählen!", "Fehler");
        }

        //Zufallsgerichtsknopfundsoweiteryeahoh!!!elf!1!
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                Random r = new Random();
                Random rand = new Random(r.Next());
                int toSelect = rand.Next(0, listBox1.Items.Count);
                listBox1.SelectedIndex = toSelect;
            }
        }

        //Import
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String[] fileNames = openFileDialog1.FileNames;
                foreach (string s in fileNames)
                {
                    Form2 f2 = new Form2(s);
                    f2.ShowDialog();
                }
            }

            updateList();
        }
        //Export
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + listBox1.SelectedItem + ".grec", saveFileDialog1.FileName, true);
                }
            }
            else
                MessageBox.Show("Nichts ausgewählt!", "Fehler");
        }

        //Label markierer
        private void label4_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Gray;
        }
        private void label4_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.DimGray;
        }

        //Über-Fenster
        private void label4_Click(object sender, EventArgs e)
        {
            INfo infoFenster = new INfo();
            infoFenster.ShowInTaskbar = false;
            infoFenster.TopMost = true;
            infoFenster.ShowDialog();
        }

        //Internet comboBox item changed
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 1)
            {
                internetRecipes = true;
                button1.Enabled = false;
                button3.Enabled = false;
                groupBox1.Visible = false;
                pictureBox6.Image = Properties.Resources.Upload_02;
            }
            else if (((ComboBox)sender).SelectedIndex == 0)
            {
                internetRecipes = false;
                button1.Enabled = true;
                button3.Enabled = true;
                groupBox1.Visible = true;
                pictureBox6.Image = Properties.Resources.Upload_01;
            }

            updateList();
        }

        //textBox1 Text geändert
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Hochladen-Button
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (!internetRecipes)
                    FTPClass.uploadFile(@"ftp://ftp.lima-city.de/gourmetxxl/" + ((string)listBox1.SelectedItem).Replace("ü", "ue").Replace("Ü", "UE").Replace("ä", "ae").Replace("Ä", "AE").Replace("Ö", "OE").Replace("ö", "oe") + ".grec", "Hausseite", "tingle25", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + listBox1.SelectedItem + ".grec");

                else
                {
                    StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + listBox1.SelectedItem + "5342556546346563563464356653645643" + ".grec");
                    sw.AutoFlush = true;
                    sw.WriteLine(FTPClass.DateiAuslesen(@"ftp://ftp.lima-city.de/gourmetxxl/" + listBox1.SelectedItem + ".grec", "Hausseite", "tingle25"));
                    sw.Dispose();

                    Form2 f2 = new Form2(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + listBox1.SelectedItem + "5342556546346563563464356653645643" + ".grec");
                    f2.Show();

                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + listBox1.SelectedItem + "5342556546346563563464356653645643" + ".grec");
                }
            }
            else
                MessageBox.Show("Nichts ausgewählt!", "Fehler");
        }

        //Suchen-Button
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (suche.Visible)
            {
                suche.Select();
            }
            else
                suche.Show();
        }

        //SUCHEN-STARTEN-METHODE(-n)
        public void searchList()
        {
            //Vorbereiten
            updateSpeisen();
            List<Speise> speisenTemp = new List<Speise>();
            speisenTemp.Clear();

            //Suchen
            if (true)
            {
                for (int i = 0; i < speisen.Count; i++)
                {
                    bool sindAttributeRight = true;
                    for (int j = 0; j < suche.attributeToSearch.Count; j++)
                    {
                        if (!speisen[i].Attribute.Contains(suche.attributeToSearch[j]))
                        {
                            sindAttributeRight = false;
                        }
                    }

                    int dauerRight = 0;
                    if(suche.dauerToSearch == 0)
                        dauerRight = 2000;
                    else
                    {
                        dauerRight = suche.dauerToSearch;
                    }

                    if (speisen[i].Name.ToUpper().Contains(suche.nameToSearch) && dauerRight >= Convert.ToInt32(speisen[i].Dauer) && sindAttributeRight)
                    {
                        speisenTemp.Add(speisen[i]);
                    }
                }
            }

            //Speise und Liste syncen
            speisen = speisenTemp;
            listBox1.Items.Clear();
            foreach (Speise sp in speisen)
            {
                listBox1.Items.Add(sp.Name.Substring(0, sp.Name.Length - 1));
            }

            //Überprüfen auf überhaupt suche
            if (suche.nameToSearch == "" && suche.dauerToSearch == 0 && suche.attributeToSearch.Count == 0)
            {
                updateList();
            }
        }

        #region Drucken
        ////Druck-Variablen
        // Startseite zum Drucken 
        private int startSeite;

        // Anzahl der Druckseiten 
        private int anzahlSeiten;

        // aktuelle Seitenzahl 
        private int seitenNummer;

        // zu druckender Text 
        private string strPrintText;

        // Font zum Drucken
        Font fontToDruck;

        //Drucken
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && textBox1.Text != "")
            {
                printDialog1.AllowSomePages = true;

                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.DocumentName = "Gourmet-Rezept: " + ((string)listBox1.SelectedItem).TrimEnd().TrimEnd();
                    // Startwerte abhängig vom zu druckenden 
                    // Text initialisieren 
                    switch (printDialog1.PrinterSettings.PrintRange)
                    {
                        case PrintRange.AllPages:
                            strPrintText = "\r\n\r\n\r\n\r\n" + textBox1.Text;
                            startSeite = 1;
                            anzahlSeiten = printDialog1.PrinterSettings.MaximumPage;
                            break;
                        case PrintRange.SomePages:
                            strPrintText = "\r\n\r\n\r\n\r\n" + textBox1.Text;
                            startSeite = printDialog1.PrinterSettings.FromPage;
                            anzahlSeiten = printDialog1.PrinterSettings.ToPage
                                                               - startSeite + 1;
                            break;
                    }

                    // Drucken starten 
                    seitenNummer = 1;
                    printDocument1.Print();
                } 
            }
            else
                MessageBox.Show("Nichts ausgewählt!", "Fehler");
        }
        //PrintPageEreignis
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            RectangleF rectFPapier, rectFText;
            int intChars, intLines;

            // Ermitteln des Rectangles, das den gesamten Druckbereich 
            // beschreibt (inklusive Kopf- und Fußzeile) 
            rectFPapier = e.MarginBounds;

            // Ermitteln des Rectangles, das den Bereich für den 
            // Text beschreibt (ausschließlich Kopf- und Fußzeile) 
            rectFText = RectangleF.Inflate(rectFPapier, 0,
                                -2 * fontToDruck.GetHeight(e.Graphics));

            // eine gerade Anzahl von Druckzeilen ermitteln 
            int anzahlZeilen = (int)Math.Floor(rectFText.Height /
                                     fontToDruck.GetHeight(e.Graphics));

            // die Höhe des Rechtecks festlegen, das den Text enthält, damit die 
            // letzte Druckzeile nicht abgeschnitten wird 
            rectFText.Height = anzahlZeilen *
                                      fontToDruck.GetHeight(e.Graphics);

            // das StringFormat-Objekt festlegen, um den Text in einem 
            // Rechteck anzuzeigen - Text bis zum nächstliegenden Wort 
            // verkürzen 
            stringFormat.Trimming = StringTrimming.Word;

            // legt die Druckstartseite fest, wenn es sich nicht um die 
            // erste Dokumentenseite handelt 
            while ((seitenNummer < startSeite) && (strPrintText.Length > 0))
            {
                e.Graphics.MeasureString(strPrintText, fontToDruck,
                                     rectFText.Size, stringFormat,
                                     out intChars, out intLines);
                strPrintText = strPrintText.Substring(intChars);
                seitenNummer++;
            }

            // Druckjob beenden, wenn es keinen Text zum Drucken mehr gibt 
            if (strPrintText.Length == 0)
            {
                e.Cancel = true;
                return;
            }

            // den Text an das Graphics-Objekt übergeben 
            e.Graphics.DrawString(strPrintText, fontToDruck, Brushes.Black, rectFText, stringFormat);

            // Text für die nächste Seite 
            // intChars - Anzahl der Zeichen in der Zeichenfolge 
            // intLines - Anzahl der Zeilen in der Zeichenfolge 
            e.Graphics.MeasureString(strPrintText, fontToDruck,
                                     rectFText.Size, stringFormat,
                                     out intChars, out intLines);
            strPrintText = strPrintText.Substring(intChars);

            // StringFormat restaurieren 
            stringFormat = new StringFormat();

            // Dateiname in der Kopfzeile anzeigen 
            stringFormat.Alignment = StringAlignment.Center;

            Font fontToPrintKopfzeile = new System.Drawing.Font(FontFamily.GenericSansSerif, 18F, FontStyle.Bold);
            e.Graphics.DrawString((string)listBox1.SelectedItem, fontToPrintKopfzeile, Brushes.Black, rectFPapier, stringFormat);

            // Seitennummer in der Fußzeile anzeigen 
            stringFormat.LineAlignment = StringAlignment.Far;
            stringFormat.Alignment = StringAlignment.Far;

            Font fontToDruckTemp1 = new System.Drawing.Font(FontFamily.GenericSansSerif, 10F, FontStyle.Regular);
            e.Graphics.DrawString("Seite " + seitenNummer, fontToDruckTemp1,
                                 Brushes.Black, rectFPapier, stringFormat);

            // Gourmet-Werbung (;-D) in der Fußzeile anzeigen 
            stringFormat.LineAlignment = StringAlignment.Far;
            stringFormat.Alignment = StringAlignment.Near;

            Font fontToDruckTemp2 = new System.Drawing.Font(FontFamily.GenericSansSerif, 10F, FontStyle.Italic);
            e.Graphics.DrawString("Dieses Rezept wurde mit Gourmet XXL von Stefan Programs Inc.™ erstellt und gedruckt!", fontToDruckTemp2,
                                 Brushes.Black, rectFPapier, stringFormat);

            // ermitteln, ob weitere Seiten zu drucken sind 
            seitenNummer++;
            e.HasMorePages = (strPrintText.Length > 0) &&
                             (seitenNummer < startSeite + anzahlSeiten);

            // Neuinitialisierung 
            if (!e.HasMorePages)
            {
                strPrintText = textBox1.Text;
                startSeite = 1;
                anzahlSeiten = printDialog1.PrinterSettings.MaximumPage;
                seitenNummer = 1;
            }
        }
        #endregion

        //Form wird geschlossen
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            options.Save();
        }
    }
}
