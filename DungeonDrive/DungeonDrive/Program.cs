using System;
using System.Windows.Forms;

namespace DungeonDrive
{
    public static class Program
    {
        public static void Main()
        {
            G.width = Screen.PrimaryScreen.Bounds.Width;
            G.height = Screen.PrimaryScreen.Bounds.Height;

            Application.Run(G.form);
        }
    }
}
