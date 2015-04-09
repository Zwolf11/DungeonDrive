using System;
using System.Windows.Forms;
using System.Drawing;

namespace DungeonDrive
{
    class SkillStreeState : State
    {

        public SkillStreeState(MainForm form) : base(form) { }
        private GameState state { get { return (GameState)parent; } }
        public override void keyDown(object sender, KeyEventArgs e) { 
        }
        public override void mouseMove(object sender, MouseEventArgs e) { 
        }

        public override void mouseDown(object sender, MouseEventArgs e) { 
        }
        public override void mouseUp(object sender, MouseEventArgs e) { 
        }
        public override void tick(object sender, EventArgs e) { 
        }
        public override void keyUp(object sender, KeyEventArgs e) { 
        }
        public override void paint(object sender, PaintEventArgs e) { 
        }
    }
}
