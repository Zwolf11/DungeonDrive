using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DungeonDrive
{
    public partial class InventoryForm : Form
    {
        LinkedList<System.Windows.Forms.Button> buttonList = new LinkedList<System.Windows.Forms.Button>();
        public InventoryForm(Inventory inventory)
        {
            
            InitializeComponent();
            this.button1.Click += button1_Click;
            initialization(inventory.getItemList());
            
        }

        private void initialization(LinkedList<Item> itemList)
        {
            int length = itemList.Count();
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

            for (int i = 0; i < length; i++ )
            {
                Item item = itemList.ElementAt(i);
                String icon = item.itemImage;
                Console.Write("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + icon);
                buttonList.ElementAt(i).BackgroundImage = Image.FromFile(@icon);
            }
        }



        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "eat this berry and you will die";
            String s = "HP_Potion_m.png";
            Image bmp = Image.FromFile(@s);
            
            this.button1.BackgroundImage = (Image)bmp;
        //     bmp = new Bitmap(DungeonDrive.Properties.Resources.wand_5);

          //  buttonList.ElementAt(1).BackgroundImage = (Image)bmp;
        }

        void button1_MouseEnter(object sender, EventArgs e)
        {
        //    this.button1.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.c3));
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // this.button1.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.c2));
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button40_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void button39_Click(object sender, EventArgs e)
        {

        }

        private void button31_Click(object sender, EventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {

        }

        private void button37_Click(object sender, EventArgs e)
        {

        }

        public void callInventory() { 
        
        }

    }
}