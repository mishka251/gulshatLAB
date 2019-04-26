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
    public partial class Shop : Form
    {
        public Shop()
        {
            InitializeComponent();
        }

        private void Shop_Load(object sender, EventArgs e)
        {
            reLoad();

        }
        void reLoad()
        {
            dataGridView1.Columns.Clear();
    DataTable table = myDB.SELECT_ALL("Shop");
            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;
           // MessageBox.Show("In");
            string table = "Shop";
            DataTable newTable = new DataTable();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                newTable.Columns.Add(new DataColumn(dataGridView1.Columns[i].Name));

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                object[] arr = new object[dataGridView1.ColumnCount];
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    arr[j] = dataGridView1.SelectedRows[i].Cells[j].Value;
                newTable.Rows.Add(arr);
            }
            myDB.INSERT(table, newTable);
           // MessageBox.Show("End");
            reLoad();
        }
    }
}
