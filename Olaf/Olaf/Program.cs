using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Olaf
{
    class Program
    {
        
        public static Menu Menu;
        private static Obj_AI_Hero Player;
        public static List<Spell> SpellList = new List<Spell>();

        public static bool ComboActive;
        public static bool useQ;
        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static SpellSlot SumIgnite = ObjectManager.Player.GetSpellSlot("SummonerDot");
        public static void Main(string[] args)
        {
            Game.OnGameStart += Game_Start;
            if (Game.Mode == GameMode.Running)
            {
                Game_Start(new EventArgs());
            }
        }
            
        public static void Game_Start(EventArgs args)
        {
            Menu = new Menu("Olaf", "Olaf", true);

            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(TargetSelectorMenu);
            Menu.AddSubMenu(TargetSelectorMenu);


            Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));


            //------------Combo
            Menu.AddSubMenu(new Menu("Combo", "Combo"));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseQ", "Use Q").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseW", "Use W").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseE", "Use E").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseR", "Use R").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("Ignite", "Use Ignite").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("RandE", "Use R before E if killable").SetValue(false));

            Menu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            //-------------end Combo


            Menu.AddSubMenu(new Menu("Farm", "Farm"));
            Menu.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(false));
            Menu.SubMenu("Farm").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(false));
            Menu.SubMenu("Farm").AddItem(new MenuItem("FarmActive", "Farm!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));

            Menu.AddSubMenu(new Menu("Clear", "Clear"));
            Menu.SubMenu("Clear").AddItem(new MenuItem("UseQFarm2", "Use Q").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("UseEFarm2", "Use E").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("ClearActive", "Clear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            Menu.AddToMainMenu();

            Player = ObjectManager.Player;

            Q = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 325);
            R = new Spell(SpellSlot.R);

            Q.SetSkillshot(.25f, 50f, 1600, false, SkillshotType.SkillshotLine);

            Game.PrintChat("Olaf Loaded.");

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnUpdate;
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            List<Vector2> pos = new List<Vector2>();

            bool qFarm = Menu.Item("UseQFarm").GetValue<bool>();
            bool qFarm2 = Menu.Item("UseQFarm2").GetValue<bool>();
            var farmActive = Menu.Item("FarmActive").GetValue<KeyBind>().Active;
            var clearActive = Menu.Item("ClearActive").GetValue<KeyBind>().Active;
            bool ER = Menu.Item("RandE").GetValue<bool>();

            if ((qFarm && farmActive) || (qFarm2 && clearActive))
            {
                var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, 650, MinionTypes.All,
                    MinionTeam.NotAlly, MinionOrderTypes.Health);
                foreach (var minion in Minions)
                {
                    if (minion != null)
                    {
                        pos.Add(minion.Position.To2D());
                    }
                }

                var pred = LeagueSharp.Common.MinionManager.GetBestLineFarmLocation(pos, 50, 1000);
                if (pos.Any())
                {
                    Q.Cast(pred.Position);
                }
            }

            var comboActive = Menu.Item("ComboActive").GetValue<KeyBind>().Active;

            if ((!comboActive))
                return;

            var useR = Menu.Item("UseR").GetValue<bool>();

            if (isCCd.Class1.IsCCd(Player) && useR)
            {
                if(LeagueSharp.Common.Utility.CountEnemysInRange(1000) > 0)
                R.Cast();
            }

            var ignite = Menu.Item("Ignite").GetValue<bool>();
            var useQ5 = Menu.Item("UseQ").GetValue<bool>();
            var useW = Menu.Item("UseW").GetValue<bool>();
            var useE = Menu.Item("UseE").GetValue<bool>();


            if (ignite)
            {
                var t = SimpleTs.GetTarget(600, SimpleTs.DamageType.Physical);
                var igniteDmg = DamageLib.getDmg(t, DamageLib.SpellType.IGNITE);
                if (t != null && SumIgnite != SpellSlot.Unknown &&
                                ObjectManager.Player.SummonerSpellbook.CanUseSpell(SumIgnite) == SpellState.Ready)
                {
                    if (igniteDmg > t.Health)
                    {
                        ObjectManager.Player.SummonerSpellbook.CastSpell(SumIgnite, t);
                    }
                }
            }
            
            if (useQ5 && Q.IsReady())
            {
                //Game.PrintChat("inQ");
                var t = SimpleTs.GetTarget(1000, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    //PredictionInput p = new PredictionInput();
                    var g = Q.GetPrediction(t);
                    //var pred = Prediction.GetPrediction(
                        Q.Cast(g.CastPosition);
                }
            }

            


            /* expiremental */
            if (ER)
            {
                if (R.IsReady() && E.IsReady())
                {
                    var t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                    if (t.IsValidTarget())
                    {
                        var dmg = DamageLib.CalcPhysicalDmg(E.GetDamage(t), t);
                        if (dmg > t.Health)
                            return;
                        //var lvl = R.Level;
                        var rdmg = new int();

                        switch (R.Level)
                        {
                            case 1:
                                rdmg = 40;
                                break;
                            case 2:
                                rdmg = 60;
                                break;
                            case 3:
                                rdmg = 80;
                                break;
                        }

                        var eandrdmg = dmg + (rdmg*.40);

                        if (eandrdmg > t.Health)
                        {
                            R.Cast();
                            E.Cast(t);
                        }
                    }
                }
            }

            /* expiremental */



            /*
             * if(R.level == 1)
             * {
             * 
             * }
             * 
             * elseif(R.level == 2)
             * {
             * 
             * }
             * 
             * elseif(R.level == 3)
             * {
             * 
             * }
             * 
             */


            if (useW && W.IsReady())
            {
                var rTarget = SimpleTs.GetTarget(225, SimpleTs.DamageType.Physical);

                if (rTarget.IsValidTarget())
                {
                    W.Cast();
                }
            }

            if (useE && E.IsReady())
            {
                var t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    E.Cast(t);
                }
            }
            
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            useQ = Menu.Item("UseEFarm").GetValue<bool>();
            var clearActive = Menu.Item("ClearActive").GetValue<KeyBind>().Active;
            var useQ2 = Menu.Item("UseEFarm2").GetValue<bool>();
            var farmActive = Menu.Item("FarmActive").GetValue<KeyBind>().Active;
            if ((useQ && farmActive) || (useQ2 && clearActive))
            {
                var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, 1000, MinionTypes.All,
                    MinionTeam.NotAlly, MinionOrderTypes.Health);
                foreach (var minion in from minion in Minions where minion != null let targetEDam = DamageLib.getDmg(minion, DamageLib.SpellType.E, DamageLib.StageType.Default) where (minion.Health < targetEDam) select minion)
                {
                    Utility.DrawCircle(minion.ServerPosition, 150, System.Drawing.Color.Red);
                    if (minion.IsValidTarget(E.Range) && (E.IsReady()))
                    {
                        E.Cast(minion);
                    }
                }
            }
        }
    }
}

namespace isCCd
{
    public class Class1
    {

        public static bool IsCCd(Obj_AI_Hero hero)
        {
            var cc = new List<BuffType>
            {
                BuffType.Taunt,
                BuffType.Blind,
                BuffType.Charm,
                BuffType.Fear,
                BuffType.Polymorph,
                BuffType.Stun,
                BuffType.Silence,
                BuffType.Snare
            };

            return cc.Any(hero.HasBuffOfType);
        }
        /*
        public static bool isTaunted(Obj_AI_Hero hero)
        {
            if()
        }

         */
    }
}
