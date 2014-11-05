using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Collision = LeagueSharp.Common.Collision;

namespace GrabAutos
{
    class Program
    {
        private static Menu config;
        private static Spell Q;
        private static PredictionInput p;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if(ObjectManager.Player.ChampionName != "Blitzcrank") return;
            config = new Menu("AutoGrab", "grab", true);
            Menu menuTS = new Menu("Selector: ", "targ");
            SimpleTs.AddToMenu(menuTS);
            config.AddSubMenu(menuTS);
            config.AddItem(new MenuItem("grabem", "Pull While AA'ing")).SetValue(new KeyBind(32, KeyBindType.Press));
            config.AddToMainMenu();
            p = new PredictionInput {Delay = .25f, Radius = 35, Speed = 1800};
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (config.Item("grabem").GetValue<KeyBind>().Active)
            {
                GamePacket g = new GamePacket(args.PacketData);
                if (g.Header == 0xFE)
                {
                    g.Position = 1;
                    var k = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(g.ReadInteger());
                    if (k is Obj_AI_Hero && k.IsEnemy)
                    {
                        if (Vector3.Distance(k.Position, ObjectManager.Player.Position) <= 925)
                        {
                            //Game.PrintChat(k.Name + " auto'd");
                            List<Vector3> v = new List<Vector3> {k.Position};
                            var l = LeagueSharp.Common.Collision.GetCollision(v, p);
                            if (l.Count == 0)
                            {
                                Game.PrintChat("casting q");
                                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, k.Position);
                            }
                        }
                    }
                }
            }
        }
    }
}
