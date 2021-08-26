using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;

namespace СНТ
{
    public partial class ReviewForm : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private Point point;
        Schtorka schtorka = new Schtorka();
        private string vvodNameTabel { get; set; }
        private void Сhallenge_Table_Abonent()
        {
            try
            {
                if (vvodNameTabel != null)
                {
                    dataAdapter = new SqlDataAdapter($"SELECT * FROM {vvodNameTabel}", sqlConnection);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, $"{vvodNameTabel}");
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else MessageBox.Show("Выберите СНТ");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Free_P()
        {
            try
            {
                SqlCommand commandPFree = new SqlCommand($"SELECT round(SUM(P),2) FROM  {vvodNameTabel}", sqlConnection);
                var p_free = commandPFree.ExecuteScalar();

                SqlCommand commandPMax = new SqlCommand($"SELECT P_max FROM [Name_SNT] WHERE Name_SNT ='{vvodNameTabel}'", sqlConnection);
                var p_max = commandPMax.ExecuteScalar();

               labelMax.Text = Convert.ToSingle(p_max).ToString();

               labelFreeP.Text = Math.Round(Convert.ToSingle(p_max) - Convert.ToSingle(p_free), 2).ToString();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Сhallenge()
        {
            Сhallenge_Table_Abonent();
            Free_P();
        }
        private void СhallengeNameSNT()
        {
            dataAdapter = new SqlDataAdapter($"SELECT Name_SNT FROM [Name_SNT]", sqlConnection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, $"Name_SNT");
            dataGridView2.DataSource = dataSet.Tables[0];
        }
        public ReviewForm()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.GorelektrosetNew;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            point = new Point(e.X, e.Y);
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - point.X;
                this.Top += e.Y - point.Y;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти? ", "Закрытия приложения СНТ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Blue;
        }
        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
        }
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            sqlConnection.Close();
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
        private void ReviewForm_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SNT_BD"].ConnectionString);
            sqlConnection.Open();
            СhallengeNameSNT();
            
            label6.Text = NameUser.textNameUser;

            if (label6.Text != "Admin")
            {
                button3.Visible = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (NameSNT.Text!=null)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        Excel.Application application = new Excel.Application();
                        application.Application.Workbooks.Add(Type.Missing);
                        for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                        {
                            application.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                        }
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                            }
                        }
                        application.Columns.AutoFit();
                        application.Visible = true;
                    }
                }
                else MessageBox.Show("Введите  СНТ");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dataAdapter = new SqlDataAdapter($"SELECT * FROM [Name_SNT]", sqlConnection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, $"Name_SNT");
            dataGridView2.DataSource = dataSet.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                Excel.Application application = new Excel.Application();
                application.Application.Workbooks.Add(Type.Missing);
                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                {
                    application.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value;
                    }
                }
                application.Columns.AutoFit();
                application.Visible = true;
                СhallengeNameSNT();
            }
        }
        private void NameSNT_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    vvodNameTabel = NameSNT.Text;
                    Сhallenge();
                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
                Color.FromName("GradientInactiveCaption"), ButtonBorderStyle.Solid);
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            label10.Text = null;
            schtorka.OpenSchtorkaLeva_Pravo(panel1, panel2);
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            schtorka.CloseStorkaPravo_Levo(panel2, -314);
            label10.Text = "Н\nа\nи\nм\nе\nн\nо\nв\nа\nн\nи\nя\n \nС\nН\nТ";
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
            schtorka.CloseStorkaPravo_Levo(panel2, -314);
            label10.Text = "Н\nа\nи\nм\nе\nн\nо\nв\nа\nн\nи\nя\n \nС\nН\nТ";
        }
    }
}
