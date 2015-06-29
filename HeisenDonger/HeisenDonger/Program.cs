using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace HeisenDonger
{
    class Program
    {
        private static Menu Config;
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        private static double empoweredWDamage;

        public static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Heimerdinger")
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 50);
            W = new Spell(SpellSlot.W, 1075);
            E = new Spell(SpellSlot.E, 920);
            R = new Spell(SpellSlot.R);

            W.SetSkillshot(.25f, 40, 2500, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(.25f, 120, 1000, false, SkillshotType.SkillshotCircle);

            Config = new Menu("HeisenDonger", "HeisenDonger", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));
            Config.AddSubMenu(new Menu("Logic+Spacebar=win :^)", "kappa"));
            Config.AddToMainMenu();   //who needs a menu when you're busy memeing :^)

            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static Obj_AI_Hero target = new Obj_AI_Hero();
        private static void Game_OnUpdate(EventArgs args)  
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) //because who needs farming logic on heimerdonger
            {
                target = TargetSelector.GetTarget(1275, TargetSelector.DamageType.Magical); //W range = 1325, but account for small delay in shooting rockets
                if (target != null && target.IsValid)
                {
                    ComboQ();
                    ComboW();
                    ComboE();
                }
            }
        }

        private static void ComboQ()
        {
            if (!Q.IsReady())
            {
                return;
            }

            if (R.IsReady() && ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) <= 400).Count() >
                2)  //teamfight going on
            {
                R.Cast();
                Q.Cast(Player.Position);
            }

            else if (ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) <= 400).Count() > 1)  //gank/small engage beginning?
            {
                Q.Cast(Player.Position);
            }

            else if (
                ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) <= 350).Count() ==
                1)   //target is going aggressive on you (withing 250 units)
            {
                Q.Cast(Player.Position);
            }
        }

        private static void ComboW()
        {
            if (!W.IsReady())
            {
                return;
            }

            if (R.IsReady() && target.Distance(Player.Position) < 1275 && ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) < 500).Count() < 3)  //don't want to use ult for W if there are a bunch of enemies nearby
            {
                calcEmpWDmg();
                if (empoweredWDamage > target.Health && Player.GetSpellDamage(target,SpellSlot.W) < target.Health)   //hopefully every rocket hits :^)
                {
                    R.Cast();
                    W.CastIfHitchanceEquals(target, HitChance.High);
                }
            }

            if (target.Distance(Player.Position) < 1275)  //harass them down
            {
                W.CastIfHitchanceEquals(target, HitChance.High);    
            }
        }

        private static void ComboE()
        {
            if (!E.IsReady())
            {
                return;
            }

            if (R.IsReady() &&
                ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) < 900).Count() > 2)
            {
                var counter = 0;
                foreach (var k in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.Distance(Player.Position) < 900))   //I'm sorry...
                {
                    foreach (
                        var j in
                            ObjectManager.Get<Obj_AI_Hero>().Where(x => x!=k && x.IsEnemy && x.Distance(Player.Position) < 900))
                    {
                        if (j.Distance(k.Position) < 420)  //diameter of empowered E
                        {
                            counter++;
                            foreach (
                                var o in
                                    ObjectManager.Get<Obj_AI_Hero>()
                                        .Where(x => x != j && x != k && x.IsEnemy && x.Distance(Player.Position) < 900))
                            {
                                if (o.Distance(j.Position) < 320) //pray they're in a line? --lower the diameter a bit to allow for some tolerance of linear collision of champions   
                                {
                                    counter++;
                                }
                            }
                        }
                    }

                    if (counter >= 3)
                    {
                        target = k;
                    }
                }

                if (counter >= 3)
                {
                    R.Cast();
                    E.CastIfHitchanceEquals(target, HitChance.High);
                }
            }

            if (target.Distance(Player.Position) < 920)
            {
                E.CastIfHitchanceEquals(target, HitChance.High);
            }
        }

        private static readonly int[] wDamage = new[] {0,500, 690, 865};  //assuming all rockets hit; enemies take reduced damage for each rocket that hits
        private static void calcEmpWDmg()
        {
            empoweredWDamage = wDamage[Player.Spellbook.Spells[3].Level]+(1.25*Player.TotalMagicalDamage());   //assuming almost all rockets hit (prediction pls)
            //Console.WriteLine(empoweredWDamage.ToString());
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser targ)
        {
            if (!E.IsReady())
            {
                return;
            }

            if (targ.Sender.IsValid && targ.Sender.Distance(Player.Position) < 920)
            {
                E.Cast(targ.End);
            }
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero targ, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!E.IsReady())
            {
                return;
            }

            if (targ.IsValid && targ.Distance(Player.Position) < 920)
            {
                E.Cast(targ.Position);
            }
        }
    }
}
