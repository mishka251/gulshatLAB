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
    public partial class WorkForm : Form
    {
        int id;
        public WorkForm(int id = 0)
        {
            InitializeComponent();
            this.id = id;
        }
        Dictionary<string, string> ShopNameForId;
        Dictionary<string, int> ShopIDForName;
        Dictionary<int, List<string>> possibleShopsForTovID;

        void changeLastColumn()
        {
            var column = dataGridView1.Columns[dataGridView1.Columns.Count - 1];
            List<object> oldVals = new List<object>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                oldVals.Add(dataGridView1[dataGridView1.Columns.Count - 1, i].Value);
            }
            dataGridView1.Columns.Remove(column);
            var newCol = new DataGridViewComboBoxColumn();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int tovar = int.Parse((string)dataGridView1.Rows[i].Cells[7].Value);
                //newCol.
                foreach (string shopID in possibleShopsForTovID[tovar])
                    newCol.Items.Add(ShopNameForId[shopID]);
            }
            newCol.HeaderText = column.HeaderText;
            dataGridView1.Columns.Add(newCol);
            //DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
            // cell.
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                int tovar = int.Parse((string)dataGridView1.Rows[i].Cells[7].Value);
                //newCol.
                DataGridViewComboBoxCell cell = dataGridView1[9, i] as DataGridViewComboBoxCell;
                cell.Items.Clear();
                foreach (string shopID in possibleShopsForTovID[tovar])
                    cell.Items.Add(ShopNameForId[shopID]);
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(oldVals[i]!=null&&(string)oldVals[i]!="")
                dataGridView1[9, i].Value = ShopNameForId[ (string)oldVals[i]];
            }
            dataGridView1.DataError += DataGridView1_DataError;
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dataGridView1.Columns[e.ColumnIndex];
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView1[e.ColumnIndex, e.RowIndex];

            var colItems = col.Items;
            var cellItems = cell.Items;
            var obj = cell.Value;

            MessageBox.Show("Хуйня произошла");
        }

        void LoadShops()
        {

            DataTable shops = myDB.SELECT_ALL("Shop");

            ShopIDForName = new Dictionary<string, int>();
            ShopNameForId = new Dictionary<string, string>();
            for (int i = 0; i < shops.Rows.Count; i++)
            {
                var row = shops.Rows[i];
                ShopIDForName.Add((string)row.ItemArray[1], int.Parse((string)row.ItemArray[0]));
                ShopNameForId.Add((string)row.ItemArray[0], (string)row.ItemArray[1]);
            }
        }

        void LoadShopsForTovars()
        {
            possibleShopsForTovID = new Dictionary<int, List<string>>();

            List<int> tovars = new List<int>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                int tovarID = int.Parse((string)table.Rows[i].ItemArray[7]);
                if (!tovars.Contains(tovarID))
                    tovars.Add(tovarID);
            }

            for (int i = 0; i < tovars.Count; i++)
            {
                DataTable table = myDB.callProc("ShopsForTovar", new List<myTuple> { new myTuple("@TovarID", tovars[i]) });
                possibleShopsForTovID.Add(tovars[i], new List<string>());
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    possibleShopsForTovID[tovars[i]].Add((string)table.Rows[j].ItemArray[0]);
                }



            }

        }

        DataTable table;

        void LoadTable()
        {
            dataGridView1.Columns.Clear();
            table = myDB.SELECT(
               new List<string>() { "*" },
                new List<string>() { "ZakazInfo" },
                new List<myTuple>()
                {
                    new myTuple("ZakazID", id)
                }

                );
            dataGridView1.DataSource = table;
            dataGridView1.AllowUserToAddRows = false;
            LoadShops();
            LoadShopsForTovars();
            changeLastColumn();
        }


        private void WorkForm_Load(object sender, EventArgs e)
        {

            LoadTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[9].Value != null)
                {
                    int shopID = ShopIDForName[(string)dataGridView1.Rows[i].Cells[9].Value];
                    int zakazID = int.Parse((string)dataGridView1.Rows[i].Cells[0].Value);
                    int tovarID = int.Parse((string)dataGridView1.Rows[i].Cells[7].Value);
                    myDB.Update(
                        "TovarInZakaz",
                        new List<myTuple> { new myTuple("ID_Shop", shopID) },
                        new List<myTuple> { new myTuple("ID_Tovar", tovarID), new myTuple("ID_Zakaz", zakazID) }

                       );
                }
            }

            LoadTable();
        }
    }
}
