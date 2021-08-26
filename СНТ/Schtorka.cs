using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace СНТ
{
    public partial class Schtorka
    {
        bool expectation;
        public void OpenSchtorkaLeva_Pravo(System.Windows.Forms.Panel panel1, 
            System.Windows.Forms.Panel panel2,int x=0)
        {
            while (!expectation && panel1.Location.X > panel2.Location.X)
            {
                expectation = true;
                panel2.Location = new Point(panel2.Location.X + 1, panel2.Location.Y);
                expectation = false;
            }
        }
        public void CloseStorkaPravo_Levo(System.Windows.Forms.Panel panel2, int x = 0)
        {
            while (!expectation && panel2.Location.X > x)
            {
                expectation = true;
                panel2.Location = new Point(panel2.Location.X - (x*(-1)), panel2.Location.Y);
                expectation = false;
            }
        }
        public void OpenSchtorkaPravo_Levo(System.Windows.Forms.Panel panel1,
            System.Windows.Forms.Panel panel2, int x = 0)
        {
            while (!expectation && panel1.Location.X + x < panel2.Location.X)
            {
                expectation = true;
                panel2.Location = new Point(panel2.Location.X - 1, panel2.Location.Y);
                expectation = false;
            }
        }
        public void CloseSchtorkaLevo_Pravo(System.Windows.Forms.Panel panel2, int x = 0)
        {
            while (!expectation && panel2.Location.X < x)
            {
                expectation = true;
                panel2.Location = new Point(panel2.Location.X + 1, panel2.Location.Y);
                expectation = false;
            }
        }
        public void OpenSchtorkaNize_Verch(System.Windows.Forms.Panel panel1, int y=0)
        {
            while (!expectation && y < panel1.Location.Y)
            {
                expectation = true;
                panel1.Location = new Point(panel1.Location.X, panel1.Location.Y - 1);
                expectation = false;
            }
        }
        public void CloseSchtorkaVerch_Nize(System.Windows.Forms.Panel panel1,int y)
        {
            while (!expectation && y > panel1.Location.Y)
            {
                expectation = true;
                panel1.Location = new Point(panel1.Location.X, panel1.Location.Y + 1);
                expectation = false;
            }
        }


    }
}
