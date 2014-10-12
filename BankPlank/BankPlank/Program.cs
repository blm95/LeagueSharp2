using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace BankPlank
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

        //public static SpellSlot SumIgnite = ObjectManager.Player.GetSpellSlot("SummonerDot");
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_Start;
        }

        private static void Game_Start(EventArgs args)
        {
            Menu = new Menu("BankPlank", "BankPlank", true);

            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(TargetSelectorMenu);
            Menu.AddSubMenu(TargetSelectorMenu);


            Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));


            //------------Combo
            Menu.AddSubMenu(new Menu("Combo", "Combo"));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseQ", "Use Q").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("Ignite", "Use Ignite").SetValue(true));

            Menu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            //-------------end Combo

            Menu.AddSubMenu(new Menu("Misc", "Misc"));
            Menu.SubMenu("Misc").AddItem(new MenuItem("AutoW", "AutoW CC").SetValue(true));
            Menu.SubMenu("Misc").AddItem(new MenuItem("filler", "-------Auto W if HP < % hp-------"));
            Menu.SubMenu("Misc").AddItem(new MenuItem("AutoWhpbool", "AutoW based on low HP?").SetValue(true));
            Menu.SubMenu("Misc").AddItem(new MenuItem("AutoWhp", "AutoW if hp <").SetValue(new Slider(40, 100, 0)));

            Menu.AddSubMenu(new Menu("Farm", "Farm"));
            Menu.SubMenu("Farm").AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(false));
            Menu.SubMenu("Farm").AddItem(new MenuItem("FarmActive", "Farm!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));

            Menu.AddToMainMenu();

            Player = ObjectManager.Player;

            Q = new Spell(SpellSlot.Q, 625);
            W = new Spell(SpellSlot.W);

            Game.PrintChat("BankPlank Loaded.");

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var useW = Menu.Item("AutoW").GetValue<bool>();
            var useWhp = Menu.Item("AutoWhpbool").GetValue<bool>();

            if (isCCd.Class1.IsCCd(Player) && useW)
            {
                W.Cast();
            }

            if (useWhp && Menu.Item("AutoWhp").GetValue<Slider>().Value >= ((Player.Health / Player.MaxHealth) * 100))
            {
                W.Cast();
            }

            var comboActive = Menu.Item("ComboActive").GetValue<KeyBind>().Active;

            if ((!comboActive))
                return;

           // var ignite = Menu.Item("Ignite").GetValue<bool>();
            var useQ5 = Menu.Item("UseQ").GetValue<bool>();
            

            /*if (ignite)
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

             */
            if (useQ5 && Q.IsReady())
            {
                //Game.PrintChat("inQ");
                var t = SimpleTs.GetTarget(625, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    //PredictionInput p = new PredictionInput();

                    //var pred = Prediction.GetPrediction(
                    Q.Cast(t);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            useQ = Menu.Item("UseQFarm").GetValue<bool>();
            var farmActive = Menu.Item("FarmActive").GetValue<KeyBind>().Active;
            if ((useQ && farmActive))
            {
                
                var Minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, 1000, MinionTypes.All,
                    MinionTeam.NotAlly, MinionOrderTypes.Health);
                foreach (var minion in from minion in Minions where minion != null let targetQDam = Damage.GetDamageSpell(ObjectManager.Player,minion, SpellSlot.Q) where (minion.Health < targetQDam.CalculatedDamage) select minion)//DamageLib.getDmg(minion, DamageLib.SpellType.Q, DamageLib.StageType.Default) where (minion.Health < targetQDam) select minion)
                {
                    Utility.DrawCircle(minion.ServerPosition, 150, System.Drawing.Color.Red);
                    if (minion.IsValidTarget(Q.Range) && (Q.IsReady()))
                    {
                        Q.Cast(minion);
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
