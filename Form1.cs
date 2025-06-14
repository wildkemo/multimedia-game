using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multimedia_game
{

    public class Bullet
    {
        public Rectangle size;
        public Pen Pen = new Pen(Color.Yellow, 5);
        public int speed;
        public int face = 1;
        public int X, Y;
        public int dx, dy;
    }
    public partial class Form1: Form
    {
        Timer timer = new Timer();
        Bitmap original = new Bitmap("background.png");
        Graphics g2;
        Graphics g;
        Bitmap cropped;
        Rectangle croprect;
        Pen pen = new Pen(Color.Black, 10);

        int bullettimer = 0;
        int timebetbullet = 5;

        Hero hero = new Hero();
        Enemy1Data enemy1data = new Enemy1Data();
        Enemy1 e1;
        Ladder ladder1;
        WizardData wizarddata = new WizardData();
        Wizard wizard;
        Elevator elevator;

        List<String> leftimages = new List<String>();
        List<String> rightimages = new List<String>();
        List<Enemy1> enemies = new List<Enemy1>();
        List<Ladder> ladders = new List<Ladder>();
        List<ScrollObject> scrollObjects = new List<ScrollObject>();






        int w;
        int h;

        int start = 2160;
        int currentstart = 2160;
        int previousstart = 2160;
        int futurepreviousstart = 2160;
        int r = 0;
        bool rightflag = true;
        bool jumpright = false;
        bool jumpleft = false;
        int ct = 0;
        int ct2 = 0;
        int flag = 0;


        List<Bullet> bullets = new List<Bullet>();
        bool k = false;


        public Form1()
        {
            //InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load1;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                k = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (hero.middle().X < elevator.pos.X + elevator.img.Width && (hero.stage == 2 || hero.stage == 3))
            {
                hero.status = "climbing";
                hero.pos.X = elevator.pos.X -50;
                hero.pos.Y = elevator.pos.Y - 200;
                elevate();
                //ct2++;
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].UpdateAnimation();
            }

            animateEnemies();


            jumpRight();
            jumpLeft();
            bulletmove1();
            bulletmove2();


            animateWizard();

            if (k)
            {
                drawbullet();
            }

            bullettimer++;

            drawbuffer(g);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.D)
            {
                e1.status = "death";
                //e1.dead = true;
            }
            if (e.KeyCode == Keys.A)
            {
                wizard.pos.Y -= 100;
                wizard.status = "attack";
            }
            if (e.KeyCode == Keys.I)
            {
                wizard.pos.Y += 100;
                wizard.status = "idle";


            }

            if (e.KeyCode == Keys.Up)
            {
                animateWizard();

                for (int i = 0; i < ladders.Count; i++)
                {
                    if ((hero.middle().X >= ladders[i].pos.X && hero.middle().X <= ladders[i].pos.X + ladders[i].img.Width) && (hero.feet().Y >= ladders[i].pos.Y + 220))
                    {
                        hero.status = "climbing";

                        start -= 20;

                        for (int j = 0; j < scrollObjects.Count; j++)
                        {

                            scrollObjects[j].pos = new Point(scrollObjects[j].pos.X, scrollObjects[j].pos.Y + 15);
                        }


                    }
                    else
                    {
                        hero.status = "normal";
                        currentstart = start;
                        futurepreviousstart = currentstart;
                        hero.stage = 2;

                        //MessageBox.Show(currentstart.ToString());
                        //MessageBox.Show(hero.middle().ToString());


                    }
                }
                drawbuffer(g);
            } 
            else if(e.KeyCode == Keys.Down)
            {
                animateWizard();

                for (int i = 0; i < ladders.Count; i++)
                {
                    if(hero.stage == 1)
                    {
                        if ((hero.middle().X >= ladders[i].pos.X && hero.middle().X <= ladders[i].pos.X + ladders[i].img.Width) && (hero.feet().Y < ladders[i].pos.Y + ladders[i].img.Height - 380))
                        {
                            hero.status = "climbing";



                            start += 20;

                            for (int j = 0; j < scrollObjects.Count; j++)
                            {

                                scrollObjects[j].pos = new Point(scrollObjects[j].pos.X, scrollObjects[j].pos.Y - 15);
                            }


                        }
                        else
                        {
                            hero.status = "normal";
                            currentstart = start;

                        }
                    }
                }
                drawbuffer(g);
            }
            else if (e.KeyCode == Keys.Right)
            {
                hero.face = 1;
                if (hero.status != "climbing")
                {
                    
                    animateEnemies();
                    animateWizard();

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
                            hero.status = "normal";
                            //gravity();

                            //if (hero.pos.Y < h - 256)
                            //{
                            //    hero.pos.Y += 15;
                            //    start += 15;
                            //}

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

            }
            else if (e.KeyCode == Keys.Left)
            {
                hero.face = 2;
                if (hero.status != "climbing")
                {
                    animateEnemies();
                    animateWizard();


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
                            hero.status = "normal";
                            //gravity();


                            //if (hero.pos.Y < h - 256)
                            //{
                            //    hero.pos.Y += 15;
                            //    start += 15;
                            //}

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
                
            }
            else if (e.KeyCode == Keys.Space)
            {
                hero.status = "normal";

                if (rightflag == true)
                {
                    jumpright = true;

                }
                else
                {
                    jumpleft = true;
                }
            }

            if(e.KeyCode == Keys.F)
            {
                k = true;
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
            e1.w = w;
            e1.h = h;
            enemies.Add(e1);
            scrollObjects.Add(e1);


            ladder1 = new Ladder();
            ladder1.pos.X = w - 200;
            ladder1.pos.Y = -150;
            ladder1.id = 1;
            ladder1.img = new Bitmap("ladder2.png");
            ladders.Add(ladder1);
            scrollObjects.Add(ladder1);




            /////////////////////////////// IDLE WIZARD //////////////////////////////////////////////////////////

            wizarddata.idle.Add("idle_wizard/frame_0.png");
            wizarddata.idle.Add("idle_wizard/frame_1.png");
            wizarddata.idle.Add("idle_wizard/frame_2.png");
            wizarddata.idle.Add("idle_wizard/frame_3.png");
            wizarddata.idle.Add("idle_wizard/frame_4.png");
            wizarddata.idle.Add("idle_wizard/frame_5.png");
            wizarddata.idle.Add("idle_wizard/frame_6.png");
            wizarddata.idle.Add("idle_wizard/frame_7.png");


            /////////////////////////////// ATTACK WIZARD //////////////////////////////////////////////////////////

            wizarddata.attack.Add("attack_wizard/frame_0.png");
            wizarddata.attack.Add("attack_wizard/frame_1.png");
            wizarddata.attack.Add("attack_wizard/frame_2.png");
            wizarddata.attack.Add("attack_wizard/frame_3.png");
            wizarddata.attack.Add("attack_wizard/frame_4.png");
            wizarddata.attack.Add("attack_wizard/frame_5.png");
            wizarddata.attack.Add("attack_wizard/frame_6.png");
            wizarddata.attack.Add("attack_wizard/frame_7.png");


            /////////////////////////////// HIT WIZARD //////////////////////////////////////////////////////////

            wizarddata.hit.Add("hit_wizard/frame_0.png");
            wizarddata.hit.Add("hit_wizard/frame_1.png");
            wizarddata.hit.Add("hit_wizard/frame_2.png");


            /////////////////////////////// DEATH WIZARD //////////////////////////////////////////////////////////

            wizarddata.death.Add("death_wizard/frame_0.png");
            wizarddata.death.Add("death_wizard/frame_1.png");
            wizarddata.death.Add("death_wizard/frame_2.png");
            wizarddata.death.Add("death_wizard/frame_3.png");
            wizarddata.death.Add("death_wizard/frame_4.png");
            wizarddata.death.Add("death_wizard/frame_5.png");
            wizarddata.death.Add("death_wizard/frame_6.png");


            wizard = new Wizard(wizarddata);
            wizard.pos.X = 250;
            wizard.pos.Y = -550;
            scrollObjects.Add(wizard);

            elevator = new Elevator();
            elevator.pos.X = 0;
            elevator.pos.Y = -180;
            scrollObjects.Add(elevator);


        }



       /* public void UpdateAnimation()
        {
            *//*if(health<=0)
            {

            }*//*
        }*/
        void drawbullet()
        {

            int maxbullet = 100;

            if (bullettimer < timebetbullet || bullets.Count >= maxbullet)
            {
                return;
            }

            Bullet pnn = new Bullet();
            pnn.X = hero.pos.X + (hero.img.Width / 2);
            pnn.Y = hero.pos.Y + (hero.img.Height / 2);
            pnn.dx = 10;
            pnn.dy = 10;
            pnn.size = new Rectangle(hero.middle().X, hero.middle().Y, 10, 10);
            //pnn.speed = 10;
            pnn.face = hero.face;

            if(pnn.face == 1)
            {
            pnn.speed = 10;
            }
            else
            {
                pnn.speed = -10;
            }
             bullets.Add(pnn);
            


            bullettimer = 0;

        }

        void bulletmove1()
        {

            for (int i = 0; i < bullets.Count; i++) {

                bullets[i].X += bullets[i].speed;

                if (bullets[i].X < 0 || bullets[i].X > w)
                {
                    bullets.RemoveAt(i);
                }

                for (int j = 0; j < enemies.Count; j++)
                {

                    Enemy1 enemy = enemies[j];

                    int bulletL = bullets[i].X;
                    int bulletR = bullets[i].X + bullets[i].dx;
                    int bulletT = bullets[i].Y;
                    int bulletB = bullets[i].Y + bullets[i].dy;


                    int enemyL = enemy.pos.X;
                    int enemyR = enemy.pos.X + enemy.img.Width;
                    int enemyT = enemy.pos.Y;
                    int enemyB = enemy.pos.Y + enemy.img.Height;


                    if (bulletR > enemyL && bulletL < enemyR && bulletT < enemyB && bulletB > enemyT)
                    {
                        enemy.health -= 20;

                        bullets.RemoveAt(i);

                        if (enemy.health <= 0)
                        {
                            enemies.RemoveAt(i);
                        }

                        break;
                    }
                }
            }



           /* for (int i = bullets.Count - 1; i >= 0; i--)
            {
                int bullet = bullets[i];
                bullet.size.X += bullet.speed;

                // Remove bullet if it goes off-screen
                if (bullet.size.X > w)
                {
                    bullets.RemoveAt(i);
                }
            }*/
        }


        void bulletmove2()
        {/*
            for (int i = 0; i < bullets.Count; i++)
            {

                bullets[i].size.X += bullets[i].speed;

            }*/
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
            Pen pen = new Pen(Color.White);

            g2.DrawImage(original,          // source
               new Rectangle(0, 0, w, h),  // where to draw in new bitmap
               croprect,                  // what part to copy from source
               GraphicsUnit.Pixel        // unit type (pixels)
             );

            if (ladder1 != null)
            {
                g2.DrawImage(ladder1.img, ladder1.pos);

            }


            for(int i = 0; i < enemies.Count; i++)
            {
                g2.DrawImage(enemies[i].img, enemies[i].pos);
            }

            for (int i=0;i<bullets.Count;i++ )
            {
                g2.FillRectangle(Brushes.Purple, bullets[i].X, bullets[i].Y, bullets[i].dx, bullets[i].dy);
            }

            if(wizard != null)
            {
                g2.DrawImage(wizard.img, wizard.pos);

            }

            if (elevator != null)
            {
                g2.DrawImage(elevator.img, elevator.pos);

            }

            if (hero.stage == 3)
            {
                g2.DrawLine(pen, 200, hero.feet().Y -160, w, hero.feet().Y - 160);
                
            }






            g2.DrawImage(hero.img, hero.pos);

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
                    //start -= 15;
                    //e1.pos.Y += 15;
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
                    //start += 15;
                    //e1.pos.Y -= 15;

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
                    //start -= 15;
                    //e1.pos.Y += 15;

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
                    //start += 15;
                    //e1.pos.Y -= 15;

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

                if (enemies[i].status != "death")
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
                        enemies[i].status = "move";
                        //enemies[i].t_idle = 0;
                        //enemies[i].t_attack = 0;
                    }
                }

                enemies[i].UpdateAnimation();



                enemies[i].idle();
                enemies[i].attack();
                enemies[i].move();
                enemies[i].death();

                if (enemies[i].dead == true)
                {
                    enemies.RemoveAt(i);
                }


                if(enemies[i].health <= 0 && enemies[i].t_death >= enemies[i].data.deathRight.Count - 1)
                {
                    enemies.RemoveAt(i);
                }

            }

            
        }





        private void animateWizard()
        {
            if (wizard != null)
            {
                wizard.idle();
                wizard.attack();
            }
        }




        //private void gravity()
        //{
        //    if (hero.status != "climbing")
        //    {

        //        if (start < currentstart)
        //        {
        //            start += 20;
        //            for (int i = 0; i < scrollObjects.Count; i++)
        //            {

        //                scrollObjects[i].pos = new Point(scrollObjects[i].pos.X, scrollObjects[i].pos.Y - 15);
        //            }
        //        }

                
        //    }
        //}






        private void elevate()
        {
            if(flag == 0)
            {
                if (start > 0)
                {
                    start -= 20;

                    for (int j = 0; j < scrollObjects.Count; j++)
                    {
                        scrollObjects[j].pos = new Point(scrollObjects[j].pos.X, scrollObjects[j].pos.Y + 15);

                    }

                    elevator.pos.Y -= 15;
                }
                else
                {
                    hero.status = "normal";
                    currentstart = start;
                    previousstart = futurepreviousstart;
                    futurepreviousstart = currentstart;
                    hero.stage = 3;
                    hero.pos.X += 250;
                    //hero.pos.Y += 50;
                    flag = 1;
                    //MessageBox.Show("3");    

                    
                }
            }
            else if(flag == 1)
            {
                if (start < previousstart)
                {
                    hero.stage = 2;
                    start += 20;

                    for (int j = 0; j < scrollObjects.Count; j++)
                    {
                        scrollObjects[j].pos = new Point(scrollObjects[j].pos.X, scrollObjects[j].pos.Y - 15);

                    }

                    elevator.pos.Y += 15;
                }
                else
                {
                    hero.status = "normal";
                    start = previousstart;
                    currentstart = start;
                    futurepreviousstart = currentstart;
                    previousstart = currentstart;
                    hero.stage = 2;
                    hero.pos.X += 250;
                    hero.pos.Y += 40;
                    flag = 0;
                    //MessageBox.Show("2");

                }
            }

            
        }






    }

















    public partial class Hero
    {
        public Point pos;
        public Bitmap img = new Bitmap("Rframe_0.png");
        public int health = 100;
        public string status = "normal";
        public int stage = 1;
        public int face = 1;   // 1 lama ykon bass ymyn 

        public Point middle()
        {
            Point p = new Point((this.pos.X + (this.img.Width / 2)), (this.pos.Y + (this.img.Height / 2)));
            return p;
        }

        public Point feet()
        {
            Point p = new Point( this.middle().X, this.middle().Y + this.img.Height);
            return p;
        }

    }

    public partial class Enemy1 : ScrollObject
    {
        //public new Point pos;
        public Bitmap img;
        public Enemy1Data data = new Enemy1Data();
        public int health = 100;
        public int w;
        public int h;
        public string status = "idle";
        public string direction = "left";
        public int t_idle = 0;
        public int t_attack = 0;
        public int t_move = 0;
        public int t_death = 0;

        public bool dead = false;

        public void UpdateAnimation()
        {
            if (health <= 0)
            {
                status = "death";
                t_death++;
                if (t_death >= data.deathRight.Count)
                {
                    t_death = data.deathRight.Count - 1;
                }

                if (direction == "right")
                {
                    img = new Bitmap(data.deathRight[t_death]);
                }
                else
                {
                    img = new Bitmap(data.deathLeft[t_death]);
                }
            }
            else
            {
                if(status == "idle")
                {
                    if (direction == "right")
                    {
                        if (t_idle < data.idleRight.Count - 1)
                        {
                            t_idle++;
                        }
                        else
                        {
                            t_idle = 0;
                        }
                        img = new Bitmap(data.deathRight[t_idle]);
                    }
                    else
                    {
                        if (t_idle < data.idleRight.Count - 1)
                        {
                            t_idle++;
                        }
                        else
                        {
                            t_idle = 0;
                        }
                        img = new Bitmap(data.deathLeft[t_idle]) ;
                    }
                }
                else if(status == "attack")
                {
                    if (direction == "right")
                    {
                        if (t_attack < data.attackRight.Count - 1)
                        {
                            t_attack++;
                        }
                        else
                        {
                            t_attack = 0;
                        }
                        img = new Bitmap(data.attackRight[t_attack]);
                    }
                    else
                    {
                        if (t_attack < data.attackLeft.Count - 1)
                        {
                            t_attack++;
                        }
                        else
                        {
                            t_attack = 0;
                        }
                        img = new Bitmap (data.attackLeft[t_attack]);
                    }
                }
            }
        }

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



        public void move()
        {
            if(this.status == "move")
            {
                if (this.direction == "left")
                {
                    if (this.pos.X >= this.w/2)
                    {
                        if (t_move < data.runLeft.Count - 1)
                        {
                            t_move++;
                        }
                        else
                        {
                            t_move = 0;
                        }

                        if (this.img != null)
                        {
                            this.img.Dispose();
                        }

                        this.img = new Bitmap(data.runLeft[t_move]);

                        this.pos.X -= 30;
                    }
                    else
                    {
                        this.direction = "right";
                    }
                }
                else if (this.direction == "right")
                {
                    if (this.pos.X <= this.w - 512)
                    {
                        if (t_move < data.runRight.Count - 1)
                        {
                            t_move++;
                        }
                        else
                        {
                            t_move = 0;
                        }

                        if (this.img != null)
                        {
                            this.img.Dispose();
                        }

                        this.img = new Bitmap(data.runRight[t_move]);

                        this.pos.X += 30;
                    }
                    else
                    {
                        this.direction = "left";
                    }

                }
            }

        }


        public void death()
        {
            if (this.status == "death")
            {
                if (this.direction == "left")
                {
                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.deathLeft[t_death]);

                    if (t_death < data.deathLeft.Count - 1)
                    {
                        t_death++;
                    }
                    else
                    {
                        dead = true;
                    }
                }
                else if (this.direction == "right")
                {
                    if (this.img != null)
                    {
                        this.img.Dispose();
                    }

                    this.img = new Bitmap(data.deathRight[t_death]);

                    if (t_death < data.deathRight.Count - 1)
                    {
                        t_death++;
                    }
                    else
                    {
                        dead = true;
                    }
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

    public partial class WizardData
    {
        public List<string> idle = new List<string>();
        public List<string> attack = new List<string>();
        public List<string> hit = new List<string>();
        public List<string> death = new List<string>();

    }

    public partial class Wizard : ScrollObject
    {
        public WizardData data;
        public Bitmap img;
        public string status = "idle";
        public bool dead = false;
        public int t_idle = 0;
        public int t_attack = 0;


        public Wizard(WizardData exdata)
        {
            this.data = exdata;
            img = new Bitmap(data.idle[0]);
        }

        public void idle()
        {
            if (this.status == "idle")
            {
                if (t_idle < data.idle.Count - 1)
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

                this.img = new Bitmap(data.idle[t_idle]);
            }

        }


        public void attack()
        {
            if (this.status == "attack")
            {
                if (t_attack < data.attack.Count - 1)
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

                this.img = new Bitmap(data.attack[t_attack]);
            }
        }


    }


    public partial class Ladder : ScrollObject
    {
        //public new Point pos;
        public Bitmap img;
        public int id;

    }

    public abstract class ScrollObject
    {
        public Point pos;
        

    }


    public partial class Elevator : ScrollObject
    {
        public Bitmap img = new Bitmap("ufo.png");
    }


}
