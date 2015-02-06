using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Menu = LeagueSharp.Common.Menu;
using MenuItem = LeagueSharp.Common.MenuItem;

namespace Polygon_Helper
{
    class Program
    {
        private static Menu menu;
        static void Main(string[] args)
        {
            Drawing.OnDraw += Drawing_OnDraw;
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("Copy").GetValue<KeyBind>().Active)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var d in polygonPoints.ToList())
                {
                    sb.Append("new Vector3(" + d.X + "," + d.Y + "," + d.Z + "),");
                }

                Clipboard.SetText(sb.ToString());
            }

            if (IsPointInPolygon(polygonPoints, ObjectManager.Player.Position))
            {
                Game.PrintChat("PLAYER IN POLYGON");
            }

            if (menu.Item("Connect").GetValue<KeyBind>().Active)
            {
                var mousePos = Game.CursorPos;
                if (!polygonPoints.Any(x => x.Distance(mousePos) < 75))
                {
                    polygonPoints.Add(mousePos);
                }
            }

            if (menu.Item("Delete").GetValue<KeyBind>().Active)
            {
                if (polygonPoints.Count > 2)
                {
                    polygonPoints.Clear();
                }
            }
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            menu = new Menu("Polygons", "Polygons", true);
            menu.AddItem(new MenuItem("Connect", "Connect points").SetValue(new KeyBind('G', KeyBindType.Press)));
            menu.AddItem(new MenuItem("Delete", "Delete points").SetValue(new KeyBind('V', KeyBindType.Press)));
            menu.AddItem(new MenuItem("Copy", "Copy to Clipboard").SetValue(new KeyBind('K', KeyBindType.Press)));
            menu.AddToMainMenu();
        }

        private static bool IsPointInPolygon(IList<Vector3> polygon, Vector3 point)
        {
            bool isInside = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }


        static void Drawing_OnDraw(EventArgs args)
        {
            if (polygonPoints.Count > 1)
            {
                for (int i = 0; i < polygonPoints.Count; i++)
                {
                    var q = Drawing.WorldToScreen(new Vector3(new Vector2(polygonPoints[i].X, polygonPoints[i].Y),
                        polygonPoints[i].Z));

                    var z = Drawing.WorldToScreen(new Vector3(new Vector2(polygonPoints[i + 1].X, polygonPoints[i + 1].Y),
                        polygonPoints[i + 1].Z));

                    Drawing.DrawLine(q, z, 5, System.Drawing.Color.Red);
                }
            }
        }

        private static readonly List<Vector3> polygonPoints = new List<Vector3>();
    }
}
