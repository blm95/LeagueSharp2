using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace SpellNamesandSlots
{
    internal class Program
    {
        //public static string name;
        //public static SpellSlot slots;
        //public static List<spellData> spells = new List<spellData>();
        //public static List<string> names;
        // public static string champname;
        public static Obj_AI_Base target;
        public static Dictionary<string, SpellSlot> spellData;
        public static float alldmg;
        public static Menu menu;
        private static string[] spellList;
        private static string[] onlyuse5;
        private static string[] buffNames;
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        private static Spell Item1;
        private static Spell Item2;
        private static Spell Item3;
        private static Spell Item4;
        private static Spell Item5;
        private static Spell Item6;
        public static string[] spells;
        //private static readonly List<Enemy> EnemyList = new List<Enemy>();

        //public class spellData
        //{
        //    public spellData()
        //    {
        //    }

        //    public spellData(string champName, SpellSlot slots)
        //    {
        //        name = champName;
        //        slot = slots;
        //    }
        //}

        //BetweenExtensions.

        private static void Main(string[] args)
        {
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



        public static void Game_OnGameLoad(EventArgs args)
        {
            try
            {
                
                Q = new Spell(SpellSlot.Q);
                W = new Spell(SpellSlot.W);
                E = new Spell(SpellSlot.E);
                R = new Spell(SpellSlot.R);
                Item1 = new Spell(SpellSlot.Item1);
                Item2 = new Spell(SpellSlot.Item2);
                Item3 = new Spell(SpellSlot.Item3);
                Item4 = new Spell(SpellSlot.Item4);
                Item5 = new Spell(SpellSlot.Item5);
                Item6 = new Spell(SpellSlot.Item6);
                menu = new Menu("Shield Me Please", "dmg", true);
                menu.AddItem(new MenuItem("range", "Range to check").SetValue(new Slider(600, 100, 2500)));
                var hp = new Menu("% HP Check", "hpcheck");
                hp.AddItem(new MenuItem("checkhp", "Only shield at % hp?").SetValue(false));
                hp.AddItem(new MenuItem("php", "Minimum % hp").SetValue(new Slider(30, 0, 100)));


                //menu.AddSubMenu(new Menu("% HP Check", "hpcheck"));

                var onlyshield = new Menu("Only shield X Spell(s)?", "only");
                onlyshield.AddItem(new MenuItem("onlyuse", "Only shield these skills?").SetValue(false));
                //menu.AddSubMenu(new Menu("Only shield X Spell(s)?", "only"));

                var allysupp = new Menu("Ally Support?", "allies");
                //menu.AddSubMenu(new Menu("Ally Support?", "allies"));
                allysupp.AddItem(new MenuItem("suppall", "Support allies?").SetValue(false));
                allysupp.AddSubMenu(new Menu("Only shield X Spell(s)?", "onlyallies"));
                allysupp.AddItem(new MenuItem("acheckhp", "Only shield at % hp?").SetValue(false));
                allysupp.AddItem(new MenuItem("aphp", "Minimum % hp").SetValue(new Slider(30, 0, 100)));


                //menu.SubMenu("allies").AddItem(new MenuItem("acheckhp", "Only shield at % hp?").SetValue(false));
                //menu.SubMenu("allies").AddItem(new MenuItem("aphp", "Minimum % hp").SetValue(new Slider(30, 0, 100)));
                // 
                allysupp.SubMenu("onlyallies")
                    .AddItem(new MenuItem("aonlyuse", "Only shield these skills?").SetValue(false));

                foreach (var n in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly && !x.IsMe))
                {
                    allysupp.AddSubMenu(new Menu(n.ChampionName, n.ChampionName));

                    allysupp.SubMenu(n.ChampionName)
                        .AddItem(new MenuItem("shield" + n.ChampionName, "Try to shield?").SetValue(false));
                }


                spellData = new Dictionary<string, SpellSlot>();
                menu.AddSubMenu(hp);
                //Game.PrintChat("started");

                foreach (var n in ObjectManager.Get<Obj_AI_Hero>().Where(n => n.IsEnemy))
                {
                    //n.ChampionName
                    Game.PrintChat(n.ChampionName);
                    switch (n.ChampionName)
                    {
                        case "Aatrox":
                            spellData.Add("AatroxQ", SpellSlot.Q);
                            spellData.Add("AatroxE", SpellSlot.E);
                            spellData.Add("AatroxR", SpellSlot.R);
                            break;
                        case "Ahri":
                            spellData.Add("AhriOrbofDeception", SpellSlot.Q);
                            spellData.Add("AhriOrbReturn", SpellSlot.Q);
                            spellData.Add("AhriSeduce", SpellSlot.E);
                            spellData.Add("AhriFoxFire", SpellSlot.W);
                            spellData.Add("AhriTumble", SpellSlot.R);
                            break;
                        case "Akali":
                            spellData.Add("AkaliMota", SpellSlot.Q);
                            spellData.Add("AkaliShadowSwipe", SpellSlot.E);
                            spellData.Add("AkaliShadowDance", SpellSlot.R);
                            break;
                        case "Alistar":
                            spellData.Add("Pulverize", SpellSlot.Q);
                            spellData.Add("Headbutt", SpellSlot.W);
                            break;
                        case "Amumu":
                            spellData.Add("AuraofDespair", SpellSlot.W);
                            spellData.Add("BandageToss", SpellSlot.Q);
                            spellData.Add("Tantrum", SpellSlot.E);
                            spellData.Add("CurseoftheSadMummy", SpellSlot.R);
                            break;
                        case "Anivia":
                            spellData.Add("FlashFrost", SpellSlot.Q);
                            spellData.Add("Frostbite", SpellSlot.E);
                            spellData.Add("GlacialStorm", SpellSlot.R);
                            break;
                        case "Annie":
                            spellData.Add("Disintegrate", SpellSlot.Q);
                            spellData.Add("Incinerate", SpellSlot.W);
                            spellData.Add("InfernalGuardian", SpellSlot.R);
                            break;
                        case "Ashe":
                            spellData.Add("VolleyAttack", SpellSlot.W);
                            spellData.Add("EnchantedCrystalArrow", SpellSlot.R);
                            break;
                        case "Blitzcrank":
                            spellData.Add("RocketGrab", SpellSlot.Q);
                            spellData.Add("PowerFist", SpellSlot.E);
                            spellData.Add("StaticField", SpellSlot.R);
                            break;
                        case "Brand":
                            spellData.Add("BrandBlaze", SpellSlot.Q);
                            spellData.Add("BrandFissure", SpellSlot.W);
                            spellData.Add("BrandConflagration", SpellSlot.E);
                            spellData.Add("BrandWildfire", SpellSlot.R);
                            break;
                        case "Braum":
                            spellData.Add("BraumQ", SpellSlot.Q);
                            spellData.Add("BraumR", SpellSlot.R);
                            spellData.Add("BraumRWrapper", SpellSlot.R);
                            break;
                        case "Caitlyn":
                            spellData.Add("CaitlynPiltoverPeacemaker", SpellSlot.Q);
                            spellData.Add("CaitlynEntrapment", SpellSlot.E);
                            spellData.Add("CaitlynAceintheHole", SpellSlot.R);
                            break;
                        case "Cassiopeia":
                            spellData.Add("CassiopeiaNoxiousBlast", SpellSlot.Q);
                            spellData.Add("CassiopeiaMiasma", SpellSlot.W);
                            spellData.Add("CassiopeiaTwinFang", SpellSlot.E);
                            spellData.Add("CassiopeiaPetrifyingGaze", SpellSlot.R);
                            break;
                        case "Chogath":
                            spellData.Add("FeralScream", SpellSlot.W);
                            spellData.Add("VorpalSpikes", SpellSlot.E);
                            spellData.Add("Rupture", SpellSlot.Q);
                            spellData.Add("Feast", SpellSlot.R);
                            break;
                        case "Corki":
                            spellData.Add("PhosphorusBomb", SpellSlot.Q);
                            spellData.Add("CarpetBomb", SpellSlot.W);
                            spellData.Add("GGun", SpellSlot.E);
                            spellData.Add("MissileBarrage", SpellSlot.R);
                            spellData.Add("MissileBarrage2", SpellSlot.R);
                            break;
                        case "Darius":
                            spellData.Add("DariusCleave", SpellSlot.Q);
                            spellData.Add("DariusNoxianTacticsONH", SpellSlot.W);
                            spellData.Add("DariusAxeGrabCone", SpellSlot.E);
                            spellData.Add("DariusExecute", SpellSlot.R);
                            break;
                        case "Diana":
                            spellData.Add("DianaArc", SpellSlot.Q);
                            spellData.Add("DianaOrbs", SpellSlot.W);
                            spellData.Add("DianaVortex", SpellSlot.E);
                            spellData.Add("DianaTeleport", SpellSlot.R);
                            break;
                        case "DrMundo":
                            spellData.Add("InfectedCleaverMissileCast", SpellSlot.Q);
                            spellData.Add("BurningAgony", SpellSlot.W);
                            break;
                        case "Draven":
                            spellData.Add("dravenspinning", SpellSlot.Q);
                            spellData.Add("DravenDoubleShot", SpellSlot.E);
                            spellData.Add("DravenRCast", SpellSlot.R);
                            break;
                        case "Elise":
                            spellData.Add("EliseHumanQ", SpellSlot.Q);
                            spellData.Add("EliseHumanW", SpellSlot.W);
                            spellData.Add("EliseHumanE", SpellSlot.E);
                            spellData.Add("EliseSpiderQCast", SpellSlot.Q);
                            break;
                        case "Evelynn":
                            spellData.Add("EvelynnQ", SpellSlot.Q);
                            spellData.Add("EvelynnE", SpellSlot.E);
                            spellData.Add("EvelynnR", SpellSlot.R);
                            break;
                        case "Ezreal":
                            spellData.Add("EzrealMysticShot", SpellSlot.Q);
                            //Game.PrintChat("Q added");
                            //Game.PrintChat("added");
                            spellData.Add("EzrealEssenceFlux", SpellSlot.W);
                            //Game.PrintChat("W added");
                            spellData.Add("EzrealArcaneShift", SpellSlot.E);
                            spellData.Add("EzrealTrueshotBarrage", SpellSlot.R);
                            //Game.PrintChat("R added");
                            break;
                        case "FiddleSticks":
                            spellData.Add("Terrify", SpellSlot.Q);
                            spellData.Add("Drain", SpellSlot.W);
                            spellData.Add("FiddlesticksDarkWind", SpellSlot.E);
                            spellData.Add("Crowstorm", SpellSlot.R);
                            break;
                        case "Fiora":
                            spellData.Add("FioraQ", SpellSlot.Q);
                            spellData.Add("FioraDance", SpellSlot.R);
                            break;
                        case "Fizz":
                            spellData.Add("FizzPiercingStrike", SpellSlot.Q);
                            spellData.Add("FizzSeastonePassive", SpellSlot.W);
                            spellData.Add("FizzJump", SpellSlot.E);
                            spellData.Add("FizzJumptwo", SpellSlot.E);
                            spellData.Add("FizzMarinerDoom", SpellSlot.R);
                            break;
                        case "Galio":
                            spellData.Add("GalioResoluteSmite", SpellSlot.Q);
                            spellData.Add("GalioRighteousGust", SpellSlot.E);
                            spellData.Add("GalioIdolOfDurand", SpellSlot.R);
                            break;
                        case "Gangplank":
                            spellData.Add("Parley", SpellSlot.Q);
                            spellData.Add("CannonBarrage", SpellSlot.R);
                            break;
                        case "Garen":
                            spellData.Add("GarenQ", SpellSlot.Q);
                            spellData.Add("GarenE", SpellSlot.E);
                            spellData.Add("GarenR", SpellSlot.R);
                            break;
                        case "Gnar":
                            spellData.Add("GnarQ", SpellSlot.Q);
                            spellData.Add("GnarQReturn", SpellSlot.Q);
                            spellData.Add("GnarBigQ", SpellSlot.Q);
                            spellData.Add("GnarBigW", SpellSlot.W);
                            spellData.Add("GnarE", SpellSlot.E);
                            spellData.Add("GnarBigE", SpellSlot.E);
                            spellData.Add("GnarR", SpellSlot.R);
                            break;
                        case "Gragas":
                            spellData.Add("GragasQ", SpellSlot.Q);
                            spellData.Add("GragasE", SpellSlot.E);
                            spellData.Add("GragasR", SpellSlot.R);
                            break;
                        case "Graves":
                            spellData.Add("GravesClusterShot", SpellSlot.Q);
                            spellData.Add("GravesSmokeGrenade", SpellSlot.W);
                            spellData.Add("gravessmokegrenadeboom", SpellSlot.W);
                            spellData.Add("GravesChargeShot", SpellSlot.R);
                            break;
                        case "Hecarim":
                            spellData.Add("HecarimRapidSlash", SpellSlot.Q);
                            spellData.Add("HecarimW", SpellSlot.W);
                            spellData.Add("HecarimUlt", SpellSlot.R);
                            break;
                        case "Heimerdinger":
                            spellData.Add("HeimerdingerQ", SpellSlot.Q);
                            spellData.Add("Heimerdingerwm", SpellSlot.W);
                            spellData.Add("HeimerdingerW", SpellSlot.W);
                            spellData.Add("HeimerdingerE", SpellSlot.E);
                            break;
                        case "Irelia":
                            spellData.Add("IreliaGatotsu", SpellSlot.Q);
                            spellData.Add("IreliaHitenStyle", SpellSlot.W);
                            spellData.Add("IreliaEquilibriumStrike", SpellSlot.E);
                            spellData.Add("IreliaTranscendentBlades", SpellSlot.R);
                            break;
                        case "Janna":
                            spellData.Add("HowlingGale", SpellSlot.Q);
                            spellData.Add("JannaQ", SpellSlot.Q);
                            spellData.Add("SowTheWind", SpellSlot.W);
                            break;
                        case "JarvanIV":
                            spellData.Add("JarvanIVDragonStrike", SpellSlot.Q);
                            spellData.Add("JarvanIVDemacianStandard", SpellSlot.E);
                            spellData.Add("JarvanIVCataclysm", SpellSlot.R);
                            break;
                        case "Jax":
                            spellData.Add("JaxLeapStrike", SpellSlot.Q);
                            spellData.Add("JaxEmpowerTwo", SpellSlot.W);
                            spellData.Add("JaxCounterStrike", SpellSlot.E);
                            spellData.Add("JaxRelentlessAsssault", SpellSlot.R);
                            break;
                        case "Jayce":
                            spellData.Add("JayceToTheSkies", SpellSlot.Q);
                            spellData.Add("jayceshockblast", SpellSlot.Q);
                            spellData.Add("JayceStaticField", SpellSlot.W);
                            spellData.Add("JayceThunderingBlow", SpellSlot.E);
                            spellData.Add("JayceQAccel", SpellSlot.Q);
                            break;
                        case "Jinx":
                            spellData.Add("JinxW", SpellSlot.W);
                            spellData.Add("JinxWMissle", SpellSlot.W);
                            spellData.Add("JinxE", SpellSlot.E);
                            spellData.Add("JinxR", SpellSlot.R);
                            spellData.Add("JinxRWrapper", SpellSlot.R);
                            break;
                        case "Karma":
                            spellData.Add("KarmaQ", SpellSlot.Q);
                            spellData.Add("KarmaQMantra", SpellSlot.Q);
                            spellData.Add("KarmaSpiritBind", SpellSlot.W);
                            break;
                        case "Karthus":
                            spellData.Add("LayWaste", SpellSlot.Q);
                            spellData.Add("KarthusLayWasteA2", SpellSlot.Q);
                            spellData.Add("WallOfPain", SpellSlot.W);
                            spellData.Add("Defile", SpellSlot.E);
                            spellData.Add("FallenOne", SpellSlot.R);
                            break;
                        case "Kassadin":
                            spellData.Add("NullLance", SpellSlot.Q);
                            spellData.Add("NetherBlade", SpellSlot.W);
                            spellData.Add("ForcePulse", SpellSlot.E);
                            spellData.Add("RiftWalk", SpellSlot.R);
                            break;
                        case "Katarina":
                            spellData.Add("KatarinaQ", SpellSlot.Q);
                            spellData.Add("KatarinaW", SpellSlot.W);
                            spellData.Add("KatarinaE", SpellSlot.E);
                            spellData.Add("KatarinaR", SpellSlot.R);
                            break;
                        case "Kayle":
                            spellData.Add("JudicatorReckoning", SpellSlot.Q);
                            spellData.Add("JudicatorRighteousFury", SpellSlot.E);
                            break;
                        case "Kennen":
                            spellData.Add("KennenBringTheLight", SpellSlot.W);
                            spellData.Add("KennenLightningRush", SpellSlot.E);
                            spellData.Add("KennenShurikenStorm", SpellSlot.R);
                            spellData.Add("KennenShurikenHurlMissile1", SpellSlot.Q);
                            break;
                        case "Khazix":
                            spellData.Add("khazixqlong", SpellSlot.Q);
                            spellData.Add("KhazixQ", SpellSlot.Q);
                            spellData.Add("khazixwlong", SpellSlot.W);
                            spellData.Add("KhazixW", SpellSlot.W);
                            spellData.Add("KhazixE", SpellSlot.E);
                            spellData.Add("khazixelong", SpellSlot.E);
                            break;
                        case "KogMaw":
                            spellData.Add("KogMawCausticSpittle", SpellSlot.Q);
                            spellData.Add("KogMawQ", SpellSlot.Q);
                            spellData.Add("KogMawBioArcanBarrage", SpellSlot.W);
                            spellData.Add("KogMawVoidOoze", SpellSlot.E);
                            spellData.Add("KogMawLivingArtillery", SpellSlot.R);
                            break;
                        case "Leblanc":
                            spellData.Add("LeblancChaosOrb", SpellSlot.Q);
                            spellData.Add("LeblancChaosOrbM", SpellSlot.Q);
                            spellData.Add("LeblancSlide", SpellSlot.W);
                            spellData.Add("leblacslidereturn", SpellSlot.W);
                            spellData.Add("LeblancSlideM", SpellSlot.W);
                            spellData.Add("LeblancSoulShackle", SpellSlot.E);
                            spellData.Add("LeblancSoulShackleM", SpellSlot.E);
                            break;
                        case "LeeSin":
                            spellData.Add("BlindMonkQOne", SpellSlot.Q);
                            spellData.Add("BlindMonkEOne", SpellSlot.E);
                            spellData.Add("BlindMonkRKick", SpellSlot.R);
                            break;
                        case "Leona":
                            spellData.Add("LeonaShieldOfDaybreak", SpellSlot.Q);
                            spellData.Add("LeonaSolarBarrier", SpellSlot.W);
                            spellData.Add("LeonaZenithBlade", SpellSlot.E);
                            spellData.Add("LeonaSolarFlare", SpellSlot.R);
                            break;
                        case "Lissandra":
                            spellData.Add("LissandraQ", SpellSlot.Q);
                            spellData.Add("LissandraW", SpellSlot.W);
                            spellData.Add("LissandraQShards", SpellSlot.Q);
                            spellData.Add("LissandraE", SpellSlot.E);
                            spellData.Add("LissandraR", SpellSlot.R);
                            break;
                        case "Lucian":
                            spellData.Add("LucianQ", SpellSlot.Q);
                            spellData.Add("LucianW", SpellSlot.W);
                            spellData.Add("LucianE", SpellSlot.E);
                            spellData.Add("LucianR", SpellSlot.R);
                            break;
                        case "Lulu":
                            spellData.Add("LuluQ", SpellSlot.Q);
                            spellData.Add("LuluW", SpellSlot.W);
                            spellData.Add("LuluE", SpellSlot.E);
                            spellData.Add("LuluQMissle", SpellSlot.Q);
                            break;
                        case "Lux":
                            spellData.Add("LuxLightBinding", SpellSlot.Q);
                            spellData.Add("LuxLightStrikeKugel", SpellSlot.E);
                            spellData.Add("LuxMaliceCannon", SpellSlot.R);
                            break;
                        case "Malphite":
                            spellData.Add("SeismicShard", SpellSlot.Q);
                            spellData.Add("Landslide", SpellSlot.E);
                            spellData.Add("UFSlash", SpellSlot.R);
                            break;
                        case "Malzahar":
                            spellData.Add("AlZaharNullZone", SpellSlot.W);
                            spellData.Add("AlZaharMaleficVisions", SpellSlot.E);
                            spellData.Add("AlZaharCalloftheVoid", SpellSlot.Q);
                            spellData.Add("AlZaharNetherGrasp", SpellSlot.R);
                            break;
                        case "Maokai":
                            spellData.Add("MaokaiTrunkLine", SpellSlot.Q);
                            spellData.Add("MaokaiUnstableGrowth", SpellSlot.W);
                            spellData.Add("MaokaiSapling2", SpellSlot.E);
                            spellData.Add("MaokaiDrain3", SpellSlot.R);
                            break;
                        case "MasterYi":
                            spellData.Add("AlphaStrike", SpellSlot.Q);
                            break;
                        case "MissFortune":
                            spellData.Add("MissFortuneRicochetShot", SpellSlot.Q);
                            spellData.Add("MissFortuneScattershot", SpellSlot.E);
                            spellData.Add("MissFortuneBulletTime", SpellSlot.R);
                            break;
                        case "Mordekaiser":
                            spellData.Add("MordekaiserMaceOfSpades", SpellSlot.Q);
                            spellData.Add("MordekaiserCreepinDeathCast", SpellSlot.W);
                            spellData.Add("MordekaiserSyphoneOfDestruction", SpellSlot.E);
                            spellData.Add("MordekaiserChildrenOfTheGrave", SpellSlot.R);
                            break;
                        case "Morgana":
                            spellData.Add("TormentedSoil", SpellSlot.W);
                            spellData.Add("DarkBindingMissile", SpellSlot.Q);
                            spellData.Add("SoulShackles", SpellSlot.R);
                            break;
                        case "Nami":
                            spellData.Add("NamiQ", SpellSlot.Q);
                            spellData.Add("NamiR", SpellSlot.R);
                            spellData.Add("NamiE", SpellSlot.E);
                            spellData.Add("NamiW", SpellSlot.W);
                            break;
                        case "Nasus":
                            spellData.Add("NasusQ", SpellSlot.Q);
                            spellData.Add("NasusW", SpellSlot.W);
                            spellData.Add("NasusE", SpellSlot.E);
                            spellData.Add("NasusR", SpellSlot.R);
                            break;
                        case "Nautilus":
                            spellData.Add("NautilusAnchorDrag", SpellSlot.Q);
                            spellData.Add("NautilusPiercingGaze", SpellSlot.W);
                            spellData.Add("NautilusSplashZone", SpellSlot.E);
                            spellData.Add("NautilusGandLine", SpellSlot.R);
                            break;
                        case "Nidalee":
                            spellData.Add("Bushwhack", SpellSlot.W);
                            spellData.Add("JavelinToss", SpellSlot.Q);
                            spellData.Add("Takedown", SpellSlot.Q);
                            spellData.Add("Pounce", SpellSlot.W);
                            spellData.Add("Swipe", SpellSlot.E);
                            break;
                        case "Nocturne":
                            spellData.Add("NocturneDuskbringer", SpellSlot.Q);
                            spellData.Add("NocturneUnspeakableHorror", SpellSlot.E);
                            spellData.Add("NocturneParanoia", SpellSlot.R);
                            break;
                        case "Nunu":
                            spellData.Add("IceBlast", SpellSlot.E);
                            spellData.Add("AbsoluteZero", SpellSlot.R);
                            break;
                        case "Olaf":
                            spellData.Add("OlafAxeThrowCast", SpellSlot.Q);
                            spellData.Add("OlafRecklessStrike", SpellSlot.E);
                            break;
                        case "Orianna":
                            spellData.Add("OriannasQ", SpellSlot.Q);
                            spellData.Add("OrianaIzunaCommand", SpellSlot.Q);
                            spellData.Add("OriannaQend", SpellSlot.Q);
                            spellData.Add("OrianaDissonanceCommand", SpellSlot.W);
                            spellData.Add("OriannasE", SpellSlot.E);
                            spellData.Add("OrianaRedactCommand", SpellSlot.E);

                            spellData.Add("OrianaDetonateCommand", SpellSlot.R);
                            break;
                        case "Pantheon":
                            spellData.Add("PantheonQ", SpellSlot.Q);
                            spellData.Add("PantheonW", SpellSlot.W);
                            spellData.Add("PantheonE", SpellSlot.E);
                            spellData.Add("PantheonRJump", SpellSlot.R);
                            spellData.Add("PantheonRFall", SpellSlot.R);
                            break;
                        case "Poppy":
                            spellData.Add("PoppyDevastatingBlow", SpellSlot.Q);
                            spellData.Add("PoppyHeroicCharge", SpellSlot.E);
                            break;
                        case "Quinn":
                            spellData.Add("QuinnQ", SpellSlot.Q);
                            spellData.Add("QuinnE", SpellSlot.E);
                            spellData.Add("QuinnR", SpellSlot.R);
                            spellData.Add("QuinnRFinale", SpellSlot.R);
                            break;
                        case "Rengar":
                            spellData.Add("RengarQ", SpellSlot.Q);
                            spellData.Add("RengarW", SpellSlot.W);
                            spellData.Add("RengarE", SpellSlot.E);
                            break;
                        case "Riven":
                            spellData.Add("RivenTriCleave", SpellSlot.Q);
                            spellData.Add("RivenTriCleave_03", SpellSlot.Q);
                            spellData.Add("RivenMartyr", SpellSlot.W);
                            spellData.Add("rivenizunablade", SpellSlot.R);
                            spellData.Add("RivenFengShuiEngine", SpellSlot.R);
                            break;
                        case "Rumble":
                            spellData.Add("RumbleFlameThrower", SpellSlot.Q);
                            spellData.Add("RumbleGrenade", SpellSlot.E);
                            spellData.Add("RumbleCarpetBombM", SpellSlot.R);
                            spellData.Add("RumbleCarpetBomb", SpellSlot.R);
                            break;
                        case "Rammus":
                            spellData.Add("PowerBall", SpellSlot.Q);
                            spellData.Add("DefensiveBallCurl", SpellSlot.W);
                            spellData.Add("PuncturingTaunt", SpellSlot.E);
                            spellData.Add("Tremors2", SpellSlot.R);
                            break;
                        case "Renekton":
                            spellData.Add("RenektonCleave", SpellSlot.Q);
                            spellData.Add("RenektonPreExecute", SpellSlot.W);
                            spellData.Add("RenektonSliceAndDice", SpellSlot.E);
                            spellData.Add("RenektonReignOfTheTyrant", SpellSlot.R);
                            break;
                        case "Ryze":
                            spellData.Add("Overload", SpellSlot.Q);
                            spellData.Add("RunePrison", SpellSlot.W);
                            spellData.Add("SpellFlux", SpellSlot.E);
                            break;
                        case "Sejuani":
                            spellData.Add("SejuaniArcticAssault", SpellSlot.Q);
                            spellData.Add("SejuaniNorthernWinds", SpellSlot.W);
                            spellData.Add("SejuaniWintersClaw", SpellSlot.E);
                            spellData.Add("SejuaniGlacialPrisonStart", SpellSlot.R);
                            break;
                        case "Shaco":
                            spellData.Add("JackInTheBox", SpellSlot.W);
                            spellData.Add("TwoShivPoisen", SpellSlot.E);
                            break;
                        case "Shen":
                            spellData.Add("ShenShadowDash", SpellSlot.E);
                            spellData.Add("ShenVorpalStar", SpellSlot.Q);
                            break;
                        case "Shyvana":
                            spellData.Add("ShyvanaDoubleAttack", SpellSlot.Q);
                            spellData.Add("shyvanadoubleattackdragon", SpellSlot.Q);
                            spellData.Add("ShyvanaImmolationAuraqw", SpellSlot.W);
                            spellData.Add("shyvanaimmolateddragon", SpellSlot.W);
                            spellData.Add("ShyvanaFireball", SpellSlot.E);
                            spellData.Add("shyvanafireballdragon2", SpellSlot.E);
                            spellData.Add("ShyvanaTransformCast", SpellSlot.R);
                            spellData.Add("ShyvanaTransformLeap", SpellSlot.R);
                            break;
                        case "Singed":
                            spellData.Add("PoisenTrail", SpellSlot.Q);
                            spellData.Add("Fling", SpellSlot.E);
                            break;
                        case "Sion":
                            spellData.Add("CrypticGaze", SpellSlot.Q);
                            spellData.Add("DeathsCaressFull", SpellSlot.W);
                            spellData.Add("deathscaress", SpellSlot.W);
                            break;
                        case "Sivir":
                            spellData.Add("SivirQReturn", SpellSlot.Q);
                            spellData.Add("SivirQ", SpellSlot.Q);
                            spellData.Add("SivirE", SpellSlot.E);
                            break;
                        case "Skarner":
                            spellData.Add("SkarnerVirulentSlash", SpellSlot.Q);
                            spellData.Add("SkarnerFracture", SpellSlot.E);
                            spellData.Add("SkarnerFractureMissileSpell", SpellSlot.E);

                            spellData.Add("SkarnerImpale", SpellSlot.R);
                            break;
                        case "Soraka":
                            spellData.Add("Starcall", SpellSlot.Q);
                            spellData.Add("InfuseWrapper", SpellSlot.E);
                            break;
                        case "Sona":
                            spellData.Add("SonaCrescendo", SpellSlot.R);
                            spellData.Add("SonaHymnofValor", SpellSlot.Q);
                            break;
                        case "Swain":
                            spellData.Add("SwainDecrepify", SpellSlot.Q);
                            spellData.Add("SwainShadowGrasp", SpellSlot.W);
                            spellData.Add("SwainTorment", SpellSlot.E);
                            spellData.Add("SwainMetamorphism", SpellSlot.R);
                            break;
                        case "Syndra":
                            spellData.Add("SyndraQ", SpellSlot.Q);
                            spellData.Add("syndrawcast", SpellSlot.W);
                            spellData.Add("SyndraW", SpellSlot.W);
                            spellData.Add("SyndraE", SpellSlot.E);
                            spellData.Add("syndrae5", SpellSlot.E);
                            //spellData.Add("SyndraE", SpellSlot.E);
                            spellData.Add("SyndraR", SpellSlot.R);
                            break;
                        case "Talon":
                            spellData.Add("TalonNoxianDiplomacy", SpellSlot.Q);
                            spellData.Add("TalonRake", SpellSlot.W);
                            spellData.Add("TalonCutthroat", SpellSlot.E);
                            spellData.Add("TalonRakeReturn", SpellSlot.W);
                            spellData.Add("TalonShadowAssault", SpellSlot.R);
                            break;
                        case "Thresh":
                            spellData.Add("ThreshQ", SpellSlot.Q);
                            spellData.Add("ThreshEFlay", SpellSlot.E);
                            spellData.Add("ThreshRPenta", SpellSlot.R);
                            break;
                        case "Taric":
                            spellData.Add("Shatter", SpellSlot.W);
                            spellData.Add("Dazzle", SpellSlot.E);
                            spellData.Add("TaricHammerSmash", SpellSlot.R);
                            break;
                        case "Teemo":
                            spellData.Add("BlindingDart", SpellSlot.Q);
                            spellData.Add("ToxicShot", SpellSlot.E);
                            spellData.Add("BantamTrap", SpellSlot.R);
                            break;
                        case "Tristana":
                            spellData.Add("RocketJump", SpellSlot.W);
                            spellData.Add("DetonatingShot", SpellSlot.E);
                            spellData.Add("BusterShot", SpellSlot.R);
                            break;
                        case "Trundle":
                            spellData.Add("TrundleTrollSmash", SpellSlot.Q);
                            spellData.Add("TrundlePain", SpellSlot.R);
                            break;
                        case "Tryndamere":
                            spellData.Add("MockingShout", SpellSlot.W);
                            spellData.Add("slashCast", SpellSlot.E);
                            break;
                        case "TwistedFate":
                            spellData.Add("WildCards", SpellSlot.Q);
                            spellData.Add("PickACard", SpellSlot.W);
                            spellData.Add("goldcardpreattack", SpellSlot.W);
                            spellData.Add("redcardpreattack", SpellSlot.W);
                            spellData.Add("bluecardpreattack", SpellSlot.W);
                            spellData.Add("CardmasterStack ", SpellSlot.E);
                            break;
                        case "Twitch":
                            //spellData.Add("TwitchVenomCask", SpellSlot.W);
                            spellData.Add("Expunge", SpellSlot.E);
                            break;
                        case "Udyr":
                            spellData.Add("UdyrTigerStance", SpellSlot.Q);
                            spellData.Add("UdyrBearStance", SpellSlot.E);
                            spellData.Add("UdyrPhoenixStance", SpellSlot.R);
                            break;
                        case "Urgot":
                            spellData.Add("UrgotHeatseekingLineMissile", SpellSlot.Q);
                            spellData.Add("UrgotHeatseekingLineqqMissile", SpellSlot.Q);
                            spellData.Add("UrgotPlasmaGrenade", SpellSlot.E);
                            spellData.Add("UrgotPlasmaGrenadeBoom", SpellSlot.E);
                            spellData.Add("UrgotSwap2", SpellSlot.R);
                            break;
                        case "Varus":
                            spellData.Add("VarusQMissilee", SpellSlot.Q);
                            spellData.Add("VarusW", SpellSlot.W);
                            spellData.Add("VarusE", SpellSlot.E);
                            spellData.Add("VarusR", SpellSlot.R);
                            break;
                        case "Vayne":
                            spellData.Add("VayneTumble", SpellSlot.Q);
                            spellData.Add("VayneSilverBolts", SpellSlot.W);
                            spellData.Add("VayneCondemm", SpellSlot.E);
                            break;
                        case "Veigar":
                            spellData.Add("VeigarBalefulStrike", SpellSlot.Q);
                            spellData.Add("VeigarDarkMatter", SpellSlot.W);
                            spellData.Add("VeigarEventHorizon", SpellSlot.E);
                            spellData.Add("VeigarPrimordialBurst", SpellSlot.R);
                            break;
                        case "Velkoz":
                            spellData.Add("VelkozQ", SpellSlot.Q);
                            spellData.Add("VelkozQSplit", SpellSlot.Q);
                            spellData.Add("VelkozW", SpellSlot.W);
                            spellData.Add("VelkozE", SpellSlot.E);
                            spellData.Add("VelkozR", SpellSlot.R);
                            break;
                        case "Vi":
                            spellData.Add("Vi-q", SpellSlot.Q);
                            spellData.Add("ViQ", SpellSlot.Q);
                            spellData.Add("ViW", SpellSlot.W);
                            spellData.Add("ViE", SpellSlot.E);
                            spellData.Add("ViR", SpellSlot.R);
                            break;
                        case "Viktor":
                            spellData.Add("ViktorPowerTransfer", SpellSlot.Q);
                            spellData.Add("ViktorChaosStorm ", SpellSlot.R);
                            spellData.Add("ViktorDeathRay", SpellSlot.E);
                            spellData.Add("Laser", SpellSlot.E);
                            break;
                        case "Xerath":
                            spellData.Add("xeratharcanopulse2", SpellSlot.Q);
                            spellData.Add("XerathArcaneBarrage2", SpellSlot.W);
                            spellData.Add("XerathMageSpear", SpellSlot.E);
                            spellData.Add("xerathrmissilewrapper", SpellSlot.R);
                            break;
                        case "Vladimir":
                            spellData.Add("VladimirTransfusion", SpellSlot.Q);
                            spellData.Add("VladimirSanguinePool", SpellSlot.W);
                            spellData.Add("VladimirTidesofBlood", SpellSlot.E);
                            spellData.Add("VladimirHemoplague", SpellSlot.R);
                            break;
                        case "Volibear":
                            spellData.Add("VolibearQ", SpellSlot.Q);
                            spellData.Add("VolibearW", SpellSlot.W);
                            spellData.Add("VolibearE", SpellSlot.E);
                            spellData.Add("VolibearR", SpellSlot.R);
                            break;
                        case "Warwick":
                            spellData.Add("HungeringStrike", SpellSlot.Q);
                            spellData.Add("InfiniteDuress", SpellSlot.R);
                            break;
                        case "XinZhao":
                            spellData.Add("XenZhaoComboTarget", SpellSlot.Q);
                            spellData.Add("XenZhaoSweep", SpellSlot.E);
                            spellData.Add("XenZhaoParry", SpellSlot.R);
                            break;
                        case "Yasuo":
                            spellData.Add("YasuoQW", SpellSlot.Q);
                            spellData.Add("yasuoq2w", SpellSlot.Q);
                            spellData.Add("yasuoq2", SpellSlot.Q);
                            spellData.Add("yasuoq3w", SpellSlot.Q);
                            spellData.Add("yasuoq", SpellSlot.Q);
                            spellData.Add("YasuoDashWrapper ", SpellSlot.E);
                            spellData.Add("YasuoRKnockUpComboW", SpellSlot.R);
                            break;
                        case "Yorick":
                            spellData.Add("YorickSpectral", SpellSlot.Q);
                            spellData.Add("YorickDecayed", SpellSlot.W);
                            spellData.Add("YorickRavenous", SpellSlot.E);
                            break;
                        case "Zac":
                            spellData.Add("ZacQ", SpellSlot.Q);
                            spellData.Add("ZacW", SpellSlot.W);
                            spellData.Add("ZacE", SpellSlot.E);
                            spellData.Add("ZacR", SpellSlot.R);
                            break;
                        case "Zed":
                            spellData.Add("ZedShuriken", SpellSlot.Q);
                            spellData.Add("ZedPBAOEDummy", SpellSlot.E);
                            spellData.Add("zedult", SpellSlot.R);
                            break;
                        case "Ziggs":
                            spellData.Add("ZiggsQ", SpellSlot.Q);
                            spellData.Add("ZiggsQSpell", SpellSlot.Q);
                            spellData.Add("ZiggsQBounce1", SpellSlot.Q);
                            spellData.Add("ZiggsQBounce2", SpellSlot.Q);
                            spellData.Add("ZiggsW", SpellSlot.W);
                            spellData.Add("ZiggsE", SpellSlot.E);
                            spellData.Add("ZiggsR", SpellSlot.R);
                            break;
                        case "Zilean":
                            spellData.Add("TimeBomb", SpellSlot.Q);
                            spellData.Add("TimeWarp", SpellSlot.E);
                            break;
                        case "Zyra":
                            spellData.Add("ZyraQFissure", SpellSlot.Q);
                            spellData.Add("ZyraGraspingRoots", SpellSlot.E);
                            spellData.Add("zyrapassivedeathmanager", SpellSlot.E);
                            spellData.Add("ZyraBrambleZone", SpellSlot.R);
                            break;
                    }
                }

                menu.AddSubMenu(new Menu("Items/Spells", "slots"));
                menu.AddItem(new MenuItem("percent", "Shield if dmg >").SetValue(new Slider(20, 1, 100)));
                menu.AddItem(new MenuItem("killable", "Always shield Killable").SetValue(true));
                menu.AddItem(new MenuItem("skillshots", "Shield Skillshots?").SetValue(true));
                menu.SubMenu("slots").AddSubMenu(new Menu("Item Slots to use", "keys"));
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("1", "1")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("1t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("2", "2")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("2t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("3", "3")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("3t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("5", "5")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("5t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("6", "6")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("6t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("7", "7")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys").AddItem(new MenuItem("7t", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").AddSubMenu(new Menu("Spell Slots to use", "keys2"));
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("Q", "Q")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("qt", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("W", "W")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("wt", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("E", "E")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("et", "^ targeted?")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("R.", "R.")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("rt", "^ targeted?")).SetValue(false);

                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("heal", "Heal")).SetValue(false);
                menu.SubMenu("slots").SubMenu("keys2").AddItem(new MenuItem("bar", "Barrier")).SetValue(false);
                //foreach (var c in spellData)
                //{
                //    menu.AddSubMenu(new Menu("Champions", "champs"));
                //    menu.SubMenu("champs").AddItem(new MenuItem("
                //}
                menu.AddSubMenu(new Menu("Always Shield:", "always"));
                int d = 0;
                spellList = new string[spellData.Keys.Count];
                onlyuse5 = new string[spellData.Keys.Count];
                spells = new string[spellData.Keys.Count];
                buffNames = new[]
                {
                    "buffname1", "buffname2", "buffname3"
                };
                foreach (var c in spellData.Values)
                {
                    Console.WriteLine(c.ToString());
                    spellList[d] = c.ToString();
                    //onlyuse5[d] = c;
                    d++;
                    //menu.SubMenu("always").AddItem(new MenuItem(c.Key, c.Key)).SetValue(false);
                }
                int q = 0;
                foreach (var k in spellData.Keys)
                {
                    spells[q] = k;
                    q++;
                }

                for (int i = 0; i < spellList.Length; i++)
                {
                    Console.WriteLine(spellList[i].ToString());
                    allysupp.SubMenu("onlyallies").AddItem(new MenuItem(spells[i] + spellList[i].ToString(), spells[i] + spellList[i].ToString()).SetValue(false));
                    onlyshield.AddItem(new MenuItem(spells[i] + spellList[i].ToString(), spells[i] + spellList[i].ToString()).SetValue(false));
                    menu.SubMenu("always").AddItem(new MenuItem(spells[i] + spellList[i].ToString(), spells[i] + spellList[i].ToString())).SetValue(false);
                }
                menu.AddSubMenu(onlyshield);
                menu.AddSubMenu(allysupp);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }



            menu.AddToMainMenu();
            alldmg = new float();

            Game.OnGameUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //foreach (
            //    Obj_AI_Hero h in
            //        ObjectManager.Get<Obj_AI_Hero>()
            //            .Where(h => h.IsEnemy && Vector3.Distance(ObjectManager.Player.Position, h.Position) <= 2000))
            //{
            //    foreach (var c in spellData)
            //    {
            //        if (h.ChampionName == c.Key)
            //        {

            //        }
            //    }
            //}
        }

        //private static bool IsBetween<T>(this T item, T start, T end)
        //{
        //    return Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;
        //}

        private static void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            target = ObjectManager.Player;
            //spellData[args.SData.Name].ToString().
            var y = (Obj_AI_Hero)hero;

            //Game.PrintChat(args.SData.Name);
            //if (spellData.ContainsKey(args.SData.Name))
            //{
            //    Game.PrintChat(y.ChampionName);
            //    Game.PrintChat((spellData[args.SData.Name]).ToString());
            //}
            //Game.PrintChat(dmgLib2.Class1.calcDmg(y, spellData[args.SData.Name], ObjectManager.Player).ToString());
            //Game.PrintChat(menu.Item("range").GetValue<Slider>().Value.ToString());
            //Game.PrintChat(Vector3.Distance(ObjectManager.Player.Position, y.Position).ToString());
            // 
            if (args.SData.Name == "zedult" && args.Target == target)
            {
                if (Items.CanUseItem(3140))
                {
                    Items.UseItem(3140);
                }
                if (Items.CanUseItem(3137))
                {
                    Items.UseItem(3137);
                }
                if (Items.CanUseItem(3139))
                {
                    Items.UseItem(3139);
                }
                if (Items.CanUseItem(3157))
                {
                    Items.UseItem(3157);
                }
            }
            if (spells.Any(str => str.Contains(args.SData.Name)) && menu.Item(args.SData.Name + spellData[args.SData.Name]).GetValue<bool>())
            {
                if ((Vector3.Distance(ObjectManager.Player.Position, args.End) <= 200 &&
                     menu.Item("skillshots").GetValue<bool>()) ||
                    Vector3.Distance(ObjectManager.Player.Position, args.End) <= 5)
                {

                    if (menu.Item("heal").GetValue<bool>())
                    {
                        foreach (var spell in ObjectManager.Player.SummonerSpellbook.Spells)
                        {
                            if (spell.Name.ToLower().Contains("heal") && spell.State == SpellState.Ready)
                                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, spell.Slot));
                        }
                    }

                    if (menu.Item("bar").GetValue<bool>())
                    {
                        foreach (var spell in ObjectManager.Player.SummonerSpellbook.Spells)
                        {
                            if (spell.Name.ToLower().Contains("barrier") && spell.State == SpellState.Ready)
                                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, spell.Slot));
                        }
                    }

                    if (menu.Item("1").GetValue<bool>())
                        if (menu.Item("1t").GetValue<bool>())
                            Item1.Cast(ObjectManager.Player);
                        else
                        {
                            Item1.Cast();
                        }

                    if (menu.Item("2").GetValue<bool>())
                        if (menu.Item("2t").GetValue<bool>())
                            Item2.Cast(ObjectManager.Player);
                        else
                        {
                            Item2.Cast();
                        }

                    if (menu.Item("3").GetValue<bool>())
                        if (menu.Item("3t").GetValue<bool>())
                            Item3.Cast(ObjectManager.Player);
                        else
                        {
                            Item3.Cast();
                        }
                    if (menu.Item("5").GetValue<bool>())
                        if (menu.Item("5t").GetValue<bool>())
                            Item4.Cast(ObjectManager.Player);
                        else
                            Item4.Cast();
                    if (menu.Item("6").GetValue<bool>())
                        if (menu.Item("6t").GetValue<bool>())
                            Item5.Cast(ObjectManager.Player);
                        else
                            Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, SpellSlot.Item5)).Send();
                    if (menu.Item("7").GetValue<bool>())
                        if (menu.Item("7t").GetValue<bool>())
                            Item6.Cast(ObjectManager.Player);
                        else
                            Item6.Cast();

                    if (menu.Item("Q").GetValue<bool>())
                        if (menu.Item("qt").GetValue<bool>())
                            Q.Cast(ObjectManager.Player);
                        else
                            Q.Cast();
                    if (menu.Item("W").GetValue<bool>())
                        if (menu.Item("wt").GetValue<bool>())
                            W.Cast(ObjectManager.Player);
                        else
                            W.Cast();
                    if (menu.Item("E").GetValue<bool>())
                        if (menu.Item("et").GetValue<bool>())
                            E.Cast(ObjectManager.Player);
                        else
                            E.Cast();
                    if (menu.Item("R.").GetValue<bool>())
                        if (menu.Item("rt").GetValue<bool>())
                            R.Cast(ObjectManager.Player);
                        else
                            R.Cast();
                }
            }

            if (Vector3.Distance(ObjectManager.Player.Position, y.Position) <=
                menu.Item("range").GetValue<Slider>().Value
                )
            {
                if (Vector3.Distance(ObjectManager.Player.Position, args.End) <= 200 ||
                    Vector3.Distance(ObjectManager.Player.Position, args.End) <= 5)
                {
                    if ((menu.Item("checkhp").GetValue<bool>() &&
                         ((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) * 100 <
                          menu.Item("php").GetValue<Slider>().Value)) || !menu.Item("checkhp").GetValue<bool>())
                    {
                        if ((onlyuse5.Any(str => str.Contains(args.SData.Name)) &&
                             menu.Item(args.SData.Name + spellData[args.SData.Name]).GetValue<bool>() && menu.Item("onlyuse").GetValue<bool>()) ||
                            !menu.Item("onlyuse").GetValue<bool>())
                        {
                            //Game.PrintChat("spell in range");
                            var percenthp = ((ObjectManager.Player.Health -
                                              Damage.GetSpellDamage(y,ObjectManager.Player,spellData[args.SData.Name])) / //dmgLib2.Class1.calcDmg(y, spellData[args.SData.Name], ObjectManager.Player)) /
                                             ObjectManager.Player.MaxHealth) * 100;
                            if (percenthp >= menu.Item("percent").GetValue<Slider>().Value ||
                                (menu.Item("killable").GetValue<bool>() &&
                                 Damage.GetSpellDamage(y,ObjectManager.Player,spellData[args.SData.Name]) >
                                 ObjectManager.Player.Health))
                            {
                                //Game.PrintChat("using key");
                                if (menu.Item("heal").GetValue<bool>())
                                {
                                    foreach (var spell in ObjectManager.Player.SummonerSpellbook.Spells)
                                    {
                                        if (spell.Name.ToLower().Contains("heal") && spell.State == SpellState.Ready)
                                            Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, spell.Slot));
                                    }
                                }

                                if (menu.Item("bar").GetValue<bool>())
                                {
                                    foreach (var spell in ObjectManager.Player.SummonerSpellbook.Spells)
                                    {
                                        if (spell.Name.ToLower().Contains("barrier") && spell.State == SpellState.Ready)
                                            Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, spell.Slot));
                                    }
                                }

                                if (menu.Item("1").GetValue<bool>())
                                    if (menu.Item("1t").GetValue<bool>())
                                        Item1.Cast(ObjectManager.Player);
                                    else
                                    {
                                        Item1.Cast();
                                    }

                                if (menu.Item("2").GetValue<bool>())
                                    if (menu.Item("2t").GetValue<bool>())
                                        Item2.Cast(ObjectManager.Player);
                                    else
                                    {
                                        Item2.Cast();
                                    }

                                if (menu.Item("3").GetValue<bool>())
                                    if (menu.Item("3t").GetValue<bool>())
                                        Item3.Cast(ObjectManager.Player);
                                    else
                                    {
                                        Item3.Cast();
                                    }
                                if (menu.Item("5").GetValue<bool>())
                                    if (menu.Item("5t").GetValue<bool>())
                                        Item4.Cast(ObjectManager.Player);
                                    else
                                        Item4.Cast();
                                if (menu.Item("6").GetValue<bool>())
                                    if (menu.Item("6t").GetValue<bool>())
                                        Item5.Cast(ObjectManager.Player);
                                    else
                                        Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, SpellSlot.Item5)).Send();
                                if (menu.Item("7").GetValue<bool>())
                                    if (menu.Item("7t").GetValue<bool>())
                                        Item6.Cast(ObjectManager.Player);
                                    else
                                        Item6.Cast();

                                if (menu.Item("Q").GetValue<bool>())
                                    if (menu.Item("qt").GetValue<bool>())
                                        Q.Cast(ObjectManager.Player);
                                    else
                                        Q.Cast();
                                if (menu.Item("W").GetValue<bool>())
                                    if (menu.Item("wt").GetValue<bool>())
                                        W.Cast(ObjectManager.Player);
                                    else
                                        W.Cast();
                                if (menu.Item("E").GetValue<bool>())
                                    if (menu.Item("et").GetValue<bool>())
                                        E.Cast(ObjectManager.Player);
                                    else
                                        E.Cast();
                                if (menu.Item("R.").GetValue<bool>())
                                    if (menu.Item("rt").GetValue<bool>())
                                        R.Cast(ObjectManager.Player);
                                    else
                                        R.Cast();
                            }
                        }
                    }
                }
            }

            if (menu.Item("allsupp").GetValue<bool>())
            {
                foreach (
                    var c in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(x => x.IsAlly && Vector3.Distance(x.Position, target.Position) < 600 && x.IsValid))
                {
                    if ((Vector3.Distance(ObjectManager.Player.Position, args.End) <= 200 &&
                         menu.Item("skillshots").GetValue<bool>()) ||
                        Vector3.Distance(ObjectManager.Player.Position, args.End) <= 5)
                    {
                        if (menu.Item("shield" + c.ChampionName).GetValue<bool>())
                        {
                            if ((spells.Any(str => str.Contains(args.SData.Name)) &&
                                 menu.Item(args.SData.Name + spellData[args.SData.Name]).GetValue<bool>() && menu.Item("aonlyuse").GetValue<bool>()) ||
                                !menu.Item("aonlyuse").GetValue<bool>())
                            {
                                if (menu.Item("acheckhp").GetValue<bool>())
                                {
                                    if ((c.Health / c.MaxHealth) * 100 > menu.Item("aphp").GetValue<Slider>().Value)
                                    {
                                        return;
                                    }
                                }
                                if (menu.Item("heal").GetValue<bool>())
                                {
                                    foreach (var spell in ObjectManager.Player.SummonerSpellbook.Spells)
                                    {
                                        if (spell.Name.ToLower().Contains("heal") && spell.State == SpellState.Ready)
                                            Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, spell.Slot));
                                    }
                                }

                                if (menu.Item("1").GetValue<bool>())
                                    if (menu.Item("1t").GetValue<bool>())
                                        Item1.Cast(c);
                                    else
                                    {
                                        Item1.Cast();
                                    }

                                if (menu.Item("2").GetValue<bool>())
                                    if (menu.Item("2t").GetValue<bool>())
                                        Item2.Cast(c);
                                    else
                                    {
                                        Item2.Cast();
                                    }

                                if (menu.Item("3").GetValue<bool>())
                                    if (menu.Item("3t").GetValue<bool>())
                                        Item3.Cast(c);
                                    else
                                    {
                                        Item3.Cast();
                                    }
                                if (menu.Item("5").GetValue<bool>())
                                    if (menu.Item("5t").GetValue<bool>())
                                        Item4.Cast(c);
                                    else
                                        Item4.Cast();
                                if (menu.Item("6").GetValue<bool>())
                                    if (menu.Item("6t").GetValue<bool>())
                                        Item5.Cast(c);
                                    else
                                        Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, SpellSlot.Item5)).Send();
                                if (menu.Item("7").GetValue<bool>())
                                    if (menu.Item("7t").GetValue<bool>())
                                        Item6.Cast(c);
                                    else
                                        Item6.Cast();

                                if (menu.Item("Q").GetValue<bool>())
                                    if (menu.Item("qt").GetValue<bool>())
                                        Q.Cast(c);
                                    else
                                        Q.Cast();
                                if (menu.Item("W").GetValue<bool>())
                                    if (menu.Item("wt").GetValue<bool>())
                                        W.Cast(c);
                                    else
                                        W.Cast();
                                if (menu.Item("E").GetValue<bool>())
                                    if (menu.Item("et").GetValue<bool>())
                                        E.Cast(c);
                                    else
                                        E.Cast();
                                if (menu.Item("R.").GetValue<bool>())
                                    if (menu.Item("rt").GetValue<bool>())
                                        R.Cast(c);
                                    else
                                        R.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}

