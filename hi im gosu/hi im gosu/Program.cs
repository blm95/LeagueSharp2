#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace hi_im_gosu
{
    class Vayne
    {
        public static Spell E;
        public static Spell Q;
        public static Orbwalking.Orbwalker orbwalker;
        public static Menu menu;
        public static Dictionary<string, SpellSlot> spellData;
        public static string[] interrupt;
        public static string[] notarget;
        public static string[] gapcloser;
        public static Obj_AI_Hero tar;
        public const string ChampName = "Vayne";
        public static Obj_AI_Hero Player;
        private static BuffType[] buffs;
        private static Spell cleanse;
        private static Menu Itemsmenu;
        private static Menu botrk;
        private static Menu qss;

        /* Asuna VayneHunter Copypasta */
        private static readonly Vector2 midPos = new Vector2(6707.485f, 8802.744f);
        private static readonly Vector2 dragPos = new Vector2(11514, 4462);
        private static float LastMoveC;

        private static void TumbleHandler()
        {
            if (Player.Distance(midPos) >= Player.Distance(dragPos))
            {
                if (Player.Position.X < 12000 || Player.Position.X > 12070 || Player.Position.Y < 4800 ||
                Player.Position.Y > 4872)
                {
                    MoveToLimited(new Vector2(12050, 4827).To3D());
                }
                else
                {
                    MoveToLimited(new Vector2(12050, 4827).To3D());
                    Q.Cast(dragPos, true);
                }
            }
            else
            {
                if (Player.Position.X < 6908 || Player.Position.X > 6978 || Player.Position.Y < 8917 ||
                Player.Position.Y > 8989)
                {
                    MoveToLimited(new Vector2(6958, 8944).To3D());
                }
                else
                {
                    MoveToLimited(new Vector2(6958, 8944).To3D());
                    Q.Cast(midPos, true);
                }
            }
        }
        private static void MoveToLimited(Vector3 where)
        {
            if (Environment.TickCount - LastMoveC < 80)
            {
                return;
            }
            LastMoveC = Environment.TickCount;
            Player.IssueOrder(GameObjectOrder.MoveTo, where);
        }
        /* End Asuna VayneHunter Copypasta */

        static void Main(string[] args)
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
            Player = ObjectManager.Player;
            //Utils.PrintMessage("Vayne loaded");
            if (Player.ChampionName != ChampName) return;
            spellData = new Dictionary<string, SpellSlot>();
            //Game.PrintChat("Riven");
            menu = new Menu("Gosu", "Gosu", true);
            //Orbwalker
            menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            orbwalker = new Orbwalking.Orbwalker(menu.SubMenu("Orbwalker"));
            //TS
            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            // SimpleTs.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(TargetSelectorMenu);

            menu.AddItem(
                new MenuItem("aaqaa", "Auto -> Q -> AA").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));
            //menu.AddSubMenu(new Menu("Combo", "combo"));
            //menu.SubMenu("combo").AddItem(new MenuItem("laugh", "Cancel w/ Laugh")).SetValue(false);
            menu.AddItem(
                new MenuItem("UseET", "Use E (Toggle)").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            menu.AddItem(new MenuItem("UseEInterrupt", "Use E To Interrupt").SetValue(true));
            menu.AddItem(
                new MenuItem("PushDistance", "E Push Distance").SetValue(new Slider(425, 475, 300)));
            menu.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            menu.AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            menu.AddItem(
                new MenuItem("UseEaa", "Use E after auto").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Toggle)));
            menu.AddSubMenu(new Menu("Gapcloser List", "gap"));
            menu.AddSubMenu(new Menu("Gapcloser List 2", "gap2"));
            menu.AddSubMenu(new Menu("Interrupt List", "int"));
            menu.AddItem(new MenuItem("restrictq", "Restrict Q usage?").SetValue(true));

            menu.AddItem(new MenuItem("walltumble", "Wall Tumble"))
                .SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Press));

            Itemsmenu = menu.AddSubMenu(new Menu("Items", "Items"));
            Itemsmenu.AddItem(new MenuItem("BOTRK", "Use BOTRK").SetValue(true));
            botrk = Itemsmenu.AddSubMenu(new Menu("BOTRK Settings", "usebotrk"));
            botrk.AddItem(new MenuItem("myhp", "Use if my HP < %")).SetValue(new Slider(20, 0, 100));
            botrk.AddItem(new MenuItem("theirhp", "Use if enemy HP < %")).SetValue(new Slider(20, 0, 100));
            Itemsmenu.AddItem(new MenuItem("Ghostblade", "Use Ghostblade").SetValue(true));
            Itemsmenu.AddItem(new MenuItem("QSS", "Use QSS/Merc Scimitar/Cleanse").SetValue(true));
            qss = menu.SubMenu("Items").AddSubMenu(new Menu("QSS/Merc/Cleanse Settings", "useqss"));

            buffs = new[]
            {
                BuffType.Blind, BuffType.Charm, BuffType.CombatDehancer, BuffType.Fear, BuffType.Flee, BuffType.Knockback,
                BuffType.Knockup, BuffType.Polymorph, BuffType.Silence, BuffType.Sleep, BuffType.Snare, BuffType.Stun,
                BuffType.Suppression, BuffType.Taunt
            };

            for (int i=0; i < buffs.Length;i++)
            {
               qss.AddItem(new MenuItem(buffs[i].ToString(), buffs[i].ToString()).SetValue(true));
            }
            
            Q = new Spell(SpellSlot.Q, 0f);
            E = new Spell(SpellSlot.E, float.MaxValue);

            gapcloser = new[]
            {
                "AkaliShadowDance", "Headbutt", "DianaTeleport", "IreliaGatotsu", "JaxLeapStrike", "JayceToTheSkies",
                "MaokaiUnstableGrowth", "MonkeyKingNimbus", "Pantheon_LeapBash", "PoppyHeroicCharge", "QuinnE",
                "XenZhaoSweep", "blindmonkqtwo", "FizzPiercingStrike", "RengarLeap"
            };
            notarget = new[]
            {
                "AatroxQ", "GragasE", "GravesMove", "HecarimUlt", "JarvanIVDragonStrike", "JarvanIVCataclysm", "KhazixE",
                "khazixelong", "LeblancSlide", "LeblancSlideM", "LeonaZenithBlade", "UFSlash", "RenektonSliceAndDice",
                "SejuaniArcticAssault", "ShenShadowDash", "RocketJump", "slashCast"
            };
            interrupt = new[]
            {
                "KatarinaR", "GalioIdolOfDurand", "Crowstorm", "Drain", "AbsoluteZero", "ShenStandUnited", "UrgotSwap2",
                "AlZaharNetherGrasp", "FallenOne", "Pantheon_GrandSkyfall_Jump", "VarusQ", "CaitlynAceintheHole",
                "MissFortuneBulletTime", "InfiniteDuress", "LucianR"
            };
            for (int i = 0; i < gapcloser.Length; i++)
            {
                menu.SubMenu("gap").AddItem(new MenuItem(gapcloser[i], gapcloser[i])).SetValue(true);
            }
            for (int i = 0; i < notarget.Length; i++)
            {
                menu.SubMenu("gap2").AddItem(new MenuItem(notarget[i], notarget[i])).SetValue(true);
            }
            for (int i = 0; i < interrupt.Length; i++)
            {
                menu.SubMenu("int").AddItem(new MenuItem(interrupt[i], interrupt[i])).SetValue(true);
            }

            var cde = ObjectManager.Player.Spellbook.GetSpell(ObjectManager.Player.GetSpellSlot("SummonerBoost"));
            if (cde != null)
            {
                if (cde.Slot != SpellSlot.Unknown)  //trees
                {
                    cleanse = new Spell(cde.Slot);
                }
            }
            menu.AddSubMenu(new Menu("Harass Options", "harass"));
            menu.SubMenu("harass").AddItem(new MenuItem("hq", "Use Q Harass").SetValue(true));
            menu.SubMenu("harass").AddItem(new MenuItem("he", "Use E Harass").SetValue(true));
            E.SetTargetted(0.25f, 2200f);
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
            Game.OnUpdate += Game_OnGameUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            menu.AddToMainMenu();
        }

        public static void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            //Game.PrintChat(args.SData.Name);
            if (menu.Item("UseEInterrupt").GetValue<bool>()  && interrupt.Any(x=>x.Contains(args.SData.Name)) &&
                menu.Item(args.SData.Name).GetValue<bool>() && Player.Distance(hero) <= 550)
            {
               // Game.PrintChat("in");
                //if (interrupt.Any(str => str.Contains(args.SData.Name)))
                //{
                  //  Game.PrintChat("in2");
                    E.Cast(hero);
                //}
            }

            if (gapcloser.Any(str => str.Contains(args.SData.Name)) && menu.Item(args.SData.Name).GetValue<bool>() && args.Target.IsMe)
            {
                E.Cast(hero);
            }

            if (notarget.Any(str => str.Contains(args.SData.Name)) &&
                Vector3.Distance(args.End, ObjectManager.Player.Position) <= 300 && hero.IsValidTarget(550f) &&
                menu.Item(args.SData.Name).GetValue<bool>())
            {
                if(!args.SData.Name == "GravesMove")
                {
                    E.Cast(hero);
                }
                else
                {
                    if(ObjectManager.Player.Distance(args.End) < ObjectManager.Player.Distance(hero.Position))
                    {
                        E.Cast(hero);
                    }
                }
            }

            if (args.SData.Name.ToLower() == "zedult" && args.Target.IsMe)
            {
                if (Items.CanUseItem(3140))
                {
                    Utility.DelayAction.Add(1000, () => Items.UseItem(3140));
                }
                else if (Items.CanUseItem(3139))
                {
                    Utility.DelayAction.Add(1000, () => Items.UseItem(3139));
                }
                else if (cleanse != null && cleanse.IsReady())
                {
                    Utility.DelayAction.Add(1000, () => cleanse.Cast());
                }
            }
        }

        public static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe) return;
            if (!(target is Obj_AI_Hero)) return;

            tar = (Obj_AI_Hero)target;

            if (menu.Item("aaqaa").GetValue<KeyBind>().Active)
            {
                if (Q.IsReady())
                {
                    Q.Cast(Game.CursorPos);
                }
                Orbwalking.Orbwalk(TargetSelector.GetTarget(625, TargetSelector.DamageType.Physical), Game.CursorPos);
            }
            if (Itemsmenu.Item("BOTRK").GetValue<bool>() && ((tar.Health / tar.MaxHealth) < botrk.Item("theirhp").GetValue<Slider>().Value) && ((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) < botrk.Item("myhp").GetValue<Slider>().Value))
            {
                //Game.PrintChat("in");
                if (Items.CanUseItem(3153))
                {
                    Items.UseItem(3153, tar);
                }
            }

            if (Itemsmenu.Item("Ghostblade").GetValue<bool>())
            {
                if (Items.CanUseItem(3142))
                {
                    Items.UseItem(3142);
                }
            }
            
            if (menu.Item("UseEaa").GetValue<KeyBind>().Active)
            {
                E.Cast((Obj_AI_Base)target);
                menu.Item("UseEaa").SetValue<KeyBind>(new KeyBind("G".ToCharArray()[0], KeyBindType.Toggle));
            }

            if (((orbwalker.ActiveMode.ToString() != "Combo" || !menu.Item("UseQC").GetValue<bool>()) &&
                 ((orbwalker.ActiveMode.ToString() != "Mixed" || !menu.Item("hq").GetValue<bool>()) || !Q.IsReady())))
                return;

            if (menu.Item("restrictq").GetValue<bool>())
            {
                var after = ObjectManager.Player.Position +
                            Normalize(Game.CursorPos - ObjectManager.Player.Position)*300;
                //Game.PrintChat("After: {0}", after);
                var disafter = Vector3.DistanceSquared(after, tar.Position);
                //Game.PrintChat("DisAfter: {0}", disafter);
                //Game.PrintChat("first calc: {0}", (disafter) - (630*630));
                if ((disafter < 630*630) && disafter > 150*150)
                {
                    Q.Cast(Game.CursorPos);
                }
                if (Vector3.DistanceSquared(tar.Position, ObjectManager.Player.Position) > 630*630 &&
                    disafter < 630*630)
                {
                    Q.Cast(Game.CursorPos);
                }
            }
            else
            {
                Q.Cast(Game.CursorPos);
            }
            //Q.Cast(Game.CursorPos);
        }

        public static Vector3 Normalize(Vector3 A)
        {
            double distance = Math.Sqrt(A.X * A.X + A.Y * A.Y);
            return new Vector3(new Vector2((float)(A.X / distance)), (float)(A.Y / distance));
        }

        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("walltumble").GetValue<KeyBind>().Active)
            {
                TumbleHandler();
            }

            if (menu.Item("aaqaa").GetValue<KeyBind>().Active)
            {
                Orbwalking.Orbwalk(TargetSelector.GetTarget(625, TargetSelector.DamageType.Physical),Game.CursorPos);
            }

            if (Itemsmenu.Item("QSS").GetValue<bool>())
            {
                for (int i = 0; i < buffs.Length; i++)
                {
                    if (ObjectManager.Player.HasBuffOfType(buffs[i]) && qss.Item(buffs[i].ToString()).GetValue<bool>())
                    {
                        if (Items.CanUseItem(3140))
                        {
                            Items.UseItem(3140);
                        }
                        else if (Items.CanUseItem(3139))
                        {
                            Items.UseItem(3140);
                        }
                        else if (cleanse != null && cleanse.IsReady())
                        {
                            cleanse.Cast();
                        }
                    }
                }
            }
            if (!E.IsReady()) return; //||
            //(orbwalker.ActiveMode.ToString() != "Combo" || !menu.Item("UseEC").GetValue<bool>()) &&
            //!menu.Item("UseET").GetValue<KeyBind>().Active)) return;
            if (((orbwalker.ActiveMode.ToString() == "Combo" && menu.Item("UseEC").GetValue<bool>()) || (orbwalker.ActiveMode.ToString() == "Mixed" && menu.Item("he").GetValue<bool>()) || menu.Item("UseET").GetValue<KeyBind>().Active))
                foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                                     let prediction = E.GetPrediction(hero)
                                     where NavMesh.GetCollisionFlags(
                                         prediction.UnitPosition.To2D()
                                             .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                                 -menu.Item("PushDistance").GetValue<Slider>().Value)
                                             .To3D())
                                         .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                                             prediction.UnitPosition.To2D()
                                                 .Extend(ObjectManager.Player.ServerPosition.To2D(),
                                                     -(menu.Item("PushDistance").GetValue<Slider>().Value / 2))
                                                 .To3D())
                                             .HasFlag(CollisionFlags.Wall)
                                     select hero)
                {
                    E.Cast(hero);
                }
        }
    }
}
