using System;
using System.Drawing;
using System.Windows.Forms;


namespace СНТ
{
    public partial class StartGreeting : Form
    {
        public StartGreeting()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(100, 0, 0);
            this.TransparencyKey = Color.FromArgb(100, 0, 0);

            pictureBox1.Image = Properties.Resources.newSNT;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void StartGreeting_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Opacity += 0.1d;
            }
        }
    }
}
