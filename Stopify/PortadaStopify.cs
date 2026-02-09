using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stopify
{
    public partial class PortadaStopify : Form
    {
        public PortadaStopify()
        {
            InitializeComponent();
        }

        private void btnStopify_Click(object sender, EventArgs e)
        {
            Form1 formPrincipal = new Form1();
            formPrincipal.FormClosed += (s, args) => this.Close();
            formPrincipal.Show();
            this.Hide();
        }
    }
}
