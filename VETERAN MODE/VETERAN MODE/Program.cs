using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace VETERAN_MODE
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Game_OnGameLoad(EventArgs args)
        {
            Game.PrintChat("VETERAN MODE ENABLED");
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if(args.PacketData[0] == 0x87)
                args.Process = false;
        }
    }
}
