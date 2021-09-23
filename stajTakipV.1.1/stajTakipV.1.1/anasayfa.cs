using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stajTakipV._1._1
{
    public partial class anasayfa : Form
    {
        public anasayfa()
        {
            InitializeComponent();
        }

        private void StajerGorevleri_Click(object sender, EventArgs e)
        {
            StajyerGorevleri sayfa1 = new StajyerGorevleri();
            //  this.Hide();
            sayfa1.ShowDialog();
            this.Show();
        }

        private void BasvuruFormu_Click(object sender, EventArgs e)
        {
            BasvuruFormu sayfa2 = new BasvuruFormu();
            //  this.Hide();
            sayfa2.ShowDialog();
            this.Show();
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tbmyo.nku.edu.tr/");
        }

        private void gösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void gizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void çıkışToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void veriTabanınıAçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string yol2 = Environment.CurrentDirectory.ToString();
            System.Diagnostics.Process.Start(yol2 + "\\stajTakipData.accdb");
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
