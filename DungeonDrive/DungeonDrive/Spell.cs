using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DungeonDrive
{
    public class Spell
    {
        public Bitmap[] spellIcon = new Bitmap[SkillStreeState.skillLevel];
        public String spellDesc;
        public Spell() { }
    }

    public class LighteningBall : Spell {
        
        //public static Bitmap[] Icon = new Bitmap[SkillStreeState.skillLevel];
        private Bitmap[] animation= new Bitmap[20];
        public int level = 1;
        GameState state;
        Unit unit;
        public void cast() {

            Projectile proj1 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.2, 15);
            proj1.isMagic = true;
            proj1.animation = this.animation;
            Projectile proj2 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.3, 15);
            proj2.isMagic = true;
            proj2.animation = this.animation;
            Projectile proj3 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.4, 15);
            proj3.isMagic = true;
            proj3.animation = this.animation;
            Projectile proj4 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.5, 15);
            proj4.isMagic = true;
            proj4.animation = this.animation;
            Projectile proj5 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.6, 15);
            //proj5.isMagic = true;
            proj5.proj_img = Properties.Resources.lightening;
            proj5.animation = this.animation;

            if (this.unit is Hero) { }
            else
            {

                proj1.friendlyFire = false;
                proj2.friendlyFire = false;
                proj3.friendlyFire = false;
                proj4.friendlyFire = false;
            }

            state.room.projectiles.Add(proj1);
            state.room.projectiles.Add(proj2);
            state.room.projectiles.Add(proj3);
            if (this.level == 2)
            {
                state.room.projectiles.Add(proj4);
            }
            else if (this.level == 3)
            {
                state.room.projectiles.Add(proj5);
            }
        }

        public void setLighteningBall(GameState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;       
        }
     
        public LighteningBall()

        {
            for (int i = 0; i < SkillStreeState.skillLevel; i++)
            {
                spellIcon[i] = Properties.Resources.ghost0;
            }
            spellIcon[0] = Properties.Resources.LighteningBall1;
            spellIcon[1] = Properties.Resources.LighteningBall2;
            spellIcon[2] = Properties.Resources.LighteningBall3;
            this.animation[0] = Properties.Resources.lighteningball_1_20_1;
            this.animation[1] = Properties.Resources.lighteningball_1_20_2;
            this.animation[2] = Properties.Resources.lighteningball_1_20_3;
            this.animation[3] = Properties.Resources.lighteningball_1_20_4;
            this.animation[4] = Properties.Resources.lighteningball_1_20_5;
            this.animation[5] = Properties.Resources.lighteningball_1_20_6;
            this.animation[6] = Properties.Resources.lighteningball_1_20_7;
            this.animation[7] = Properties.Resources.lightening;
            this.animation[8] = Properties.Resources.lighteningball_1_20_9;
            this.animation[9] = Properties.Resources.lighteningball_1_20_10;
            this.animation[10] = Properties.Resources.lighteningball_1_20_11;
            this.animation[11] = Properties.Resources.lighteningball_1_20_12;
            this.animation[12] = Properties.Resources.lighteningball_1_20_13;
            this.animation[13] = Properties.Resources.lighteningball_1_20_14;
            this.animation[14] = Properties.Resources.lighteningball_1_20_15;
            this.animation[15] = Properties.Resources.lighteningball_1_20_16;
            this.animation[16] = Properties.Resources.lighteningball_1_20_17;
            this.animation[17] = Properties.Resources.lighteningball_1_20_18;
            this.animation[18] = Properties.Resources.lighteningball_1_20_19;
            this.animation[19] = Properties.Resources.lightening;


        }

    
    }
}
