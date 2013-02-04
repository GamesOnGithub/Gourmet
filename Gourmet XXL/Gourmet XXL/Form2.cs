using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Gourmet_XXL
{
    public partial class Form2 : Form
    {
        public Form2(string pathToOpen = "")
        {
            InitializeComponent();

            //Pfad übergeben
            if (pathToOpen != "")
            {
                this.Text = "Rezept bearbeiten";

                Speise speiseToOpen = Form1.grecDateiAuslesen(pathToOpen);

                textBox1.Text = speiseToOpen.Name.Replace("5342556546346563563464356653645643", "");

                #region Checkboxen
                string[] temp1 = speiseToOpen.Attribute.Split(';');
                foreach (string s in temp1)
                {
                    string tempS = s.Trim();
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (((string)checkedListBox1.Items[i]) == tempS)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
                #endregion

                textBox2.Text = speiseToOpen.Beschreibung;
                numericUpDown1.Value = Convert.ToInt32(speiseToOpen.Dauer);

                switch (speiseToOpen.Schwierigkeit)
                {
                    case "1": radioButton1.Checked = true; break;
                    case "2": radioButton2.Checked = true; break;
                    case "3": radioButton3.Checked = true; break;
                }

                //Zutaten
                listBox1.Items.Clear();
                String[] tempAS = new string[1];
                tempAS[0] = "\r\n";
                String[] tempZ = speiseToOpen.Zutaten.Split(tempAS, StringSplitOptions.RemoveEmptyEntries);
                foreach (String s in tempZ)
                    listBox1.Items.Add(s);

                textBox4.Text = speiseToOpen.Zubereitung;
                textBox3.Text = speiseToOpen.Beilagen;
                numericUpDown2.Value = Convert.ToInt32(speiseToOpen.Personen);

                textBox2.Text = textBox2.Text.TrimEnd();
                textBox3.Text = textBox3.Text.TrimEnd();
                textBox4.Text = textBox4.Text.TrimEnd();
            }
        }

        StreamWriter sw;
        bool closenIstErlaubt = false;

        //Speichern
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Kein Name eingegeben!", "Fehler");
                return;
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + textBox1.Text + ".grec"))
            {
                if (MessageBox.Show("Datei existiert bereits. Soll \"" + 
                    Path.GetFileNameWithoutExtension(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + textBox1.Text + ".grec") + 
                    "\" überschrieben werden?", "Frage", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Gourmet XXL\\" + "\\recipes\\" + textBox1.Text + ".grec");

            sw.AutoFlush = true;

            foreach (string s in checkedListBox1.CheckedItems)
            {
                sw.WriteLine(s);
            }

            sw.WriteLine(";");

            sw.WriteLine(textBox2.Text);

            sw.WriteLine(";");

            sw.WriteLine(numericUpDown1.Value.ToString());

            sw.WriteLine(";");

            if (radioButton1.Checked)
                sw.WriteLine("1");
            if (radioButton2.Checked)
                sw.WriteLine("2");
            if (radioButton3.Checked)
                sw.WriteLine("3");

            sw.WriteLine(";");

            foreach (String s in listBox1.Items)
            {
                sw.WriteLine(s);
            }

            sw.WriteLine(";");
            sw.WriteLine(textBox4.Text);

            sw.WriteLine(";");
            sw.WriteLine(numericUpDown2.Value.ToString());

            sw.WriteLine(";");
            sw.WriteLine(textBox3.Text);

            sw.Close();
            sw.Dispose();

            closenIstErlaubt = true;
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closenIstErlaubt)
            {
                if (MessageBox.Show("Nicht gespeichert! Alle Änderungen gehen verloren!\r\nWollen sie trotzdem schließen?", "Frage", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    closenIstErlaubt = true;
            }

            e.Cancel = !closenIstErlaubt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            if (f3.name != "" && f3.menge != "")
                listBox1.Items.Add(f3.menge + "  " + f3.name);
            else if (f3.name != "" && f3.menge == "")
                listBox1.Items.Add(f3.name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
