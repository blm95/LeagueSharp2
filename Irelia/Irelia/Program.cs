using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Irelia
{
    class Program
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;
        private static Obj_AI_Hero target;

        private const int XOffset = 10;
        private const int YOffset = 20;
        private const int Width = 103;
        private const int Height = 8;

        private static readonly Render.Text Text = new Render.Text(
            0, 0, "", 11, new ColorBGRA(255, 0, 0, 255), "monospace");

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Irelia")
                return;

            Q = new Spell(SpellSlot.Q, 650f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 425f);
            R = new Spell(SpellSlot.R, 1000f);

            R.SetSkillshot(.25f, 65f, 1000f, false, SkillshotType.SkillshotLine);

            Menu = new Menu(Player.ChampionName, Player.ChampionName, true);
            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);
            Menu spellMenu = Menu.AddSubMenu(new Menu("Spells", "Spells"));

            spellMenu.AddItem(new MenuItem("useQ", "use Q").SetValue(true));
            spellMenu.AddItem(
                new MenuItem("useQdistance", "Use Q if enemy > X distance").SetValue(new Slider(450, 0, 650)));
            spellMenu.AddItem(new MenuItem("useQchase", "Use Q to chase?").SetValue(true));
            spellMenu.AddItem(new MenuItem("qchasekey", "Key to chase").SetValue(new KeyBind('G', KeyBindType.Press)));
            spellMenu.AddItem(new MenuItem("useQflee", "Use Q to flee?").SetValue(true));
            spellMenu.AddItem(new MenuItem("qfleekey", "Key to flee").SetValue(new KeyBind('Z', KeyBindType.Press)));
            spellMenu.AddItem(new MenuItem("useW", "use W").SetValue(true));
            spellMenu.AddItem(new MenuItem("useWhp", "Use W at % hp").SetValue(new Slider(70, 0, 100)));
            spellMenu.AddItem(new MenuItem("useE", "use E").SetValue(true));
            spellMenu.AddItem(new MenuItem("useEstunonly", "Only use E to stun?").SetValue(false));
            spellMenu.AddItem(new MenuItem("useEflee", "Use E to flee?").SetValue(true));
            spellMenu.AddItem(new MenuItem("useR", "use R").SetValue(true));
            spellMenu.AddItem(new MenuItem("useRhp", "Use R at % hp").SetValue(new Slider(70, 0, 100)));
            spellMenu.AddItem(new MenuItem("useRdistance", "Use R if target >= X distance away").SetValue(new Slider(500, 0, 1000)));
            spellMenu.AddItem(new MenuItem("useRnologic", "Use R in combo no matter what").SetValue(false));

            Menu Drawings = Menu.AddSubMenu(new Menu("Drawings", "Drawings"));
            Drawings.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            Drawings.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            Drawings.AddItem(new MenuItem("drawR", "Draw R").SetValue(true));

            Menu Farm = Menu.AddSubMenu(new Menu("Farming", "Farming"));
            Farm.AddItem(new MenuItem("qFarm", "Use Q for last hitting").SetValue(false));
            Farm.AddItem(new MenuItem("qFarmMana", "Min % mana to lasthit with Q").SetValue(new Slider(60, 1, 100)));
            Farm.AddItem(new MenuItem("qClear", "Use Q for laneclear").SetValue(true));
            Farm.AddItem(new MenuItem("qClearMana", "Min % mana to laneclear with Q").SetValue(new Slider(30, 1, 100)));

            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Drawing.OnDraw += Drawing_OnDraw;

            var lel = Menu.AddSubMenu((new Menu("Dont use E on", "DontE")));

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
                lel.AddItem(new MenuItem("DontE" + enemy.ChampionName, enemy.ChampionName).SetValue(false));

            Menu.AddToMainMenu();
            Game.OnUpdate += Game_OnUpdate;
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && sender.Distance(Player) <= E.Range && sender.HealthPercent >= Player.HealthPercent)
            {
                E.Cast(sender);
            }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (Menu.Item("drawQ").GetValue<bool>())
            {
                if (Q.IsReady())
                {
                    Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Green);
                }
                else
                {
                    Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Red); 
                }
            }
            if (Menu.Item("drawE").GetValue<bool>())
            {
                if (E.IsReady())
                {
                    Drawing.DrawCircle(Player.Position, E.Range, System.Drawing.Color.Green);
                }
                else
                {
                    Drawing.DrawCircle(Player.Position, E.Range, System.Drawing.Color.Red);
                }
            }
            if (Menu.Item("drawR").GetValue<bool>())
            {
                if (R.IsReady())
                {
                    Drawing.DrawCircle(Player.Position, R.Range, System.Drawing.Color.Green);
                }
                else
                {
                    Drawing.DrawCircle(Player.Position, R.Range, System.Drawing.Color.Red);
                }
            }

            if (target != null)
            {
                var barPos = target.HPBarPosition;
                double damage = Player.CalcDamage(target, Damage.DamageType.Physical, Player.TotalAttackDamage*3);

                if (Q.IsReady())
                {
                    damage += Player.GetSpellDamage(target, SpellSlot.Q);
                }
                if (W.IsReady())
                {
                    damage += Player.GetSpellDamage(target, SpellSlot.W)*3;
                }
                if (E.IsReady())
                {
                    damage += Player.GetSpellDamage(target, SpellSlot.E);
                }
                if (R.IsReady())
                {
                    damage += Player.GetSpellDamage(target, SpellSlot.R)*3;
                }

                var percentHealthAfterDamage = (target.Health - damage)/target.MaxHealth;
                var xPos = (float) (barPos.X + XOffset + Width*percentHealthAfterDamage);

                if (damage > target.Health)
                {
                    Text.X = (int) barPos.X + XOffset;
                    Text.Y = (int) barPos.Y + YOffset - 13;
                    Text.text = ((int) (target.Health - damage)).ToString();
                    Text.OnEndScene();
                }

                Drawing.DrawLine(xPos, barPos.Y + YOffset, xPos, barPos.Y + YOffset + Height, 2,
                    System.Drawing.Color.Yellow);
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Menu.Item("useQflee").GetValue<bool>() && Menu.Item("qfleekey").GetValue<KeyBind>().Active)
            {
                Flee();
            }

            target = TargetSelector.GetTarget(1800f, TargetSelector.DamageType.Physical);

            if (Menu.Item("useQchase").GetValue<bool>() && Menu.Item("qchasekey").GetValue<KeyBind>().Active)
            {
                Chase();
            }

            if ((Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit && Menu.Item("qFarm").GetValue<bool>() && Player.ManaPercent >= Menu.Item("qFarmMana").GetValue<Slider>().Value) || (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && Menu.Item("qClear").GetValue<bool>() && Player.ManaPercent >= Menu.Item("qClearMana").GetValue<Slider>().Value))
            {
                qFarm();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target != null && target.IsValid)
            {
                UseQ();
                UseW();
                UseE();
                UseR();
            }
        }

        private static void Flee()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (Q.IsReady())
            {
                //var distance = 1000f;
                //Obj_AI_Minion preferredMinion = null;
                //foreach (var g in ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Distance(Player) <= Q.Range))
                //{
                //    var miniondist = g.Distance(g);
                //    if (miniondist < distance && Player.GetSpellDamage(g, SpellSlot.Q) >= g.Health)
                //    {
                //        distance = miniondist;
                //        preferredMinion = g;
                //    }
                //}
                //if (preferredMinion != null && preferredMinion.IsValid)
                //{
                //    Q.Cast(preferredMinion);
                //}
                //else
                //{
                    var k =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(x => Player.IsFacing(x) && x.IsEnemy && x.Distance(Player) <= Q.Range)
                            .OrderByDescending(x => x.Distance(Player))
                            .First();

                    if (k.IsValid)
                    {
                        Q.Cast(k);
                    }
                //}
            }
            
            if (E.IsReady() && Menu.Item("useEflee").GetValue<bool>())
            {
                var j = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.Distance(Player) <= E.Range).First();
                if (j != null && j.IsValid)
                {
                    E.Cast(j);
                }
            }
        }

        private static void Chase()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (Q.IsReady())
            {  
                var distance = 1000f;
                Obj_AI_Minion preferredMinion = null;
                foreach (
                    var k in ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.Distance(Player) <= Q.Range))
                {
                    var miniondist = k.Distance(target);
                    if (miniondist < distance)
                    {
                        distance = miniondist;
                        preferredMinion = k;
                    }
                }
                Q.Cast(preferredMinion);
            }
        }

        private static void qFarm()
        {
            if (Q.IsReady())
            {
                foreach (
                    var k in
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(x => x.IsEnemy && x.Distance(Player) <= Q.Range))
                {
                    if (k.Health <= Player.GetSpellDamage(k, SpellSlot.Q))
                    {
                        Q.Cast(k);
                    }
                }
            }
        }

        private static void UseQ()
        {
            if (Q.IsReady())
            {
                if (Player.Distance(target) <= Q.Range)
                {
                    if ((Menu.Item("useQ").GetValue<bool>() &&
                         Player.Distance(target) >= Menu.Item("useQdistance").GetValue<Slider>().Value ||
                         Player.GetSpellDamage(target, SpellSlot.Q) >= target.Health))
                    {
                        Q.Cast(target);
                    }
                }
                else
                {
                    var distance = 1000f;
                    Obj_AI_Minion preferredMinion = null;
                    foreach (var k in ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Distance(Player) <= Q.Range))
                    {
                        var miniondist = k.Distance(target);
                        if (miniondist < distance && Player.GetSpellDamage(k,SpellSlot.Q) >= k.Health)
                        {
                            distance = miniondist;
                            preferredMinion = k;
                        } 
                    }
                    if (preferredMinion != null)
                    {
                        Q.Cast(preferredMinion);
                    }
                }
            }
        }
        private static void UseW()
        {
            if (W.IsReady())
            {
                if (Menu.Item("useW").GetValue<bool>() &&
                    Player.HealthPercent <= Menu.Item("useWhp").GetValue<Slider>().Value && Player.Distance(target) <= 250)
                {
                    W.Cast();
                }
            }
        }
        private static void UseE()
        {
            if (E.IsReady())
            {
                if (Menu.Item("useE").GetValue<bool>() || Player.GetSpellDamage(target, SpellSlot.E) >= target.Health)
                {
                    if (Menu.Item("useEstunonly").GetValue<bool>())
                    {
                        if (Player.HealthPercent <= target.HealthPercent)
                        {
                            var useR = (Menu.Item("DontE" + target.ChampionName) != null &&
                            Menu.Item("DontE" + target.ChampionName).GetValue<bool>() == false);
                            if (useR)
                            {
                                E.Cast(target);
                            }
                        }
                    }
                    else
                    {
                        var useR = (Menu.Item("DontE" + target.ChampionName) != null &&
                            Menu.Item("DontE" + target.ChampionName).GetValue<bool>() == false);
                        if (useR)
                        {
                            E.Cast(target);
                        }
                    }
                }
            }
        }
        private static void UseR()
        {
            if (R.IsReady())
            {
                if (Menu.Item("useR").GetValue<bool>())
                {
                    if ((Player.HealthPercent <= Menu.Item("useRhp").GetValue<Slider>().Value &&
                        Player.Distance(target) >= Menu.Item("useRdistance").GetValue<Slider>().Value) || Player.GetSpellDamage(target, SpellSlot.R) * 3 >= target.Health || Menu.Item("useRnologic").GetValue<bool>())
                    {
                        R.Cast(target);
                    }
                }
            }
        }
    }
}
