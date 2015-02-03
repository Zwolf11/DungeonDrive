using System;
using System.Windows.Forms;

namespace DungeonDrive
{
    public static class InputHandler
    {
        public static void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                G.form.Close();

            G.keys[e.KeyCode] = true;
        }

        public static void keyUp(object sender, KeyEventArgs e)
        {
            G.keys.Remove(e.KeyCode);
        }
    }
}
