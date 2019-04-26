using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace gulshatLABA
{
    public partial class Menu : Form
    {
        public Menu()
        {

            InitializeComponent();
            myDB.Init();
        }


       
        private void button5_Click(object sender, EventArgs e)
        {
            Zakaz z = new Zakaz();
            z.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tovar t = new Tovar();
            t.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Shop s = new Shop();
            s.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Employe e1 = new Employe();
            e1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client c = new Client();
            c.Show();
        }
    }
}
