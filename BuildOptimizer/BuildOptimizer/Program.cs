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

        private static Menu menu;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            menu = new Menu("Build Optimizer","BO",true);

            menu.AddItem(new MenuItem("xpos", "X block")).SetValue(new Slider(1300, 0, 2500));
            menu.AddItem(new MenuItem("ypos", "Y block")).SetValue(new Slider(940, 0, 2500));

            //menu.AddItem(new MenuItem("ADX", "AD display position X")).SetValue(new Slider(1300, 0, 2500));
            //menu.AddItem(new MenuItem("ADY", "AD display position Y")).SetValue(new Slider(940, 0, 2500));

            //menu.AddItem(new MenuItem("APX", "AP display position X")).SetValue(new Slider(1300, 0, 2500));
            //menu.AddItem(new MenuItem("APY", "AP display position Y")).SetValue(new Slider(960, 0, 2500));

            //menu.AddItem(new MenuItem("ArmorX", "Armor display position X")).SetValue(new Slider(1300, 0, 2500));
            //menu.AddItem(new MenuItem("ArmorY", "Armor display position Y")).SetValue(new Slider(980, 0, 2500));

            //menu.AddItem(new MenuItem("MRX", "MR display position X")).SetValue(new Slider(1300, 0, 2500));
            //menu.AddItem(new MenuItem("MRY", "MR display position Y")).SetValue(new Slider(1000, 0, 2500));

            //menu.AddItem(new MenuItem("badArmorX", "Armor inaccuracy X")).SetValue(new Slider(1480, 0, 2500));
            //menu.AddItem(new MenuItem("badArmorY", "Armor inaccuracy Y")).SetValue(new Slider(980, 0, 2500));

            //menu.AddItem(new MenuItem("badMRX", "MR inaccuracy X")).SetValue(new Slider(1480, 0, 2500));
            //menu.AddItem(new MenuItem("badMRY", "MR inaccuracy Y")).SetValue(new Slider(1000, 0, 2500));

            foreach (var k in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy))
            {
                ADstats.Add(k.ChampionName,k.TotalAttackDamage);
                APstats.Add(k.ChampionName,k.TotalMagicalDamage);
                ArmorStats.Add(k.ChampionName,k.Armor);
                MRStats.Add(k.ChampionName,k.SpellBlock);
            }

            menu.AddToMainMenu();

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

            var xpos = menu.Item("xpos").GetValue<Slider>().Value;
            var ypos = menu.Item("ypos").GetValue<Slider>().Value;

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
            Drawing.DrawText(xpos, ypos, System.Drawing.Color.Red, "Total Enemy AD: " + (int)totalAD);
            Drawing.DrawText(xpos, ypos+20, System.Drawing.Color.LightSkyBlue, "Total Enemy AP: " + (int)totalAP);
            Drawing.DrawText(xpos, ypos+40, System.Drawing.Color.DarkOrange, "Total Enemy Armor: " + (int)totalArmor);
            if (Armoralert)
            {
                Drawing.DrawText(xpos+180, ypos+40, System.Drawing.Color.Red, "** not accurate"); 
            }
            Drawing.DrawText(xpos, ypos+60, System.Drawing.Color.SpringGreen, "Total Enemy MR: " + (int)totalMR);
            if (MRalert)
            {
                Drawing.DrawText(xpos+180, ypos+60, System.Drawing.Color.Red, "** not accurate");
            }
        }
    }
}
