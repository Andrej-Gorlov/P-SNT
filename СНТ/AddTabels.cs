using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace СНТ
{
    public partial class AddTabels : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private SqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private Point point;
        private string vvodNewNameTabel { get; set; }
        private bool newRowAdding = false;

        private void Сhallenge()
        {
            try
            {
                    dataAdapter = new SqlDataAdapter($"SELECT *, N'Изменить' AS [Сhanges] FROM Name_SNT", sqlConnection);

                    sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                    sqlCommandBuilder.GetInsertCommand();
                    sqlCommandBuilder.GetUpdateCommand();
                    sqlCommandBuilder.GetDeleteCommand();

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "Name_SNT");
                    dataGridView1.DataSource = dataSet.Tables["Name_SNT"];

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell dataGridViewLinkCell = new DataGridViewLinkCell();
                        dataGridView1[7, i] = dataGridViewLinkCell;
                    }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public AddTabels()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.GorelektrosetNew;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void ReloadData()
        {
            dataSet.Tables["Name_SNT"].Clear();
            dataAdapter.Fill(dataSet, "Name_SNT");
            dataGridView1.DataSource = dataSet.Tables["Name_SNT"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell gridCell = new DataGridViewLinkCell();
                dataGridView1[7, 1] = gridCell;
            }
        }
        private void NewTable()
        {
            try
            {
                if (vvodNewNameTabel != null)
                {
                    dataAdapter = new SqlDataAdapter($"CREATE TABLE[dbo].[{vvodNewNameTabel}]" +
               $"([Id] INT IDENTITY(1, 1) NOT NULL," +
               $"[F_I_O]  NVARCHAR(50) NULL," +
               $"[Region] NVARCHAR(50) NULL," +
               $"[KTP_TP] NVARCHAR(50) NULL," +
               $"[P] FLOAT(53) NULL," +
               $"PRIMARY KEY CLUSTERED([Id] ASC));", sqlConnection);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                    vvodNewNameTabel = null;
                    sqlConnection.Open();
                }
                else
                    MessageBox.Show("введите названия СНТ в поле ввода");
            }
            catch (Exception isk) 
            { 
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void DeletTable()
        {
            try
            {
                dataAdapter = new SqlDataAdapter($"DROP TABLE {vvodNewNameTabel};", sqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                dataAdapter = new SqlDataAdapter($"DELETE [Name_SNT]  WHERE Name_SNT='{vvodNewNameTabel}'", sqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                sqlConnection.Close();
                vvodNewNameTabel = null;
                sqlConnection.Open();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddTabels_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SNT_BD"].ConnectionString);
            sqlConnection.Open();
            Сhallenge();
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
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                vvodNewNameTabel = Name_SNT.Text;

                SqlCommand command = new SqlCommand(
                       "INSERT INTO [Name_SNT] (Name_SNT, Address, P_max, KTP_TP, Predcedatel,Phone_number) VALUES " +
                       "(@Name_SNT, @Address, @P_max, @KTP_TP,@Predcedatel,@Phone_number)"
                       , sqlConnection);

                command.Parameters.AddWithValue("Name_SNT", Name_SNT.Text);
                command.Parameters.AddWithValue("Address", Address.Text);
                command.Parameters.AddWithValue("P_max", P_max.Text);
                command.Parameters.AddWithValue("KTP_TP", KTP_TP.Text);
                command.Parameters.AddWithValue("Predcedatel", Predcedatel.Text);
                command.Parameters.AddWithValue("Phone_number", Phone_number.Text);
                command.ExecuteNonQuery().ToString();

                Name_SNT.Clear();
                Address.Clear();
                P_max.Clear();
                KTP_TP.Clear();
                Predcedatel.Clear();
                Phone_number.Clear();

                NewTable();
                Сhallenge();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (task == "Изменить")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Name_SNT"].Rows[r]["Name_SNT"] = dataGridView1.Rows[r].Cells["Name_SNT"].Value;
                        dataSet.Tables["Name_SNT"].Rows[r]["Address"] = dataGridView1.Rows[r].Cells["Address"].Value;
                        dataSet.Tables["Name_SNT"].Rows[r]["P_max"] = dataGridView1.Rows[r].Cells["P_max"].Value;
                        dataSet.Tables["Name_SNT"].Rows[r]["KTP_TP"] = dataGridView1.Rows[r].Cells["KTP_TP"].Value;
                        dataSet.Tables["Name_SNT"].Rows[r]["Predcedatel"] = dataGridView1.Rows[r].Cells["Predcedatel"].Value;
                        dataSet.Tables["Name_SNT"].Rows[r]["Phone_number"] = dataGridView1.Rows[r].Cells["Phone_number"].Value;
                        dataAdapter.Update(dataSet, "Name_SNT");
                    }
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
                    dataGridView1[7, rowIndex] = linkCell;
                    //dataGridViewRow.Cells["Сhanges"].Value = "Изменить";

                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            //валидация
            if (dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
            }
            else if ((dataGridView1.CurrentCell.ColumnIndex == 0)||(dataGridView1.CurrentCell.ColumnIndex == 1))
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(Column_KeyPress);
            }
        }
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            // запрет ввода симвалов кроме запятой 
            Regex pat = new Regex(@"[\b]|[0-9,]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
                e.Handled = true;
            // запрет ввод в ячейку с id
            else if ((dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].IsInEditMode == true)||(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].IsInEditMode == true))
            {
                string vlCell = ((TextBox)sender).Text;
                bool temp = (vlCell.IndexOf(".") == -1);
                if (!Char.IsLetter(e.KeyChar) && (e.KeyChar != 8) && !Char.IsWhiteSpace(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void P_max_KeyPress(object sender, KeyPressEventArgs e)
        {
            // запрет ввода симвалов кроме запятой 
            Regex pat = new Regex(@"[\b]|[0-9,.]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
                e.Handled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdapter = new SqlDataAdapter($"SELECT * FROM [Name_SNT]", sqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, $"Name_SNT");
                dataGridView1.DataSource = dataSet.Tables[0];

                //канкретное названия таблицы пример
                // SELECT F_I_O FROM Druzhba_1so WHERE F_I_O = 'cvcv'

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
                    Сhallenge();
                }
                else MessageBox.Show("Выберите СНТ");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConnection.Close();
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
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

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxDeletTable.Text!=null)
                {
                    if (MessageBox.Show("Удалить СНТ? ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        vvodNewNameTabel = textBoxDeletTable.Text;
                        DeletTable();
                        Сhallenge();
                        textBoxDeletTable.Clear();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный ввод наименования СНТ", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Name_SNT_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex pat = new Regex(@"[\b]|[0-9A-z_]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
                e.Handled = true;
        }
        private void textBoxDeletTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox control = (TextBox)sender;
            if (e.KeyChar == (int)Keys.Space)
                e.KeyChar = '\0';
            else if (e.KeyChar == '-')
                if (control.SelectionStart != 0)
                    e.Handled = true;
        }
    }
}
