using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gourmet_XXL
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string menge = "";
        public string name = "";

        private Boolean isAllowedToClose = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                menge = textBox1.Text;
                name = textBox2.Text;
                isAllowedToClose = true;
            }
            else
            {
                MessageBox.Show("Kein Name!", "Fehler");

                return;
            }

            this.Close();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isAllowedToClose)
                if (MessageBox.Show("Sind sie sicher, dass sie ohne Hinzufügen einer Zutat beenden wollen?", "Frage", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox2.Select(0, 0);
        }
    }
}
