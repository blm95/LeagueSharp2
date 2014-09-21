using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Input;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;
using SharpDX;
using Label = System.Reflection.Emit.Label;
using Menu = LeagueSharp.Common.Menu;
using MenuItem = LeagueSharp.Common.MenuItem;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

//using SharpDX.Direct3D9;
//using Color = System.Drawing.Color;

namespace RivenSharp
{
    internal class RivenSharp
    {
        //public const string CharName = "Riven";
        private static void Main(string[] args)
        {
            //if (ObjectManager.Player.ChampionName != CharName)
            // return;
            /* CallBAcks */
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

        }



        public static Menu Config;

        //public static Spell Q;
        //public static Spell W;
        //public static Spell E;
        //public static Spell R;
        //public static float lastQ = 0;
        //public static bool combo = false;
        //public static int qCasted = 0;
        //public static bool usingQ = false;
        //public static int stacks = 0;
        //public static int stage = 0;
        //public static float stack0 = 0;
        //public static float stage0 = 0;
        ////public static Obj_AI_Hero target;
        //public static bool qCast = false;
        //public static bool fullCombo = false;
        ////private static SpellDataInst _ignite;
        //private static Obj_AI_Hero _player;
        //public static bool usedR = false;
        //public static bool eCombo = false;
        //public static bool dontR = false;
        //public static bool stopped = true;

        public static string promptValue;
        public static string prompt;
        public static string[] keys;
        public static Dictionary<string, string> dict = new Dictionary<string, string>();
        public static int i = 0;


        public static void Game_OnGameLoad(EventArgs args)
        {
            try
            {
               // string promptValue = Prompt.ShowDialog("Test", "123");
                //var input = "";
                //var c = SimpleDialog.InputBox.ShowDialog("Bad Mannered", "Input a douchey thing to say", out input);

                //CustomEvents.Game.PrintChat("Riven");
                Config = new Menu("Asshole", "Asshole", true);
                //Orbwalker
                //Config.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
                //Riven.orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalker"));
                //TS
                //var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
                //SimpleTs.AddToMenu(TargetSelectorMenu);
                //Config.AddSubMenu(TargetSelectorMenu);

                Config.AddSubMenu(new Menu("Combo", "combo"));
                Config.SubMenu("combo")
                    .AddItem(new MenuItem("combopress", "Asshole"))
                    .SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press));
                Config.SubMenu("combo")
                    .AddItem(new MenuItem("add", "Add Multiple Phrases").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press)));
                    //.SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press));
                Config.SubMenu("combo").AddItem(new MenuItem("combopress2", "Save Input?").SetValue(false));
                //Config.SubMenu("combo")
                //    .AddItem(new MenuItem("combopress2", "Aborted"))
                //    .SetValue(new KeyBind("I".ToCharArray()[0], KeyBindType.Press));
                //Config.SubMenu("combo")
                //    .AddItem(new MenuItem("combopress3", "Fuck You"))
                //    .SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Press));
                //Config.SubMenu("combo").AddItem(new MenuItem("laugh", "Cancel w/ Laugh")).SetValue(false);
                //Config.SubMenu("combo").AddItem(new MenuItem("dance", "Cancel w/ Dance")).SetValue(true);


                Config.AddToMainMenu();


                Game.OnGameUpdate += OnGameUpdate;

                ////CustomEvents.Game.OnGameSendPacket += OnGameSendPacket;
                ////CustomEvents.Game.OnGameProcessPacket += OnGameProcessPacket;
                //Q = new Spell(SpellSlot.Q, 260);
                //W = new Spell(SpellSlot.W, 125);
                //E = new Spell(SpellSlot.E, 325);
                //R = new Spell(SpellSlot.R, 900);
                //R.SetSkillshot(.3f, 200, 1450, false, SkillshotType.SkillshotCone);
                ////Drawing.OnDraw += Draws;

            }

            catch (Exception e)
            {
                Console.WriteLine("test: " + e.ToString());
            }
        }

        public static void OnGameUpdate(EventArgs args)
        {
            int counter = 0;
            string line;
            
            // Read the file and display it line by line.
            if (Config.Item("combopress").GetValue<KeyBind>().Active)
            {
                if (Config.Item("combopress2").GetValue<bool>() && promptValue != "")
                {
                    /*Config.SubMenu("combo")
                    .AddItem(new MenuItem("combopress", "Asshole"))
                    .SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press));
                     */
                     
                    switch (prompt)
                    {
                        case "1":
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "2":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "3":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "4":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "5":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                    }
                }
                else
                {


                    //var input = "";
                    //var c = SimpleDialog.InputBox.ShowDialog("Bad Mannered", "Input a douchey thing to say", out input);
                    promptValue = Prompt.ShowDialog("Bad Mannered", "Input a douchey thing to say");
                    prompt = Prompt.ShowDialog("NumTimes", "How many times do you want it spammed? (max 10)");

                    switch (prompt)
                    {
                        case "1":
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "2":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "3":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "4":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "5":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "6":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "7":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "8":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "9":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                        case "10":
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            Game.Say("/all {0}", promptValue);
                            break;
                    }
                }


                //Game.Say("/all {0}", promptValue);
                Config.Item("combopress").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press));
                
            }

            if (Keyboard.IsKeyDown(Key.CapsLock))
            {
                var dir = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //Game.PrintChat(dir.ToString());
                //Game.PrintChat(dir.Directory.ToString());
                //Game.PrintChat(dir.DirectoryName);
                try
                {
                    System.IO.StreamReader file =
                        new System.IO.StreamReader(dir.DirectoryName + "\\lol.txt");
                    //Game.PrintChat(file.);
                    while ((line = file.ReadLine()) != null)
                    {
                        Game.Say("/all {0}", line);
                    }
                    file.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                

                

            }

            if (Config.Item("add").GetValue<KeyBind>().Active)
            {
                Config.Item("add").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press));
                promptValue = Prompt.ShowDialog("Bad Mannered", "Input a douchey thing to say");
                prompt = Prompt.ShowDialog("NumTimes", "How many times do you want it spammed? (max 10)");
                var ke = Prompt.ShowDialog("lel", "KeyBind?");
                Config.SubMenu("combo")
                    .AddItem(new MenuItem(ke.ToUpper().ToCharArray()[0].ToString(), promptValue))
                    .SetValue(new KeyBind(ke.ToUpper().ToCharArray()[0], KeyBindType.Press));
                //keys[i] = promptValue;
                dict.Add(ke.ToUpper().ToCharArray()[0].ToString(), promptValue);
                switch (prompt)
                {
                    case "1":
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "2":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "3":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "4":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "5":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "6":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "7":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "8":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "9":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                    case "10":
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        Game.Say("/all {0}", promptValue);
                        break;
                }
                i++;
                Config.Item("add").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press));
            }
            foreach (var c in dict)
            {
                if (Config.Item(c.Key.ToString()).GetValue<KeyBind>().Active)
                {
                    switch (prompt)
                    {
                        case "1":
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "2":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "3":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "4":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "5":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "6":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "7":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "8":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "9":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                        case "10":
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            Game.Say("/all {0}", c.Value);
                            break;
                    }
                    //Game.Say();
                }
            }



            //if (Config.Item("combopress").GetValue<KeyBind>().Active)
            //{
            //    //stopped = false;
            //    //dofullcombo();
            //    //return;
            //    Game.Say("a");
            //    Game.Say("d");
            //    Game.Say("o");
            //    Game.Say("p");
            //    Game.Say("t");
            //    Game.Say("e");
            //    Game.Say("d");
            //}

            //if (Config.Item("combopress2").GetValue<KeyBind>().Active)
            //{
            //    Game.Say("you");
            //    Game.Say("were");
            //    Game.Say("a");
            //    Game.Say("failed");
            //    Game.Say("abortion");
            //}

            //if (Config.Item("combopress3").GetValue<KeyBind>().Active)
            //{
            //    Game.Say("f");
            //    Game.Say("u");
            //    Game.Say("c");
            //    Game.Say("k");
            //    Game.Say("y");
            //    Game.Say("o");
            //    Game.Say("u");

            //}
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Control textLabel = new Control() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button()
            {
                Text = "Ok", Left = 350, Width = 100, Top = 70
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }

}

