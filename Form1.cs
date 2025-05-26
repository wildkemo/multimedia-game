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
        Enemy1Data enemy1data = new Enemy1Data();
        Enemy1 e1;

        List<String> leftimages = new List<String>();
        List<String> rightimages = new List<String>();
        List<Enemy1> enemies = new List<Enemy1>();




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



            animateEnemies();


            jumpRight();
            jumpLeft();


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
                animateEnemies();

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

                        if (hero.img != null)
                        {
                            hero.img.Dispose();
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
                        if (hero.img != null)
                        {
                            hero.img.Dispose();
                        }

                        hero.img = new Bitmap(rightimages[0]);
                    }
                }

            }
            else if (e.KeyCode == Keys.Left)
            {
                animateEnemies();

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

                        if (hero.img != null)
                        {
                            hero.img.Dispose();
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
                        if (hero.img != null)
                        {
                            hero.img.Dispose();
                        }

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





            ////////////////   LOADING ENEMY IDLE IMAGES /////////////////////
            
            enemy1data.idleRight.Add("enemy1right/frame_0.png");
            enemy1data.idleRight.Add("enemy1right/frame_1.png");
            enemy1data.idleRight.Add("enemy1right/frame_2.png");
            enemy1data.idleRight.Add("enemy1right/frame_3.png");
            enemy1data.idleRight.Add("enemy1right/frame_4.png");
            enemy1data.idleRight.Add("enemy1right/frame_5.png");
            enemy1data.idleRight.Add("enemy1right/frame_6.png");
            enemy1data.idleRight.Add("enemy1right/frame_7.png");
            enemy1data.idleRight.Add("enemy1right/frame_8.png");

            enemy1data.idleLeft.Add("enemy1left/frame_0.png");
            enemy1data.idleLeft.Add("enemy1left/frame_1.png");
            enemy1data.idleLeft.Add("enemy1left/frame_2.png");
            enemy1data.idleLeft.Add("enemy1left/frame_3.png");
            enemy1data.idleLeft.Add("enemy1left/frame_4.png");
            enemy1data.idleLeft.Add("enemy1left/frame_5.png");
            enemy1data.idleLeft.Add("enemy1left/frame_6.png");
            enemy1data.idleLeft.Add("enemy1left/frame_7.png");
            enemy1data.idleLeft.Add("enemy1left/frame_8.png");



            ////////////////   LOADING ENEMY RUN IMAGES /////////////////////

            enemy1data.runRight.Add("enemy1right/frame_23.png");
            enemy1data.runRight.Add("enemy1right/frame_24.png");
            enemy1data.runRight.Add("enemy1right/frame_25.png");
            enemy1data.runRight.Add("enemy1right/frame_26.png");
            enemy1data.runRight.Add("enemy1right/frame_27.png");
            enemy1data.runRight.Add("enemy1right/frame_28.png");

            enemy1data.runLeft.Add("enemy1left/frame_23.png");
            enemy1data.runLeft.Add("enemy1left/frame_24.png");
            enemy1data.runLeft.Add("enemy1left/frame_25.png");
            enemy1data.runLeft.Add("enemy1left/frame_26.png");
            enemy1data.runLeft.Add("enemy1left/frame_27.png");
            enemy1data.runLeft.Add("enemy1left/frame_28.png");


            ////////////////   LOADING ENEMY ATTACK IMAGES /////////////////////

            enemy1data.attackRight.Add("enemy1right/frame_46.png");
            enemy1data.attackRight.Add("enemy1right/frame_47.png");
            enemy1data.attackRight.Add("enemy1right/frame_48.png");
            enemy1data.attackRight.Add("enemy1right/frame_49.png");
            enemy1data.attackRight.Add("enemy1right/frame_50.png");
            enemy1data.attackRight.Add("enemy1right/frame_51.png");
            enemy1data.attackRight.Add("enemy1right/frame_52.png");
            enemy1data.attackRight.Add("enemy1right/frame_53.png");
            enemy1data.attackRight.Add("enemy1right/frame_54.png");
            enemy1data.attackRight.Add("enemy1right/frame_55.png");
            enemy1data.attackRight.Add("enemy1right/frame_56.png");
            enemy1data.attackRight.Add("enemy1right/frame_57.png");

            enemy1data.attackLeft.Add("enemy1left/frame_46.png");
            enemy1data.attackLeft.Add("enemy1left/frame_47.png");
            enemy1data.attackLeft.Add("enemy1left/frame_48.png");
            enemy1data.attackLeft.Add("enemy1left/frame_49.png");
            enemy1data.attackLeft.Add("enemy1left/frame_50.png");
            enemy1data.attackLeft.Add("enemy1left/frame_51.png");
            enemy1data.attackLeft.Add("enemy1left/frame_52.png");
            enemy1data.attackLeft.Add("enemy1left/frame_53.png");
            enemy1data.attackLeft.Add("enemy1left/frame_54.png");
            enemy1data.attackLeft.Add("enemy1left/frame_55.png");
            enemy1data.attackLeft.Add("enemy1left/frame_56.png");
            enemy1data.attackLeft.Add("enemy1left/frame_57.png");




            ////////////////   LOADING ENEMY HURT IMAGES /////////////////////

            enemy1data.hurtRight.Add("enemy1right/frame_69.png");
            enemy1data.hurtRight.Add("enemy1right/frame_70.png");
            enemy1data.hurtRight.Add("enemy1right/frame_71.png");
            enemy1data.hurtRight.Add("enemy1right/frame_72.png");
            enemy1data.hurtRight.Add("enemy1right/frame_73.png");

            enemy1data.hurtLeft.Add("enemy1left/frame_69.png");
            enemy1data.hurtLeft.Add("enemy1left/frame_70.png");
            enemy1data.hurtLeft.Add("enemy1left/frame_71.png");
            enemy1data.hurtLeft.Add("enemy1left/frame_72.png");
            enemy1data.hurtLeft.Add("enemy1left/frame_73.png");



            ////////////////   LOADING ENEMY DEATH IMAGES /////////////////////

            enemy1data.deathRight.Add("enemy1right/frame_92.png");
            enemy1data.deathRight.Add("enemy1right/frame_93.png");
            enemy1data.deathRight.Add("enemy1right/frame_94.png");
            enemy1data.deathRight.Add("enemy1right/frame_95.png");
            enemy1data.deathRight.Add("enemy1right/frame_96.png");
            enemy1data.deathRight.Add("enemy1right/frame_97.png");
            enemy1data.deathRight.Add("enemy1right/frame_98.png");
            enemy1data.deathRight.Add("enemy1right/frame_99.png");
            enemy1data.deathRight.Add("enemy1right/frame_100.png");
            enemy1data.deathRight.Add("enemy1right/frame_101.png");
            enemy1data.deathRight.Add("enemy1right/frame_102.png");
            enemy1data.deathRight.Add("enemy1right/frame_103.png");
            enemy1data.deathRight.Add("enemy1right/frame_104.png");
            enemy1data.deathRight.Add("enemy1right/frame_105.png");
            enemy1data.deathRight.Add("enemy1right/frame_106.png");
            enemy1data.deathRight.Add("enemy1right/frame_107.png");
            enemy1data.deathRight.Add("enemy1right/frame_108.png");
            enemy1data.deathRight.Add("enemy1right/frame_109.png");
            enemy1data.deathRight.Add("enemy1right/frame_110.png");
            enemy1data.deathRight.Add("enemy1right/frame_111.png");
            enemy1data.deathRight.Add("enemy1right/frame_112.png");
            enemy1data.deathRight.Add("enemy1right/frame_113.png");
            enemy1data.deathRight.Add("enemy1right/frame_114.png");

            enemy1data.deathLeft.Add("enemy1left/frame_92.png");
            enemy1data.deathLeft.Add("enemy1left/frame_93.png");
            enemy1data.deathLeft.Add("enemy1left/frame_94.png");
            enemy1data.deathLeft.Add("enemy1left/frame_95.png");
            enemy1data.deathLeft.Add("enemy1left/frame_96.png");
            enemy1data.deathLeft.Add("enemy1left/frame_97.png");
            enemy1data.deathLeft.Add("enemy1left/frame_98.png");
            enemy1data.deathLeft.Add("enemy1left/frame_99.png");
            enemy1data.deathLeft.Add("enemy1left/frame_100.png");
            enemy1data.deathLeft.Add("enemy1left/frame_101.png");
            enemy1data.deathLeft.Add("enemy1left/frame_102.png");
            enemy1data.deathLeft.Add("enemy1left/frame_103.png");
            enemy1data.deathLeft.Add("enemy1left/frame_104.png");
            enemy1data.deathLeft.Add("enemy1left/frame_105.png");
            enemy1data.deathLeft.Add("enemy1left/frame_106.png");
            enemy1data.deathLeft.Add("enemy1left/frame_107.png");
            enemy1data.deathLeft.Add("enemy1left/frame_108.png");
            enemy1data.deathLeft.Add("enemy1left/frame_109.png");
            enemy1data.deathLeft.Add("enemy1left/frame_110.png");
            enemy1data.deathLeft.Add("enemy1left/frame_111.png");
            enemy1data.deathLeft.Add("enemy1left/frame_112.png");
            enemy1data.deathLeft.Add("enemy1left/frame_113.png");
            enemy1data.deathLeft.Add("enemy1left/frame_114.png");






            e1 = new Enemy1(enemy1data);
            e1.pos = new Point((w/2) - 256, h - 450);
            enemies.Add(e1);






        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void drawbuffer(Graphics g)
        {
            if (cropped != null)
            {
                cropped.Dispose();
            }
                
            if (g2 != null)
            {
                g2.Dispose();
            }
                

            cropped = new Bitmap(w, h);
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

            for(int i = 0; i < enemies.Count; i++)
            {
                g2.DrawImage(enemies[i].img, enemies[i].pos);
            }




        }



        private void jumpRight()
        {
            if (jumpright == true && hero.pos.X < w - 256)
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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
        }

        private void jumpLeft()
        {
            if (jumpleft == true && hero.pos.X > 0)
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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

                    if (hero.img != null)
                    {
                        hero.img.Dispose();
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
        }


        private void animateEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {

                if (hero.middle().X >= enemies[i].middle().X - 250 && hero.middle().X <= enemies[i].middle().X + 250)
                {
                    if (hero.middle().X - enemies[i].middle().X < 0)
                    {
                        enemies[i].direction = "left";
                    }
                    else
                    {
                        enemies[i].direction = "right";
                    }

                    enemies[i].status = "attack";

                }

                else
                {
                    enemies[i].status = "idle";
                    //enemies[i].t_idle = 0;
                    //enemies[i].t_attack = 0;
                }

            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].idle();
                enemies[i].attack();

            }
        }















    }

    public partial class Hero
    {
        public Point pos;
        public Bitmap img = new Bitmap("Rframe_0.png");

        public Point middle()
        {
            Point p = new Point((this.pos.X + (this.img.Width / 2)), (this.pos.Y + (this.img.Height / 2)));
            return p;
        }
    }

    public partial class Enemy1
    {
        public Point pos;
        public Bitmap img;
        public Enemy1Data data = new Enemy1Data();
        public string status = "idle";
        public string direction = "left";
        public int t_idle = 0;
        public int t_attack = 0;


        public Enemy1(Enemy1Data exdata)
        {
            this.data = exdata;
            img = new Bitmap(data.idleLeft[0]);
        }

        public Point middle()
        {
            Point p = new Point((this.pos.X + (this.img.Width / 2)), (this.pos.Y + (this.img.Height / 2)));
            return p;
        }

        public void idle()
        {
            if(this.status == "idle")
            {
                if(this.direction == "left")
                {
                    if (t_idle < data.idleLeft.Count - 1)
                    {
                        t_idle++;
                    }
                    else
                    {
                        t_idle = 0;
                    }

                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.idleLeft[t_idle]);
                }
                else if(this.direction == "right")
                {
                    if (t_idle < data.idleRight.Count - 1)
                    {
                        t_idle++;
                    }
                    else
                    {
                        t_idle = 0;
                    }

                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.idleRight[t_idle]);
                }
            }
            
        }


        public void attack()
        {
            if (this.status == "attack")
            {
                if (this.direction == "left")
                {
                    if (t_attack < data.attackLeft.Count - 1)
                    {
                        t_attack++;
                    }
                    else
                    {
                        t_attack = 0;
                    }

                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.attackLeft[t_attack]);
                }
                else if (this.direction == "right")
                {
                    if (t_attack < data.attackRight.Count - 1)
                    {
                        t_attack++;
                    }
                    else
                    {
                        t_attack = 0;
                    }

                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.attackRight[t_attack]);
                }
            }

        }


    }

    public partial class Enemy1Data
    {
        public List<string> idleRight = new List<string>();
        public List<string> runRight = new List<string>();
        public List<string> attackRight = new List<string>();
        public List<string> hurtRight = new List<string>();
        public List<string> deathRight = new List<string>();

        public List<string> idleLeft = new List<string>();
        public List<string> runLeft = new List<string>();
        public List<string> attackLeft = new List<string>();
        public List<string> hurtLeft = new List<string>();
        public List<string> deathLeft = new List<string>();

    }


}
