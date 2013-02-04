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
    public partial class DruckModeForm : Form
    {
        public DruckModeForm()
        {
            InitializeComponent();
        }

        private int selectedIndex = -1;
        private bool cancelClose = true;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                button1.Enabled = true;
                selectedIndex = comboBox1.SelectedIndex;
            }
            else
            {
                button1.Enabled = false;
                selectedIndex = -1;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cancelClose = false;
            this.Close();
        }

        public int ShowDialogWithIndexReturn()
        {
            this.ShowDialog();
            return selectedIndex;
        }

        private void DruckModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancelClose)
                selectedIndex = -1;
        }
    }
}
