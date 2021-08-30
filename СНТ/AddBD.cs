using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace СНТ
{
    public partial class AddBD : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private SqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private Point point;
        Lazy<Schtorka> schtorka = new Lazy<Schtorka>();
        private string vvodNameTabel { get; set; }
        private bool newRowAdding = false;
        public AddBD()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.GorelektrosetNew;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void ReloadData()
        {
            dataSet.Tables[$"{vvodNameTabel}"].Clear();
            dataAdapter.Fill(dataSet, $"{vvodNameTabel}");
            dataGridView1.DataSource = dataSet.Tables[$"{vvodNameTabel}"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell gridCell = new DataGridViewLinkCell();
                dataGridView1[5, 1] = gridCell;
            }
        }
        private void СhallengeNameSNT()
        {
            dataAdapter = new SqlDataAdapter($"SELECT Name_SNT FROM [Name_SNT]", sqlConnection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, $"Name_SNT");
            dataGridView2.DataSource = dataSet.Tables[0];
        }
        private void AddBD_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SNT_BD"].ConnectionString);
            sqlConnection.Open();
            СhallengeNameSNT();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vvodNameTabel != null)
                {
                    SqlCommand command = new SqlCommand(
                       $"INSERT INTO [{vvodNameTabel}] (F_I_O, Region, KTP_TP, P) VALUES (@F_I_O, @Region, @KTP_TP, @P)"
                       , sqlConnection);

                    command.Parameters.AddWithValue("F_I_O", FIO.Text);
                    command.Parameters.AddWithValue("Region", REGIONS.Text);
                    command.Parameters.AddWithValue("KTP_TP", KTP_TP.Text);
                    command.Parameters.AddWithValue("P", P.Text);

                    command.ExecuteNonQuery().ToString();

                    FIO.Clear();
                    REGIONS.Clear();
                    KTP_TP.Clear();
                    P.Clear();

                    Сhallenge();
                }
                else MessageBox.Show("Выберите СНТ");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Сhallenge()
        {
            try
            {
                if (vvodNameTabel != null)
                {
                    dataAdapter = new SqlDataAdapter($"SELECT *, N'Удалить' AS [Delete] FROM {vvodNameTabel}", sqlConnection);

                    sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                    sqlCommandBuilder.GetInsertCommand();
                    sqlCommandBuilder.GetUpdateCommand();
                    sqlCommandBuilder.GetDeleteCommand();

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, $"{vvodNameTabel}");
                    dataGridView1.DataSource = dataSet.Tables[$"{vvodNameTabel}"];

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell dataGridViewLinkCell = new DataGridViewLinkCell();
                        dataGridView1[5, i] = dataGridViewLinkCell;
                    }
                }
                else MessageBox.Show("Выберите СНТ");
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
                if (e.ColumnIndex == 5)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (task == "Удалить")
                    {
                        if (MessageBox.Show("Удалить эту строку? ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowInd = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowInd);
                            dataSet.Tables[$"{vvodNameTabel}"].Rows[rowInd].Delete();
                            dataAdapter.Update(dataSet, $"{vvodNameTabel}");
                        }
                    }
                    else if (task == "Добавить")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables[$"{vvodNameTabel}"].NewRow();

                        row["F_I_O"] = dataGridView1.Rows[rowIndex].Cells["F_I_O"].Value;
                        row["Region"] = dataGridView1.Rows[rowIndex].Cells["Region"].Value;
                        row["KTP_TP"] = dataGridView1.Rows[rowIndex].Cells["KTP_TP"].Value;
                        row["P"] = dataGridView1.Rows[rowIndex].Cells["P"].Value;

                        dataSet.Tables[$"{vvodNameTabel}"].Rows.Add(row);
                        dataSet.Tables[$"{vvodNameTabel}"].Rows.RemoveAt(dataSet.Tables[$"{vvodNameTabel}"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Удалить";

                        dataAdapter.Update(dataSet, $"{vvodNameTabel}");
                        newRowAdding = false;
                    }
                    else if (task == "Изменить")
                    {
                        int r = e.RowIndex;

                         dataSet.Tables[$"{vvodNameTabel}"].Rows[r]["F_I_O"] = dataGridView1.Rows[r].Cells["F_I_O"].Value;
                         dataSet.Tables[$"{vvodNameTabel}"].Rows[r]["Region"] = dataGridView1.Rows[r].Cells["Region"].Value;
                         dataSet.Tables[$"{vvodNameTabel}"].Rows[r]["KTP_TP"] = dataGridView1.Rows[r].Cells["KTP_TP"].Value;
                         dataSet.Tables[$"{vvodNameTabel}"].Rows[r]["P"] = dataGridView1.Rows[r].Cells["P"].Value;

                         dataAdapter.Update(dataSet, $"{vvodNameTabel}");
                         dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Удалить";
                    }
                    ReloadData();
                }
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void button3_Click(object sender, EventArgs e)
        {
            sqlConnection.Close();
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
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
                    dataGridView1[5, lastRow] = linkCell;
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
                    dataGridView1[5, rowIndex] = linkCell;

                    dataGridViewRow.Cells["Delete"].Value = "Изменить";
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
            if (dataGridView1.CurrentCell.ColumnIndex==4)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox!=null)
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(Column_KeyPress);
            }
        }
        private void Column_KeyPress (object sender, KeyPressEventArgs e)
        {
            // запрет ввода симвалов кроме запятой 
            Regex pat = new Regex(@"[\b]|[0-9,]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
                e.Handled = true;
            // запрет ввод в ячейку с id
            else if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].IsInEditMode == true)
            {
                string vlCell = ((TextBox)sender).Text;
                bool temp = (vlCell.IndexOf(".") == -1);
                if (!Char.IsLetter(e.KeyChar) && (e.KeyChar != 8) && !Char.IsWhiteSpace(e.KeyChar))
                    e.Handled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (vvodNameTabel != null)
                {
                    dataAdapter = new SqlDataAdapter(SELECTbox.Text, sqlConnection);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                    MessageBox.Show("Выберите СНТ");
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
        private void P_KeyPress(object sender, KeyPressEventArgs e)
        {
            // запрет ввода симвалов кроме запятой 
            Regex pat = new Regex(@"[\b]|[0-9.]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
                e.Handled = true;
        }
        static async void OpenBuukAsync()
        {
            await Task.Run(() => OpenBuuk());                
        }
        private void label9_Click(object sender, EventArgs e)
        {
            OpenBuukAsync();
        }
        private static void OpenBuuk()
        {
            try
            {
                string myPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                Process proc = Process.Start("Основные_команды_SQL_и_БД_СНТ.txt", myPath);
                proc.WaitForExit();
                proc.Close();
            }
            catch (Exception isk)
            {
                MessageBox.Show(isk.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.ForeColor = Color.Black;
        }
        private void label9_MouseEnter(object sender, EventArgs e)
        {
            label9.ForeColor = Color.Blue;
        }
        private void NameSNT_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox control = (TextBox)sender;
            if (e.KeyChar == (int)Keys.Space)
                e.KeyChar = '\0';
            else if (e.KeyChar == '-')
                if (control.SelectionStart != 0)
                    e.Handled = true;
        }
        private void NameSNT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vvodNameTabel = NameSNT.Text;
                Сhallenge();
            }
        }
        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            label10.Text = " ";
            schtorka.Value.OpenSchtorkaLeva_Pravo(panel1, panel2);
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            schtorka.Value.CloseStorkaPravo_Levo(panel2, -327);
            schtorka.Value.CloseSchtorkaVerch_Nize(panel3, 746);
            label10.Text = "Н\nа\nи\nм\nе\nн\nо\nв\nа\nн\nи\nя\n \nС\nН\nТ";
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
               Color.FromName("GradientInactiveCaption"), ButtonBorderStyle.Solid);
        }
        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
            schtorka.Value.CloseStorkaPravo_Levo(panel2, -327);
            label10.Text = "Н\nа\nи\nм\nе\nн\nо\nв\nа\nн\nи\nя\n \nС\nН\nТ";
            schtorka.Value.CloseSchtorkaVerch_Nize(panel3, 746);
        }
        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            schtorka.Value.OpenSchtorkaNize_Verch(panel3, 299);
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel3.ClientRectangle,
               Color.FromName("GradientInactiveCaption"), ButtonBorderStyle.Solid);
        }
    }
}
