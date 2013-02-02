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
    public partial class ReleaseLogForm : Form
    {
        public ReleaseLogForm()
        {
            InitializeComponent();
        }

        private void ReleaseLogForm_Load(object sender, EventArgs e)
        {
            textBox1.Select(textBox1.Text.Length, 0);
        }
    }
}
