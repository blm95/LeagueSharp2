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
    internal class Program
    {
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        private static TargetSelector ts;
        //private static GameObject ward;
        private static Vector3 spawn;
        private static bool recalling = false;
        private static Menu menu;
        //private static int[] ids;
        //private static int index = 0;
        private static double count;
        private static bool stopdoingshit = false;
        private static double foundturret;
        private static Obj_AI_Turret turret;
        // private static string[] stufftosay;
        // private static string[] deaths;
        // private static int deathcounter = 0;
        // private static int saycounter = 0;
        // private static double timedead;
        private static double gamestart;
        private static ItemToShop nextItem;
        private static List<ItemToShop> buyThings;
        private static List<Obj_AI_Hero> allies;
        private static int i = 0;
        private static bool stopfollowingshittarget = false;


        //public static List<Vector3> _WardSpots;
        //public static List<WardSpot> _SafeWardSpots;

        private static readonly string[] ad =
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Graves", "KogMaw",
            "MissFortune", "Quinn", "Sivir", "Tristana", "Twitch", "Varus", "Vayne", "Jinx", "Lucian"
        };

        private static readonly string[] ap =
        {
            "Ahri", "Akali", "Anivia", "Annie", "Brand", "Cassiopeia", "Diana",
            "FiddleSticks", "Fizz", "Gragas", "Heimerdinger", "Karthus", "Kassadin", "Katarina", "Kayle", "Kennen",
            "Leblanc", "Lissandra", "Lux", "Malzahar", "Mordekaiser", "Morgana", "Nidalee", "Orianna", "Ryze", "Sion",
            "Swain", "Syndra", "Teemo", "TwistedFate", "Veigar", "Viktor", "Vladimir", "Xerath", "Ziggs", "Zyra",
            "Velkoz"
        };

        private static readonly string[] bruiser =
        {
            "Darius", "Elise", "Evelynn", "Fiora", "Gangplank", "Gnar", "Jayce",
            "Pantheon", "Irelia", "JarvanIV", "Jax", "Khazix", "LeeSin", "Nocturne", "Olaf", "Poppy", "Renekton",
            "Rengar", "Riven", "Shyvana", "Trundle", "Tryndamere", "Udyr", "Vi", "MonkeyKing", "XinZhao", "Aatrox",
            "Rumble", "Shaco", "MasterYi"
        };

        private static Vector3 followpos;
        public static bool canBuyItems = true;
        private static Obj_AI_Hero follow;
        private static double followtime;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Soraka") return;
            //InitializeSafeWardSpots();
            //InitializeWardSpots();
            allies = new List<Obj_AI_Hero>();
            //BushRevealer c = new BushRevealer();
            Q = new Spell(SpellSlot.Q, 970);
            W = new Spell(SpellSlot.W, 550);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R);
            //Game.PrintChat("in1");
            ts = new TargetSelector(1025, TargetSelector.TargetingMode.AutoPriority);
            // stufftosay = new[] { "brb", "need to b", "sec" };
            // deaths = new[] {"oops", "lol", "rip", "laggg"};
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
            //Game.PrintChat("in2");
            menu.AddSubMenu(new Menu("Follow:", "follower"));
            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly && !x.IsMe))
            {
                allies.Add(ally);
                if (ad.Contains(ally.ChampionName))
                    menu.SubMenu("follower").AddItem(new MenuItem(ally.ChampionName, ally.ChampionName).SetValue(true));
                else
                {
                    menu.SubMenu("follower").AddItem(new MenuItem(ally.ChampionName, ally.ChampionName).SetValue(false));
                }
            }
            // Game.PrintChat("hi");
            buyThings = new List<ItemToShop>
            {
                new ItemToShop()
                {
                    goldReach = 500,
                    itemsMustHave = new List<int>{3301},
                    itemIds = new List<int>{3096}
                },
                new ItemToShop()
                {
                    goldReach = 360,
                    itemsMustHave = new List<int>{3096},
                    itemIds = new List<int>{1004,1004}
                },
                new ItemToShop()
                {
                    goldReach = 500,
                    itemsMustHave = new List<int>{1004,1004},
                    itemIds = new List<int>{1033}
                },
                new ItemToShop()
                {
                    goldReach = 180,
                    itemsMustHave = new List<int>{1033,1004,1004},
                    itemIds = new List<int>{3028}
                },
                new ItemToShop()
                {
                    goldReach = 325,
                    itemsMustHave = new List<int>{3028},
                    itemIds = new List<int>{1001}
                },
                new ItemToShop()
                {
                    goldReach = 675,
                    itemsMustHave = new List<int>{1001},
                    itemIds = new List<int>{3009}
                },
                new ItemToShop()
                {
                    goldReach = 400,
                    itemsMustHave = new List<int>{3009},
                    itemIds = new List<int>{1028}
                },
                new ItemToShop()
                {
                    goldReach = 450,
                    itemsMustHave = new List<int>{1028},
                    itemIds = new List<int>{3067}
                },
                new ItemToShop()
                {
                    goldReach = 400,
                    itemsMustHave = new List<int>{3067},
                    itemIds = new List<int>{1028}
                },
                new ItemToShop()
                {
                    goldReach = 800,
                    itemsMustHave = new List<int>{1028},
                    itemIds = new List<int>{3211}
                },
                new ItemToShop()
                {
                    goldReach = 700,
                    itemsMustHave = new List<int>{3211},
                    itemIds = new List<int>{3065}
                },
                new ItemToShop()
                {
                    goldReach = 2900,
                    itemsMustHave = new List<int>{3065},
                    itemIds = new List<int>{3116}
                }
            };
            //Game.PrintChat("hi2");
            // Game.PrintChat("in3");
            var sequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 1, 1, 4, 1, 3 };
            var level = new AutoLevel(sequence);
            gamestart = Game.Time;
            menu.AddToMainMenu();
            nextItem = buyThings[0];
            //Game.PrintChat("in4");
            //ids = new[] { 3096, 1004, 1004, 1033, 1001, 3028, 3174, 3009, 1028, 3067, 1028, 3211, 3065, 3069, 1028, 2049, 2045 };

            //follow = ObjectManager.Get<Obj_AI_Hero>().First(x => x.IsAlly && menu.Item(x.ChampionName).GetValue<bool>()); //??
            //   ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.IsAlly && ap.Contains(x.ChampionName)) ??
            //    ObjectManager.Get<Obj_AI_Hero>().First(x => x.IsAlly && !x.IsMe);
            //if (follow != null)
            //followpos = follow.Position;
            followtime = Game.Time;
            //Game.PrintChat("in5");
            //int counter = 0;
            //foreach (var item in ids)
            //{
            //    if (Items.HasItem(item) && counter > index)
            //    {
            //        index = counter;
            //        Game.PrintChat(index.ToString());
            //    }
            //    counter++;
            //}
            // Game.PrintChat("in6");

            if (Game.Time < 300)
            {
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(3301)).Send();
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(3340)).Send();
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(2003)).Send();
            }
            //Game.OnGameNotifyEvent += Game_OnGameNotifyEvent;
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;

            //follow = ObjectManager.Get<Obj_AI_Hero>().First(x => ad.Contains(x.ChampionName));
            //Obj_AI_Base.OnCreate += Obj_AI_Base_OnCreate;
        }

        //static void Game_OnGameNotifyEvent(GameNotifyEventArgs args)
        //{
        //    args.
        //}

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            GamePacket p = new GamePacket(args.PacketData);
            if (p.Header != Packet.S2C.TowerAggro.Header) return;
            if (Packet.S2C.TowerAggro.Decoded(args.PacketData).TargetNetworkId != ObjectManager.Player.NetworkId)
                return;
            if (Game.Time - foundturret > 20 && !recalling)
            {
                var turret2 =
                    ObjectManager.Get<Obj_AI_Turret>()
                        .Where(x => x.Distance(ObjectManager.Player) < 3500 && x.IsAlly);

                if (turret2.Any())
                {
                    stopdoingshit = true;
                    turret = turret2.First();
                    foundturret = Game.Time;
                }
            }


            if (!stopdoingshit || recalling) return;
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, turret);
            if (!(ObjectManager.Player.Distance(turret) <= 350) || !(Game.Time - count > 15)) return;
            //               Game.Say(stufftosay[saycounter]);
            //               saycounter++;
            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall);

            recalling = true;
            count = Game.Time;
        }

        internal class ItemToShop
        {
            public int goldReach;
            public List<int> itemIds;
            public List<int> itemsMustHave;
        }

        private static bool checkItemcount(ItemToShop its)
        {
            bool[] usedItems = new bool[7];
            int itemsMatch = 0;
            foreach (int t in its.itemsMustHave)
            {
                //Game.PrintChat(t.ToString());
                for (int i = 0; i < ObjectManager.Player.InventoryItems.Count(); i++)
                {
                    if (usedItems[i])
                        continue;
                    if (t != (decimal)ObjectManager.Player.InventoryItems[i].Id) continue;
                    usedItems[i] = true;
                    //Game.PrintChat("iasdgfasd");
                    itemsMatch++;
                    break;
                }
            }
            return itemsMatch == its.itemsMustHave.Count;
        }

        public static void checkItemInventory()
        {

            if (!canBuyItems)
                return;
            for (int i = buyThings.Count - 1; i >= 0; i--)
            {
                //Game.PrintChat(i.ToString());
                if (!checkItemcount(buyThings[i])) continue;
                nextItem = buyThings[i];
                // Game.PrintChat("in");
                if (i == buyThings.Count - 1)
                {
                    canBuyItems = false;
                }

                return;
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //if (Utility.InShopRange())
            //{
            //   stopdoingshit = false;
            //   recalling = false;
            // }
            //Game.PrintChat("in1");
            if (!stopfollowingshittarget)
            {
                follow =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .First(x => !x.IsMe && x.IsAlly && menu.Item(x.ChampionName).GetValue<bool>()) ??
                    ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.IsAlly && ap.Contains(x.ChampionName)) ??
                    ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.IsAlly && bruiser.Contains(x.ChampionName)) ??
                    ObjectManager.Get<Obj_AI_Hero>().First(x => x.IsAlly && !x.IsMe);
                followpos = follow.Position;
            }
            //Game.PrintChat(follow.ChampionName);
            // if (deathcounter == 4)
            //      deathcounter = 0;





            if (Game.Time - gamestart > 480)
            {
                follow = allies[i];
                i++;
                gamestart = Game.Time;
            }


            // Game.PrintChat("insadfsadfd");

            // if (ObjectManager.Player.IsDead && Game.Time - timedead > 80)
            //{
            //     Game.Say(deaths[deathcounter]);
            //     deathcounter++;
            //     timedead = Game.Time;
            // }

            //foreach (var c in _WardSpots.Where(x => x.Distance(ObjectManager.Player.Position) < 600))
            //{
            //    InventorySlot wardSlot = Wards.GetWardSlot();

            //    if (wardSlot != null && wardSlot.Id != ItemId.Unknown)
            //    {
            //        wardSlot.UseItem(c);
            //    }
            //}

            //foreach (var c in _SafeWardSpots.Where(x => x.WardPosition.Distance(ObjectManager.Player.Position) < 600))//x.Distance(ObjectManager.Player.Position) < 600))
            //{
            //    InventorySlot wardSlot = Wards.GetWardSlot();

            //    if (wardSlot != null && wardSlot.Id != ItemId.Unknown)
            //    {
            //        wardSlot.UseItem(c.WardPosition);
            //    }
            //}

            //foreach (var item in ids)
            //{
            //    if (Items.HasItem(ids[counter]) && counter < index + 1)
            //    {
            //        index = counter;
            //    }
            //    counter++;
            //}

            //if (Utility.InShopRange())
            //{
            //    Game.PrintChat(index.ToString());
            //    Game.PrintChat(ids[index].ToString());

            //    foreach (var item in ids)
            //    {

            //            if (!Items.HasItem(ids[index]))
            //            {
            //                Game.PrintChat(ids[index].ToString());
            //                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(ids[index])).Send();
            //                index++;
            //                return;
            //            }


            //    }
            //}


            //foreach (var item in ids)
            //{
            //    if (Items.HasItem(ids[counter]) && counter < index + 1)
            //    {
            //        index = counter;
            //    }
            //    counter++;
            //}

            //Game.PrintChat("lel");
            if (Utility.InShopRange())
            {
                // Game.PrintChat("in range");
                foreach (var item in nextItem.itemIds)
                {
                    // Game.PrintChat(item.ToString());
                    if (!Items.HasItem(item))
                    {
                        Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(item, ObjectManager.Player.NetworkId))
                            .Send();

                    }
                }

                checkItemInventory();
            }
            //Game.PrintChat("hue");
            //if (saycounter == 3)
            //    saycounter = 0;
            //if (menu.Item("on").GetValue<KeyBind>().Active)
            //{
            // Game.PrintChat(index.ToString());
            // if (Items.HasItem(ids[index]))
            //    index++;
            Console.WriteLine("Recalling = " + recalling);

            Console.WriteLine("stop: " + stopdoingshit);
            // Game.PrintChat(follow.ChampionName);
            if (Game.Time - foundturret > 25)
                stopdoingshit = false;
            //if (Utility.InShopRange())
            //{
            //    Game.PrintChat(index.ToString());
            //    if (!Items.HasItem(ids[index]))
            //    {
            //        Game.PrintChat(ids[index].ToString());
            //        Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(ids[index])).Send();
            //        index++;
            //    }
            //}

            if (Game.Time - followtime > 40 && followpos.Distance(follow.Position) <= 100)
            {
                follow = ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.IsAlly && ap.Contains(x.ChampionName)) ?? ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.IsAlly && bruiser.Contains(x.ChampionName));
                followpos = follow.Position;
                followtime = Game.Time;
                stopfollowingshittarget = true;
            }

            if (follow.IsDead)
            {
                follow = ObjectManager.Get<Obj_AI_Hero>().First(x => !x.IsMe && x.Distance(ObjectManager.Player) < 1300);
            }

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
                            .Where(x => x.Distance(ObjectManager.Player) < 3500 && x.IsAlly);

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
                        // Game.Say(stufftosay[saycounter]);
                        //  saycounter++;
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

                    if (ts.Target.Distance(ObjectManager.Player) < Q.Range && Q.IsReady() && !Utility.UnderTurret(ObjectManager.Player, true))
                    {
                        Q.Cast(ts.Target);
                    }

                    if (ts.Target.Distance(ObjectManager.Player) < E.Range && E.IsReady() && !Utility.UnderTurret(ObjectManager.Player, true))
                    {
                        E.Cast(ts.Target);
                    }

                    if (!(follow.Distance(ObjectManager.Player.Position) > 350)) return;
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
                    //Game.PrintChat("following");
                }

                else
                {
                    Random y = new Random();
                    var turret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .First(x => x.Distance(ObjectManager.Player) < 3500 && x.IsAlly);
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

//        public class WardSpot   // trus copy pasta kappa, thx
//        {
//            public Vector3 MagneticPosition { get; private set; }

//            public Vector3 ClickPosition { get; private set; }

//            public Vector3 WardPosition { get; private set; }

//            public Vector3 MovePosition { get; private set; }

//            public WardSpot(Vector3 magneticPosition, Vector3 clickPosition,
//                                Vector3 wardPosition, Vector3 movePosition)
//            {
//                MagneticPosition = magneticPosition;
//                ClickPosition = clickPosition;
//                WardPosition = wardPosition;
//                MovePosition = movePosition;
//            }
//        }

//        private static void InitializeWardSpots()
//        {
//            _WardSpots = new List<Vector3>
//            {
//                new Vector3(2823.37f, 7617.03f, 55.03f),
//                new Vector3(7422.0f, 3282.0f, 46.53f),
//                new Vector3(10148.0f, 2839.0f, 44.41f),
//                new Vector3(6269.0f, 4445.0f, 42.51f),
//                new Vector3(7151.64f, 4719.66f, 51.67f),
//                new Vector3(4354.54f, 7079.51f, 53.67f),
//                new Vector3(4728.0f, 8336.0f, 51.29f),
//                new Vector3(6762.52f, 2918.75f, 55.68f),
//                new Vector3(11217.39f, 6841.89f, 54.87f),
//                new Vector3(6610.35f, 11064.61f, 54.45f),
//                new Vector3(3883.0f, 11577.0f, 39.87f),
//                new Vector3(7775.0f, 10046.49f, 43.14f),
//                new Vector3(6867.68f, 9567.63f, 57.01f),
//                new Vector3(9720.86f, 7501.50f, 54.85f),
//                new Vector3(9233.13f, 6094.48f, -44.63f),
//                new Vector3(7282.69f, 1148992.53f, 52.59f),
//                new Vector3(10180.18f, 4969.32f, -62.32f),
//                new Vector3(8875.13f, 5390.57f, -64.07f),
//                new Vector3(3920.88f, 9477.78f, -60.42f),
//                new Vector3(5017.27f, 8954.09f, -62.70f),
//                new Vector3(12731.25f, 9132.66f, 50.32f),
//                new Vector3(12731.25f, 9132.66f, 50.32f),
//                new Vector3(9260.02f, 8582.67f, 54.62f),
//                new Vector3(4749.79f, 5890.76f, 53.559f),
//                new Vector3(5983.58f, 1547.98f, 52.99f),
//                new Vector3(1213.70f, 5324.73f, 58.77f),
//                new Vector3(9641.6591796875f, 6368.748046875f, 53.01416015625f),
//                new Vector3(8081.4360351563f, 4683.443359375f, 55.9482421875f),
//                new Vector3(5943.51953125f, 9792.4091796875f, 53.189331054688f),
//                new Vector3(4379.513671875f, 8093.740234375f, 42.734619140625f),
//                new Vector3(4222.724609375f, 7038.5805664063f, 53.612548828125f),
//                new Vector3(9068.0224609375f, 11186.685546875f, 53.22705078125f),
//                new Vector3(7970.822265625f, 10005.072265625f, 53.527709960938f),
//                new Vector3(4978.1943359375f, 3042.6975097656f, 54.343017578125f),
//                new Vector3(7907.6357421875f, 11629.322265625f, 49.947143554688f),
//                new Vector3(7556.0654296875f, 11739.625f, 50.61547851625f),
//                new Vector3(5973.4853515625f, 11115.6875f, 54.348999023438f),
//                new Vector3(5732.8198242188f, 10289.76953125f, 53.397827148438f),
//                new Vector3(12073.184570313f, 4795.50390625f, 52.322265625f),
//                new Vector3(4044.1313476563f, 11600.502929688f, 48.591918945313f),
//                new Vector3(5597.6669921875f, 12491.047851563f, 39.739379882813f),
//                new Vector3(10070.202148438f, 4132.4536132813f, -60.332153320313f),
//                new Vector3(8320.2890625f, 4292.8090820313f, 56.473876953125f),
//                new Vector3(9603.5205078125f, 7872.2368164063f, 54.713745117188f),
//                new Vector3(7812f, 5177f, 56f),
//                new Vector3(11653f, 9408f, 50f),
//                new Vector3(8748f, 2038f, 54f),
//                new Vector3(7909f, 3282f, 56f),
//                new Vector3(6222f, 2852f, 53f),
//                new Vector3(6033f, 4484f, 51f),
//                new Vector3(8274f, 4259f, 56f),
//                new Vector3(5035f, 3233f, 54f),
//                new Vector3(5314f, 3360f, 54f),
//                new Vector3(2364f, 7234f, 54f),
//                new Vector3(1917f, 9663f, 53f),
//                new Vector3(2720f, 10591f, -63f),
//                new Vector3(2519f, 11176f, -64f),
//                new Vector3(5325f, 12470f, 39f),
//                new Vector3(7989f, 6222f, -64f),
//                new Vector3(7721f, 6026f, -64f),
//                new Vector3(11748f, 1119f, 48f),
//                new Vector3(6382f, 8300f, -63f),
//                new Vector3(6105f, 8049f, -57f),
//                new Vector3(4747f, 8954f, -63f),
//                new Vector3(6372f, 11253f, 54f),
//                new Vector3(6063f, 11148f, 54f),
//                new Vector3(7531f, 11706f, 50f),
//                new Vector3(7822f, 11537f, 49f),
//                new Vector3(8634f, 11139f, 52f),
//                new Vector3(9008f, 11194f, 55f),
//                new Vector3(11631f, 7265f, 55f),
//                new Vector3(11393f, 3835f, -54f),
//                new Vector3(11483f, 3458f, -55f),
//                new Vector3(5839f, 9843f, 53f),
//                new Vector3(5733f, 10202f, 53f),
//                new Vector3(966f, 12358f, 40f),
//                new Vector3(1533f, 12934f, 34f),
//                new Vector3(2296f, 13323f, 29f),
//                new Vector3(2388f, 5078f, 55f),
//                new Vector3(7965f, 10001f, 53f),
//                new Vector3(12327f, 1565f, 48f),
//                new Vector3(12775f, 2076f, 48f),
//                new Vector3(13217f, 2793f, 48f),
//                new Vector3(7022f, 7065f, 54f),
//                new Vector3(6308f, 6591f, 55f),
//                new Vector3(3982f, 4424f, 53f),
//                new Vector3(5085f, 5437f, 54f),
//                new Vector3(7714f, 7764f, 53f),
//                new Vector3(6119f, 9442f, 55f),
//                new Vector3(426f, 7946f, 47f),
//                new Vector3(7417f, 659f, 52f),
//                new Vector3(8978f, 9013f, 54f),
//                new Vector3(10002f, 10141f, 51f),
//                new Vector3(9237f, 13256f, 80f),
//                new Vector3(13012f, 9449f, 50f),
//                new Vector3(1109f, 5111f, 57f),
//                new Vector3(4917f, 1240f, 53f),
//                new Vector3(5550f, 1317f, 53f),
//                new Vector3(8428f, 13214f, 46f),
//                new Vector3(9909f, 11533f, 106f),
//                new Vector3(9833f, 12533f, 106f),
//                new Vector3(11448f, 10277f, 106f),
//                new Vector3(12307f, 10041f, 106f),
//                new Vector3(11418f, 11703f, 106f),
//                new Vector3(1775f, 4411f, 108f),
//                new Vector3(2643f, 4229f, 105f),
//                new Vector3(4102f, 2895f, 110f),
//                new Vector3(4206f, 1948f, 108f),
//                new Vector3(2563f, 2717f, 130f),
//                new Vector3(12039f, 1336f, 48f),
//                new Vector3(13018f, 2433f, 48f),
//                new Vector3(6495f, 2789f, 55f),
//                new Vector3(7672f, 3211f, 54f)
//            };


//            //new
//            //_WardSpots.Add(new Vector3(7969.15625f, 3307.5673828125f, 56.940795898438f));

//            //new2
//            //_WardSpots.Add(new Vector3(9623f, 6358f, 2f));
//            //_WardSpots.Add(new Vector3(9127f, 5416f, -64f));
//            // _WardSpots.Add(new Vector3(12197f, 4912f, 55f));
//            //_WardSpots.Add(new Vector3(8147f, 4666f, 55f));
//        }

//        private static void InitializeSafeWardSpots()
//        {
//            _SafeWardSpots = new List<WardSpot>
//            {
//                new WardSpot(new Vector3(9695.0f, 3465.0f, 43.02f),
//                    new Vector3(9843.38f, 3125.16f, 43.02f),
//                    new Vector3(9946.10f, 3064.81f, 43.02f),
//                    new Vector3(9595.0f, 3665.0f, 43.02f)),
//                new WardSpot(new Vector3(4346.10f, 10964.81f, 36.62f),
//                    new Vector3(4214.93f, 11202.01f, 36.62f),
//                    new Vector3(4146.10f, 11314.81f, 36.62f),
//                    new Vector3(4384.36f, 10680.41f, 36.62f)),
//                new WardSpot(new Vector3(2349.0f, 10387.0f, 44.20f),
//                    new Vector3(2257.97f, 10783.37f, 44.20f),
//                    new Vector3(2446.10f, 10914.81f, 44.20f),
//                    new Vector3(2311.0f, 10185.0f, 44.20f)),
//                new WardSpot(new Vector3(4946.52f, 6474.56f, 54.71f),
//                    new Vector3(4891.98f, 6639.05f, 53.62f),
//                    new Vector3(4546.10f, 6864.81f, 53.78f),
//                    new Vector3(5217.0f, 6263.0f, 54.95f)),
//                new WardSpot(new Vector3(5528.96f, 7615.20f, 45.64f),
//                    new Vector3(5688.96f, 7825.20f, 45.64f),
//                    new Vector3(5796.10f, 7914.81f, 45.64f),
//                    new Vector3(5460.13f, 7469.77f, 45.64f)),
//                new WardSpot(new Vector3(7745.0f, 4065.0f, 47.71f),
//                    new Vector3(7927.65f, 4239.77f, 47.71f),
//                    new Vector3(8146.10f, 4414.81f, 47.71f),
//                    new Vector3(7645.0f, 4015.0f, 47.71f)),
//                new WardSpot(new Vector3(9057.0f, 8245.0f, 45.73f),
//                    new Vector3(9230.7f, 7892.22f, 66.39f),
//                    new Vector3(9446.10f, 7814.81f, 54.66f),
//                    new Vector3(8895.0f, 8313.0f, 54.89f)),
//                new WardSpot(new Vector3(9025.78f, 6591.64f, 46.27f),
//                    new Vector3(9200.08f, 6425.05f, 43.21f),
//                    new Vector3(9396.10f, 6264.81f, 23.72f),
//                    new Vector3(8795.0f, 6815.0f, 56.11f)),
//                new WardSpot(new Vector3(8530.27f, 6637.38f, 46.98f),
//                    new Vector3(8539.27f, 6637.38f, 46.98f),
//                    new Vector3(8396.10f, 6464.81f, 46.98f),
//                    new Vector3(8779.17f, 6804.70f, 46.98f)),
//                new WardSpot(new Vector3(11889.0f, 4205.0f, 42.84f),
//                    new Vector3(11974.23f, 3807.21f, 42.84f),
//                    new Vector3(11646.10f, 3464.81f, 42.84f),
//                    new Vector3(11939.0f, 4255.0f, 42.84f)),
//                new WardSpot(new Vector3(6299.0f, 10377.75f, 45.47f),
//                    new Vector3(6030.24f, 10292.37f, 54.29f),
//                    new Vector3(5846.10f, 10164.81f, 53.94f),
//                    new Vector3(6447.0f, 10463.0f, 54.63f))
//            };

//            // Dragon -> Tri Bush
//            // Nashor -> Tri Bush

//            // Blue Top -> Solo Bush

//            // Blue Mid -> round Bush // Inconsistent Placement

//            // Blue Mid -> River Lane Bush

//            // Blue Lizard -> Dragon Pass Bush

//            // Purple Mid -> Round Bush // Inconsistent Placement

//            // Purple Mid -> River Round Bush

//            // Purple Mid -> River Lane Bush

//            // Purple Bottom -> Solo Bush

//            // Purple Lizard -> Nashor Pass Bush // Inconsistent Placement
//        }
//    }

//    internal class BushRevealer //By Beaving & Blm95
//    {
//        private readonly List<PlayerInfo> _playerInfo = new List<PlayerInfo>();
//        private int _lastTimeWarded;

//        public BushRevealer()
//        {
//            _playerInfo = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy).Select(x => new PlayerInfo(x)).ToList();
//            Game.OnGameUpdate += Game_OnGameUpdate;
//        }


//        private void Game_OnGameUpdate(EventArgs args)
//        {
//            int time = Environment.TickCount;

//            foreach (PlayerInfo playerInfo in _playerInfo.Where(x => x.Player.IsVisible))
//                playerInfo.LastSeen = time;

//            Wards.WardItem ward = Wards.GetWardItem();
//            if (ward == null)
//                return;


//            foreach (Obj_AI_Hero enemy in _playerInfo.Where(x =>
//                x.Player.IsValid &&
//                !x.Player.IsVisible &&
//                !x.Player.IsDead &&
//                x.Player.Distance(ObjectManager.Player.ServerPosition) < 1000 && //check real ward range later
//                time - x.LastSeen < 2500).Select(x => x.Player))
//            {
//                Vector3 bestWardPos = GetWardPos(enemy.ServerPosition, 165, 2);

//                if (bestWardPos == enemy.ServerPosition || bestWardPos == Vector3.Zero ||
//                    !(bestWardPos.Distance(ObjectManager.Player.ServerPosition) < ward.Range)) continue;
//                if (_lastTimeWarded != 0 && Environment.TickCount - _lastTimeWarded <= 500) continue;
//                InventorySlot wardSlot = Wards.GetWardSlot();

//                if (wardSlot == null || wardSlot.Id == ItemId.Unknown) continue;
//                wardSlot.UseItem(bestWardPos);
//                _lastTimeWarded = Environment.TickCount;
//            }

//        }

//        private class PlayerInfo
//        {
//            public readonly Obj_AI_Hero Player;
//            public int LastSeen;

//            public PlayerInfo(Obj_AI_Hero player)
//            {
//                Player = player;
//            }
//        }

//        private static Vector3 GetWardPos(Vector3 lastPos, int radius = 165, int precision = 3)
//        {
//            //Vector3 averagePos = Vector3.Zero;

//            int count = precision;
//            //int calculated = 0;

//            while (count > 0)
//            {
//                int vertices = radius;

//                var wardLocations = new WardLocation[vertices];
//                double angle = 2 * Math.PI / vertices;

//                for (int i = 0; i < vertices; i++)
//                {
//                    double th = angle * i;
//                    var pos = new Vector3((float)(lastPos.X + radius * Math.Cos(th)),
//                        (float)(lastPos.Y + radius * Math.Sin(th)), 0); //wardPos.Z
//                    wardLocations[i] = new WardLocation(pos, NavMesh.IsWallOfGrass(pos));
//                }

//                var grassLocations = new List<GrassLocation>();

//                for (var i = 0; i < wardLocations.Length; i++)
//                {
//                    if (!wardLocations[i].Grass) continue;
//                    if (i != 0 && wardLocations[i - 1].Grass)
//                        grassLocations.Last().Count++;
//                    else
//                        grassLocations.Add(new GrassLocation(i, 1));
//                }

//                GrassLocation grassLocation = grassLocations.OrderByDescending(x => x.Count).FirstOrDefault();

//                if (grassLocation != null) //else: no pos found. increase/decrease radius?
//                {
//                    var midelement = (int)Math.Ceiling(grassLocation.Count / 2f);
//                    //averagePos += wardLocations[grassLocation.Index + midelement - 1].Pos; //uncomment if using averagePos
//                    lastPos = wardLocations[grassLocation.Index + midelement - 1].Pos; //comment if using averagePos
//                    radius = (int)Math.Floor(radius / 2f); //precision recommended: 2-3; comment if using averagePos

//                    //calculated++; //uncomment if using averagePos
//                }

//                count--;
//            }

//            return lastPos; //averagePos /= calculated; //uncomment if using averagePos
//        }

//        private class WardLocation
//        {
//            public readonly bool Grass;
//            public readonly Vector3 Pos;

//            public WardLocation(Vector3 pos, bool grass)
//            {
//                Pos = pos;
//                Grass = grass;
//            }
//        }

//        private class GrassLocation
//        {
//            public readonly int Index;
//            public int Count;

//            public GrassLocation(int index, int count)
//            {
//                Index = index;
//                Count = count;
//            }
//        }


//    }

//    internal class Wards
//    {
//        public enum WardType
//        {
//            Stealth,
//            Vision,
//            Temp,
//            TempVision
//        }

//        public static readonly List<WardItem> WardItems = new List<WardItem>();

//        static Wards()
//        {
//            WardItems.Add(new WardItem(3360, "Feral Flare", "", 1000, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(2043, "Vision Ward", "VisionWard", 600, 180, WardType.Vision));
//            WardItems.Add(new WardItem(2044, "Stealth Ward", "SightWard", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3154, "Wriggle's Lantern", "WriggleLantern", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(2045, "Ruby Sightstone", "ItemGhostWard", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(2049, "Sightstone", "ItemGhostWard", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(2050, "Explorer's Ward", "ItemMiniWard", 600, 60, WardType.Stealth));
//            WardItems.Add(new WardItem(3340, "Greater Stealth Totem", "", 600, 120, WardType.Stealth));
//            WardItems.Add(new WardItem(3361, "Greater Stealth Totem", "", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3362, "Greater Vision Totem", "", 600, 180, WardType.Vision));
//            WardItems.Add(new WardItem(3366, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
//            WardItems.Add(new WardItem(3367, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
//            WardItems.Add(new WardItem(3368, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
//            WardItems.Add(new WardItem(3369, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
//            WardItems.Add(new WardItem(3371, "Bonetooth Necklace", "", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3375, "Head of Kha'Zix", "", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3205, "Quill Coat", "", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3207, "Spirit of the Ancient Golem", "", 600, 180, WardType.Stealth));
//            WardItems.Add(new WardItem(3342, "Scrying Orb", "", 2500, 2, WardType.Temp));
//            WardItems.Add(new WardItem(3363, "Farsight Orb", "", 4000, 2, WardType.Temp));
//            WardItems.Add(new WardItem(3187, "Hextech Sweeper", "", 800, 5, WardType.TempVision));
//            WardItems.Add(new WardItem(3159, "Grez's Spectral Lantern", "", 800, 5, WardType.Temp));
//            WardItems.Add(new WardItem(3364, "Oracle's Lens", "", 600, 10, WardType.TempVision));
//        }

//        public static WardItem GetWardItem()
//        {
//            return WardItems.FirstOrDefault(x => Items.HasItem(x.Id) && Items.CanUseItem(x.Id));
//        }

//        public static InventorySlot GetWardSlot()
//        {
//            return (from ward in WardItems where Items.CanUseItem(ward.Id) select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)ward.Id)).FirstOrDefault();
//        }

//        public class WardItem
//        {
//            public readonly int Id;
//            public int Duration;
//            public String Name;
//            public int Range;
//            public String SpellName;
//            public WardType Type;

//            public WardItem(int id, string name, string spellName, int range, int duration, WardType type)
//            {
//                Id = id;
//                Name = name;
//                SpellName = spellName;
//                Range = range;
//                Duration = duration;
//                Type = type;
//            }
//        }
//    }
//}
