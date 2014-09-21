using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Windows.Input;

namespace LeagueSharpTemp
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
        static void Main(string[] args)
        {
            Game.OnGameStart += Game_OnGameStart;

            if (Game.Mode == GameMode.Running)
                Game_OnGameStart(new EventArgs());
        }

        private static void Game_OnGameStart(EventArgs args)
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        static void Game_OnGameUpdate(EventArgs args)
        {

        }

        static int lastTimeWarded;

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
                    wardLocations[i] = new WardLocation(pos, NavMesh.IsWallOfGrass(pos));
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

        static void Drawing_OnDraw(EventArgs args)
        {
            Obj_AI_Hero enemy = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy).First();

            if (enemy.Distance(ObjectManager.Player.ServerPosition) <= 1000) //check real ward range later
            {
                int radius = 165;

                Vector3 bestWardPos = GetWardPos(enemy.ServerPosition, radius, 2);

                if (bestWardPos != enemy.ServerPosition && bestWardPos != Vector3.Zero)
                {
                    if (!enemy.IsVisible && (lastTimeWarded == 0 || Environment.TickCount - lastTimeWarded > 1000) && Vector3.Distance(ObjectManager.Player.Position, bestWardPos) <= 600 && Keyboard.IsKeyDown(Key.Space))
                    {
                        LeagueSharp.Common.Items.GetWardSlot().UseItem(bestWardPos);
                        lastTimeWarded = Environment.TickCount;
                    }

                    Utility.DrawCircle(bestWardPos, radius, System.Drawing.Color.Red);
                }

                Utility.DrawCircle(enemy.ServerPosition, radius, System.Drawing.Color.Yellow);
            }
        }
    }
}