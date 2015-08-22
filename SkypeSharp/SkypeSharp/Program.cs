using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SKYPE4COMLib;

namespace SkypeSharp
{
    internal class Program
    {
        private static Skype k;
        private static string text;
        private static string sender;
        private static string skypename;
        private static Menu config;
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        private static void Main(string[] args)
        {
            Console.WriteLine("Loading Skype#...");
            k = new Skype();
            k.Attach(7, false);
            k.MessageStatus +=
                new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);
            Game.OnInput += Game_OnInput;
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            //k.SendMessage("shiver.x", "test");

        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            config = new Menu("Skype#","Skype#",true);
            config.AddItem(new MenuItem("xpos", "X Pos").SetValue(new Slider(0, 0, 2560)));
            config.AddItem(new MenuItem("ypos", "Y Pos")).SetValue(new Slider(0, 0, 1440));
            config.AddItem(new MenuItem("clear", "Clear Messages")).SetValue(new KeyBind('P', KeyBindType.Press));
            config.AddToMainMenu();
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static string savedmsg;

        private static Dictionary<string,List<string>> skypeMsgs = new Dictionary<string, List<string>>(); 
        private static Dictionary<string,System.Drawing.Color> colors = new Dictionary<string, Color>(); 
        private static List<string> savedmsgs = new List<string>();
        private static void Drawing_OnDraw(EventArgs args)
        {
            //Console.WriteLine("ok...");
            if (config.Item("clear").GetValue<KeyBind>().Active)
            {
                savedmsgs.Clear();
                skypeMsgs.Clear();
            }
            var xpos = config.Item("xpos").GetValue<Slider>().Value;
            //Console.WriteLine("X: "+xpos);
            var ypos = config.Item("ypos").GetValue<Slider>().Value;

            if (savedmsgs.Count > 0)
            {
                //Console.WriteLine("More than one count...");
                foreach (var o in savedmsgs)
                {
                    Drawing.DrawText(xpos, ypos, System.Drawing.Color.Yellow, o);
                    ypos += 11;
                }
            }

            //if (skypeMsgs.Count > 0)
            //{
            //    Console.WriteLine("# Keys: "+skypeMsgs.Keys.Count);
            //    foreach (var c in skypeMsgs.Keys)
            //    {
            //        Console.WriteLine("Name: "+c);
            //        Console.WriteLine("# values: "+skypeMsgs[c].Count);
            //        foreach (var i in skypeMsgs[c])
            //        {
            //            //foreach (var l in i)
            //            //{
            //            Console.WriteLine("Msg: "+i);
            //                Drawing.DrawText(xpos, ypos,colors[c], i);
            //                ypos += 11;
            //            //}
            //        }
            //    }
            //}
            //if (savedmsg != null)
            //{
            //    Drawing.DrawText(config.Item("xpos").GetValue<Slider>().Value, config.Item("ypos").GetValue<Slider>().Value, System.Drawing.Color.Yellow, savedmsg);
            //}
            if (text != null)
            {
                //Console.WriteLine("Adding message...");
                float gameTime = Game.ClockTime;
                var timespan = TimeSpan.FromSeconds(gameTime-25);
                //Console.WriteLine(timespan.ToString(@"mm\:ss"));
                //string time = minutes + ":" + seconds;
                string msg = "[" + timespan.ToString(@"mm\:ss") + "] " + sender + " (" + skypename + ") says: " + text;
                savedmsgs.Add(msg);
                //if (!skypeMsgs.ContainsKey(skypename))
                //{
                //    savedmsgs.Add(msg);
                //    skypeMsgs.Add(skypename, savedmsgs);
                //    Game.PrintChat("Added: "+skypename+"..."+savedmsgs[0]);
                //    savedmsgs.Clear();
                //    Game.PrintChat("Length of dictionary: "+skypeMsgs.Count);
                //    Game.PrintChat("# Keys: " + skypeMsgs.Keys.Count);
                //    Random randomGen = new Random();
                //    KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                //    KnownColor randomColorName = names[randomGen.Next(names.Length)];
                //    Color randomColor = Color.FromKnownColor(randomColorName);
                //    colors.Add(skypename, randomColor);
                //}
                //else
                //{
                //    skypeMsgs[skypename].Add(msg);
                //    Game.PrintChat("Added2: " + skypename + "..." + msg);
                //    //Random randomGen = new Random();
                //    //KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                //    //KnownColor randomColorName = names[randomGen.Next(names.Length)];
                //    //Color randomColor = Color.FromKnownColor(randomColorName);
                //    //colors[skypename] = randomColor;
                //}
                ////savedmsg = sender + " (" + skypename + ") says: " + text;
                //Game.PrintChat("Received message...");

                //sender = null;
                text = null;
            }
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        private static void Game_OnInput(GameInputEventArgs args)
        {
            var d = args.Input;
            if (d.Contains("/sm"))
            {
                string[] msg = d.Split(' ');
                //var q = d.Substring(d.LastIndexOf(' ') + 1);
                var q = msg[1];
                //Game.PrintChat("Q: "+q);
                string l = null;
                for (int i = 2; i < msg.Length; i++)
                {
                    l += msg[i]+" ";
                }
                //Game.PrintChat("L: "+l);
                k.SendMessage(q, l);
                float gameTime = Game.ClockTime;
                var timespan = TimeSpan.FromSeconds(gameTime - 25);
                Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender+": "+l);
            }

            if (d.Contains("/sr"))
            {
                string[] msg = d.Split(' ');
                string l = null;
                float gameTime = Game.ClockTime;
                var timespan = TimeSpan.FromSeconds(gameTime - 25);
                switch (msg[1])
                {
                    case "1":
                        k.SendMessage(skypename, "in game atm");
                        Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender + ": " + "in game atm");
                        return;
                    case "2":
                        k.SendMessage(skypename, "hold on");
                        Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender + ": " + "hold on");
                        return;
                    case "3":
                        k.SendMessage(skypename, "busy atm");
                        Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender + ": " + "busy atm");
                        return;
                    case "4":
                        k.SendMessage(skypename, "brb");
                        Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender + ": " + "brb");
                        return;
                    default:
                        break;
                }
                for (int i = 1; i < msg.Length; i++)
                {
                    l += msg[i]+" ";
                }
                k.SendMessage(skypename, l);
                
                Game.PrintChat("[" + timespan.ToString(@"mm\:ss") + "] To: " + sender + ": " + l);
            }
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        private static void skype_MessageStatus(ChatMessage msg,
            TChatMessageStatus status)
        {
            //Console.WriteLine("recv'd msg...    ");
            if (msg.Sender.DisplayName != "" && msg.Sender.DisplayName != " ")
            {
                //if (msg.Sender. != msg.Sender.Handle)
                //{
                //if (msg.Body.IndexOf("/c") == 0)
                //{
                // Remove trigger string and make lower case
                //string command = msg.Body.Remove(0, "/c".Length).ToLower();
                string command = msg.Body;
                text = msg.Body;
                sender = msg.Sender.DisplayName;
                skypename = msg.Sender.Handle;
                // Send processed message back to skype chat window
                //Game.PrintChat(msg.Sender.DisplayName + " says: " + command);
                //Game.PrintChat("Sender: "+msg.Sender.Handle);
                //k.SendMessage(msg.Sender.Handle, msg.Sender.DisplayName +
                //" Says: " + command); //ProcessCommand(command));
                //}
                //}
            }
        }
    }
}
