using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace TeachingLeagueSharp
{
    class Program
    {
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        private static TargetSelector ts;
        private static GameObject ward;
        private static Vector3 spawn;
        private static bool recalling = false;
        private static Menu menu;
        private static int[] ids;
        private static int index = 0;
        private static double count;
        private static bool stopdoingshit = false;
        private static double foundturret;
        private static Obj_AI_Turret turret;
        private static string[] stufftosay;
        private static int saycounter = 0;

        private static Obj_AI_Hero follow;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            stufftosay = new[] { "brb", "need to b", "sec" };
            Console.WriteLine("in");
            if (ObjectManager.Player.ChampionName != "Soraka") return;
            Console.WriteLine("in2");
            Q = new Spell(SpellSlot.Q, 970);
            W = new Spell(SpellSlot.W, 550);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R);
            ts = new TargetSelector(1025, TargetSelector.TargetingMode.AutoPriority);
            spawn =
                ObjectManager.Get<GameObject>()
                    .First(x => x.Type == GameObjectType.obj_SpawnPoint && x.Team == ObjectManager.Player.Team)
                    .Position;
            menu = new Menu("Soraka Slack", "slack", true);
            menu.AddItem(new MenuItem("on", "Start Slacking!").SetValue(new KeyBind(32, KeyBindType.Toggle)));
            menu.AddItem(new MenuItem("user", "Use R?").SetValue(true));
            menu.AddItem(new MenuItem("usew", "Use W?").SetValue(true));
            menu.AddItem(new MenuItem("allyhpw", "Ally % HP for W").SetValue(new Slider(30, 0, 93)));
            menu.AddItem(new MenuItem("wabovehp", "Use W when my hp > x%").SetValue(new Slider(20, 0, 99)));
            menu.AddItem(new MenuItem("allyhpr", "Ally % HP for R").SetValue(new Slider(30, 0, 50)));
            menu.AddItem(new MenuItem("hpb", "B if hp < %").SetValue(new Slider(15, 0, 80)));

            menu.AddSubMenu(new Menu("Follow:", "follower"));
            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly))
            {
                menu.SubMenu("follower").AddItem(new MenuItem(ally.ChampionName, ally.ChampionName).SetValue(false));
            }
            var sequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 1, 1, 4, 1, 3 };
            var level = new AutoLevel(sequence);

            menu.AddToMainMenu();
            ids = new[] { 3096, 1004, 1004, 1033, 1001, 3028, 3174, 3009, 1028, 3067, 1028, 3211, 3065, 3069, 1028, 2049, 2045 };

            foreach (var item in ids)
            {
                if (Items.HasItem(item))
                    index++;
            }
            Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(3301)).Send();
            Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
            Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
            Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
            Game.OnGameUpdate += Game_OnGameUpdate;

            //follow = ObjectManager.Get<Obj_AI_Hero>().First(x => ad.Contains(x.ChampionName));
            //Obj_AI_Base.OnCreate += Obj_AI_Base_OnCreate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //if (Utility.InShopRange())
            //{
            //   stopdoingshit = false;
            //   recalling = false;
            // }
            //follow = ObjectManager.Get<Obj_AI_Hero>().First(x => menu.Item(x.ChampionName).GetValue<bool>());
            if (saycounter == 2)
                saycounter = 0;
            if (menu.Item("on").GetValue<KeyBind>().Active)
            {
                // Game.PrintChat(index.ToString());
                if (Items.HasItem(ids[index]))
                    index++;
                Console.WriteLine("Recalling = " + recalling);

                Console.WriteLine("stop: " + stopdoingshit);
                // Game.PrintChat(follow.ChampionName);
                if (Game.Time - foundturret > 25)
                    stopdoingshit = false;
                if (Utility.InShopRange())
                {
                    if (!Items.HasItem(ids[index]))
                    {
                        Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(ids[index])).Send();

                    }
                }

                follow = ObjectManager.Get<Obj_AI_Hero>().First(x => menu.Item(x.ChampionName).GetValue<bool>());
                Console.WriteLine(follow.IsDead);
                if ((follow.IsDead ||
                     (follow.Distance(ObjectManager.Player.Position) > 5000 && !Utility.InShopRange() &&
                      spawn.Distance(follow.Position) < 1500) ||
                     ObjectManager.Player.Health / ObjectManager.Player.MaxHealth * 100 <
                     menu.Item("hpb").GetValue<Slider>().Value))
                {

                    if (Game.Time - foundturret > 20 && !recalling)
                    {
                        var turret2 =
                            ObjectManager.Get<Obj_AI_Turret>()
                                .Where(x => x.Distance(ObjectManager.Player) < 5000 && x.IsAlly);

                        if (turret2.Any())
                        {
                            stopdoingshit = true;
                            turret = turret2.First();
                            foundturret = Game.Time;
                        }
                    }


                    if (stopdoingshit && !recalling)
                    {
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, turret);
                        if (ObjectManager.Player.Distance(turret) <= 350 && Game.Time - count > 15)
                        {
                            Game.Say(stufftosay[saycounter]);
                            saycounter++;
                            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall);

                            recalling = true;
                            count = Game.Time;
                        }
                    }
                }

                //Game.PrintChat((Game.Time - count).ToString());
                if ((Game.Time - count > 15 && Game.Time - count < 17)) //|| Utility.InShopRange())
                {
                    stopdoingshit = false;
                    recalling = false;
                }

                if (!recalling && !stopdoingshit && W.IsReady())
                {
                    var allies2 =
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                x =>
                                    x.IsAlly && x.Health / x.MaxHealth * 100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
                                    !x.IsDead && x.Distance(ObjectManager.Player.Position) < 550);
                    var objAiHeroes = allies2 as Obj_AI_Hero[] ?? allies2.ToArray();
                    if (objAiHeroes.Any() &&
                        ObjectManager.Player.Health / ObjectManager.Player.MaxHealth * 100 >
                        menu.Item("wabovehp").GetValue<Slider>().Value)
                        W.Cast(objAiHeroes.First());
                }

                if (menu.Item("user").GetValue<bool>() && R.IsReady())
                {
                    var allies =
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                x =>
                                    x.IsAlly && x.Health / x.MaxHealth * 100 < menu.Item("allyhpr").GetValue<Slider>().Value &&
                                    !x.IsDead);
                    if (allies.Any())
                    {
                        if (R.IsReady())
                            R.Cast();
                    }
                }

                if (!recalling && !stopdoingshit)
                {
                    if (follow.Distance(ObjectManager.Player.Position) > 500)
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, follow);
                    if (!follow.IsDead)
                    {
                        if (W.IsReady() && menu.Item("usew").GetValue<bool>() &&
                            ObjectManager.Player.Health / ObjectManager.Player.MaxHealth * 100 >
                            menu.Item("wabovehp").GetValue<Slider>().Value)
                        {
                            if (follow.Health / follow.MaxHealth * 100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
                                follow.Distance(ObjectManager.Player.Position) < 550 &&
                                ObjectManager.Player.Health / ObjectManager.Player.MaxHealth * 100 >
                                menu.Item("wabovehp").GetValue<Slider>().Value)
                            {
                                W.Cast(follow);
                            }
                            else if (follow.Health / follow.MaxHealth * 100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
                                     follow.Distance(ObjectManager.Player.Position) > 550)
                            {
                                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, follow.Position);
                            }
                        }

                        if (ts.Target.Distance(ObjectManager.Player) < Q.Range && Q.IsReady())
                        {
                            Q.Cast(ts.Target);
                        }

                        if (ts.Target.Distance(ObjectManager.Player) < E.Range && E.IsReady())
                        {
                            E.Cast(ts.Target);
                        }

                        Random x = new Random();
                        var xPos = ((spawn.X - follow.Position.X) / Vector3.Distance(follow.Position, spawn)) * 300 +
                                   follow.Position.X -
                                   x.Next(25, 150);
                        var yPos = ((spawn.Y - follow.Position.Y) / Vector3.Distance(follow.Position, spawn)) * 300 +
                                   follow.Position.Y -
                                   x.Next(25, 150);
                        var vec = new Vector3(xPos, yPos, follow.Position.Z);
                        if (
                            NavMesh.GetCollisionFlags(
                                vec.To2D().Extend(ObjectManager.Player.Position.To2D(), 150).To3D())
                                .HasFlag(CollisionFlags.None))
                            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, vec);
                    }

                    else
                    {
                        Random y = new Random();
                        var turret =
                            ObjectManager.Get<Obj_AI_Turret>()
                                .First(x => x.Distance(ObjectManager.Player) < 2000 && x.IsAlly);
                        var xPos = ((spawn.X - turret.Position.X) / Vector3.Distance(turret.Position, spawn)) * 300 +
                                   turret.Position.X -
                                   y.Next(25, 150);
                        var yPos = ((spawn.Y - turret.Position.Y) / Vector3.Distance(turret.Position, spawn)) * 300 +
                                   turret.Position.Y -
                                   y.Next(25, 150);

                        var vec = new Vector3(xPos, yPos, follow.Position.Z);
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, vec);
                    }
                }
            }
        }
    }
}
