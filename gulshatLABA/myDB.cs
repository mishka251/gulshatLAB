using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace gulshatLABA
{
    static class myDB
    {
        static SqlConnection sqlConnection;
        static myDB()
        {
            var curDir = Directory.GetCurrentDirectory();
            var projDir = Directory.GetParent(curDir).Parent.FullName;


            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                AttachDBFilename = projDir + @"\newDB.mdf",
                IntegratedSecurity = true
            };

            sqlConnection = new SqlConnection
            {
                ConnectionString = sb.ConnectionString
            };

            sqlConnection.Open();
            if (sqlConnection.State != ConnectionState.Open)
            {
                MessageBox.Show("Ошибка подключения к БД. Приложение будет закрыто");
                Application.Exit();
            }

        }

        public static void Init()
        {

        }

        public static DataTable SELECT(
            List<string> fields,
            List<string> tables,
            List<string> conditions = null
            )
        {

            string f = String.Join(", ", fields);
            string t = String.Join(", ", tables);
            string cond = conditions == null ? "" : "WHERE " + String.Join(" AND ", conditions);

            string SQL = $"SELECT {f} FROM {t} {cond}";

            DataTable res = new DataTable();


            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = SQL;
            var reader = command.ExecuteReader();
            int cols = reader.FieldCount;

            for (int i = 0; i < cols; i++)
                res.Columns.Add(new DataColumn(fields[i]));

            object[] row = new object[cols];
            while (reader.Read())
            {
                for (int i = 0; i < cols; i++)
                    row[i] = reader[i];

                res.Rows.Add(row);
            }
            reader.Close();
            return res;
        }


        public static DataTable SELECT_ALL(
   string table,
    List<string> conditions = null
    )
        {

            string cond = conditions == null ? "" : "WHERE " + String.Join(" AND ", conditions);

            string SQL = $"SELECT * FROM {table} {cond}";

            DataTable res = new DataTable();


            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = SQL;
            var reader = command.ExecuteReader();
            int cols = reader.FieldCount;

            for (int i = 0; i < cols; i++)
                res.Columns.Add(new DataColumn(reader.GetName(i)));

            object[] row = new object[cols];
            while (reader.Read())
            {
                for (int i = 0; i < cols; i++)
                    row[i] = reader[i];

                res.Rows.Add(row);
            }
            reader.Close();
            return res;
        }


        public static void INSERT(string table, DataTable values)
        {
            SqlCommand cmd = sqlConnection.CreateCommand();

            string[] lines = new string[values.Rows.Count];
            for (int i = 0; i < lines.Length; i++)
            {



                string[] param = new string[values.Columns.Count];
                for (int j = 0; j < values.Columns.Count; j++)
                {
                    string par_name = $"@param{i}{j}";
                    param[j] = par_name;
                    cmd.Parameters.Add(new SqlParameter(par_name, values.Rows[i][j]));
                }
                lines[i] = "(" + String.Join(",", param )+ ")";

            }

            string vals = String.Join(", ", lines);
            cmd.CommandText = $"INSERT INTO {table} VALUES {vals};";
            cmd.ExecuteNonQuery();
        }


        public static void DELETE(string table, string condition = "", object[] param=null)
        {
            SqlCommand command = sqlConnection.CreateCommand();

            
            string SQL = $"DELETE FROM {table}";
            if (condition != "")
            {
                SQL += $" WHERE {condition}";
                for (int i = 0; i < param.Length; i++)
                    command.Parameters.Add(new SqlParameter($"@param{i}", param[i]));
            }
            command.CommandText = SQL;
            command.ExecuteNonQuery();
        }



    }
}
