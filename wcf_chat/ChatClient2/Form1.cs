using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient2.ServiceChat;

namespace ChatClient2
{
    public partial class Form1 : Form, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int ID;

        public Form1()
        {
            InitializeComponent();
        }

        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.Enabled = false;
                bConnDicon.Text = "Disconnect";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.Enabled = true;
                bConnDicon.Text = "Connect";
                isConnected = false;
            }
        }

        private void bConnDicon_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                }
            }
        }

        public string conpars = @"Data Source = USER3-ПК; Initial Catalog = test002; Integrated Security = True";

        public string fio;

        private void button1_Click(object sender, EventArgs e)
        {
            string loginCheck = tbLogin.Text;
            string passwordCheck = tbPassword.Text;
            string checkcmd = $"SELECT * FROM Аутентификация WHERE Логин = '" + loginCheck + "' AND Пароль = '" + passwordCheck + "'";
            SqlDataAdapter sda = new SqlDataAdapter(checkcmd, conpars);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                string s = dtbl.Rows[0][0].ToString();
                DataTable dt = new DataTable();
                string sql_query = "SELECT Пользователи.* FROM Пользователи LEFT JOIN Аутентификация ON Пользователи.idАутентификации = Аутентификация.idАутентификации WHERE Аутентификация.idАутентификации = " + s + "";
                SqlDataAdapter sda2 = new SqlDataAdapter(sql_query, conpars);
                sda2.Fill(dt);
                string s2 = dt.Rows[0][5].ToString();
                fio = dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString() + " " + dt.Rows[0][3].ToString();
                switch (s2)
                {
                    case "1":
                        Hide();
                        Teacher teacher = new Teacher();
                        teacher.labelName.Text = fio;
                        teacher.ShowDialog();
                        Close();
                        break;
                    case "2":
                        Hide();
                        Student student = new Student();
                        student.labelName.Text = fio;
                        student.ShowDialog();
                        Close();
                        break;
                    case "3":
                        Hide();
                        Manager manager = new Manager();
                        manager.labelName.Text = fio;
                        manager.ShowDialog();
                        Close();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tbLogin_Enter(object sender, EventArgs e)
        {
            if (tbLogin.Text == "Введите логин")
            {
                tbLogin.Text = "";
                tbLogin.ForeColor = Color.Black;
            }
        }

        private void tbLogin_Leave(object sender, EventArgs e)
        {
            if (tbLogin.Text == "")
            {
                tbLogin.Text = "Введите логин";
                tbLogin.ForeColor = Color.FromArgb(53, 63, 89);
            }
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            if (tbPassword.Text == "Введите пароль")
            {
                tbPassword.Text = "";
                tbPassword.ForeColor = Color.Black;
            }
        }

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            if (tbPassword.Text == "")
            {
                tbPassword.Text = "Введите пароль";
                tbPassword.ForeColor = Color.FromArgb(53, 63, 89);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
