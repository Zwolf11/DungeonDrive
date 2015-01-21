using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

public class Unit
{
    public double x;
    public double y;
    public double speed;

    public Unit(double x, double y, double speed)
    {
        this.x = x;
        this.y = y;
        this.speed = speed;
    }
}

public class Obstacle
{
    public int x;
    public int y;
    public int width;
    public int height;

    public Obstacle(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

public class Room
{
    public int width;
    public int height;
    public List<Obstacle> obstacles;

    public Room(int width, int height) : this(width, height, new List<Obstacle>()) { }

    public Room(int width, int height, List<Obstacle> obstacles)
    {
        this.width = width;
        this.height = height;
        this.obstacles = obstacles;
    }
}

public class Program : Form
{
    private int width;
    private int height;
    private int size = 30;
    private Room room;
    private Unit hero;
    private Hashtable keys = new Hashtable();

    private Random rand = new Random();
    private Timer timer = new Timer();

    public Program()
    {
        this.Text = "Dungeon Drive (D:)";
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
        this.KeyDown += this.keyDown;
        this.KeyUp += this.keyUp;
        this.Paint += this.paint;
        this.DoubleBuffered = true;

        width = Screen.PrimaryScreen.Bounds.Width;
        height = Screen.PrimaryScreen.Bounds.Height;
        room = new Room(rand.Next(3, 60), rand.Next(3, 60));
        room.obstacles.Add(new Obstacle(0, 0, 3, 2));
        hero = new Unit(0, 0, 0.3);
        keys[Keys.W] = false;
        keys[Keys.A] = false;
        keys[Keys.S] = false;
        keys[Keys.D] = false;

        timer.Interval = 17;
        timer.Tick += this.tick;
        timer.Start();
    }

    private void keyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
            this.Close();

        keys[e.KeyCode] = true;
    }

    private void keyUp(object sender, KeyEventArgs e)
    {
        keys[e.KeyCode] = false;
    }

    private void tick(object sender, EventArgs e)
    {
        if ((bool)keys[Keys.W] && ! (bool)keys[Keys.S])
        {
            if ((bool)keys[Keys.A] && !(bool)keys[Keys.D])
            {
                hero.x -= Math.Sqrt(2) / 2 * hero.speed;
                hero.y -= Math.Sqrt(2) / 2 * hero.speed; 
            }
            else if((bool)keys[Keys.D] && !(bool)keys[Keys.A])
            {
                hero.x += Math.Sqrt(2) / 2 * hero.speed;
                hero.y -= Math.Sqrt(2) / 2 * hero.speed; 
            }
            else
            {
                hero.y -= hero.speed;
            }
        }
        else if ((bool)keys[Keys.S] && !(bool)keys[Keys.W])
        {
            if ((bool)keys[Keys.A] && !(bool)keys[Keys.D])
            {
                hero.x -= Math.Sqrt(2) / 2 * hero.speed;
                hero.y += Math.Sqrt(2) / 2 * hero.speed; 
            }
            else if ((bool)keys[Keys.D] && !(bool)keys[Keys.A])
            {
                hero.x += Math.Sqrt(2) / 2 * hero.speed;
                hero.y += Math.Sqrt(2) / 2 * hero.speed; 
            }
            else
            {
                hero.y += hero.speed;
            }
        }
        else if ((bool)keys[Keys.A] && !(bool)keys[Keys.D])
        {
            hero.x -= hero.speed;
        }
        else if ((bool)keys[Keys.D] && !(bool)keys[Keys.A])
        {
            hero.x += hero.speed;
        }

        this.Invalidate();
    }

    private void paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        for (int i = 0; i < room.width; i++)
            for (int j = 0; j < room.height; j++)
                g.DrawRectangle(Pens.Black, (int)(i * size + width / 2 - hero.x * size - size / 2), (int)(j * size + height / 2 - hero.y * size - size / 2), size, size);

        foreach(Obstacle obstacle in room.obstacles)
            g.FillRectangle(Brushes.Blue, (int)(obstacle.x * size + width / 2 - hero.x * size - size / 2), (int)(obstacle.y * size + height / 2 - hero.y * size - size / 2), size * obstacle.width, size * obstacle.height);

        g.FillEllipse(Brushes.Red, width / 2 - size / 2, height / 2 - size / 2, size, size);
    }

    public static void Main()
    {
        Application.Run(new Program());
    }
}