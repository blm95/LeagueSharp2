using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Menu = LeagueSharp.Common.Menu;
using MenuItem = LeagueSharp.Common.MenuItem;

namespace SharpVision
{
    class WardLocation
    {
        public Vector3 Pos;
        public bool Grass;

        public WardLocation(Vector3 pos, bool grass)
        {
            Pos = pos;
            Grass = grass;
        }
    }

    class GrassLocation
    {
        public int Index;
        public int Count;

        public GrassLocation(int index, int count)
        {
            Index = index;
            Count = count;
        }

        void Update(int index)
        {

        }
    }
    class Program
    {
        private static Menu menu;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            //Game.PrintChat(ObjectManager.Player.ChampionName);
            //menu = new Menu("SharpVision", "SharpVision",true);
            //menu.AddItem(new MenuItem("v3add", "Print vector3")).SetValue(new KeyBind('G', KeyBindType.Press));
            //menu.AddToMainMenu();
            new Vision();
            //Drawing.OnDraw += Drawing_OnDraw;
        }
    }

    internal class Vision
    {
        //private static Orbwalking.Orbwalker Orbwalker;
        static Vector3 GetWardPos(Vector3 lastPos, int radius = 165, int precision = 3)
        {
            Vector3 averagePos = Vector3.Zero;

            int count = precision;
            //int calculated = 0;

            while (count > 0)
            {
                int vertices = radius;

                WardLocation[] wardLocations = new WardLocation[vertices];
                double angle = 2 * Math.PI / vertices;

                for (int i = 0; i < vertices; i++)
                {
                    double th = angle * i;
                    Vector3 pos = new Vector3((float)(lastPos.X + radius * Math.Cos(th)), (float)(lastPos.Y + radius * Math.Sin(th)), 0); //wardPos.Z
                    wardLocations[i] = new WardLocation(pos, NavMesh.IsWallOfGrass(pos.X,pos.Y,15));
                }

                List<GrassLocation> grassLocations = new List<GrassLocation>();

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
                    int midelement = (int)Math.Ceiling((float)grassLocation.Count / 2f);
                    //averagePos += wardLocations[grassLocation.Index + midelement - 1].Pos; //uncomment if using averagePos
                    lastPos = wardLocations[grassLocation.Index + midelement - 1].Pos; //comment if using averagePos
                    radius = (int)Math.Floor((float)radius / 2f); //precision recommended: 2-3; comment if using averagePos

                    //calculated++; //uncomment if using averagePos
                }

                count--;
            }

            return lastPos;//averagePos /= calculated; //uncomment if using averagePos
        }
        private readonly Menu menu;

        private static List<Vector3> btrj = new List<Vector3>
        {
            new Vector3(8220.802f, 10317.73f, 49.52979f),
            new Vector3(6309.327f, 10160.37f, 53.76758f),
            new Vector3(6601.623f, 11485.75f, 54.30273f),
            new Vector3(8249.981f, 11880.36f, 56.47681f),
            new Vector3(9433.619f, 11405.02f, 52.35803f),
            new Vector3(4389.655f, 11699.53f, 56.20984f),
            new Vector3(5984.331f, 12723.89f, 52.84729f)
        };

        private static List<Vector3> rtbj = new List<Vector3>
        {
            new Vector3(3662.149f, 10572.16f, -68.9812f),
            new Vector3(3536.412f, 8614.428f, 51.36267f),
            new Vector3(2737.07f, 7430.499f, 50.5332f),
            new Vector3(2626.522f, 5826.093f, 55.24133f),
            new Vector3(4656.315f, 7237.356f, 50.54712f),
            new Vector3(2282.413f, 9499.798f, 52.5625f)
        };

        private static List<Vector3> rtrj = new List<Vector3>
        {
            new Vector3(6440.534f, 4545.276f, 48.52856f),
            new Vector3(7132.777f, 3174.451f, 52.54138f),
            new Vector3(5575.772f, 3553.339f, 51.42151f),
            new Vector3(9170.073f, 2061.694f, 62.91931f),
            new Vector3(10426.78f, 3197.096f, 50.18909f),
            new Vector3(11699.53f, 4070.296f, -71.01721f),
            new Vector3(8675.587f, 4522.393f, 52.39221f),
            new Vector3(8547.483f, 4925.519f, 52.56323f),
            new Vector3(8215.596f, 3435.467f, 52.14319f)
        };

        private static List<Vector3> btbj = new List<Vector3>
        {
            new Vector3(11244.64f, 5976.256f, 49.97241f),
            new Vector3(11624.24f, 7127.95f, 51.72742f),
            new Vector3(10036.42f, 6598.407f, 48.80627f),
            new Vector3(10053.92f, 7927.768f, 51.75037f),
            new Vector3(11977.99f, 7574.393f, 52.29492f),
            new Vector3(12069.17f, 9322.568f, 52.30627f),
            new Vector3(2266.744f,10069.5f,54.2655f), //top tri blue side
            new Vector3(5345.119f,9201.656f,-71.24072f), //baron bush
            new Vector3(9330.775f,5683.915f,-71.24084f) //drag bush
        };
         
        public Vision()
        {
            wards.AddRange(btbj);
            wards.AddRange(rtrj);
            wards.AddRange(rtbj);
            wards.AddRange(btrj);
            menu = new Menu("SharpVision", "SharpVision", true);
            //Orbwalker = new Orbwalking.Orbwalker(menu.SubMenu("Orbwalking"));
            menu.AddItem(new MenuItem("PlaceWard", "Ward Closest Position to Cursor").SetValue(new KeyBind('Z', KeyBindType.Press)));
            menu.AddItem(new MenuItem("wVision", "Show Ward Vision?").SetValue(true));
            menu.AddItem(new MenuItem("hVision", "Show Hero/Minion Vision?").SetValue(true));
            //menu.AddItem(new MenuItem("Copy", "Copy to Clipboard").SetValue(new KeyBind('K', KeyBindType.Press)));
            //menu.AddItem(new MenuItem("Connect", "Add point").SetValue(new KeyBind('T', KeyBindType.Press)));
            //menu.AddItem(new MenuItem("Delete", "Delete points").SetValue(new KeyBind('V', KeyBindType.Press)));
            menu.AddItem(new MenuItem("v3add", "Show Predicted Vision")).SetValue(new KeyBind('G', KeyBindType.Press));
            menu.AddItem(new MenuItem("keypressward", "Auto-Ward on Combo").SetValue(new KeyBind(32, KeyBindType.Press)));
            menu.AddItem(new MenuItem("toggleward", "Toggle Auto-Ward").SetValue(new KeyBind('T', KeyBindType.Toggle)));
            menu.AddItem(new MenuItem("permashow", "Permashow Ward Positions?")).SetValue(true);
            menu.AddToMainMenu();
            lastwardtime = Environment.TickCount;
            Drawing.OnDraw += Game_OnDraw;
            //Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnCreate += Obj_AI_Base_OnCreate;
        }

        private void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {
            if (Environment.TickCount - wardtime > 8000)
            {
                if (sender.IsEnemy)
                {
                    if ((sender.Name.Contains("Rengar_Base_R_Alert") &&
                         ObjectManager.Player.HasBuff("rengarralertsound")) ||
                        sender.Name == "LeBlanc_Base_P_poof.troy")
                    {
                        if (Items.HasItem(2043))
                        {
                            Items.UseItem(2043, sender.Position);
                            wardtime = Environment.TickCount;
                        }
                    }
                }
            }
        }

        private static Dictionary<int,Vector3> blinkpos = new Dictionary<int, Vector3>();
        private static Dictionary<int, Vector3> startpos = new Dictionary<int, Vector3>();
        private static int wardtime;

        private void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var keypresson = menu.Item("keypressward").GetValue<KeyBind>().Active;
            var toggled = menu.Item("toggleward").GetValue<KeyBind>().Active;
            if (Environment.TickCount - wardtime > 8000)
            {
                if (sender.IsEnemy)
                {
                    var time = Environment.TickCount;
                    var spellname = args.SData.Name.ToLower();
                    if (spellname == "deceive")
                    {
                        blinkpos.Add(time, args.Start + (args.End - args.Start).Normalized()*400);
                        startpos.Add(time, sender.Position);
                        if (keypresson || toggled)
                        {
                            if (ObjectManager.Player.Distance(args.Start + (args.End - args.Start).Normalized()*400) <
                                650)
                            {
                                if (Items.HasItem(2043))
                                {
                                    Items.UseItem(2043, args.Start + (args.End - args.Start).Normalized()*400);
                                    wardtime = Environment.TickCount;
                                }
                                else if (Items.HasItem(3364))
                                {
                                    Items.UseItem(3364, args.Start + (args.End - args.Start).Normalized()*400);
                                    wardtime = Environment.TickCount;
                                }
                            }
                        }
                    }
                    else if (spellname == "vaynetumble")
                    {
                        if (sender.HasBuff("VayneInquisition"))
                        {
                            blinkpos.Add(time, args.Start + (args.End - args.Start).Normalized()*300);
                            startpos.Add(time, sender.Position);
                            if (ObjectManager.Player.Distance(args.Start + (args.End - args.Start).Normalized()*300) <
                                650)
                            {
                                if (keypresson || toggled)
                                {
                                    if (Items.HasItem(2043))
                                    {
                                        Items.UseItem(2043, args.Start + (args.End - args.Start).Normalized()*300);
                                        wardtime = Environment.TickCount;
                                    }
                                    else if (Items.HasItem(3364))
                                    {
                                        Items.UseItem(3364, args.Start + (args.End - args.Start).Normalized()*300);
                                        wardtime = Environment.TickCount;
                                    }
                                }
                            }
                        }
                    }
                    else if (spellname == "summonerflash" || spellname == "khazixe" || spellname == "ezreale")
                    {
                        //if (!sender.IsVisible)
                        //{
                        blinkpos.Add(time, args.Start + (args.End - args.Start).Normalized()*425);
                        startpos.Add(time, sender.Position);
                        if (ObjectManager.Player.Distance(args.End) < 1100)
                        {
                            if (keypresson || toggled)
                            {
                                var wardslot = Items.GetWardSlot();
                                if (wardslot != null)
                                {
                                    Items.UseItem((int) wardslot.Id,
                                        args.Start + (args.End - args.Start).Normalized()*425);
                                    wardtime = Environment.TickCount;
                                }
                                else if (Items.HasItem(2043))
                                {
                                    Items.UseItem(2043, args.Start + (args.End - args.Start).Normalized()*425);
                                    wardtime = Environment.TickCount;
                                }
                                else if (Items.HasItem(3364))
                                {
                                    Items.UseItem(3364, args.Start + (args.End - args.Start).Normalized()*425);
                                    wardtime = Environment.TickCount;
                                }
                                else if (ObjectManager.Player.ChampionName == "Caitlyn")
                                {
                                    //Game.PrintChat("casting w...");
                                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W,
                                        args.Start + (args.End - args.Start).Normalized()*425);
                                }
                            }
                        }
                        // }
                    }
                    else if (spellname == "hideinshadows" || spellname == "talonshadowassault" ||
                             spellname == "monkeykingdecoy" || spellname == "khazixrlong" || spellname == "khazixr" ||
                             spellname == "akalismokebomb")
                    {
                        blinkpos.Add(time, sender.Position);
                        startpos.Add(time, sender.Position); if (keypresson || toggled)
                        {
                            if (ObjectManager.Player.Distance(sender.Position) < 650)
                            {
                                if (Items.HasItem(2043))
                                {
                                    Items.UseItem(2043, sender.Position);
                                    wardtime = Environment.TickCount;
                                }
                                else if (Items.HasItem(3364))
                                {
                                    Items.UseItem(3364, sender.Position);
                                    wardtime = Environment.TickCount;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int lastwardtime;

        private static Items.Item ScryingOrb2 = new Items.Item(3363, 3500f);
        private static Items.Item ScryingOrb = new Items.Item(3342, 3500f);


        public static bool intersectsObj(Vector3 position, bool checkbrush = false)
        {
            if (!NavMesh.GetCollisionFlags(Game.CursorPos).HasFlag(CollisionFlags.Grass) && !NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Grass))
            {
                return !NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Building) &&
                       NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall);
            }
            else
            {
                if (position.Distance(origbushspot) < 5)
                {
                    return false;
                }
                if (NavMesh.GetCollisionFlags(Game.CursorPos).HasFlag(CollisionFlags.Grass))
                {
                    if (NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Grass) &&
                        position.Distance(Game.CursorPos) > 515)
                    {
                        return true;
                    }

                    return !NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Building) &&
                           NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall);
                }
            }
            return true;
        }

        private static int lastTimeWarded;
        private static Vector3 origbushspot;
        private static readonly List<Vector3> polygonPoints = new List<Vector3>();
        private static List<Vector3> wards = new List<Vector3>(); 

        private void Game_OnDraw(EventArgs args)
        {
            foreach (var k in blinkpos)
            {
                if (Environment.TickCount - k.Key < 3000)
                {
                    var end = Drawing.WorldToScreen(k.Value);
                    var start = Drawing.WorldToScreen(startpos[k.Key]);
                    Drawing.DrawLine(start, end, 1, System.Drawing.Color.Yellow);
                    Drawing.DrawCircle(k.Value, 125, System.Drawing.Color.Red);
                }
                else
                {
                    blinkpos.Remove(k.Key);
                    startpos.Remove(k.Key);
                }
            }
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && !x.IsDead))
            {
                if (enemy.Distance(ObjectManager.Player.ServerPosition) <= 1200) //check real ward range later
                {
                    int radius = 165;

                    Vector3 bestWardPos = GetWardPos(enemy.ServerPosition, radius, 2);

                    if (bestWardPos != enemy.ServerPosition && bestWardPos != Vector3.Zero)
                    {
                        if (!enemy.IsVisible && (lastTimeWarded == 0 || Environment.TickCount - lastTimeWarded > 1000) &&
                            Vector3.Distance(ObjectManager.Player.Position, bestWardPos) <= 600 &&
                            menu.Item("keypressward").GetValue<KeyBind>().Active)
                        {
                            var wardslot = Items.GetWardSlot();
                            if (wardslot != null)
                            {
                                Items.UseItem((int) wardslot.Id, bestWardPos);
                            }
                            if (ScryingOrb.IsReady())
                            {
                                ScryingOrb.Cast(bestWardPos);
                            }
                            if (ScryingOrb2.IsReady())
                            {
                                ScryingOrb2.Cast(bestWardPos);
                            }
                            lastTimeWarded = Environment.TickCount;
                        }

                        Utility.DrawCircle(bestWardPos, radius, System.Drawing.Color.Red);
                    }

                    //Utility.DrawCircle(enemy.ServerPosition, radius, System.Drawing.Color.Yellow);
                }
            }

            if (menu.Item("permashow").GetValue<bool>())
            {
                foreach (var k in wards)
                {
                    Drawing.DrawCircle(k, 200, System.Drawing.Color.Red);
                }
            }

            if (menu.Item("PlaceWard").GetValue<KeyBind>().Active)
            {
                if (!menu.Item("permashow").GetValue<bool>())
                {
                    foreach (var k in wards)
                    {
                        Drawing.DrawCircle(k, 200, System.Drawing.Color.Red);
                    }
                }
                var wardslot = Items.GetWardSlot();
                if (wardslot != null && wardslot.Id != (ItemId)2050 && wardslot.Id != (ItemId)3154 &&
                    wardslot.Id != (ItemId)3350)
                {
                    if (Environment.TickCount - lastwardtime > 800)
                    {
                        float closestpos = 10000;
                        var wardpos = new Vector3();
                        foreach (var k in wards)
                        {
                            var dist = k.Distance(Game.CursorPos);
                            if (dist < closestpos)
                            {
                                closestpos = dist;
                                wardpos = k;
                            }

                            //if (Game.CursorPos.Distance(k) < 50)
                            //{
                            //var wardslot = Items.GetWardSlot();
                            //if (wardslot != null && wardslot.Id != (ItemId) 2050 && wardslot.Id != (ItemId) 3154 &&
                            //    wardslot.Id != (ItemId) 3350)
                            //{
                            //    //Game.PrintChat("Slot: " + (SpellSlot) (wardslot.Slot + 4) + " Name: " +
                            //    //               wardslot.IData.DisplayName);
                            //Items.UseItem((int) wardslot.Id, k);
                            //lastwardtime = Environment.TickCount;
                            //}
                            // }
                        }
                        Items.UseItem((int)wardslot.Id, wardpos);
                        lastwardtime = Environment.TickCount;
                    }
                }
            }
            //if (menu.Item("Copy").GetValue<KeyBind>().Active)
            //{
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var d in polygonPoints.ToList())
            //    {
            //        sb.Append("new Vector3(" + d.X + "f," + d.Y + "f," + d.Z + "f),");
            //    }

            //    Clipboard.SetText(sb.ToString());
            //}

            //if (menu.Item("Delete").GetValue<KeyBind>().Active)
            //{
            //    if (polygonPoints.Count > 2)
            //    {
            //        polygonPoints.Clear();
            //    }
            //}

            //if (polygonPoints.Count > 1)
            //{
            //    for (int i = 0; i < polygonPoints.Count - 1; i++)
            //    {
            //        var q = Drawing.WorldToScreen(new Vector3(new Vector2(polygonPoints[i].X, polygonPoints[i].Y),
            //            polygonPoints[i].Z));

            //        var z = Drawing.WorldToScreen(new Vector3(new Vector2(polygonPoints[i + 1].X, polygonPoints[i + 1].Y),
            //            polygonPoints[i + 1].Z));

            //        Drawing.DrawLine(q, z, 5, System.Drawing.Color.Red);
            //    }
            //}

            //if (menu.Item("Connect").GetValue<KeyBind>().Active)
            //{
            //    var mousePos = Game.CursorPos;
            //    if (!polygonPoints.Any(x => x.Distance(mousePos) < 75))
            //    {
            //        polygonPoints.Add(mousePos);
            //    }
            //}

            if (menu.Item("v3add").GetValue<KeyBind>().Active)
            {
                //if (menu.Item("mVision").GetValue<bool>())
                //{
                //    showVision(System.Drawing.Color.Blue, 1100);
                //}
                if (menu.Item("wVision").GetValue<bool>())
                {
                    showVision(System.Drawing.Color.LightGreen, 900);
                }
                if (menu.Item("hVision").GetValue<bool>())
                {
                    showVision(System.Drawing.Color.Red, 1350);
                }
            }
        }

        private static void showVision(System.Drawing.Color color, int range)
        {
            var result = Game.CursorPos;
            origbushspot = result;
            int tamNhin = range; //result is Obj_AI_Hero || result is Obj_AI_Turret ? 1300 : 1200;
            var listPoint = new List<Vector2>();
            for (int i = 0; i <= 360; i += 1)
            {
                double cosX = Math.Cos(i * Math.PI / 180);
                double sinX = Math.Sin(i * Math.PI / 180);
                var outer = new Vector3(
                    (float)(result.X + tamNhin * cosX), (float)(result.Y + tamNhin * sinX),
                    ObjectManager.Player.Position.Z);
                for (int j = 0; j < tamNhin; j += 100)
                {
                    var position = new Vector3(
                        (float)(result.X + j * cosX), (float)(result.Y + j * sinX),
                        ObjectManager.Player.Position.Z);
                    if (!intersectsObj(position, true))
                    {
                        continue;
                    }
                    if (j != 0)
                    {
                        int left = j - 99, right = j;
                        do
                        {
                            int middle = (left + right) / 2;
                            position = new Vector3(
                                (float)(result.X + middle * cosX), (float)(result.Y + middle * sinX),
                                ObjectManager.Player.Position.Z);
                            if (intersectsObj(position, true))
                            {
                                right = middle;
                            }
                            else
                            {
                                left = middle + 1;
                            }
                        } while (left < right);
                    }
                    outer = position;
                    break;
                }
                listPoint.Add(Drawing.WorldToScreen(outer));
            }
            for (int i = 0; i < listPoint.Count - 1; i++)
            {
                Drawing.DrawLine(listPoint[i], listPoint[i + 1], 1, color);
            }
        }
    }
}
