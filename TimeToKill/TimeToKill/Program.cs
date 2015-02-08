using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace TimeToKill
{
    class Program
    {
        private static Obj_AI_Hero Player;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static bool Vayne = false;
        private static bool Cait = false;
        private static bool Mf = false;
        private static bool Satan = false;
        private static bool Jinx = false;
        private static bool Draven = false;
        private static bool KogMaw = false;
        private static bool Corki = false;

        private static double atkspd;

        static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            switch (ObjectManager.Player.ChampionName)
            {
                case "Vayne":
                    Vayne = true;
                    break;
                case "Caitlyn":
                    Cait = true;
                    break;
                case "MissFortune":
                    Mf = true;
                    break;
                case "Teemo":
                    Satan = true;
                    break;
                case "Jinx":
                    Jinx = true;
                    break;
                case "Corki":
                    Corki = true;
                    break;
                case "Draven":
                    Draven = true;
                    break;
                case "KogMaw":
                    KogMaw = true;
                    break;
            }

            switch (ObjectManager.Player.ChampionName)
            {
                case "Aatrox":
                    atkspd = 0.651;
                    break;
                case "Ahri":
                    atkspd = 0.668;
                    break;
                case "Akali":
                    atkspd = 0.694;
                    ////mult = 3.1;
                    break;
                case "Alistar":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Amumu":
                    atkspd = 0.638;
                    ////mult = 3.1;
                    break;
                case "Anivia":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Annie":
                    atkspd = 0.579;
                    ////mult = 3.1;
                    break;
                case "Ashe":
                    atkspd = 0.658;
                    ////mult = 3.1;
                    break;
                case "Blitzcrank":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Brand":
                    atkspd = 0.625;
                    break;
                case "Braum":
                    atkspd = 0.644;
                    ////mult = 3.1;
                    break;
                case "Caitlyn":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Cassiopeia":
                    atkspd = 0.647;
                    ////mult = 3.1;
                    break;
                case "Chogath":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Corki":
                    atkspd = 0.625;
                    ////mult = 3.1;
                    break;
                case "Darius":
                    atkspd = 0.679;
                    //mult = 3.1;
                    break;
                case "Diana":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "DrMundo":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Draven":
                    atkspd = 0.679;
                    //mult = 3.1;
                    break;
                case "Elise":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Evelynn":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Ezreal":

                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "FiddleSticks":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Fiora":
                    atkspd = 0.672;
                    //mult = 3.1;
                    break;
                case "Fizz":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Galio":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Gangplank":
                    atkspd = 0.651;
                    //mult = 3.1;
                    break;
                case "Garen":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Gnar":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Gragas":
                    atkspd = 0.651;
                    //mult = 3.1;
                    break;
                case "Graves":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Hecarim":
                    atkspd = 0.67;
                    //mult = 3.1;
                    break;
                case "Heimerdinger":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Irelia":
                    atkspd = 0.665;
                    //mult = 3.1;
                    break;
                case "Janna":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "JarvanIV":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Jax":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Jayce":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Jinx":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Karma":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Karthus":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Kassadin":
                    atkspd = 0.64;
                    //mult = 3.1;
                    break;
                case "Katarina":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Kayle":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Kennen":
                    atkspd = 0.69;
                    //mult = 3.1;
                    break;
                case "Khazix":
                    atkspd = 0.668;
                    //mult = 3.1;
                    break;
                case "KogMaw":
                    atkspd = 0.665;
                    //mult = 3.1;
                    break;
                case "Leblanc":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "LeeSin":
                    atkspd = 0.651;
                    //mult = 3.1;
                    break;
                case "Leona":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Lissandra":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Lucian":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Lulu":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Lux":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Malphite":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Malzahar":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Maokai":
                    atkspd = 0.694;
                    //mult = 3.1;
                    break;
                case "MasterYi":
                    atkspd = 0.679;
                    //mult = 3.1;
                    break;
                case "MissFortune":
                    atkspd = 0.656;
                    //mult = 3.1;
                    break;
                case "Mordekaiser":
                    atkspd = 0.694;
                    //mult = 3.1;
                    break;
                case "Morgana":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Nami":
                    atkspd = 0.644;
                    //mult = 3.1;
                    break;
                case "Nasus":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Nautilus":
                    atkspd = 0.613;
                    //mult = 3.1;
                    break;
                case "Nidalee":
                    atkspd = 0.67;
                    //mult = 3.1;
                    break;
                case "Nocturne":
                    atkspd = 0.668;
                    //mult = 3.1;
                    break;
                case "Nunu":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Olaf":
                    atkspd = 0.694;
                    //mult = 3.1;
                    break;
                case "Orianna":

                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Pantheon":
                    atkspd = 0.679;
                    //mult = 3.1;
                    break;
                case "Poppy":
                    atkspd = 0.638;
                    //mult = 3.1;
                    break;
                case "Quinn":
                    atkspd = 0.668;
                    //mult = 3.1;
                    break;
                case "Rengar":
                    atkspd = 0.679;
                    //mult = 3.1;
                    break;
                case "Riven":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Rumble":
                    atkspd = 0.644;
                    //mult = 3.1;
                    break;
                case "Rammus":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Renekton":
                    atkspd = 0.665;
                    //mult = 3.1;
                    break;
                case "Ryze":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Sejuani":
                    atkspd = 0.67;
                    //mult = 3.1;
                    break;
                case "Shaco":
                    atkspd = 0.694;
                    //mult = 3.1;
                    break;
                case "Shen":
                    atkspd = 0.651;
                    //mult = 3.1;
                    break;
                case "Shyvana":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Singed":
                    atkspd = 0.613;
                    //mult = 3.1;
                    break;
                case "Sion":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Sivir":
                    atkspd = 0.658;
                    //mult = 3.1;
                    break;
                case "Skarner":

                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Soraka":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Sona":
                    atkspd = 0.644;
                    //mult = 3.1;
                    break;
                case "Swain":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Syndra":

                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Talon":
                    atkspd = 0.668;
                    //mult = 3.1;
                    break;
                case "Thresh":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Taric":
                    atkspd = 0.625;
                    //mult = 3.1;
                    break;
                case "Teemo":
                    atkspd = 0.69;
                    //mult = 3.1;
                    break;
                case "Tristana":
                    atkspd = 0.656;
                    //mult = 3.1;
                    break;
                case "Trundle":
                    atkspd = 0.67;
                    //mult = 3.1;
                    break;
                case "Tryndamere":
                    atkspd = 0.67;
                    //mult = 3.1;
                    break;
                case "TwistedFate":
                    atkspd = 0.651;
                    break;
                case "Twitch":
                    atkspd = 0.679;
                    break;
                case "Udyr":
                    atkspd = 0.658;
                    break;
                case "Urgot":
                    atkspd = 0.644;
                    break;
                case "Varus":
                    atkspd = 0.658;
                    break;
                case "Vayne":
                    atkspd = 0.658;
                    break;
                case "Veigar":
                    atkspd = 0.625;
                    break;
                case "Velkoz":
                    atkspd = 0.625;
                    break;
                case "Vi":
                    atkspd = 0.644;
                    break;
                case "Viktor":
                    atkspd = 0.625;
                    break;
                case "Xerath":
                    atkspd = 0.625;
                    break;
                case "Vladimir":
                    atkspd = 0.658;
                    break;
                case "Volibear":
                    atkspd = 0.658;
                    break;
                case "Warwick":
                    atkspd = 0.679;
                    break;
                case "XinZhao":
                    atkspd = 0.672;
                    break;
                case "Yasuo":
                    atkspd = 0.658;
                    break;
                case "Yorick":
                    atkspd = 0.625;
                    break;
                case "Zac":
                    atkspd = 0.638;
                    break;
                case "Zed":
                    atkspd = 0.658;
                    break;
                case "Ziggs":
                    atkspd = 0.656;
                    break;
                case "Zilean":
                    atkspd = 0.625;
                    break;
                case "Zyra":
                    atkspd = 0.625;
                    break;
                case "MonkeyKing":
                    atkspd = 0.658;
                    break;
            }
            Drawing.OnDraw += Game_OnGameUpdate;
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            double temp = 0;
            foreach (
                var c in
                    ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsVisible && x.IsEnemy && x.IsValid ))//&& x.Distance(ObjectManager.Player) < 2500))
            {
                var dmg = Player.GetAutoAttackDamage(c);
                
                var howmanyautos = c.Health/dmg;

                if (howmanyautos > 3 && Vayne && Player.Spellbook.GetSpell(SpellSlot.W).State != SpellState.NotLearned)
                {
                    var g = new float[] {0,20, 30, 40, 50, 60};
                    var h = new double[]{0,.04, .05, .06, .07, .08};

                    howmanyautos = (c.Health) / (dmg + ((g[Player.Spellbook.GetSpell(SpellSlot.W).Level] + (c.MaxHealth*h[Player.Spellbook.GetSpell(SpellSlot.W).Level])))/3);
                }

                else if (howmanyautos > 7 && Cait)
                {
                    howmanyautos = (c.Health) / (dmg + (Player.CalcDamage(c, Damage.DamageType.Physical,
                            1.5*(Player.BaseAttackDamage + Player.FlatPhysicalDamageMod))/7));
                }

                else if (Satan)
                {
                    if (Player.Spellbook.GetSpell(SpellSlot.E).State != SpellState.NotLearned)
                    {
                        howmanyautos = c.Health/
                                       (Player.CalcDamage(c, Damage.DamageType.Magical,
                                           Player.Spellbook.GetSpell(SpellSlot.E).Level*10 +
                                           Player.FlatMagicDamageMod*.3) + dmg);
                    }
                }

                else if (Mf)
                {
                    howmanyautos = c.Health / (Player.GetSpellDamage(c, SpellSlot.W) + dmg); 
                }

                else if (Jinx)
                {
                    var mult = new double[] {0,1.30, 1.55, 1.80, 2.05, 2.30};
                    temp = atkspd*mult[Player.Spellbook.GetSpell(SpellSlot.Q).Level];  //assume max stack of Q before engaging
                    if (Player.HasBuff("JinxQ"))
                    {
                        howmanyautos = c.Health / (Player.CalcDamage(c, Damage.DamageType.Physical, .1 * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod)) + dmg);  
                    }
                }

                else if (Draven)
                {
                    howmanyautos = c.Health/((Player.GetSpellDamage(c, SpellSlot.Q))+dmg);  //assume axes 24/7
                }

                else if (KogMaw)
                {
                    howmanyautos = c.Health/(dmg + Player.GetSpellDamage(c, SpellSlot.W));
                }

                else if (Corki)
                {
                    howmanyautos = c.Health/(dmg + ((Player.BaseAttackDamage + Player.FlatMagicDamageMod))*.1);
                }

                double howmuchtime = 0;
                if (temp == 0)
                {
                    howmuchtime = howmanyautos/(atkspd*Player.AttackSpeedMod);
                }

                else
                {
                    howmuchtime = howmanyautos / (temp); 
                }
                Drawing.DrawText(c.HPBarPosition.X,c.HPBarPosition.Y-22,System.Drawing.Color.DeepSkyBlue,"Time: "+String.Format("{0:0.00}", howmuchtime) + "  Autos: "+String.Format("{0:0.00}", howmanyautos));
            }
        }


    }
}
