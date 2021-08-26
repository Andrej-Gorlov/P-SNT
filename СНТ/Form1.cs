using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace СНТ
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        Point point;
        public string nameUser5 { get; set; }
        private string ChechPassword(string login)
        {
            SqlCommand commandPMax = new SqlCommand($"SELECT Password FROM [LogPas] WHERE Login ='{login}'", sqlConnection);
            var pas = commandPMax.ExecuteScalar();

            if (pas != null)
            {
                return pas.ToString();
            }
            else
                return "";
        }
        private string ChechLogin(string login)
        {
            SqlCommand commandPMax = new SqlCommand($"SELECT Login FROM [LogPas] WHERE Login ='{login}'", sqlConnection);
            var log = commandPMax.ExecuteScalar();
            if (log != null)
            {
                return log.ToString();
            }
            else
                return "";
        }
        private void ChechUser(string login)
        {
            SqlCommand commandPMax = new SqlCommand($"SELECT Name FROM [LogPas] WHERE Login ='{login}'", sqlConnection);
            var name = commandPMax.ExecuteScalar();
            NameUser.textNameUser = name.ToString();
        }
        public Form1()
        {
            InitializeComponent();
            this.passe.AutoSize = false;
            this.passe.Size = new Size(this.passe.Size.Width, 70);

            this.login.AutoSize = false;
            this.login.Size = new Size(this.passe.Size.Width, 70);

        }
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void close_MouseEnter(object sender, EventArgs e)
        {
            close.ForeColor = Color.Blue;
        }
        private void close_MouseLeave(object sender, EventArgs e)
        {
            close.ForeColor = Color.Black;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                this.Left += e.X - point.X;
                this.Top += e.Y - point.Y;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            point = new Point(e.X, e.Y);
        }
        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - point.X;
                this.Top += e.Y - point.Y;
            }
        }
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            point = new Point(e.X, e.Y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (login.Text != null && passe.Text != null)
            {
                if (login.Text == "root" && passe.Text == "root")
                {
                    NameUser.textNameUser = "Admin";
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else if ((login.Text == ChechLogin(login.Text))
                    && (passe.Text == ChechPassword(login.Text)))
                {
                    ChechUser(login.Text);
                    ReviewForm mainForm = new ReviewForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Вы ввели неверный логин/пароль");
            }
            else MessageBox.Show("Введите логин и пароль");
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
        private void passe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SNT_BD"].ConnectionString);
            sqlConnection.Open();
        }
    }
}
