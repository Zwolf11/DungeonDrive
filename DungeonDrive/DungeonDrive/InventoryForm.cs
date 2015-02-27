using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace DungeonDrive
{
    public partial class InventoryForm : Form
    {
        LinkedList<System.Windows.Forms.Button> buttonList = new LinkedList<System.Windows.Forms.Button>();
        LinkedList<Item> itemList = new LinkedList<Item>();
        private System.Windows.Forms.Button mostRecentButton;
        private SoundPlayer potion1;
        Item selected;
        
        public InventoryForm(Inventory inventory)
        {
            
            InitializeComponent();
            this.button1.Click += button1_Click;
            initialization(inventory.getItemList());
            
        }
        public void updateInventoryForm(Inventory inventory)
        {
            this.button1.Click += button1_Click;
            initialization(inventory.getItemList());
        }/*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }*/
        private void MyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
        private void initialization(LinkedList<Item> _itemList)
        {
            this.itemList = _itemList;
            int length = _itemList.Count();
            buttonList.AddLast(this.button1);
            buttonList.AddLast(this.button2);
            buttonList.AddLast(this.button3);
            buttonList.AddLast(this.button4);
            buttonList.AddLast(this.button5);
            buttonList.AddLast(this.button6);
            buttonList.AddLast(this.button7);
            buttonList.AddLast(this.button8);
            buttonList.AddLast(this.button9);
            buttonList.AddLast(this.button10);
            buttonList.AddLast(this.button11);
            buttonList.AddLast(this.button12);
            buttonList.AddLast(this.button13);
            buttonList.AddLast(this.button14);
            buttonList.AddLast(this.button15);
            buttonList.AddLast(this.button16);
            buttonList.AddLast(this.button17);
            buttonList.AddLast(this.button18);
            buttonList.AddLast(this.button19);
            buttonList.AddLast(this.button20);
            buttonList.AddLast(this.button21);
            buttonList.AddLast(this.button22);
            buttonList.AddLast(this.button23);
            buttonList.AddLast(this.button24);
            buttonList.AddLast(this.button25);
            buttonList.AddLast(this.button26);
            buttonList.AddLast(this.button27);
            buttonList.AddLast(this.button28);
            buttonList.AddLast(this.button29);
            buttonList.AddLast(this.button30);
            buttonList.AddLast(this.button31);
            buttonList.AddLast(this.button32);
            buttonList.AddLast(this.button33);
            buttonList.AddLast(this.button34);
            buttonList.AddLast(this.button35);
            buttonList.AddLast(this.button36);
            buttonList.AddLast(this.button37);
            buttonList.AddLast(this.button38);
            buttonList.AddLast(this.button39);
            buttonList.AddLast(this.button40);
            for (int i = 0; i < 40; i++ )
            {
                buttonList.ElementAt(i).Enabled = false;
            }
            for (int i = 0; i < length; i++ )
            {
                Item item = _itemList.ElementAt(i);
                String icon = item.itemImage;
                Console.Write("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + icon);
                buttonList.ElementAt(i).BackgroundImage = Image.FromFile(@icon);
                buttonList.ElementAt(i).Enabled = true;
            }
        }



        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button1;
            this.selected = this.itemList.ElementAt(0);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
            potion1 = new SoundPlayer(@"potion2.wav");
            potion1.Play();
            G.hero.hp += 10;
            if (G.hero.hp > G.hero.full_hp) { G.hero.hp = G.hero.full_hp; }
            
        }





        private void button4_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button4;
            this.selected = this.itemList.ElementAt(3);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button9;
            this.selected = this.itemList.ElementAt(8);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button40;
            this.selected = this.itemList.ElementAt(39);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button17;
            this.selected = this.itemList.ElementAt(16);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button2;
            this.selected = this.itemList.ElementAt(1);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
            potion1 = new SoundPlayer(@"potion.wav");
            potion1.Play();
            G.hero.hp += 2;
            if (G.hero.hp > G.hero.full_hp) { G.hero.hp = G.hero.full_hp; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button3;
            this.selected = this.itemList.ElementAt(2);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button5;
            this.selected = this.itemList.ElementAt(4);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button6;
            this.selected = this.itemList.ElementAt(5);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button7;
            this.selected = this.itemList.ElementAt(6);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button8;
            this.selected = this.itemList.ElementAt(7);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button16;
            this.selected = this.itemList.ElementAt(15);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button15;
            this.selected = this.itemList.ElementAt(14);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button14;
            this.selected = this.itemList.ElementAt(13);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button13;
            this.selected = this.itemList.ElementAt(12);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button12;
            this.selected = this.itemList.ElementAt(11);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button11;
            this.selected = this.itemList.ElementAt(10);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button10;
            this.selected = this.itemList.ElementAt(9);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }



        private void button18_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button18;
            this.selected = this.itemList.ElementAt(17);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button19;
            this.selected = this.itemList.ElementAt(18);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button20;
            this.selected = this.itemList.ElementAt(19);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button21;
            this.selected = this.itemList.ElementAt(20);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button22;
            this.selected = this.itemList.ElementAt(21);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button23;
            this.selected = this.itemList.ElementAt(22);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button24;
            this.selected = this.itemList.ElementAt(23);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button32;
            this.selected = this.itemList.ElementAt(31);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button39;
            this.selected = this.itemList.ElementAt(38);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button31;
            this.selected = this.itemList.ElementAt(30);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button30;
            this.selected = this.itemList.ElementAt(29);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button38;
            this.selected = this.itemList.ElementAt(37);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            this.mostRecentButton = this.button37;
            this.selected = this.itemList.ElementAt(36);
            this.richTextBox1.Text = this.selected.getDesc();
            this.textBox2.Text = this.selected.getName();
        }

        public void callInventory() { 
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button43_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "Simple Instructions: \n Switching Attacks:   T\nAttack:   left/right click\n Move:   w/s/a/d\nInventory:   Space\nQuit:   ESC\n";
            
        }

        private void button44_Click(object sender, EventArgs e)
        {

        }

        private void button44_Click_1(object sender, EventArgs e)
        {
            G.quickButton1 = this.mostRecentButton;
            
        }

    }
}