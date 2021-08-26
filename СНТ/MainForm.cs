using System;
using System.Drawing;
using System.Windows.Forms;

namespace СНТ
{
    
    public partial class MainForm : Form
    {
        private Point point2;
        public MainForm()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти? ", "Закрытия приложения СНТ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Blue;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - point2.X;
                this.Top += e.Y - point2.Y;
            }

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            point2 = new Point(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddBD mainForm = new AddBD();
            mainForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ReviewForm mainForm = new ReviewForm();
            mainForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddTabels mainForm = new AddTabels();
            mainForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.GorelektrosetNew;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddRemoveUser mainForm = new AddRemoveUser();
            mainForm.Show();
        }
    }
}
