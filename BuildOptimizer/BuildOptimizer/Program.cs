using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace BuildOptimizer
{
    class Program
    {
        private static Dictionary<string, float> ADstats = new Dictionary<string, float>();
        private static Dictionary<string, float> APstats = new Dictionary<string, float>();

        private static Dictionary<string, float> ArmorStats = new Dictionary<string, float>();
        private static Dictionary<string, float> MRStats = new Dictionary<string, float>();

        private static float totalAD;
        private static float totalAP;
        private static float totalArmor;
        private static float totalMR;

        private static bool MRalert = false;
        private static bool Armoralert = false;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            foreach (var k in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy))
            {
                ADstats.Add(k.ChampionName,k.TotalAttackDamage);
                APstats.Add(k.ChampionName,k.TotalMagicalDamage);
                ArmorStats.Add(k.ChampionName,k.Armor);
                MRStats.Add(k.ChampionName,k.SpellBlock);
            }
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            totalAD = 0;
            totalAP = 0;
            totalArmor = 0;
            totalMR = 0;

            MRalert = false;
            Armoralert = false;

            foreach (var k in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.IsVisible))
            {
                if (k.TotalAttackDamage != ADstats[k.ChampionName])
                {
                    ADstats[k.ChampionName] = k.TotalAttackDamage;
                }

                if (k.TotalMagicalDamage != APstats[k.ChampionName])
                {
                    APstats[k.ChampionName] = k.TotalMagicalDamage;
                }

                if (k.Armor != ArmorStats[k.ChampionName])
                {
                    ArmorStats[k.ChampionName] = k.Armor;
                }

                if (k.SpellBlock != MRStats[k.ChampionName])
                {
                    MRStats[k.ChampionName] = k.SpellBlock;
                }
            }

            foreach (var k in ADstats.Values)
            {
                totalAD += k;
            }

            foreach (var k in APstats.Values)
            {
                totalAP += k;
            }

            foreach (var k in ArmorStats.Values)
            {
                totalArmor += k;
            }

            foreach (var k in MRStats.Values)
            {
                totalMR += k;
            }

            foreach (var k in ArmorStats.Values)
            {
                if (k > totalArmor/3)
                {
                    Armoralert = true;
                }
            }

            foreach (var k in MRStats.Values)
            {
                if (k > totalMR / 3)
                {
                    MRalert = true;
                }
            }

            //var k = Drawing.WorldToScreen(new Vector3(960, 540, 0));
            Drawing.DrawText(1300, 940, System.Drawing.Color.Red, "Total Enemy AD: " + (int)totalAD);
            Drawing.DrawText(1300, 960, System.Drawing.Color.LightSkyBlue, "Total Enemy AP: " + (int)totalAP);
            Drawing.DrawText(1300, 980, System.Drawing.Color.DarkOrange, "Total Enemy Armor: " + (int)totalArmor);
            if (Armoralert)
            {
                Drawing.DrawText(1480, 980, System.Drawing.Color.Red, "** not accurate"); 
            }
            Drawing.DrawText(1300, 1000, System.Drawing.Color.SpringGreen, "Total Enemy MR: " + (int)totalMR);
            if (MRalert)
            {
                Drawing.DrawText(1480, 1000, System.Drawing.Color.Red, "** not accurate");
            }
        }
    }
}
