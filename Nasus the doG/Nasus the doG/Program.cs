using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace nasus
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

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
            Menu = new Menu("Nasus", "Nasus", true);


            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(TargetSelectorMenu);
            Menu.AddSubMenu(TargetSelectorMenu);


            Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));

            Menu.AddSubMenu(new Menu("Combo", "Combo"));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseQ", "Use Q").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseW", "Use W").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseE", "Use E").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseR", "Use R").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("fill", "--- ULT OPTIONS ---"));
            Menu.SubMenu("Combo").AddItem(new MenuItem("CountR", "Min amount of enemies nearby").SetValue(new Slider(1, 5, 0)));
            Menu.SubMenu("Combo").AddItem(new MenuItem("HPForR", "Use if HP < X%").SetValue(new Slider(1, 100, 0)));
            Menu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu.AddSubMenu(new Menu("Farm", "Farm"));
            Menu.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(true));
            Menu.SubMenu("Farm").AddItem(new MenuItem("UseEFarm", "Use E").SetValue(true));
            Menu.SubMenu("Farm").AddItem(new MenuItem("FarmActive", "Farm!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));

            Menu.AddSubMenu(new Menu("Clear", "Clear"));
            Menu.SubMenu("Clear").AddItem(new MenuItem("UseQFarm2", "Use Q").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("UseEFarm2", "Use E").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("ClearActive", "Clear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            var mana = Menu.AddSubMenu(new Menu("Mana Limiter", "Mana Limiter"));
            mana.AddItem(new MenuItem("comboMana", "Combo Mana %").SetValue(new Slider(1, 100, 0)));
            mana.AddItem(new MenuItem("harassMana", "Harass Mana %").SetValue(new Slider(30, 100, 0)));



            var misc = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            var lel = misc.AddSubMenu((new Menu("Dont use W on", "DontW")));


            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
                lel.AddItem(new MenuItem("DontW" + enemy.BaseSkinName, enemy.BaseSkinName).SetValue(false));


            Menu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_Update;


            Player = ObjectManager.Player;

            Q = new Spell(SpellSlot.Q, 200);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 650);
            R = new Spell(SpellSlot.R, 1);
            E.SetSkillshot(0, 400, 5000, false, SkillshotType.SkillshotCircle);



            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Game.PrintChat("Nasus loaded.");
        }

        private static void Game_Update(EventArgs args)
        {
            List<Vector2> pos = new List<Vector2>();

            bool eFarm = Menu.Item("UseEFarm").GetValue<bool>();
            bool eFarm2 = Menu.Item("UseEFarm2").GetValue<bool>();
            var farmActive = Menu.Item("FarmActive").GetValue<KeyBind>().Active;
            var clearActive = Menu.Item("ClearActive").GetValue<KeyBind>().Active;
            if ((eFarm && farmActive) || (eFarm2 && clearActive))
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

                var pred = LeagueSharp.Common.MinionManager.GetBestCircularFarmLocation(pos, 400, 650);
                if (pos.Any())
                {
                    E.Cast(pred.Position);
                }
            }
            var comboActive = Menu.Item("ComboActive").GetValue<KeyBind>().Active;
         
            if ((!comboActive || (!Orbwalking.CanMove(100))))
                return;
            var useQ5 = Menu.Item("UseQ").GetValue<bool>();
            var useW = Menu.Item("UseW").GetValue<bool>();
            var useE = Menu.Item("UseE").GetValue<bool>();
            var useR = Menu.Item("UseR").GetValue<bool>();

            if (useW && W.IsReady())
            {
                var rTarget = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                if (rTarget != null)
                    useW = (Menu.Item("DontW" + rTarget.BaseSkinName) != null &&
                            Menu.Item("DontW" + rTarget.BaseSkinName).GetValue<bool>() == false);
                if (rTarget.IsValidTarget() && useW)
                {
                    W.CastOnUnit(rTarget);
                }
            }

            if (useQ5 && Q.IsReady())
            {
                var t = SimpleTs.GetTarget(300, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    Q.Cast();
                }
            }

            if (useE && E.IsReady())
            {
                var t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    var pred = E.GetPrediction(t);
                    E.Cast(pred.CastPosition);
                }
            }

            if (useR && R.IsReady() &&
                Menu.Item("CountR").GetValue<Slider>().Value >= LeagueSharp.Common.Utility.CountEnemysInRange(650) &&
                Menu.Item("HPForR").GetValue<Slider>().Value >= ((Player.Health / Player.MaxHealth) * 100))
            {
                R.Cast();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            useQ = Menu.Item("UseQFarm").GetValue<bool>();
            var clearActive = Menu.Item("ClearActive").GetValue<KeyBind>().Active;
            var useQ2 = Menu.Item("UseQFarm2").GetValue<bool>();
            var farmActive = Menu.Item("FarmActive").GetValue<KeyBind>().Active;
            if ((useQ && farmActive) || (useQ2 && clearActive))
            {
                var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, 1000, MinionTypes.All,
                    MinionTeam.NotAlly, MinionOrderTypes.Health);
                foreach (var minion in from minion in Minions where minion != null let targetQDam = Damage.GetDamageSpell(ObjectManager.Player, minion, SpellSlot.Q) where (minion.Health < targetQDam.CalculatedDamage) select minion)
                {
                    Q.Cast(minion);
                }
            }
        }

    }
}


