using ChatClient2.ServiceChat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace ChatClient2
{
    public partial class Teacher : Form
    {

        public List<string> data = new List<string>();
        public List<string>zadanie = new List<string>();

        private readonly string TemplateFileName = @"c:\case.docx";

        bool isConnected = false;

        ServiceChatClient client;

        int ID;

        public Teacher()
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

        public DataTable GetDB(string myConnectionString, string mySelectQuery, ComboBox combo)
        {
            DataTable dt = new DataTable(); ;

            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlDataAdapter myAdapter = new SqlDataAdapter(mySelectQuery, myConnection);

            myConnection.Open();

            try
            {
                combo.Text = null;
                dt.Clear();
                myAdapter.Fill(dt);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            myConnection.Close();

            return dt;
        }

        private void Teacher_Load(object sender, EventArgs e)
        {
            String[] fio3 = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            panel3.BringToFront();

            string sql_connect = strDBconnect("USER3-ПК", "test002");

            string sql_query = "SELECT idФакультета, Сокращение FROM Факультеты";

            comboFaculty.DisplayMember = "Сокращение";

            comboFaculty.ValueMember = "idФакультета";

            comboFaculty.DataSource = GetDB(sql_connect, sql_query, comboFaculty);

            SqlConnection myConnection = new SqlConnection(sql_connect);

            myConnection.Open();

            string query = "SELECT Пользователи.Фамилия, Пользователи.Имя, Работы.Дата, Работы.Название FROM Пользователи LEFT JOIN Работы ON Пользователи.idПользователя = Работы.idПользователя WHERE Работы.idПреподавателя = " + SelectID("SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + fio3[0] + "'");

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
                dataGridView1.Rows.Add(s);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
            button9.BackColor = Color.FromArgb(245, 32, 62);
            button2.BackColor = Color.FromArgb(45, 54, 76);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(245, 32, 62);
            button9.BackColor = Color.FromArgb(45, 54, 76);

            panel4.BringToFront();

            dataGridView3.Rows.Clear();

            String[] fio = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string sql_connect = strDBconnect("USER3-ПК", "test002");

            SqlConnection myConnection = new SqlConnection(sql_connect);

            myConnection.Open();

            string query = "SELECT idКейса, Название FROM Кейсы WHERE idПользователя = " + SelectID("SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + fio[0] + "'");

            SqlCommand command = new SqlCommand(query, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
            }

            reader.Close();

            myConnection.Close();

            foreach (string[] s in data)
                dataGridView3.Rows.Add(s);
        }

        private void comboFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sql_connect = strDBconnect("USER3-ПК", "test002");

            string sql_query = "SELECT Дисциплины.Название, Дисциплины.idДисциплины FROM Дисциплины LEFT JOIN Факультеты ON Факультеты.idФакультета = Дисциплины.idФакультета WHERE Дисциплины.idФакультета = " + comboFaculty.SelectedValue.ToString() + "";

            comboSubject.DisplayMember = "Название";

            comboSubject.ValueMember = "idДисциплины";

            comboSubject.DataSource = GetDB(sql_connect, sql_query, comboSubject);

        }

        private void comboSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql_connect = strDBconnect("USER3-ПК", "test002");

            string sql_query = "SELECT Группы.Номер, Группы.idГруппы FROM Группы LEFT JOIN Дисциплины ON Дисциплины.idДисциплины = Группы.idДисциплины WHERE Группы.idДисциплины = " + comboSubject.SelectedValue.ToString() + "";

            comboGroup.DisplayMember = "Номер";

            comboGroup.ValueMember = "idГруппы";

            comboGroup.DataSource = GetDB(sql_connect, sql_query, comboGroup);
        }

        private void comboGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql_connect = strDBconnect("USER3-ПК", "test002");

            string sql_query = "SELECT Предметы.Название, Предметы.idПредмета FROM Предметы LEFT JOIN Группы ON Группы.idГруппы = Предметы.idГруппы WHERE Предметы.idГруппы = " + comboGroup.SelectedValue.ToString() + "";

            comboLesson.DisplayMember = "Название";

            comboLesson.ValueMember = "idПредмета";

            comboLesson.DataSource = GetDB(sql_connect, sql_query, comboLesson);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            data.Add(comboFaculty.Text);
            data.Add(comboSubject.Text);
            data.Add(comboGroup.Text);
            data.Add(comboLesson.Text);
            data.Add(textBoxCount.Text);

            panel5.BringToFront();

            label5.Text = "Задание 1";

            counter = Convert.ToInt32(data[4]);
        }

        int clickCount = 0;
        int clickCount2 = 6;

        public int counter;
        public string list;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ++clickCount;
            ++clickCount2;
            int rowNumber = dataGridView2.Rows.Add();

            switch (clickCount)
            {
                case 1:
                    data.Add(textBoxHead.Text);
                    data.Add(textBoxDescription.Text);
                    
                    break;
            }
            
            if(clickCount != counter)
            {
                label5.Text = string.Format("Задание {0}" , clickCount + 1);
                data.Add(textBoxTask.Text);
                zadanie.Add(textBoxTask.Text);
                
                dataGridView2.Rows[rowNumber].Cells["Column5"].Value = clickCount;
                dataGridView2.Rows[rowNumber].Cells["Column6"].Value = data[clickCount2];
            }
            else
            {
                data.Add(textBoxTask.Text);
                zadanie.Add(textBoxTask.Text);
                textBoxHead.Text = "";
                textBoxDescription.Text = "";
                textBoxTask.Text = "";
                textBoxCount.Text = "";
                dataGridView2.Rows[rowNumber].Cells["Column5"].Value = clickCount;
                dataGridView2.Rows[rowNumber].Cells["Column6"].Value = data[clickCount2];
                panel5.BringToFront();

                var wordApp = new Word.Application();

                wordApp.Visible = false;

                try
                {
                    var wordDocument = wordApp.Documents.Open(TemplateFileName);
                    ReplaceWordStub("{NameCase}", data[5], wordDocument);
                    ReplaceWordStub("{NameDis}", data[1], wordDocument);
                    ReplaceWordStub("{NamePred}", data[3], wordDocument);
                    String[] fio2 = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    ReplaceWordStub("{fio}", fio2[0] + " " + fio2[1] + " " + fio2[2], wordDocument);
                    int count = 1;
                    for (int i = 0; i < zadanie.Count; i++)
                    {
                        list += "Задание №" + count + ": " + zadanie[i] + "\v";
                        count++;

                    }
                    
                    ReplaceWordStub("{zadanie}", list, wordDocument);


                    string path = @"c:\Server\" + data[0].ToString() + @"\" + data[1].ToString() + @"\" + data[2].ToString() + @"\" + data[3].ToString() + @"\" + data[5].ToString() + ".docx";
                    wordDocument.SaveAs(path);
                    wordApp.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка!");
                }

                String[] fio = labelName.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string query = "SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + fio[0] +"'";


                string sql_connection = strDBconnect("USER3-ПК", "test002");

                string sql_query = "INSERT INTO Кейсы (Название, idПользователя, idФакультета, idДисциплины, idПредмета, idГруппы) VALUES ('" + data[5].ToString() + "', '" + SelectID("SELECT idПользователя FROM Пользователи WHERE Фамилия = '" + fio[0] +"'") + "', '" + SelectID("SELECT idФакультета FROM Факультеты WHERE Сокращение = '" + data[0] + "'") + "', '" + SelectID("SELECT idДисциплины FROM Дисциплины WHERE Название = '" + data[1] + "'") + "', '" + SelectID("SELECT idПредмета FROM Предметы WHERE Название = '" + data[3] + "'") + "', '" + SelectID("SELECT idГруппы FROM Группы WHERE Номер = '" + data[2] + "'") + "')";

                using (SqlConnection connection = new SqlConnection(sql_connection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql_query, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string SelectID(string query)
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

        public void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;

            range.Find.ClearFormatting();

            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            Close();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            string sql_connect = strDBconnect("USER3-ПК", "test002");

            saveFileDialog1.Filter = "WordDocument(*.docx)|*.docx|Text files(*.txt)|*.txt|All files(*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            System.IO.File.Copy(@"C:\Server\Работы\" + znachenie.ToString() + ".docx", filename, true);
            MessageBox.Show("Файл сохранен");
        }
        public string znachenie;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            znachenie = Convert.ToString(dataGridView1[3, e.RowIndex].Value);
        }

        public string znachenie2;

        private string CreateCommand(string connectionString)
        {
            string path = "";

            string[] data = new string[5];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT idФакультета, idПредмета, idГруппы, idДисциплины, Название FROM Кейсы WHERE Название = '" + znachenie2 + "'", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data[0] = reader[0].ToString();
                    data[1] = reader[1].ToString();
                    data[2] = reader[2].ToString();
                    data[3] = reader[3].ToString();
                    data[4] = reader[4].ToString();
                }

                path = @"C:\Server\" + SelectID("SELECT Сокращение FROM Факультеты WHERE idФакультета = " + data[0]) + @"\" + SelectID("SELECT Название FROM Дисциплины WHERE idДисциплины = " + data[3]) + @"\" + SelectID("SELECT Номер FROM Группы WHERE idГруппы = " + data[2]) + @"\" + SelectID("SELECT Название FROM Предметы WHERE idПредмета = " + data[1]) + @"\" + znachenie2 + ".docx";
                reader.Close();

                connection.Close();
            }
            return path;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql_connect = strDBconnect("USER3-ПК", "test002");

            saveFileDialog2.Filter = "WordDocument(*.docx)|*.docx|Text files(*.txt)|*.txt|All files(*.*)|*.*";

            string query = "SELECT idФакультета, idПредмета, idГруппы, idДисциплины, Название FROM Кейсы WHERE Название = " + znachenie2;

            if (saveFileDialog2.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog2.FileName;

            System.IO.File.Copy(CreateCommand(sql_connect), filename, true);

            MessageBox.Show("Файл сохранен");

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            znachenie2 = Convert.ToString(dataGridView3[1, e.RowIndex].Value);
        }
    }
}
