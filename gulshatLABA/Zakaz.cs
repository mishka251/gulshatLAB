using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gulshatLABA
{
    public partial class Zakaz : Form
    {
        public Zakaz()
        {
            InitializeComponent();

        }

        private void Zakaz_Load(object sender, EventArgs e)
        {
            DataTable zakazy = myDB.SELECT_ALL("Zakaz");
            dataGridView1.DataSource = zakazy;
        }
    }
}
