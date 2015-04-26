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
        public int duration;
        public int cd = 5;
        public int maxFrame;
        public String spellName = "";
        public String spellDesc;
        public Spell() {}
        public virtual void cast(GameState state, Unit unit) {}
        public virtual void tick() { }
    }

    public class LighteningBall : Spell {
        
        //public static Bitmap[] Icon = new Bitmap[SkillStreeState.skillLevel];
        public Bitmap[] animation= new Bitmap[20];
        public int level = 1;
        GameState state;
        Unit unit;
        public override void cast(GameState state, Unit unit)
        {
            setLighteningBall(state, unit);
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
            : base()

        {
            this.spellName = "Lightening Ball";
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

    public class RuneOfFire : Spell {
        public Bitmap[] animation = new Bitmap[20];
        public int level = 1;
        GameState state;
        Unit unit;
        public void setRuneOfFrost(GameState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;
        }
        public RuneOfFire() : base()

        {
            this.spellName = "Rune Of Fire";
            for (int i = 0; i < SkillStreeState.skillLevel; i++)
            {
                spellIcon[i] = Properties.Resources.ghost0;
            }
            spellIcon[0] = Properties.Resources.rune_1;
            spellIcon[1] = Properties.Resources.rune_2;
            spellIcon[2] = Properties.Resources.rune_3;
            this.animation[0] = Properties.Resources.RUNEOFFIRE9;
            this.animation[1] = Properties.Resources.RUNEOFFIRE9;
            this.animation[2] = Properties.Resources.RUNEOFFIRE8;
            this.animation[3] = Properties.Resources.RUNEOFFIRE8;
            this.animation[4] = Properties.Resources.RUNEOFFIRE8;
            this.animation[5] = Properties.Resources.RUNEOFFIRE7;
            this.animation[6] = Properties.Resources.RUNEOFFIRE7;
            this.animation[7] = Properties.Resources.RUNEOFFIRE7;
            this.animation[8] = Properties.Resources.RUNEOFFIRE6;
            this.animation[9] = Properties.Resources.RUNEOFFIRE6;
            this.animation[10] = Properties.Resources.RUNEOFFIRE6;
            this.animation[11] = Properties.Resources.RUNEOFFIRE6;
            this.animation[12] = Properties.Resources.RUNEOFFIRE7;
            this.animation[13] = Properties.Resources.RUNEOFFIRE7;
            this.animation[14] = Properties.Resources.RUNEOFFIRE7;
            this.animation[15] = Properties.Resources.RUNEOFFIRE6;
            this.animation[16] = Properties.Resources.RUNEOFFIRE6;
            this.animation[17] = Properties.Resources.RUNEOFFIRE6;
            this.animation[18] = Properties.Resources.RUNEOFFIRE6;
            this.animation[19] = Properties.Resources.RUNEOFFIRE6;
        }

        public override void cast(GameState state, Unit unit)
        {
            setRuneOfFrost( state, unit);
            staticProjectiles proj1 = new staticProjectiles(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0, 5);
            proj1.isMagic = true;
            proj1.animation = this.animation;
            proj1.dmg = 0.1;
            proj1.radius = 3;

            if (this.unit is Hero) { }
            else
            {
                proj1.friendlyFire = false;
            }
            state.room.projectiles.Add(proj1);

        }
    }

    public class EnergyBarrier : Spell
    {

        public Bitmap[] animation = new Bitmap[32];
        public int level = 1;
        GameState state;
        Unit unit;
        public void setEnergyBarrier(GameState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;
        }
        public EnergyBarrier()
            : base()
        {
            this.maxFrame = 32;
            this.spellName = "Energy Barrier";
            for (int i = 0; i < SkillStreeState.skillLevel; i++)
            {
                spellIcon[i] = Properties.Resources.ghost0;
            }
            spellIcon[0] = Properties.Resources.EB1;
            spellIcon[1] = Properties.Resources.EB2;
            spellIcon[2] = Properties.Resources.EB3;
            this.animation[0] = Properties.Resources.EnergyBarrier1;
            this.animation[1] = Properties.Resources.EnergyBarrier2;
            this.animation[2] = Properties.Resources.EnergyBarrier3;
            this.animation[3] = Properties.Resources.EnergyBarrier4;
            this.animation[4] = Properties.Resources.EnergyBarrier5;
            this.animation[5] = Properties.Resources.EnergyBarrier6;
            this.animation[6] = Properties.Resources.EnergyBarrier7;
            this.animation[7] = Properties.Resources.EnergyBarrier8;
            this.animation[8] = Properties.Resources.EnergyBarrier9;
            this.animation[9] = Properties.Resources.EnergyBarrier10;
            this.animation[10] = Properties.Resources.EnergyBarrier11;
            this.animation[11] = Properties.Resources.EnergyBarrier12;
            this.animation[12] = Properties.Resources.EnergyBarrier13;
            this.animation[13] = Properties.Resources.EnergyBarrier14;
            this.animation[14] = Properties.Resources.EnergyBarrier15;
            this.animation[15] = Properties.Resources.EnergyBarrier15;
            this.animation[16] = Properties.Resources.EnergyBarrier17;
            this.animation[17] = Properties.Resources.EnergyBarrier18;
            this.animation[18] = Properties.Resources.EnergyBarrier19;
            this.animation[19] = Properties.Resources.EnergyBarrier20;
            this.animation[20] = Properties.Resources.EnergyBarrier21;
            this.animation[21] = Properties.Resources.EnergyBarrier22;
            this.animation[22] = Properties.Resources.EnergyBarrier23;
            this.animation[23] = Properties.Resources.EnergyBarrier24;
            this.animation[24] = Properties.Resources.EnergyBarrier25;
            this.animation[25] = Properties.Resources.EnergyBarrier26;
            this.animation[26] = Properties.Resources.EnergyBarrier27;
            this.animation[27] = Properties.Resources.EnergyBarrier28;
            this.animation[28] = Properties.Resources.EnergyBarrier29;
            this.animation[29] = Properties.Resources.EnergyBarrier30;
            this.animation[30] = Properties.Resources.EnergyBarrier31;
            this.animation[31] = Properties.Resources.EnergyBarrier32;
            
        }

        public override void cast(GameState state, Unit unit)
        {
            setEnergyBarrier(state, unit);
            barrierProjectiles proj1 = new barrierProjectiles(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0, 5);
            proj1.isMagic = true;
            proj1.animation = this.animation;
            proj1.dmg = 0.1;
            proj1.radius = 3;
            proj1.maxFrame = this.maxFrame;
            if (this.unit is Hero) { }
            else
            {
                proj1.friendlyFire = false;
            }
            state.room.projectiles.Add(proj1);

        }
    }

    public class CrusingFireBall : Spell{
        public Bitmap[] animation = new Bitmap[32];
        public int level = 1;
        GameState state;
        Unit unit;

        public CrusingFireBall()
            : base()
        {
            this.maxFrame = 31;
            this.spellName = "Crusing Fire";
            for (int i = 0; i < SkillStreeState.skillLevel; i++)
            {
                spellIcon[i] = Properties.Resources.ghost0;
            }
            spellIcon[0] = Properties.Resources.CrusingfireIcon1;
            spellIcon[1] = Properties.Resources.CrusingfireIcon2;
            spellIcon[2] = Properties.Resources.CrusingfireIcon3;
            this.animation[0] = Properties.Resources.CrusingFire1;
            this.animation[1] = Properties.Resources.CrusingFire1;
            this.animation[2] = Properties.Resources.CrusingFire2;
            this.animation[3] = Properties.Resources.CrusingFire2;
            this.animation[4] = Properties.Resources.CrusingFire3;
            this.animation[5] = Properties.Resources.CrusingFire3;
            this.animation[6] = Properties.Resources.CrusingFire4;
            this.animation[7] = Properties.Resources.CrusingFire5;
            this.animation[8] = Properties.Resources.CrusingFire6;
            this.animation[9] = Properties.Resources.CrusingFire7;
            this.animation[10] = Properties.Resources.CrusingFire8;
            this.animation[11] = Properties.Resources.CrusingFire9;
            this.animation[12] = Properties.Resources.CrusingFire10;
            this.animation[13] = Properties.Resources.CrusingFire11;
            this.animation[14] = Properties.Resources.CrusingFire12;
            this.animation[15] = Properties.Resources.CrusingFire13;
            this.animation[16] = Properties.Resources.CrusingFire14;
            this.animation[17] = Properties.Resources.CrusingFire14;
            this.animation[18] = Properties.Resources.CrusingFire13;
            this.animation[19] = Properties.Resources.CrusingFire12;
            this.animation[20] = Properties.Resources.CrusingFire11;
            this.animation[21] = Properties.Resources.CrusingFire10;
            this.animation[22] = Properties.Resources.CrusingFire9;
            this.animation[23] = Properties.Resources.CrusingFire8;
            this.animation[24] = Properties.Resources.CrusingFire7;
            this.animation[25] = Properties.Resources.CrusingFire6;
            this.animation[26] = Properties.Resources.CrusingFire5;
            this.animation[27] = Properties.Resources.CrusingFire4;
            this.animation[28] = Properties.Resources.CrusingFire3;
            this.animation[29] = Properties.Resources.CrusingFire2;
            this.animation[30] = Properties.Resources.CrusingFire1;
                       
        }
        public override void cast(GameState state, Unit unit)
        {
            setCrusingFire(state, unit);
            CircleAroundProjectiles proj1 = new CircleAroundProjectiles(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.3,2, unit);
            proj1.isMagic = true;
            proj1.animation = this.animation;
            proj1.dmg = 0.1;
            proj1.radius = 1;
            proj1.maxFrame = this.maxFrame;
            if (this.unit is Hero) { }
            else
            {
                proj1.friendlyFire = false;
            }
            state.room.projectiles.Add(proj1);

        }
        public void setCrusingFire(GameState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;
        }
    }

    public class Pyroblast : Spell{
        public Bitmap[] animation = new Bitmap[32];
        public int level = 1;
        GameState state;
        Unit unit;
        public Pyroblast()
            : base()
        {
            this.maxFrame = 31;
            this.spellName = "Pyroblast";
            for (int i = 0; i < SkillStreeState.skillLevel; i++)
            {
                spellIcon[i] = Properties.Resources.ghost0;
            }
            spellIcon[0] = Properties.Resources.pyroblast1;
            spellIcon[1] = Properties.Resources.pyroblast2;
            spellIcon[2] = Properties.Resources.pyroblast3;
            this.animation[0] = Properties.Resources.fireball_1_64_1;
            this.animation[1] = Properties.Resources.fireball_1_64_2;
            this.animation[2] = Properties.Resources.fireball_1_64_3;
            this.animation[3] = Properties.Resources.fireball_1_64_4;
            this.animation[4] = Properties.Resources.fireball_1_64_5;
            this.animation[5] = Properties.Resources.fireball_1_64_6;
            this.animation[6] = Properties.Resources.fireball_1_64_7;
            this.animation[7] = Properties.Resources.fireball_1_64_8;
            this.animation[8] = Properties.Resources.fireball_1_64_9;
            this.animation[9] = Properties.Resources.fireball_1_64_10;
            this.animation[10] = Properties.Resources.fireball_1_64_11;
            this.animation[11] = Properties.Resources.fireball_1_64_12;
            this.animation[12] = Properties.Resources.fireball_1_64_13;
            this.animation[13] = Properties.Resources.fireball_1_64_14;
            this.animation[14] = Properties.Resources.fireball_1_64_15;
            this.animation[15] = Properties.Resources.fireball_1_64_16;
            this.animation[16] = Properties.Resources.fireball_1_64_17;
            this.animation[17] = Properties.Resources.fireball_1_64_18;
            this.animation[18] = Properties.Resources.fireball_1_64_19;
            this.animation[19] = Properties.Resources.fireball_1_64_20;
            this.animation[20] = Properties.Resources.fireball_1_64_21;
            this.animation[21] = Properties.Resources.fireball_1_64_22;
            this.animation[22] = Properties.Resources.fireball_1_64_23;
            this.animation[23] = Properties.Resources.fireball_1_64_24;
            this.animation[24] = Properties.Resources.fireball_1_64_25;
            this.animation[25] = Properties.Resources.fireball_1_64_26;
            this.animation[26] = Properties.Resources.fireball_1_64_27;
            this.animation[27] = Properties.Resources.fireball_1_64_28;
            this.animation[28] = Properties.Resources.fireball_1_64_29;
            this.animation[29] = Properties.Resources.fireball_1_64_30;
            this.animation[30] = Properties.Resources.fireball_1_64_31;
                       
        }
        public override void cast(GameState state, Unit unit)
        {
            setPyroblast(state, unit);
            //knockBackProjectile proj1 = new knockBackProjectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.1, 12, unit);
            //Projectile proj1 = new Projectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.2, 15);
            knockBackProjectile proj1 = new knockBackProjectile(state, unit.x, unit.y, Math.Cos(unit.dir), Math.Sin(unit.dir), 0.1, 15, unit);
            proj1.isMagic = true;
            proj1.animation = this.animation;
            proj1.dmg = 0.1;
            proj1.radius = 0.5;
            //proj1.maxFrame = this.maxFrame;
            if (this.unit is Hero) { }
            else
            {
                proj1.friendlyFire = false;
            }
            state.room.projectiles.Add(proj1);

        }
        public void setPyroblast(GameState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;
        }

    }
}
