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
    public partial class CreateZakaz : Form
    {
        public CreateZakaz()
        {
            InitializeComponent();
        }

        private void CreateZakaz_Load(object sender, EventArgs e)
        {
            DataTable table = myDB.SELECT_ALL("Tovar");
            dataGridView1.DataSource = table;
        }

        int getEmpId()
        {
            DataTable emp = myDB.SELECT(
   new List<string>() { "id" },
    new List<string>() { "Employe" },
    new List<myTuple>() {
                    new myTuple("Name", tbEmpName.Text ),
                    new myTuple("Surname", tbEmpFam.Text ),
                    new myTuple("Patronymic", tbEmpOt.Text)
    });

            return int.Parse((string)emp.Rows[0].ItemArray[0]);

        }

        bool hasEmp()
        {
            DataTable emp = myDB.SELECT(
   new List<string>() { "id" },
    new List<string>() { "Employe" },
    new List<myTuple>() {
                    new myTuple("Name", tbEmpName.Text ),
                    new myTuple("Surname", tbEmpFam.Text ),
                    new myTuple("Patronymic", tbEmpOt.Text)
    });

            return emp.Rows.Count > 0;

        }


        int getClId()
        {
            DataTable cl = myDB.SELECT(
       new List<string>() { "id" },
        new List<string>() { "Client" },
        new List<myTuple>() {
                    new myTuple("Name", tbClName.Text ),
                    new myTuple("Surname", tbClFam.Text ),
                    new myTuple("Patronymic", tbClOt.Text)
        });

            return int.Parse((string)cl.Rows[0].ItemArray[0]);

        }

        bool hasCl()
        {

            DataTable cl = myDB.SELECT(
       new List<string>() { "id" },
        new List<string>() { "Client" },
        new List<myTuple>() {
                    new myTuple("Name", tbClName.Text ),
                    new myTuple("Surname", tbClFam.Text ),
                    new myTuple("Patronymic", tbClOt.Text)
        });

            return cl.Rows.Count > 0;

        }


        void insertNewEmp()
        {
            myDB.INSERT(
                "Employe",
                new List<myTuple>()
                {
                    new myTuple("Name", tbEmpName.Text),
                    new myTuple("Surname", tbEmpFam.Text),
                    new myTuple("Patronymic", tbEmpOt.Text)
                });
        }

        void insertNewClient()
        {
            myDB.INSERT(
                "Client",
                new List<myTuple>()
                {
                    new myTuple("Name", tbClName.Text),
                    new myTuple("Surname", tbClFam.Text),
                    new myTuple("Patronymic", tbClOt.Text)
                });
        }


        void addNewZakaz(int client, int employe)
        {
            myDB.INSERT(
                   "Zakaz",
                   new List<myTuple>
                   {
                   new myTuple("ID_Client", client),
                   new myTuple("ID_Employe", employe),
                   new myTuple("date", DateTime.Now.Date)
                   }
                    );

        }


        int getZakazId(int client, int employe)
        {

            DataTable cl = myDB.SELECT(
                new List<string>() { "Id" },
                new List<string>() { "Zakaz" },
                new List<myTuple>() {
                   new myTuple("ID_Client", client),
                   new myTuple("ID_Employe", employe),
                   new myTuple("date", DateTime.Now.Date)
            });

            return int.Parse((string)cl.Rows[0].ItemArray[0]);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (
                tbEmpFam.Text == ""
                || tbEmpName.Text == ""
                || tbClName.Text == ""
                || tbClFam.Text == ""
                )
            {
                MessageBox.Show("Заполни");
                return;
            }

            List<int> tovarIDs = new List<int>();
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                int id = int.Parse((string)dataGridView1.SelectedRows[i].Cells[0].Value);
                tovarIDs.Add(id);
            }


            if (!hasEmp())
                insertNewEmp();

            int empId = getEmpId();

            if (!hasCl())
                insertNewClient();

            int clId = getClId();

            addNewZakaz(clId, empId);
            int zakId = getZakazId(clId, empId);







            DataTable dt = new DataTable();
            dt.Columns.Add("ID_Zakaz");
            dt.Columns.Add("ID_Tovar");

            for(int i =0; i<tovarIDs.Count; i++)
            {
                dt.Rows.Add(zakId, tovarIDs[i]);
            }

            myDB.INSERT("TovarInZakaz", dt);
            MessageBox.Show("Ok");

        }
    }
}
