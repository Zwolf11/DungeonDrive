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

            buttonList.AddLast(this.button1);
            buttonList.AddLast(this.button2);
            buttonList.AddLast(this.button3);
            buttonList.AddLast(this.button4);
            buttonList.AddLast(this.button5);
            buttonList.AddLast(this.button6);
            buttonList.AddLast(this.button7);
            buttonList.AddLast(this.button8);
            buttonList.AddLast(this.button9);
            //hideAllButtons();
            int size = itemList.Count();
            
            for (int i = 0; i < size; i++)
            {

             //   buttonList.ElementAt(i).BackgroundImage = (System.Drawing.Image)(Properties.Resources.e1);
            }
        }

        private void hideAllButtons()
        {

            foreach (System.Windows.Forms.Button button in buttonList)
            {

                button.Hide();
            }

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "eat this berry and you will die";
            Console.WriteLine("sasdfasdfsafds!!!!");
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
    }
}