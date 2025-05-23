using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multimedia_game
{
    public partial class Form1: Form
    {
        Bitmap original = new Bitmap("background.png");
        Graphics g2;
        Graphics g;
        Bitmap cropped;
        Rectangle croprect;


        int w;
        int h;

        int start = 2160;
        public Form1()
        {
            //InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load1;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                if(start > 50)
                {
                    start -= 50;
                }
            } 
            else if(e.KeyCode == Keys.Down)
            {
                if(start < 2160)
                {
                    start += 50;
                }
            }

            drawbuffer(g);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gpaint = e.Graphics;
            drawbuffer(gpaint);
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            w = this.Width;
            h = this.Height;
            g = this.CreateGraphics();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void drawbuffer(Graphics g)
        {
            cropped = null;
            cropped = new Bitmap(w, h);
            g2 = null;
            g2 = Graphics.FromImage(cropped);
            drawscene(g2);
            g.DrawImage(cropped, 0, 0);

        }

        private void drawscene(Graphics g2)
        {
            g2.Clear(Color.White);
            croprect = new Rectangle(0, start, 1920, 1080);

            g2.DrawImage(original,          // source
               new Rectangle(0, 0, w, h),  // where to draw in new bitmap
               croprect,                  // what part to copy from source
               GraphicsUnit.Pixel        // unit type (pixels)
             );   
            





        }
    }
}
