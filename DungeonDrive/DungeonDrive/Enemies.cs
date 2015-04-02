using System;
using System.Drawing;

namespace DungeonDrive
{
    public class Bat : Unit
    {
        private Bitmap[] imgs = new Bitmap[3];

        public Bat(GameState state, double x, double y) : base(state, x, y)
        {
            this.full_hp = 30;
            this.hp = full_hp;
            this.atk_dmg = 1;
            this.speed = 0.1;
            this.radius = 0.45;
            this.origin_x = x;
            this.origin_y = y;
            this.center_x = x + radius;
            this.center_y = y + radius;
            this.level = 1;
            this.exp = 2;
            this.dropWpnFac = 0.3;
            // for testing
            this.dropWpnFac = 1;
            this.status = "Normal";

            imgs[0] = new Bitmap(Properties.Resources.bat0);
            imgs[1] = new Bitmap(Properties.Resources.bat1);
            imgs[2] = new Bitmap(Properties.Resources.bat2);
        }

        public void move()
        {
            double xNext;
            double yNext;

            if (Math.Abs(state.hero.x - x) < 7 && Math.Abs(state.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                this.moving = true;
                xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
            }
            else if (this.moving)
            {
                //Move towards original position
                xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
                if ((Math.Round(this.x, 1) == this.origin_x || Math.Round(this.y, 1) == this.origin_y))
                {
                    //Original position has been reached
                    this.moving = false;
                    this.x = origin_x;
                    this.y = origin_y;
                    return;
                }
            }
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            double xNext;
            double yNext;

            //If bat units are below a certain HP threshold, they will start running from the player
            //Only a basic placeholder for future additions. Eventually, I will add more dynamic behaviors on top of this (ex. bats' escape route will prioritize nearby mobs and then turn on the player
            if (this.hp < 10)
            {
                xNext = x - Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed * 0.6;
                yNext = y - Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed * 0.6;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;

                /*foreach (Unit enemy in state.room.enemies)
                {
                    if (Math.Sqrt(Math.Pow(x - enemy.x, 2) + Math.Pow(y - enemy.y, 2)) < 3)
                    {
                        this.moving = true;
                        xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                        yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                        tryMove(xNext, yNext);
                        this.center_x = x + radius;
                        this.center_y = y + radius;
                    }
                }*/

                return;
            }

            move();

            if (state.room.currentRoom.Equals(state.graveyard))
            {
                this.full_hp = 15;
                this.hp = full_hp;
                this.atk_dmg = atk_dmg*3;
                this.speed = speed + (speed * 0.4);
            }

            //tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.DrawImage(imgs[(int)animFrame], DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            animFrame = (animFrame + 0.1) % imgs.Length;
            //g.FillEllipse(Brushes.Red, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            drawHpBar(g);
            if (this.displayname)
                drawFileName(g);
        }
    }

    public class Skeleton : Unit
    {
        private Bitmap[] imgs = new Bitmap[3];

        public Skeleton(GameState state, double x, double y)
            : base(state, x, y)
        {
            this.full_hp = 15;
            this.hp = full_hp;
            this.atk_dmg = 2;
            this.speed = 0.03;
            this.radius = 0.4;
            this.origin_x = x;
            this.origin_y = y;
            this.center_x = x + radius;
            this.center_y = y + radius;
            this.level = 1;
            this.exp = 2;
            this.dropWpnFac = 0.2;
            this.status = "Normal";

            imgs[0] = new Bitmap(Properties.Resources.skeleton0);
            imgs[1] = new Bitmap(Properties.Resources.skeleton1);
            imgs[2] = new Bitmap(Properties.Resources.skeleton2);
        }

        public void move()
        {
            double xNext;
            double yNext;

            if (Math.Abs(state.hero.x - x) < 7 && Math.Abs(state.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                this.moving = true;
                xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
            }
            else if (this.moving)
            {
                //Move towards original position
                xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
                if ((Math.Round(this.x, 1) == this.origin_x || Math.Round(this.y, 1) == this.origin_y))
                {
                    //Original position has been reached
                    this.moving = false;
                    this.x = origin_x;
                    this.y = origin_y;
                    return;
                }
            }
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            move();

            //double xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
            //double yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;

            if (state.room.currentRoom.Equals(state.graveyard))
            {
                this.full_hp = 10;
                this.hp = full_hp;
                this.atk_dmg = atk_dmg * 3;
                this.speed = speed + (speed * 0.4);
            }

            //tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.DrawImage(imgs[(int)animFrame], DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            animFrame = (animFrame + 0.1) % imgs.Length;
            //g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            drawHpBar(g);
            if (this.displayname)
                drawFileName(g);
        }
    }

    public class Snake : Unit
    {
        private Bitmap[] imgs = new Bitmap[3];
        private Random rand;

        public Snake(GameState state, double x, double y)
            : base(state, x, y)
        {
            this.full_hp = 10;
            this.hp = full_hp;
            this.atk_dmg = 2;
            this.speed = 0.1;
            this.radius = 0.2;
            this.origin_x = x;
            this.origin_y = y;
            this.center_x = x + radius;
            this.center_y = y + radius;
            this.level = 1;
            this.exp = 3;
            this.dropWpnFac = 0.2;
            this.status = "Normal";
            this.lunge = true;

            imgs[0] = new Bitmap(Properties.Resources.snake0);
            imgs[1] = new Bitmap(Properties.Resources.snake1);
            imgs[2] = new Bitmap(Properties.Resources.snake2);

            rand = new Random();
        }

        public void move()
        {
            double xNext;
            double yNext;

            if (Math.Abs(state.hero.x - x) < 7 && Math.Abs(state.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                this.moving = true;
                xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
            }
            else if (this.moving)
            {
                //Move towards original position
                xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
                if ((Math.Round(this.x, 1) == this.origin_x || Math.Round(this.y, 1) == this.origin_y))
                {
                    //Original position has been reached
                    this.moving = false;
                    this.x = origin_x;
                    this.y = origin_y;
                    return;
                }
            }
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            move();

            //double xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
            //double yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;

            /*if ((state.hero.x - x) < 3 && (state.hero.x - x) > 0.2 && (state.hero.y - y) < 3 && (state.hero.y - y) > 0.2)
                this.speed = 0.15;
            else
                this.speed = 0.03;*/

            if (state.room.currentRoom.Equals(state.graveyard))
            {
                this.full_hp -= 5;
                this.hp = full_hp;
                this.atk_dmg = atk_dmg + (atk_dmg * 0.30);
                this.speed = speed + (speed * 0.4);
            }

            if (Math.Sqrt(Math.Pow(state.hero.x - x, 2) + Math.Pow(state.hero.y - y, 2)) < state.hero.radius + radius)
            {
                if (atk_cd[0])
                {
                    int random = rand.Next(0, 100);
                    if (random <= 30)
                        statusChanged(state.hero, "poison");
                }
            }

            /*if ((state.hero.x - x) < 3 && (state.hero.y - y) < 3)
            {
                if (this.lunge)
                {
                    this.speed = 0.4;
                    this.lunge = false;
                }

                if (Math.Sqrt(Math.Pow(state.hero.x - x, 2) + Math.Pow(state.hero.y - y, 2)) < state.hero.radius + radius)
                {
                    this.speed = 0.1;
                }
            }*/

            //tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.DrawImage(imgs[(int)animFrame], DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            animFrame = (animFrame + 0.1) % imgs.Length;
            //g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            drawHpBar(g);
            if (this.displayname)
                drawFileName(g);
        }
    }

    public class Ghost : Unit
    {
        private Bitmap[] imgs = new Bitmap[3];
        private Random rand;

        public Ghost(GameState state, double x, double y)
            : base(state, x, y)
        {
            this.full_hp = 5;
            this.hp = full_hp;
            this.atk_dmg = 3;
            this.speed = 0.05;
            this.radius = 0.3;
            this.origin_x = x;
            this.origin_y = y;
            this.center_x = x + radius;
            this.center_y = y + radius;
            this.level = 1;
            this.exp = 4;
            this.dropWpnFac = 0.1;
            this.status = "Normal";
            this.teleport = true;
            this.phase = true;

            imgs[0] = new Bitmap(Properties.Resources.ghost0);
            imgs[1] = new Bitmap(Properties.Resources.ghost1);
            imgs[2] = new Bitmap(Properties.Resources.ghost2);

            rand = new Random();
        }

        public void move()
        {
            double xNext;
            double yNext;

            if (Math.Abs(state.hero.x - x) < 7 && Math.Abs(state.hero.y - y) < 7)
            {
                //Player draws aggro from bats if he is close enough
                this.moving = true;
                xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
            }
            else if (this.moving)
            {
                //Move towards original position
                xNext = x + Math.Cos(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                yNext = y + Math.Sin(Math.Atan2(this.origin_y - y, this.origin_x - x)) * speed;
                tryMove(xNext, yNext, this);
                this.center_x = x + radius;
                this.center_y = y + radius;
                if ((Math.Round(this.x, 1) == this.origin_x || Math.Round(this.y, 1) == this.origin_y))
                {
                    //Original position has been reached
                    this.moving = false;
                    this.x = origin_x;
                    this.y = origin_y;
                    return;
                }
            }
        }

        public override void act()
        {
            /*if (knockback)
                knockBacked();*/

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            move();

            //double xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
            //double yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;

            /*if ((state.hero.x - x) < 3 && (state.hero.x - x) > 0.2 && (state.hero.y - y) < 3 && (state.hero.y - y) > 0.2)
                this.speed = 0.15;
            else
                this.speed = 0.03;*/

            if (state.room.currentRoom.Equals(state.graveyard))
            {
                this.full_hp -= 5;
                this.hp = full_hp;
                this.atk_dmg = atk_dmg + (atk_dmg * 0.30);
                this.speed = speed + (speed * 0.4);
            }



            if (Math.Sqrt(Math.Pow(state.hero.x - x, 2) + Math.Pow(state.hero.y - y, 2)) < state.hero.radius + radius)
            {
                if (atk_cd[0])
                {
                    int random = rand.Next(0, 100);
                    if (random <= 25)
                        statusChanged(state.hero, "curse");
                }
            }

            /*if ((state.hero.x - x) < 3 && (state.hero.y - y) < 3)
            {
                if (this.lunge)
                {
                    this.speed = 0.4;
                    this.lunge = false;
                }

                if (Math.Sqrt(Math.Pow(state.hero.x - x, 2) + Math.Pow(state.hero.y - y, 2)) < state.hero.radius + radius)
                {
                    this.speed = 0.1;
                }
            }*/

            //tryMove(xNext, yNext);

            if (Math.Abs(state.hero.x - x) < 5 && Math.Abs(state.hero.y - y) < 5 && this.teleport)
            {
                this.teleport = false;

                //If hero is 5 units close to the boss, have the boss teleport behind the player

                if ((state.hero.x - x) < 0 && (state.hero.y - y) < 0)
                {
                    //Boss is in bottom-right direction with respect to hero
                    x = state.hero.x - 0.5;
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) < 0 && (state.hero.y - y) > 0)
                {
                    //Boss is in upper-right direction with respect to hero
                    x = state.hero.x - 0.5;
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) < 0)
                {
                    //Boss is in bottom-left direction with respect to hero
                    x = state.hero.x + 0.5;
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) > 0)
                {
                    //Boss is in upper-left direction with respect to hero
                    x = state.hero.x + 0.5;
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) == 0 && (state.hero.y - y) < 0)
                {
                    //Boss is directly south of the player
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) == 0 && (state.hero.y - y) > 0)
                {
                    //Boss is directly north of the player
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) < 0 && (state.hero.y - y) == 0)
                {
                    //Boss is directly east of the player
                    x = state.hero.x - 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) == 0)
                {
                    //Boss is directly west of the player
                    x = state.hero.x + 0.5;
                    return;
                }
            }

        }

        public override void draw(Graphics g)
        {
            g.DrawImage(imgs[(int)animFrame], DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            animFrame = (animFrame + 0.1) % imgs.Length;
            //g.FillEllipse(Brushes.SaddleBrown, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            drawHpBar(g);
            if (this.displayname)
                drawFileName(g);
        }
    }

    /*public class Boss : Unit
    {
        public Boss(double x, double y)
            : base(x, y)
        {
            this.full_hp = 200;
            this.hp = full_hp;
            this.atk_dmg = 5;
            this.speed = 0.02;
            this.radius = 0.6;
            this.teleport = true;
        }

        public override void act()
        {
            if (knockback)
                knockBacked();

            if (sleep_sec > 0)
            {
                sleep_sec--;
                return;
            }

            double xNext = x + Math.Cos(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;
            double yNext = y + Math.Sin(Math.Atan2(state.hero.y - y, state.hero.x - x)) * speed;

            if (Math.Abs(state.hero.x - x) < 5 && Math.Abs(state.hero.y - y) < 5 && this.teleport)
            {
                this.teleport = false;

                //If hero is 5 units close to the boss, have the boss teleport behind the player

                if ((state.hero.x - x) < 0 && (state.hero.y - y) < 0)
                {
                    //Boss is in bottom-right direction with respect to hero
                    x = state.hero.x - 0.5;
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) < 0 && (state.hero.y - y) > 0)
                {
                    //Boss is in upper-right direction with respect to hero
                    x = state.hero.x - 0.5;
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) < 0)
                {
                    //Boss is in bottom-left direction with respect to hero
                    x = state.hero.x + 0.5;
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) > 0)
                {
                    //Boss is in upper-left direction with respect to hero
                    x = state.hero.x + 0.5;
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) == 0 && (state.hero.y - y) < 0)
                {
                    //Boss is directly south of the player
                    y = state.hero.y - 0.5;
                    return;
                }
                else if ((state.hero.x - x) == 0 && (state.hero.y - y) > 0)
                {
                    //Boss is directly north of the player
                    y = state.hero.y + 0.5;
                    return;
                }
                else if ((state.hero.x - x) < 0 && (state.hero.y - y) == 0)
                {
                    //Boss is directly east of the player
                    x = state.hero.x - 0.5;
                    return;
                }
                else if ((state.hero.x - x) > 0 && (state.hero.y - y) == 0)
                {
                    //Boss is directly west of the player
                    x = state.hero.x + 0.5;
                    return;
                }
            }
            else
                this.speed = 0.02;

            tryMove(xNext, yNext);
        }

        public override void draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, DrawX, DrawY, (int)(radius * 2 * state.size), (int)(radius * 2 * state.size));
            drawHpBar(g);        }
    }*/
}
