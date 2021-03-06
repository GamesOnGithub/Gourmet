﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gourmet_XXL
{
    public partial class SuchForm : Form
    {
        public SuchForm(Form1 f1)
        {
            InitializeComponent();
            form1 = f1;
        }

        //Variablen
        public string nameToSearch = "";
        public List<string> attributeToSearch = new List<string>();
        public int dauerToSearch = 0;
        Form1 form1;

        //Closen verhindern
        private void SuchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            //Nullsetzen
            textBox1.Text = "";
            numericUpDown1.Value = 0;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            if (button1.Visible)
            {
                button1.PerformClick();
            }

            this.Hide();
            form1.Select();
        }

        //Zuweisen und so und co.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!button1.Visible)
            {
                nameToSearch = textBox1.Text.ToUpper();

                if (nameToSearch == "")
                    nameToSearch = " ";

                form1.searchList();
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!button1.Visible)
            {
                dauerToSearch = (int)numericUpDown1.Value;

                form1.searchList();
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!button1.Visible)
            {
                attributeToSearch.Clear();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                    {
                        attributeToSearch.Add((string)checkedListBox1.Items[i]);
                    }
                }

                form1.searchList();
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, MouseEventArgs e)
        {
            if (!button1.Visible)
            {
                attributeToSearch.Clear();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                    {
                        attributeToSearch.Add((string)checkedListBox1.Items[i]);
                    }
                }

                form1.searchList();
            }
        }

        //Suchen-Button hiden/showen
        public void setSearchButtonVisible(bool value)
        {
            button1.Visible = value;
        }

        //Suchen-Button klicken
        private void button1_Click(object sender, EventArgs e)
        {
            //Text
            nameToSearch = textBox1.Text.ToUpper();

            if (nameToSearch == "")
                nameToSearch = " ";

            //Dauer
            dauerToSearch = (int)numericUpDown1.Value;

            //Attribute
            attributeToSearch.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                {
                    attributeToSearch.Add((string)checkedListBox1.Items[i]);
                }
            }

            form1.searchList();
        }
    }
}
