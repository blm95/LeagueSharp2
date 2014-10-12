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
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            var c = BitConverter.ToString(args.PacketData);
            if(c[0].ToString() + c[1].ToString() == "87")
                args.Process = false;
        }
    }
}
