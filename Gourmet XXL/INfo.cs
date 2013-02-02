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
    public partial class INfo : Form
    {
        public INfo()
        {
            InitializeComponent();
        }

        private void INfo_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReleaseLogForm logform = new ReleaseLogForm();
            logform.ShowDialog();
            logform.Dispose();
        }
    }
}
