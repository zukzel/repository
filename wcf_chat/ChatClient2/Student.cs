using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient2.ServiceChat;
using System.Data.SqlClient;

namespace ChatClient2
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }

        public string strDBconnect(string dbserver, string dbname)
        {
            string Server = dbserver;

            string Database = dbname;

            string MyConString = "Data Source = " + Server + "; Initial Catalog = " + Database + "; Integrated Security = True";

            return MyConString;
        }

        private void Student_Load(object sender, EventArgs e)
        {
            textBoxPath.Enabled = false;

            string sql_connect = strDBconnect("USER3-ПК", "test002");
            SqlConnection connection = new SqlConnection(sql_connect);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            string qwery = "SELECT Фамилия, Имя, Отчество FROM Пользователи WHERE idРоли = 1";
            command.CommandText = qwery;

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
            }
            connection.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string SelectID(string query)
        {
            string rezult;
            string sql_connection = strDBconnect("USER3-ПК", "test002");
            using (SqlConnection connection = new SqlConnection(sql_connection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                rezult = Convert.ToString(command.ExecuteScalar());
            }
            return rezult;
        }

        private string SelectID2(string query)
        {
            string rezult;
            string sql_connection = strDBconnect("USER3-ПК", "test002");
            using (SqlConnection connection = new SqlConnection(sql_connection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                rezult = Convert.ToString(command.ExecuteNonQuery());
            }
            return rezult;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            panel4.BringToFront();
            button2.BackColor = Color.FromArgb(245, 32, 62);
            button9.BackColor = Color.FromArgb(45, 54, 76);

            dataGridView2.Rows.Clear();

            String[] fio = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            

            string sql_connect = strDBconnect("USER3-ПК", "test002");

            SqlConnection myConnection = new SqlConnection(sql_connect);

            myConnection.Open();

            string query = "SELECT Пользователи.Фамилия, Пользователи.Имя, Пользователи.Отчество, Кейсы.Название FROM Пользователи LEFT JOIN Кейсы ON Пользователи.idПользователя = Кейсы.idПользователя WHERE Кейсы.idГруппы = " + SelectID("SELECT idГруппы FROM Пользователи WHERE Фамилия = '" + fio[0] +"'");

            SqlCommand command = new SqlCommand(query, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
            }

            reader.Close();

            myConnection.Close();

            foreach (string[] s in data)
                dataGridView2.Rows.Add(s);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string sql_connect = strDBconnect("USER3-ПК", "test002");

            saveFileDialog1.Filter = "WordDocument(*.docx)|*.docx|Text files(*.txt)|*.txt|All files(*.*)|*.*";

            //string path = @"C:\Server\ФЕМИ\Прикладная информатика\НТ-404оПИэ\Информационная безопасность\Разработка частной модели угроз.docx";

            //SelectID("SELECT Сокращение FROM Факультеты WHERE idФакультета = '" + fio[0] + "'");

            string query = "SELECT idФакультета, idПредмета, idГруппы, idДисциплины, Название FROM Кейсы WHERE Название = " + znachenie;

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            System.IO.File.Copy(CreateCommand(sql_connect), filename, true);
            MessageBox.Show("Файл сохранен");

            //label3.Text = SelectID2("SELECT idФакультета, idПредмета, idГруппы, idДисциплины, Название FROM Кейсы WHERE Название = '" + znachenie + "'");

            //label3.Text = Convert.ToString(CreateCommand(sql_connect));
        }

        public string znachenie;

        private string CreateCommand(string connectionString)
        {
            string path = "";

            string[] data = new string[5];
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT idФакультета, idПредмета, idГруппы, idДисциплины, Название FROM Кейсы WHERE Название = '" + znachenie + "'", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data[0] = reader[0].ToString();
                    data[1] = reader[1].ToString();
                    data[2] = reader[2].ToString();
                    data[3] = reader[3].ToString();
                    data[4] = reader[4].ToString();
                }

                path = @"C:\Server\" + SelectID("SELECT Сокращение FROM Факультеты WHERE idФакультета = " + data[0]) + @"\" + SelectID("SELECT Название FROM Дисциплины WHERE idДисциплины = " + data[3]) + @"\" + SelectID("SELECT Номер FROM Группы WHERE idГруппы = " + data[2]) + @"\" + SelectID("SELECT Название FROM Предметы WHERE idПредмета = " + data[1]) + @"\" + znachenie + ".docx";
                reader.Close();

                connection.Close();
            }
            return path;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            znachenie = Convert.ToString(dataGridView2[3, e.RowIndex].Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
            button9.BackColor = Color.FromArgb(245, 32, 62);
            button2.BackColor = Color.FromArgb(45, 54, 76);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialog1.FileName;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || textBoxPath.Text == "")
            {
                MessageBox.Show("Введены не все данные!");
            }
            else
            {
                string curDate = DateTime.Now.ToShortDateString();

                string sql_connect = strDBconnect("USER3-ПК", "test002");

                String[] fileName = textBoxPath.Text.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                String[] fio = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                String[] combo = comboBox1.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string fileName2 = fileName.Last();

                int index = fileName2.IndexOf('.');

                string fileName3 = fileName2.Remove(index);

                string family = combo.First();

                string query = "INSERT INTO Работы (idПреподавателя, idПользователя, Дата, Название) VALUES ('" + SelectID("SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + family + "'") + "', '" + SelectID("SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + fio[0] + "'") + "', '" + curDate.ToString() + "', '" + fileName3.ToString() + "')";

                using (SqlConnection connection = new SqlConnection(sql_connect))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                System.IO.File.Copy(textBoxPath.Text, @"C:\Server\Работы\" + fileName.Last() + "", true);

                MessageBox.Show("Ваша работа успешно отправлена!");

                textBoxPath.Text = "";

                comboBox1.Text = "";
            }
        }
    }
}
