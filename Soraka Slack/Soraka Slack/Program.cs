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
        private static readonly string[] ad =
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Graves", "KogMaw",
            "MissFortune", "Quinn", "Sivir", "Tristana", "Twitch", "Varus", "Vayne", "Jinx", "Lucian"
        };

        private static Obj_AI_Hero follow;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Soraka") return;
            BushRevealer c = new BushRevealer();
            Q = new Spell(SpellSlot.Q, 970);
            W = new Spell(SpellSlot.W, 550);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R);
            ts = new TargetSelector(1025, TargetSelector.TargetingMode.AutoPriority);
            stufftosay = new[] { "brb", "need to b", "sec" };
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
                if(ad.Contains(ally.ChampionName))
                    menu.SubMenu("follower").AddItem(new MenuItem(ally.ChampionName, ally.ChampionName).SetValue(true));
                else
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
            Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(3340)).Send();
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
            if (saycounter == 3)
                saycounter = 0;
            //if (menu.Item("on").GetValue<KeyBind>().Active)
            //{
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

            if (follow.IsDead)
            {
                follow = ObjectManager.Get<Obj_AI_Hero>().First(x => x.Distance(ObjectManager.Player) < 1300);
            }

            Console.WriteLine(follow.IsDead);
            if ((follow.IsDead ||
                 (follow.Distance(ObjectManager.Player.Position) > 5000 && !Utility.InShopRange() &&
                  spawn.Distance(follow.Position) < 1500) ||
                 ObjectManager.Player.Health/ObjectManager.Player.MaxHealth*100 <
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
                                x.IsAlly && x.Health/x.MaxHealth*100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
                                !x.IsDead && x.Distance(ObjectManager.Player.Position) < 550);
                var objAiHeroes = allies2 as Obj_AI_Hero[] ?? allies2.ToArray();
                if (objAiHeroes.Any() &&
                    ObjectManager.Player.Health/ObjectManager.Player.MaxHealth*100 >
                    menu.Item("wabovehp").GetValue<Slider>().Value)
                    W.Cast(objAiHeroes.First());
            }

            if (menu.Item("user").GetValue<bool>() && R.IsReady())
            {
                var allies =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            x =>
                                x.IsAlly && x.Health/x.MaxHealth*100 < menu.Item("allyhpr").GetValue<Slider>().Value &&
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
                        ObjectManager.Player.Health/ObjectManager.Player.MaxHealth*100 >
                        menu.Item("wabovehp").GetValue<Slider>().Value)
                    {
                        if (follow.Health/follow.MaxHealth*100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
                            follow.Distance(ObjectManager.Player.Position) < 550 &&
                            ObjectManager.Player.Health/ObjectManager.Player.MaxHealth*100 >
                            menu.Item("wabovehp").GetValue<Slider>().Value)
                        {
                            W.Cast(follow);
                        }
                        else if (follow.Health/follow.MaxHealth*100 < menu.Item("allyhpw").GetValue<Slider>().Value &&
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
                    var xPos = ((spawn.X - follow.Position.X)/Vector3.Distance(follow.Position, spawn))*300 +
                               follow.Position.X -
                               x.Next(25, 150);
                    var yPos = ((spawn.Y - follow.Position.Y)/Vector3.Distance(follow.Position, spawn))*300 +
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
                    var xPos = ((spawn.X - turret.Position.X)/Vector3.Distance(turret.Position, spawn))*300 +
                               turret.Position.X -
                               y.Next(25, 150);
                    var yPos = ((spawn.Y - turret.Position.Y)/Vector3.Distance(turret.Position, spawn))*300 +
                               turret.Position.Y -
                               y.Next(25, 150);

                    var vec = new Vector3(xPos, yPos, follow.Position.Z);
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, vec);
                }
            }

        }
    }

    internal class BushRevealer //By Beaving & Blm95
    {
        private readonly List<PlayerInfo> _playerInfo = new List<PlayerInfo>();
        private int _lastTimeWarded;

        public BushRevealer()
        {
            _playerInfo = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy).Select(x => new PlayerInfo(x)).ToList();
            Game.OnGameUpdate += Game_OnGameUpdate;
        }


        private void Game_OnGameUpdate(EventArgs args)
        {
            int time = Environment.TickCount;

            foreach (PlayerInfo playerInfo in _playerInfo.Where(x => x.Player.IsVisible))
                playerInfo.LastSeen = time;

            Wards.WardItem ward = Wards.GetWardItem();
            if (ward == null)
                return;


            foreach (Obj_AI_Hero enemy in _playerInfo.Where(x =>
                x.Player.IsValid &&
                !x.Player.IsVisible &&
                !x.Player.IsDead &&
                x.Player.Distance(ObjectManager.Player.ServerPosition) < 1000 && //check real ward range later
                time - x.LastSeen < 2500).Select(x => x.Player))
            {
                Vector3 bestWardPos = GetWardPos(enemy.ServerPosition, 165, 2);

                if (bestWardPos != enemy.ServerPosition && bestWardPos != Vector3.Zero &&
                    bestWardPos.Distance(ObjectManager.Player.ServerPosition) < ward.Range)
                {
                    if (_lastTimeWarded == 0 || Environment.TickCount - _lastTimeWarded > 500)
                    {
                        InventorySlot wardSlot = Wards.GetWardSlot();

                        if (wardSlot != null && wardSlot.Id != ItemId.Unknown)
                        {
                            wardSlot.UseItem(bestWardPos);
                            _lastTimeWarded = Environment.TickCount;
                        }
                    }
                }
            }

        }

        private class PlayerInfo
        {
            public readonly Obj_AI_Hero Player;
            public int LastSeen;

            public PlayerInfo(Obj_AI_Hero player)
            {
                Player = player;
            }
        }

        private Vector3 GetWardPos(Vector3 lastPos, int radius = 165, int precision = 3)
        {
            //Vector3 averagePos = Vector3.Zero;

            int count = precision;
            //int calculated = 0;

            while (count > 0)
            {
                int vertices = radius;

                var wardLocations = new WardLocation[vertices];
                double angle = 2 * Math.PI / vertices;

                for (int i = 0; i < vertices; i++)
                {
                    double th = angle * i;
                    var pos = new Vector3((float)(lastPos.X + radius * Math.Cos(th)),
                        (float)(lastPos.Y + radius * Math.Sin(th)), 0); //wardPos.Z
                    wardLocations[i] = new WardLocation(pos, NavMesh.IsWallOfGrass(pos));
                }

                var grassLocations = new List<GrassLocation>();

                for (int i = 0; i < wardLocations.Length; i++)
                {
                    if (wardLocations[i].Grass)
                    {
                        if (i != 0 && wardLocations[i - 1].Grass)
                            grassLocations.Last().Count++;
                        else
                            grassLocations.Add(new GrassLocation(i, 1));
                    }
                }

                GrassLocation grassLocation = grassLocations.OrderByDescending(x => x.Count).FirstOrDefault();

                if (grassLocation != null) //else: no pos found. increase/decrease radius?
                {
                    var midelement = (int)Math.Ceiling(grassLocation.Count / 2f);
                    //averagePos += wardLocations[grassLocation.Index + midelement - 1].Pos; //uncomment if using averagePos
                    lastPos = wardLocations[grassLocation.Index + midelement - 1].Pos; //comment if using averagePos
                    radius = (int)Math.Floor(radius / 2f); //precision recommended: 2-3; comment if using averagePos

                    //calculated++; //uncomment if using averagePos
                }

                count--;
            }

            return lastPos; //averagePos /= calculated; //uncomment if using averagePos
        }

        private class WardLocation
        {
            public readonly bool Grass;
            public readonly Vector3 Pos;

            public WardLocation(Vector3 pos, bool grass)
            {
                Pos = pos;
                Grass = grass;
            }
        }

        private class GrassLocation
        {
            public readonly int Index;
            public int Count;

            public GrassLocation(int index, int count)
            {
                Index = index;
                Count = count;
            }
        }
    }

    internal class Wards
    {
        public enum WardType
        {
            Stealth,
            Vision,
            Temp,
            TempVision
        }

        public static readonly List<WardItem> WardItems = new List<WardItem>();

        static Wards()
        {
            WardItems.Add(new WardItem(3360, "Feral Flare", "", 1000, 180, WardType.Stealth));
            WardItems.Add(new WardItem(2043, "Vision Ward", "VisionWard", 600, 180, WardType.Vision));
            WardItems.Add(new WardItem(2044, "Stealth Ward", "SightWard", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3154, "Wriggle's Lantern", "WriggleLantern", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(2045, "Ruby Sightstone", "ItemGhostWard", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(2049, "Sightstone", "ItemGhostWard", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(2050, "Explorer's Ward", "ItemMiniWard", 600, 60, WardType.Stealth));
            WardItems.Add(new WardItem(3340, "Greater Stealth Totem", "", 600, 120, WardType.Stealth));
            WardItems.Add(new WardItem(3361, "Greater Stealth Totem", "", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3362, "Greater Vision Totem", "", 600, 180, WardType.Vision));
            WardItems.Add(new WardItem(3366, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
            WardItems.Add(new WardItem(3367, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
            WardItems.Add(new WardItem(3368, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
            WardItems.Add(new WardItem(3369, "Bonetooth Necklace", "", 600, 120, WardType.Stealth));
            WardItems.Add(new WardItem(3371, "Bonetooth Necklace", "", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3375, "Head of Kha'Zix", "", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3205, "Quill Coat", "", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3207, "Spirit of the Ancient Golem", "", 600, 180, WardType.Stealth));
            WardItems.Add(new WardItem(3342, "Scrying Orb", "", 2500, 2, WardType.Temp));
            WardItems.Add(new WardItem(3363, "Farsight Orb", "", 4000, 2, WardType.Temp));
            WardItems.Add(new WardItem(3187, "Hextech Sweeper", "", 800, 5, WardType.TempVision));
            WardItems.Add(new WardItem(3159, "Grez's Spectral Lantern", "", 800, 5, WardType.Temp));
            WardItems.Add(new WardItem(3364, "Oracle's Lens", "", 600, 10, WardType.TempVision));
        }

        public static WardItem GetWardItem()
        {
            return WardItems.FirstOrDefault(x => Items.HasItem(x.Id) && Items.CanUseItem(x.Id));
        }

        public static InventorySlot GetWardSlot()
        {
            foreach (WardItem ward in WardItems)
            {
                if (Items.CanUseItem(ward.Id))
                {
                    return ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)ward.Id);
                }
            }
            return null;
        }

        public class WardItem
        {
            public readonly int Id;
            public int Duration;
            public String Name;
            public int Range;
            public String SpellName;
            public WardType Type;

            public WardItem(int id, string name, string spellName, int range, int duration, WardType type)
            {
                Id = id;
                Name = name;
                SpellName = spellName;
                Range = range;
                Duration = duration;
                Type = type;
            }
        }
    }
}
