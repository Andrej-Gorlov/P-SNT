using System;
using System.Windows.Forms;

namespace СНТ
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            StartGreeting startGreeting = new StartGreeting();
            DateTime time = DateTime.Now + TimeSpan.FromSeconds(3.5);
            startGreeting.Show();
            while (time> DateTime.Now)
            {
                Application.DoEvents();
            }
            startGreeting.Close();
            startGreeting.Dispose();


            Application.Run(new Form1());
        }
    }
}
