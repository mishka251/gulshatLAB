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
    public partial class SelectForm : Form
    {
        public SelectForm()
        {
            InitializeComponent();
        }

        private void SelectForm_Load(object sender, EventArgs e)
        {
           var table = myDB.SELECT_ALL("ZakazInfo");
            dataGridView1.DataSource = table;


            List<int> zakazIDs = new List<int>();

            for(int i =0; i<table.Rows.Count;i++)
            {
                zakazIDs.Add(int.Parse((string)table.Rows[i].ItemArray[0]));
            }

            foreach(int zakaz in zakazIDs.Distinct())
            {
                comboBox1.Items.Add(zakaz);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem==null)
            {
                MessageBox.Show("Выбери");

            }

            int id = (int)comboBox1.SelectedItem;

            WorkForm w = new WorkForm(id);
            w.Show();
        }
    }
}
