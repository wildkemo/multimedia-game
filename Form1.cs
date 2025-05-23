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
        Timer timer = new Timer();
        Bitmap original = new Bitmap("background.png");
        Graphics g2;
        Graphics g;
        Bitmap cropped;
        Rectangle croprect;

        Hero hero = new Hero();

        List<String> leftimages = new List<String>();
        List<String> rightimages = new List<String>();



        int w;
        int h;

        int start = 2160;
        int r = 0;
        bool rightflag = true;
        bool jumpright = false;
        bool jumpleft = false;
        int ct = 0;


        public Form1()
        {
            //InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load1;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {


            if(jumpright == true && hero.pos.X < w - 256)
            {
                if (ct < 3)
                {
                    if (r < rightimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(rightimages[r]);
                    hero.pos.X += 25;
                    hero.pos.Y -= 15;
                    start -= 15;
                }
                else if (ct >= 3 && ct < 6)
                {
                    if (r < rightimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(rightimages[r]);
                    hero.pos.X += 25;
                    
                }
                else if (ct >= 6 && ct < 9)
                {
                    if (r < rightimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(rightimages[r]);
                    hero.pos.X += 25;
                    hero.pos.Y += 15;
                    start += 15;
                }


                if (ct != 9)
                {
                    ct++;
                }
                else
                {
                    ct = 0;
                    jumpright = false;
                }
            }
            else if (jumpright == true && hero.pos.X >= w - 256)
            {
                ct = 0;
                jumpright = false;
            }









            else if (jumpleft == true && hero.pos.X > 0)
            {
                if (ct < 3)
                {
                    if (r < leftimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(leftimages[r]);
                    hero.pos.X -= 25;
                    hero.pos.Y -= 15;
                    start -= 15;
                }
                else if (ct >= 3 && ct < 6)
                {
                    if (r < leftimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(leftimages[r]);
                    hero.pos.X -= 25;

                }
                else if (ct >= 6 && ct < 9)
                {
                    if (r < leftimages.Count - 1)
                    {
                        r++;
                    }
                    else
                    {
                        r = 0;
                    }

                    hero.img = new Bitmap(leftimages[r]);
                    hero.pos.X -= 25;
                    hero.pos.Y += 15;
                    start += 15;
                }


                if (ct != 9)
                {
                    ct++;
                }
                else
                {
                    ct = 0;
                    jumpleft = false;
                }
            }
            else if (jumpleft == true && hero.pos.X <= 0)
            {
                ct = 0;
                jumpleft = false;
            }


            drawbuffer(g);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                
            } 
            else if(e.KeyCode == Keys.Down)
            {
                
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (jumpright == false && jumpleft == false)
                {
                    rightflag = true;

                    if (hero.pos.X < w - 256)
                    {

                        if (r < rightimages.Count - 1)
                        {
                            r++;
                        }
                        else
                        {
                            r = 0;
                        }

                        hero.img = new Bitmap(rightimages[r]);
                        hero.pos.X += 25;

                        if(hero.pos.Y < h - 256)
                        {
                            hero.pos.Y += 15;
                            start += 15;
                        }

                    }
                    else
                    {
                        hero.img = new Bitmap(rightimages[0]);
                    }
                }

            }
            else if (e.KeyCode == Keys.Left)
            {
                if (jumpright == false && jumpleft == false)
                {
                    rightflag = false;

                    if (hero.pos.X > 0)
                    {
                        if (r < leftimages.Count - 1)
                        {
                            r++;
                        }
                        else
                        {
                            r = 0;
                        }

                        hero.img = new Bitmap(leftimages[r]);
                        hero.pos.X -= 25;

                        if (hero.pos.Y < h - 256)
                        {
                            hero.pos.Y += 15;
                            start += 15;
                        }

                    }
                    else
                    {
                        hero.img = new Bitmap(leftimages[0]);
                    }
                }

            }
            else if (e.KeyCode == Keys.Space)
            {
                if (rightflag == true)
                {
                    jumpright = true;

                }
                else
                {
                    jumpleft = true;
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
            timer.Start();

            w = this.Width;
            h = this.Height;
            g = this.CreateGraphics();

            hero.pos = new Point(100, h - 256);

            leftimages.Add("frame_0.png");
            leftimages.Add("frame_1.png");
            leftimages.Add("frame_2.png");
            leftimages.Add("frame_3.png");
            leftimages.Add("frame_4.png");
            leftimages.Add("frame_5.png");
            leftimages.Add("frame_6.png");
            leftimages.Add("frame_7.png");
            leftimages.Add("frame_8.png");
            leftimages.Add("frame_9.png");


            rightimages.Add("Rframe_0.png");
            rightimages.Add("Rframe_1.png");
            rightimages.Add("Rframe_2.png");
            rightimages.Add("Rframe_3.png");
            rightimages.Add("Rframe_4.png");
            rightimages.Add("Rframe_5.png");
            rightimages.Add("Rframe_6.png");
            rightimages.Add("Rframe_7.png");
            rightimages.Add("Rframe_8.png");
            rightimages.Add("Rframe_9.png");




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


            g2.DrawImage(hero.img, hero.pos);




        }
    }

    public partial class Hero
    {
        public Point pos;
        public Bitmap img = new Bitmap("Rframe_0.png");


    }


}
