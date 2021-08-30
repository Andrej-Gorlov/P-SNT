using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СНТ
{
    public partial class AddRemoveUser : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlDataAdapter dataAdapter = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private DataSet dataSet = null;
        Lazy<Schtorka> schtorka = new Lazy<Schtorka>();
        private Point point;
        private string vvodLogin { get; set; }
        private bool newRowAdding = false;
        public AddRemoveUser()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.GorelektrosetNew;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void СhallengeTableLoginPassword()
        {
            dataAdapter = new SqlDataAdapter($"SELECT *, N'Удалить' AS [Delete] FROM [LogPas]", sqlConnection);

            sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
            sqlCommandBuilder.GetInsertCommand();
            sqlCommandBuilder.GetUpdateCommand();
            sqlCommandBuilder.GetDeleteCommand();

            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "[LogPas]");
            dataGridView1.DataSource = dataSet.Tables["[LogPas]"];

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell dataGridViewLinkCell = new DataGridViewLinkCell();
                dataGridView1[4, i] = dataGridViewLinkCell;
            }
        }
        private void AddUser()
        {
            try
            {
                SqlCommand command = new SqlCommand(
                      "INSERT INTO [LogPas] (Name, Login, Password) VALUES (@Name, @Login, @Password)"
                      , sqlConnection);

                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Login", Login.Text);
                command.Parameters.AddWithValue("Password", Password.Text);


                command.ExecuteNonQuery().ToString();

                textBox1.Clear();
                Login.Clear();
                Password.Clear();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteUser()
        {
            try
            {
                dataAdapter = new SqlDataAdapter($"DELETE FROM [LogPas]  WHERE Name='{vvodLogin}'", sqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                sqlConnection.Close();
                vvodLogin = null;
                sqlConnection.Open();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            sqlConnection.Close();
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Login.Text != "root" && Password.Text != "root")
                {

                    if (Login.Text != null && Password.Text != null && textBox1.Text != null)
                    {
                        AddUser();
                        СhallengeTableLoginPassword();
                    }
                    else
                        MessageBox.Show("Для добавления нового пользователя\n " +
                            " укажите в поле \"Логин\" и \"Пароль\"\n" +
                            " логин и пароль нового пользователя");
                }
                else
                    MessageBox.Show("Пользователь root уже сущиствует");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Login.Text != "root" && Password.Text != "root")
                {
                    if (Delet.Text != null)
                    {
                        if (MessageBox.Show("Удалить СНТ? ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            vvodLogin = Delet.Text;
                            DeleteUser();
                            СhallengeTableLoginPassword();
                            Delet.Clear();
                        }
                    }
                    else
                        MessageBox.Show("Ввидите в поле \"Логин\"\n" +
                            " Логин пользователя которого нужно удалить");
                }
                else
                    MessageBox.Show("Пользователя root нельзя удалить");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddRemoveUser_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SNT_BD"].ConnectionString);
            sqlConnection.Open();
            СhallengeTableLoginPassword();
        }
        private void label6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти? ", "Закрытия приложения СНТ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
        }
        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Blue;
        }
        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
        }
        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Blue;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - point.X;
                this.Top += e.Y - point.Y;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            point = new Point(e.X, e.Y);
        }
        private void ReloadData()
        {
            dataSet.Tables["[LogPas]"].Clear();
            dataAdapter.Fill(dataSet, "[LogPas]");
            dataGridView1.DataSource = dataSet.Tables["[LogPas]"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell gridCell = new DataGridViewLinkCell();
                dataGridView1[4, 1] = gridCell;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    if (task == "Удалить")
                    {
                        if (MessageBox.Show("Удалить эту строку? ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowInd = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowInd);
                            dataSet.Tables["[LogPas]"].Rows[rowInd].Delete();
                            dataAdapter.Update(dataSet, "[LogPas]");
                        }
                    }
                    else if (task == "Добавить")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["[LogPas]"].NewRow();

                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["Login"] = dataGridView1.Rows[rowIndex].Cells["Login"].Value;
                        row["Password"] = dataGridView1.Rows[rowIndex].Cells["Password"].Value;

                        dataSet.Tables["[LogPas]"].Rows.Add(row);
                        dataSet.Tables["[LogPas]"].Rows.RemoveAt(dataSet.Tables["[LogPas]"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Удалить";

                        dataAdapter.Update(dataSet, "[LogPas]");
                        newRowAdding = false;
                    }
                    else if (task == "Изменить")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["[LogPas]"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["[LogPas]"].Rows[r]["Login"] = dataGridView1.Rows[r].Cells["Login"].Value;
                        dataSet.Tables["[LogPas]"].Rows[r]["Password"] = dataGridView1.Rows[r].Cells["Password"].Value;

                        dataAdapter.Update(dataSet, "[LogPas]");
                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Удалить";
                    }
                    ReloadData();
                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[4, lastRow] = linkCell;
                    row.Cells["Delete"].Value = "Добавить";
                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow dataGridViewRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[4, rowIndex] = linkCell;

                    dataGridViewRow.Cells["Delete"].Value = "Изменить";
                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            label2.Text = " ";
            schtorka.Value.OpenSchtorkaPravo_Levo(panel1, panel2, 720);
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            schtorka.Value.CloseSchtorkaLevo_Pravo(panel2, 1046);
            label2.Text = "У\nд\nа\nл\nи\nт\nь\n \nп\nо\nл\nь\nз\nо\nв\nа\nт\nе\nл\nя";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
                Color.FromName("GradientInactiveCaption"), ButtonBorderStyle.Solid);
        }
    }
}
