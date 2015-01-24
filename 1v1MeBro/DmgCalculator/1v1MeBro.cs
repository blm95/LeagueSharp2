using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using LeagueSharp;
//using LeagueSharp.Common;
//using dmgLib;
//using SharpDX.Direct3D9;
using LeagueSharp.Common;
using SharpDX;

namespace SpellNamesandSlots
{
    internal class Program
    {
        //public static string name;
        //public static SpellSlot slots;
        //public static List<spellData> spells = new List<spellData>();
        //public static List<string> names;
        //public static string champname;
        //public static Obj_AI_Base target;
        // public static Dictionary<string, SpellSlot> spellData;
        public static double alldmg;
        public static Menu menu;
        //private TargetSelector ts;
        public static List<Obj_AI_Hero> heroesinrange;
        public static double mydmg = new double();
        public static float longestspell = new float();
        public static float mylongestspell = new float();
        public static bool drawDanger = false;
        public static double soondmg = new double();
        public static double dmgnow = new double();
        public static Dictionary<Obj_AI_Hero, double> calcdmg = new Dictionary<Obj_AI_Hero, double>();
        public static Dictionary<Obj_AI_Hero, double> calcdmg2 = new Dictionary<Obj_AI_Hero, double>();
        public static double mycost = new double();
        public static double theircost = new double();


        private const int XOffset = 10;
        private const int YOffset = 20;
        private const int Width = 103;
        private const int Height = 8;

        private const int XOffset2 = 30;
        private const int YOffset2 = 8;
        private const int Width2 = 103;
        private const int Height2 = 9;
        private static readonly Render.Text Text = new Render.Text(
            0, 0, "", 11, new ColorBGRA(255, 0, 0, 255), "monospace");
        private static readonly Render.Text Text2 = new Render.Text(
            0, 0, "", 11, new ColorBGRA(255, 0, 0, 255), "monospace");

        //private static readonly List<Enemy> EnemyList = new List<Enemy>();

        //public class spellData
        //{
        //    public spellData()
        //    {
        //    }

        //    public spellData(string champName, SpellSlot slots)
        //    {
        //        name = champName;
        //        slot = slots;
        //    }
        //}

        //BetweenExtensions.

        private static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }


        }



        public static void Game_OnGameLoad(EventArgs args)
        {
            try
            {
                menu = new Menu("1v1 Me Bro", "dmg", true);
                menu.AddItem(
                    new MenuItem("range", "Range Check").SetValue(new Slider(1500, 300, 20000)));
                menu.AddItem(
                    new MenuItem("gthan2", "if > 2 enemies CD check (self)").SetValue(new Slider(2, 0, 8)));
                menu.AddItem(
                    new MenuItem("lthan2", "if <= 2 enemies CD check (self)").SetValue(new Slider(1, 0, 8)));
                menu.AddItem(
                    new MenuItem("theircd", "Their CD check if <= 2 enemies").SetValue(new Slider(4, 0, 10)));
                menu.AddItem(
                    new MenuItem("theircd2", "Their CD check (>2)").SetValue(new Slider(4, 0, 10)));
                menu.AddToMainMenu();
                //spellData = new Dictionary<string, SpellSlot>();
                // Game.PrintChat("started");

                //foreach (var n in ObjectManager.Get<Obj_AI_Hero>())
                ////.Where(n => n.IsEnemy)
                //{

                //    Game.PrintChat(n.ChampionName);

                //    switch (n.ChampionName)
                //    {

                //    }
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine("test: " + e.ToString());
            }

            //alldmg = new double();
            Game.PrintChat("started");
            heroesinrange = new List<Obj_AI_Hero>();
            Drawing.OnDraw += Game_OnGameUpdate;
            Drawing.OnEndScene += Drawing_OnEndScene;
            //LeagueSharp.Game.OnGameProcessPacket += RecvPkt;
            //Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;

        }

        private static void Drawing_OnEndScene(EventArgs args)
        {


        }

        //public static void RecvPkt(GamePacketEventArgs args)
        //{
        //    try
        //    {
        //        var stream = new MemoryStream(args.PacketData);
        //        using (var b = new BinaryReader(stream))
        //        {
        //            int pos = 0;
        //            var length = (int)b.BaseStream.Length;
        //            while (pos < length)
        //            {
        //                int v = b.ReadInt32();
        //                if (v == 195) //OLD 194
        //                {
        //                    byte[] h = b.ReadBytes(1);

        //                }
        //                pos += sizeof(int);
        //            }
        //        }
        //    }
        //    catch (EndOfStreamException)
        //    {
        //    }
        //}


        private static void Game_OnGameUpdate(EventArgs args)
        {
            bool green = false;
            bool orange = false;
            bool red = false;
            bool yellow = false;
            bool tgreen = false;
            bool tred = false;
            bool tyellow = false;
            //double aadmg = 0;
            //double aaspeed = 0;
            //Game.PrintChat("in");
            var gthan = menu.Item("gthan2").GetValue<Slider>().Value;
            var lthan = menu.Item("lthan2").GetValue<Slider>().Value;
            var theircd = menu.Item("theircd").GetValue<Slider>().Value;
            var theircd2 = menu.Item("theircd2").GetValue<Slider>().Value;
            var range = menu.Item("range").GetValue<Slider>().Value;
            //aaspeed = ObjectManager.Player.AttackSpeedMod + (1 * ObjectManager.Player.PercentAttackSpeedMod);
            //ObjectManager.Player.
            //Game.PrintChat("ASMod: {0} + Percent Mod: {1} + mult Mod: {2}", ObjectManager.Player.AttackSpeedMod, ObjectManager.Player.PercentAttackSpeedMod, ObjectManager.Player.PercentMultiplicativeAttackSpeedMod);
            //ObjectManager.Player.PercentMultiplicativeAttackSpeedMod
            //Game.PrintChat(range.ToString());
            foreach (
                Obj_AI_Hero h in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            h =>
                                h.IsEnemy && !h.IsDead && h.IsVisible &&
                                Vector3.Distance(ObjectManager.Player.Position, h.Position) <= range))
            {

                heroesinrange.Add(h);
                //Game.PrintChat("added {0}", h.ChampionName);
            }

            foreach (var k in heroesinrange)
            {
                //aadmg = DamageLib.CalcPhysicalDmg(
                //ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod, k);
                //aaspeed = ObjectManager.Player.AttackSpeedMod + (1*ObjectManager.Player.PercentAttackSpeedMod);
                if (heroesinrange.Count() <= 2)
                {
                    foreach (var h in k.GetWaypoints())
                    {
                        if (Vector3.Distance(h.To3D(), ObjectManager.Player.Position) <= 300)
                        {
                            //var q = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            //LeagueSharp.Common.Packet.S2C.Ping.Encoded(new Packet.S2C.Ping.Struct(q[0], q[1], 0,
                            //    ObjectManager.Player.NetworkId, Packet.PingType.Danger));
                            //Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.Orange);
                            orange = true;
                        }
                    }
                }
                //Game.PrintChat(h.ChampionName);
                var spells = k.Spellbook.Spells;
                foreach (var c in spells.Where(c => c.State != SpellState.NotLearned))
                {
                    theircost += c.ManaCost;
                    if (k.Mana >= theircost)
                    {
                        if (c.CooldownExpires - Game.Time <= theircd)
                        {
                            if (c.Cooldown < theircd)
                            {
                                alldmg += Damage.GetSpellDamage(k, ObjectManager.Player, c.Slot);
                                //* (theircd / c.Cooldown);
                            }
                            //if (c.SData.CastRange[0] > longestspell)
                            //{
                            //    longestspell = c.SData.CastRange[0];
                            //}
                            else
                            {
                                // alldmg += dmgLib2.Class1.calcDmg(k, c.Slot, ObjectManager.Player);
                                alldmg += k.GetSpellDamage(ObjectManager.Player, c.Slot);
                            }

                        }

                        if (c.CooldownExpires - Game.Time <= theircd2)
                        {
                            if (c.Cooldown < theircd2)
                            {
                                soondmg += k.GetSpellDamage(ObjectManager.Player, c.Slot);
                                //*(theircd2 / c.Cooldown);
                            }

                            else
                            {
                                soondmg += k.GetSpellDamage(ObjectManager.Player, c.Slot);
                            }
                        }
                    }
                }
                foreach (var c in ObjectManager.Player.Spellbook.Spells.Where(c => c.State != SpellState.NotLearned))
                {
                    mycost += c.ManaCost;
                    //Game.PrintChat("{0} counted", c.Slot);
                    if (!(ObjectManager.Player.Mana >= mycost)) continue;
                    if (c.CooldownExpires - Game.Time <= lthan)
                    {
                        if (c.Cooldown < lthan)
                        {
                            mydmg += ObjectManager.Player.GetSpellDamage(k, c.Slot);
                            //* (lthan / c.Cooldown);
                            //mydmg += dmgLib2.Class1.calcDmg(ObjectManager.Player, c.Slot, k) * (lthan / c.Cooldown);
                        }
                        //if (c.SData.CastRange[0] > mylongestspell)
                        //{
                        //    mylongestspell = c.SData.CastRange[0];
                        //}
                        else
                        {
                            mydmg += ObjectManager.Player.GetSpellDamage(k, c.Slot);
                        }
                        //Game.PrintChat("MyDmg from {0} = {1}", c.Slot, dmgLib2.Class1.calcDmg(ObjectManager.Player, c.Slot, h));

                        //Game.PrintChat("my dmg: {0} vs: {1}", mydmg, h.ChampionName);
                    }

                    if (!(c.CooldownExpires - Game.Time <= gthan)) continue;
                    if (c.Cooldown < gthan)
                    {
                        dmgnow += ObjectManager.Player.GetSpellDamage(k, c.Slot);
                        //* (gthan / c.Cooldown);
                    }
                    else
                    {
                        dmgnow += ObjectManager.Player.GetSpellDamage(k, c.Slot);
                    }
                }

                //if (longestspell >= Vector3.Distance(ObjectManager.Player.Position, k.Position))
                //{
                //    drawDanger = true;
                //}
                calcdmg.Add(k, mydmg);
                calcdmg2.Add(k, dmgnow);
                mydmg = 0;
                mycost = 0;
                dmgnow = 0;
                theircost = 0;
                soondmg = 0;
            }

            //if (drawDanger)
            //{
            //    Utility.DrawCircle(ObjectManager.Player.Position, longestspell, System.Drawing.Color.Red);
            //}

            //else
            //{
            //Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.Green);
            //}

            foreach (var c in calcdmg)
            {
                //Game.PrintChat("dmg taken: {0}", alldmg);
                //Game.PrintChat("my 2 dmg: {0} vs: {1}", c.Value, c.Key.ChampionName);
                //Game.PrintChat("in");
                if (alldmg < ObjectManager.Player.Health)
                {
                    var u = Drawing.WorldToScreen(c.Key.Position);

                    if (heroesinrange.Count <= 2)
                    {
                        if (c.Key.Health <= c.Value)
                        {

                            // Drawing.DrawText(u[0], u[1], System.Drawing.Color.Red, "He is Killable!");
                            //Drawing.DrawCircle(c.Key.Position, 150, System.Drawing.Color.Red);
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Green);
                            green = true;
                            var y = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            Drawing.DrawText(y[0], y[1], System.Drawing.Color.LimeGreen, "{0} is killable",
                                c.Key.ChampionName);
                            //Drawing.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Red);
                            tred = true;
                            //break;
                        }

                        if (c.Value > alldmg)
                        {
                            //Game.PrintChat("I win");
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Green);
                            green = true;
                            //Drawing.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange,
                            // System.Drawing.Color.Yellow);
                            tyellow = true;
                            //break;
                        }

                        if (c.Value < alldmg)
                        {
                            //Game.PrintChat("they win");
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //   System.Drawing.Color.Yellow);
                            yellow = true;
                            //Drawing.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Green);
                            tgreen = true;
                            //break;
                        }
                    }

                    else
                    {
                        foreach (var g in calcdmg2.Where(g => g.Value > g.Key.Health))
                        {
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Green);
                            green = true;
                            var y = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            Drawing.DrawText(y[0], y[1], System.Drawing.Color.LimeGreen, "{0} is killable",
                                g.Key.ChampionName);
                            //Utility.DrawCircle(g.Key.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Red);
                            tred = true;
                            //break;
                        }
                    }



                    //Drawing.DrawText(u[0], u[1], System.Drawing.Color.SteelBlue, "After Combo: {0}", (int)(c.Key.Health - c.Value));
                }

                else
                {
                    //Game.PrintChat("im gonna die");
                    if (heroesinrange.Count >= 2)
                    {
                        if (soondmg > ObjectManager.Player.Health)
                        {
                            //var q = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            // Drawing.DrawText(q[0], q[1], System.Drawing.Color.Red, "Don't go in; you will die.");
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Red);
                            red = true;
                            //break;
                        }
                    }

                    else
                    {
                        if (alldmg > ObjectManager.Player.Health)
                        {
                            //var q = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            // Drawing.DrawText(q[0], q[1], System.Drawing.Color.Red, "Don't go in; you will die.");
                            //Drawing.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                            //System.Drawing.Color.Red);
                            red = true;
                            //break;
                        }
                    }
                }
                //hpi.unit = c.Key;
                //hpi.drawDmg(mydmg,System.Drawing.Color.Red);

                var barPos = c.Key.HPBarPosition;
                var damage = c.Value;
                var percentHealthAfterDamage = (c.Key.Health - damage) / c.Key.MaxHealth;
                var xPos = (float)(barPos.X + XOffset + Width * percentHealthAfterDamage);

                var mybarpos = ObjectManager.Player.HPBarPosition;
                var dmg = alldmg;
                var percentHealthAfterDamage2 = (ObjectManager.Player.Health - dmg) / ObjectManager.Player.MaxHealth;
                var xPos2 = (float)(mybarpos.X + XOffset2 + Width2 * percentHealthAfterDamage2);
                //Game.PrintChat(percentHealthAfterDamage2.ToString());
                if (damage > c.Key.Health)
                {
                    Text.X = (int)barPos.X + XOffset;
                    Text.Y = (int)barPos.Y + YOffset - 13;
                    Text.text = ((int)(c.Key.Health - damage)).ToString();
                    Text.OnEndScene();
                }

                if (dmg > ObjectManager.Player.Health)
                {
                    Text2.X = (int)mybarpos.X + XOffset2;
                    Text2.Y = (int)mybarpos.Y + YOffset2 - 13;
                    Text2.text = ((int)(ObjectManager.Player.Health - dmg)).ToString();
                    Text2.OnEndScene();
                }
                Drawing.DrawLine(xPos, barPos.Y + YOffset, xPos, barPos.Y + YOffset + Height, 2, System.Drawing.Color.Yellow);
                Drawing.DrawLine(xPos2, mybarpos.Y + YOffset2, xPos2, mybarpos.Y + YOffset2 + Height2, 2, System.Drawing.Color.Red );

                if (tred)
                {

                    Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.Red);
                    Utility.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.Red);
                    Utility.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.Red);

                    tyellow = false;
                    tgreen = false;
                }
                if (tgreen)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.LimeGreen);
                    tyellow = false;
                }
                if (tyellow)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.Yellow);
                }
                if (orange)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.OrangeRed);
                }
                if (red)
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.Red);
                    Utility.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange,
                        System.Drawing.Color.Red);

                    green = false;
                    yellow = false;
                }
                if (yellow)
                {
                    Utility.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.Yellow);
                    green = false;
                }
                if (green)
                {
                    Utility.DrawCircle(c.Key.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.Green);
                }


            }
            calcdmg2.Clear();
            heroesinrange.Clear();
            calcdmg.Clear();
            mycost = 0;
            theircost = 0;
            soondmg = 0;
            dmgnow = 0;
            longestspell = 0;
            mylongestspell = 0;
            alldmg = 0;
            mydmg = 0;

            //public static double alldmg;
            // public static Menu menu;
            //private TargetSelector ts;
            //public static List<Obj_AI_Hero> heroesinrange; 
            //public static double mydmg = new double();
            //public static float longestspell = new float();
            //public static float mylongestspell = new float();
            //public static bool drawDanger = false;
            //public static double soondmg = new double();
            //public static double dmgnow = new double();
            //public static Dictionary<Obj_AI_Hero, double> calcdmg = new Dictionary<Obj_AI_Hero, double>();
            //public static double mycost = new double();
            //public static double theircost = new double();
        }
    }
}