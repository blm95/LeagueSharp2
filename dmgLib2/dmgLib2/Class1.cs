using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace dmgLib2
{
    public static class Class1
    {
        private static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static double calcDmg(Obj_AI_Hero hero, SpellSlot slot, Obj_AI_Base enemy /* target */)
        {
            switch (hero.ChampionName)
            {
                case "Aatrox":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.6 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy); // 3rd hit Damage
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.6 * hero.FlatPhysicalDamageMod) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // magic dmg when casted
                        //default:
                        ////throw new InvalidSpellSlotException();
                    }
                    ////Champ = Aatrox;
                    break;
                case "Ahri":
                    switch (slot)
                    {
                        case SpellSlot.Q:


                            var waytoenemy =
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                            var wayback = (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                          (0.35 * hero.FlatMagicDamageMod);
                            return waytoenemy + wayback; // both

                        //throw new InvalidSpellSlotException();

                        case SpellSlot.W:


                            return
                                CalcMagicDmg(
                                    (24 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.64 * hero.FlatMagicDamageMod), enemy); // all 3 stacks on 1 unit

                        ////default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:



                            return
                                CalcMagicDmg(
                                    (90 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 120)) +
                                    (0.9 * hero.FlatMagicDamageMod), enemy); // max dmg to 1 unit

                        //throw new InvalidSpellSlotException();

                    }
                    ////Champ = Ahri;
                    break;
                case "Akali":
                    switch (slot)
                    {
                        case SpellSlot.Q:


                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        // q throw + hitted with something
                        ////default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.6 *
                                     (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)) +
                                    (0.3 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 75)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Akali;
                    break;
                case "Alistar":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (0 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 55)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }

                    ////Champ = Alistar;
                    break;
                case "Amumu":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            var basedmg = CalcMagicDmg(
                                (4 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 4)), enemy);
                            var percentofmaxhealth = (1.2 +
                                                      (hero.Spellbook.GetSpell(SpellSlot.W).Level * 0.3));
                            double additionalpercentper100ap = 0;
                            if (hero.FlatMagicDamageMod < 100)
                            {
                                additionalpercentper100ap = 0;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                            {
                                additionalpercentper100ap = 1;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                            {
                                additionalpercentper100ap = 2;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                            {
                                additionalpercentper100ap = 3;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                            {
                                additionalpercentper100ap = 4;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                            {
                                additionalpercentper100ap = 5;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                            {
                                additionalpercentper100ap = 6;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                            {
                                additionalpercentper100ap = 7;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                            {
                                additionalpercentper100ap = 8;
                            }
                            else if (hero.FlatMagicDamageMod >= 900)
                            {
                                additionalpercentper100ap = 9;
                            }
                            var healthbase = enemy.MaxHealth / 100 * (percentofmaxhealth + additionalpercentper100ap);
                            return basedmg + CalcMagicDmg(healthbase, enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Amumu;
                    break;
                case "Anivia":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 60)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        // when stunned (both of dmg)

                        //default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // when "Chilled"

                        //throw new InvalidSpellSlotException();

                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 40)) +
                                    (0.25 * hero.FlatMagicDamageMod), enemy); // per tick
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Anivia;
                    break;
                case "Annie":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.85 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 10)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:


                            return
                                CalcMagicDmg(
                                    (85 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        // per tick of tibbers sunfire
                        //default:
                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Annie;
                    break;
                case "Ashe":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 10)) +
                                    (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        // dmg around the explode radius
                        //default:
                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Ashe;
                    break;
                case "Blitzcrank":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod), enemy);
                        // only the additional dmg
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (125 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    ////Champ = Blitzcrank;
                    break;
                case "Brand":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.55 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 300)) +
                                    (1.5 * hero.FlatMagicDamageMod), enemy);
                        // Max possible dmg to one unit

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Brand;
                    break;
                case "Braum":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.25 * hero.MaxHealth), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Braum;
                    break;
                case "Caitlyn":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    (-20 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (1.3 *
                                     (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)),
                                    enemy); // first hit dmg

                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.8 * (hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 225)) +
                                    (2.0 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Caitlyn;
                    break;
                case "Cassiopeia":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy); // 3 hits -> all dmg
                        //default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (135 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 90)) +
                                    (1.35 * hero.FlatMagicDamageMod), enemy); // complete w dmg

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.55 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Cassiopeia;
                    break;
                case "Chogath":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                                    (0.3 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return (125 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 175));
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = ChoGath;
                    break;
                case "Corki":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.5 * hero.FlatMagicDamageMod) +
                                    (0.5 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 75)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // dmg complete

                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    (32 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 48)) +
                                    (1.6 * hero.FlatPhysicalDamageMod), enemy); // dmg complete

                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 80)) +
                                    (0.3 * hero.FlatMagicDamageMod) +
                                    ((0.1 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 0.1)) *
                                     hero.FlatPhysicalDamageMod), enemy); // normal missile

                        //default:
                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Corki;
                    break;
                case "Darius":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    (52.5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 52.5)) +
                                    (1.05 * hero.FlatPhysicalDamageMod), enemy);

                        case SpellSlot.W:
                            //double basicattack = CalcPhysicalDmg(ObjectManager.Unit.FlatPhysicalDamageMod + ObjectManager.Unit.BaseAttackDamage, enemy);
                            var bonusdmg = CalcPhysicalDmg(
                                0.2 * hero.Spellbook.GetSpell(SpellSlot.W).Level, enemy); // only the bonus dmg
                            //return basicattack + bonusdmg;
                            return bonusdmg;
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return ((140 + (180 * hero.Spellbook.GetSpell(SpellSlot.R).Level)) +
                                    (1.5 * hero.FlatPhysicalDamageMod)); // at 5 stacks

                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Darius;
                    break;
                case "Diana":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 36)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy); // all on one target_

                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 60)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Diana;
                    break;
                case "Draven":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            //double baseattack = CalcPhysicalDmg(ObjectManager.Unit.BaseAttackDamage + ObjectManager.Unit.FlatPhysicalDamageMod, enemy);
                            var bonusdmg =
                                CalcPhysicalDmg(
                                    (0.35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.1)) *
                                    (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod),
                                    enemy);
                            //return baseattack + bonusdmg;
                            return bonusdmg; // only the bonus dmg
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.5 * (hero.FlatPhysicalDamageMod)), enemy);
                        case SpellSlot.R:

                            return
                                CalcPhysicalDmg(
                                    (150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 200)) +
                                    (2.2 * hero.FlatPhysicalDamageMod), enemy); // both hit

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Draven;
                    break;
                case "DrMundo":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                            var tmpdmg =
                                CalcMagicDmg(
                                    (enemy.Health / 100) *
                                    (12 + (3 * hero.Spellbook.GetSpell(SpellSlot.Q).Level)), enemy);
                            var mindmg = CalcMagicDmg(
                                30 + (50 * hero.Spellbook.GetSpell(SpellSlot.Q).Level), enemy);
                            if (tmpdmg > mindmg)
                            {
                                return tmpdmg;
                            }
                            return mindmg;
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();

                            break;
                    }
                    //Champ = DrMundo;
                    break;
                case "Elise":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseHumanQ")
                            {
                                var basedmg =
                                    CalcMagicDmg((5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)), enemy);
                                double percentofcurrenthealth = 8;
                                double additionalpercentper100ap = 0;
                                if (hero.FlatMagicDamageMod < 100)
                                {
                                    additionalpercentper100ap = 0;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                                {
                                    additionalpercentper100ap = 3;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                                {
                                    additionalpercentper100ap = 6;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                                {
                                    additionalpercentper100ap = 9;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                                {
                                    additionalpercentper100ap = 12;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                                {
                                    additionalpercentper100ap = 15;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                                {
                                    additionalpercentper100ap = 18;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                                {
                                    additionalpercentper100ap = 21;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                                {
                                    additionalpercentper100ap = 24;
                                }
                                else if (hero.FlatMagicDamageMod >= 900)
                                {
                                    additionalpercentper100ap = 27;
                                }
                                var healthbase = enemy.Health / 100 * (percentofcurrenthealth + additionalpercentper100ap);
                                return basedmg + CalcMagicDmg(healthbase, enemy);
                            }
                            else
                            {
                                // Spider Q
                                var basedmg =
                                    CalcMagicDmg(
                                        (20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)), enemy);
                                double percentofcurrenthealth = 8;
                                double additionalpercentper100ap = 0;
                                if (hero.FlatMagicDamageMod < 100)
                                {
                                    additionalpercentper100ap = 0;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                                {
                                    additionalpercentper100ap = 3;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                                {
                                    additionalpercentper100ap = 6;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                                {
                                    additionalpercentper100ap = 9;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                                {
                                    additionalpercentper100ap = 12;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                                {
                                    additionalpercentper100ap = 15;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                                {
                                    additionalpercentper100ap = 18;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                                {
                                    additionalpercentper100ap = 21;
                                }
                                else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                                {
                                    additionalpercentper100ap = 24;
                                }
                                else if (hero.FlatMagicDamageMod >= 900)
                                {
                                    additionalpercentper100ap = 27;
                                }
                                var healthbase = (enemy.MaxHealth - enemy.Health) / 100 *
                                                 (percentofcurrenthealth + additionalpercentper100ap);
                                // of missing health
                                return basedmg + CalcMagicDmg(healthbase, enemy);
                            }
                        case SpellSlot.W:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseHumanW")
                            {
                                return
                                    CalcMagicDmg(
                                        (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                        (0.8 * hero.FlatMagicDamageMod), enemy);
                            }
                            else
                            {
                                break;
                            }
                            break;
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            break;
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            break;
                        //throw new InvalidSpellSlotException(); // switchting to spider / human
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Elise;
                    break;
                case "Evelynn":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                                    ((0.3 + hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                                     hero.FlatMagicDamageMod) +
                                    ((0.45 + hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                                     hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (1.0 * hero.FlatMagicDamageMod) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            double percentage = 15 + (5 * hero.Spellbook.GetSpell(SpellSlot.R).Level);
                            double additionalpercentper100ap = 0;
                            if (hero.FlatMagicDamageMod < 100)
                            {
                                additionalpercentper100ap = 0;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                            {
                                additionalpercentper100ap = 1;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                            {
                                additionalpercentper100ap = 2;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                            {
                                additionalpercentper100ap = 3;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                            {
                                additionalpercentper100ap = 4;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                            {
                                additionalpercentper100ap = 5;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                            {
                                additionalpercentper100ap = 6;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                            {
                                additionalpercentper100ap = 7;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                            {
                                additionalpercentper100ap = 8;
                            }
                            else if (hero.FlatMagicDamageMod >= 900)
                            {
                                additionalpercentper100ap = 9;
                            }
                            var healthbase = enemy.MaxHealth / 100 * (percentage + additionalpercentper100ap);
                            return CalcMagicDmg(healthbase, enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Evelynn;
                    break;
                case "Ezreal":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    (1.0 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.8 * (hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.75 * (hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (200 + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (1.0 *
                                     (ObjectManager.Player.FlatPhysicalDamageMod + ObjectManager.Player.BaseAttackDamage)) +
                                    (0.9 * ObjectManager.Player.FlatMagicDamageMod), enemy); // basic dmg
                        //default:
                        //throw new InvalidSpellSlotException();

                        //Champ = Ezreal;

                    }
                    break;

                case "Fiddlesticks":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (150 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 150)) +
                                    (2.25 * (hero.FlatMagicDamageMod)), enemy); // complete dmg

                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.45 * (hero.FlatMagicDamageMod)), enemy); // damage per bounce

                        // max damage to the same target_
                        //default:
                        //throw new InvalidSpellSlotException();

                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.45 * (hero.FlatMagicDamageMod)), enemy); // damage per sec

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Fiddlesticks;
                    break;
                case "Fiora":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (1.2 * hero.FlatPhysicalDamageMod), enemy); // for both jumps

                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return
                                CalcPhysicalDmg(
                                    (-20 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 340)) +
                                    (2.4 * hero.FlatPhysicalDamageMod), enemy);

                    }
                    //Champ = Fiora;
                    break;
                case "Fizz":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            var addmg =
                                CalcPhysicalDmg(
                                    hero.BaseAttackDamage + hero.FlatPhysicalDamageMod, enemy);
                            var mdmg =
                                CalcMagicDmg(
                                    (-20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                            return addmg + mdmg;
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 5)) +
                                    (0.25 * hero.FlatMagicDamageMod), enemy); // active dmg

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Fizz;
                    break;
                case "Galio":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (110 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 110)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Galio;
                    break;
                case "Gangplank":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (-5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (1.0 * (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)),
                                    enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 45)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy);
                        // per canonball, 25 max but randomly
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Gangplank;
                    break;
                case "Garen":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.4 * (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)),
                                    enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    (-15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 75)) +
                                    ((1.8 + (0.3 * hero.Spellbook.GetSpell(SpellSlot.E).Level)) *
                                     (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)),
                                    enemy); // complete e dmg

                        case SpellSlot.R:
                            var basedmg = CalcMagicDmg(
                                175 + (175 * hero.Spellbook.GetSpell(SpellSlot.R).Level), enemy);
                            double hpbonus = 0;
                            if (hero.Spellbook.GetSpell(SpellSlot.R).Level == 1)
                            {
                                hpbonus = (enemy.MaxHealth - enemy.Health) / 3.5;
                            }
                            else if (hero.Spellbook.GetSpell(SpellSlot.R).Level == 2)
                            {
                                hpbonus = (enemy.MaxHealth - enemy.Health) / 3;
                            }
                            else if (hero.Spellbook.GetSpell(SpellSlot.R).Level == 2)
                            {
                                hpbonus = (enemy.MaxHealth - enemy.Health) / 2.5;
                            }
                            return basedmg + CalcMagicDmg(hpbonus, enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Garen;
                    break;
                case "Gnar":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    -25 + hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35 +
                                    hero.FlatPhysicalDamageMod + hero.BaseAttackDamage,
                                    enemy);

                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    20 + hero.Spellbook.GetSpell(SpellSlot.W).Level * 5 +
                                    hero.FlatMagicDamageMod +
                                    enemy.MaxHealth *
                                    (0.04 + 0.02 * hero.Spellbook.GetSpell(SpellSlot.W).Level), enemy);


                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    -20 + hero.Spellbook.GetSpell(SpellSlot.E).Level * 40 +
                                    hero.MaxHealth * 0.06, enemy);


                            return
                                CalcPhysicalDmg(
                                    100 + hero.Spellbook.GetSpell(SpellSlot.R).Level * 100 +
                                    hero.FlatPhysicalDamageMod * 0.2 + hero.FlatMagicDamageMod * 0.5, enemy);


                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Gnar;
                    break;
                case "Gragas":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            double hpbonus = (enemy.MaxHealth / 100) * 7 +
                                             hero.Spellbook.GetSpell(SpellSlot.W).Level;
                            return
                                CalcMagicDmg(
                                    (-10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.3 * hero.FlatMagicDamageMod) + hpbonus, enemy);
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Gragas;
                    break;
                case "Graves":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    (42.5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 59.5)) +
                                    (1.36 * hero.FlatPhysicalDamageMod), enemy); // max dmg

                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.5 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Graves;
                    break;
                case "Hecarim":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.6 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 10)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy); // complete dmg

                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 70)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy); // max e dmg


                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Hecarim;
                    break;
                case "Heimerdinger":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (6 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 6)) +
                                    (0.15 * hero.FlatMagicDamageMod), enemy); // per hit dmg

                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (54 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 54)) +
                                    (0.92 * hero.FlatMagicDamageMod), enemy); // max dmg to 1 target_

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }
                    //Champ = Heimerdinger;
                    break;
                case "Irelia":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (-10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (1.0 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                            return 15 * hero.Spellbook.GetSpell(SpellSlot.W).Level;
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcPhysicalDmg(
                                    (160 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 160)) +
                                    (2.4 * hero.FlatPhysicalDamageMod) +
                                    (2.0 * hero.FlatMagicDamageMod), enemy); // max dmg to 1 target_

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Irelia;
                    break;
                case "Janna":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (65 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy); // max dmg

                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 55)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }
                    //Champ = Janna;
                    break;
                case "JarvanIV":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (1.2 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidCastException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.5 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = JarvanIV;
                    break;
                case "Jax":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (1.0 * hero.FlatPhysicalDamageMod) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 60)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Jax;
                    break;
                case "Jayce":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceToTheSkies")
                            {
                                return
                                    CalcPhysicalDmg(
                                        (-25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                        (1.0 * hero.FlatPhysicalDamageMod), enemy);
                            }

                            return
                                CalcPhysicalDmg(
                                    (7 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 77)) +
                                    (1.68 * hero.FlatPhysicalDamageMod), enemy); // e + q


                        case SpellSlot.W:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceStaticField")
                            {

                                return
                                    CalcMagicDmg(
                                        (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 70)) +
                                        (1.0 * hero.FlatMagicDamageMod), enemy); // complete dmg

                            }
                            return 0;
                        // return 0, no exception as when switching the name isn't directly changed and ppl will already try to calculate
                        case SpellSlot.E:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceThunderingBlow")
                            {
                                double percentage = 5 + (3 * hero.Spellbook.GetSpell(SpellSlot.E).Level);
                                return
                                    CalcMagicDmg(
                                        ((enemy.MaxHealth / 100) * percentage) + (hero.FlatPhysicalDamageMod),
                                        enemy);
                            }
                            return 0;
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }
                    //Champ = Jayce;
                    break;
                case "Jinx":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (-40 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (1.4 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            var percentage =
                                CalcPhysicalDmg(
                                    ((enemy.MaxHealth - enemy.Health) / 100) *
                                    (20 + (5 * hero.Spellbook.GetSpell(SpellSlot.R).Level)), enemy);
                            return percentage +
                                   CalcPhysicalDmg(
                                       (150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                       (1.0 * hero.FlatPhysicalDamageMod), enemy); // max dmg

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Jinx;
                    break;
                case "Karma":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy); // basic q

                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy); // basic w

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (-20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy); // mantra e (shield with dmg)
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }
                    //Champ = Karma;
                    break;
                case "Karthus":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy); // single target_ dmg

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Karthus;
                    break;
                case "Kassadin":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (55 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (55 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                                    (0.02 * hero.MaxMana), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Kassadin;
                    break;
                case "Katarina":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        // total dmg (mark + detonation)

                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                                    (0.25 * hero.FlatMagicDamageMod) +
                                    (0.6 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (225 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (2.5 * hero.FlatMagicDamageMod) +
                                    (3.75 * hero.FlatPhysicalDamageMod), enemy); // complete ult dmg

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Katarina;
                    break;
                case "Kayle":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 10)) +
                                    (0.25 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            //throw new InvalidSpellSlotException();
                            //default:
                            //throw new InvalidSpellSlotException();
                            break;
                    }
                    //Champ = Kayle;
                    break;
                case "Kennen":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.55 * hero.FlatMagicDamageMod), enemy); // active dmg

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 195)) +
                                    (1.2 * hero.FlatMagicDamageMod), enemy); // max dmg to 1 target_

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Kennen;
                    break;
                case "Khazix":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    ((30 + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                     (1.2 * ObjectManager.Player.FlatPhysicalDamageMod)) * 1.3, enemy); // isolated q


                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.2 * hero.FlatPhysicalDamageMod), enemy);

                    }
                    //Champ = Khazix;
                    break;
                case "KogMaw":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            var percentofmaxhealth = (1.0 + (hero.Spellbook.GetSpell(SpellSlot.W).Level));
                            double additionalpercentper100ap = 0;
                            if (hero.FlatMagicDamageMod < 100)
                            {
                                additionalpercentper100ap = 0;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                            {
                                additionalpercentper100ap = 1;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                            {
                                additionalpercentper100ap = 2;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                            {
                                additionalpercentper100ap = 3;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                            {
                                additionalpercentper100ap = 4;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                            {
                                additionalpercentper100ap = 5;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                            {
                                additionalpercentper100ap = 6;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                            {
                                additionalpercentper100ap = 7;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                            {
                                additionalpercentper100ap = 8;
                            }
                            else if (hero.FlatMagicDamageMod >= 900)
                            {
                                additionalpercentper100ap = 9;
                            }
                            var healthbase = enemy.MaxHealth / 100 *
                                             (percentofmaxhealth + additionalpercentper100ap);
                            return CalcMagicDmg(healthbase, enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (80 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 80)) +
                                    (0.3 * hero.FlatMagicDamageMod) +
                                    (0.5 * hero.FlatPhysicalDamageMod), enemy); // 100% bonus dmg to champs
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = KogMaw;
                    break;
                case "Leblanc":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy); // total q dmg


                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // total e dmg

                        case SpellSlot.R:

                            //throw new InvalidSpellSlotException();

                            return
                                CalcMagicDmg(
                                    (0 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy); // q as ulted version

                        //default:
                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = LeBlanc;
                    break;
                case "LeeSin":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    40 + ((hero.Spellbook.GetSpell(SpellSlot.Q).Level * 60)) +
                                    (1.8 * hero.FlatPhysicalDamageMod) +
                                    (8 * ((enemy.MaxHealth / enemy.Health) / 100)), enemy);

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    hero.FlatMagicDamageMod, enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.R).Level * 200) +
                                    (2.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = LeeSin;
                    break;
                case "Leona":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (0.30 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.40 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.40 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.80 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Leona;
                    break;
                case "Lissandra":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.40 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.70 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();

                    }
                    //Champ = Lissandra;
                    break;
                case "Lucian":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                                    hero.FlatPhysicalDamageMod, enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.3 * hero.FlatPhysicalDamageMod) +
                                    (0.9 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                                    (0.1 * hero.FlatMagicDamageMod) +
                                    (0.3 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Lucian;
                    break;
                case "Lulu":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return CalcMagicDmg((35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);

                    }
                    //Champ = Lulu;
                    break;
                case "Lux":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (200 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Lux;
                    break;
                case "Malphite":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.2 * hero.FlatMagicDamageMod) + (0.3 * hero.Armor), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.3 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Malphite;
                    break;
                case "Malzahar":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    ((hero.Spellbook.GetSpell(SpellSlot.W).Level + 3) +
                                     (0.1 * hero.FlatMagicDamageMod)) * (enemy.MaxHealth / 100), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.3 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Malzahar;
                    break;
                case "Maokai":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            var percentage = ((7.5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 1.5)) +
                                              (0.04 * hero.FlatMagicDamageMod));
                            return CalcMagicDmg((enemy.MaxHealth / 100) * percentage, enemy);
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (200 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Maokai;
                    break;
                case "MasterYi":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage), enemy);

                    }
                    //Champ = MasterYi;
                    break;
                case "MissFortune":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                                    (0.85 *
                                     (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcPhysicalDmg(
                                    (200 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 200)) +
                                    (1.6 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = MissFortune;
                    break;
                case "Mordekaiser":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (82.5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 49.5)) +
                                    (0.66 * hero.FlatMagicDamageMod) +
                                    (1.65 * hero.FlatPhysicalDamageMod), enemy);

                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 84)) +
                                    (1.2 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    ((19 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 5)) +
                                     (0.4 * hero.FlatMagicDamageMod)) * (1 - (enemy.MaxHealth / 100)), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Mordekaiser;
                    break;
                case "Morgana":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 105)) +
                                    (1.65 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (1.40 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Morgana;
                    break;
                case "Nami":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                                    (0.20 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Nami;
                    break;
                case "Nasus":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (from buff in hero.Buffs
                                     where buff.DisplayName == "NasusQStacks"
                                     select buff.Count).FirstOrDefault() +
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20)), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                                    (1.2 * hero.FlatMagicDamageMod), enemy);


                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (((hero.Spellbook.GetSpell(SpellSlot.R).Level + 2) +
                                      (0.01 * hero.FlatMagicDamageMod)) * (enemy.MaxHealth / 100)) * 15,
                                    enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Nasus;
                    break;
                case "Nautilus":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                                    (0.40 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (0.80 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Nautilus;
                    break;
                case "Nidalee":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            if (hero.Spellbook.GetSpell(SpellSlot.Q).Name == "JavelinToss")
                            {

                                return
                                    CalcMagicDmg(
                                        (37 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 100)) +
                                        (1.625 * hero.FlatMagicDamageMod), enemy); // max dmg

                            }

                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 90)) +
                                    (3.0 * hero.FlatPhysicalDamageMod), enemy); // max dmg

                        case SpellSlot.W:
                            if (hero.Spellbook.GetSpell(SpellSlot.W).Name == "Bushwhack")
                            {
                                return
                                    CalcMagicDmg(
                                        (hero.Spellbook.GetSpell(SpellSlot.W).Level * 20) +
                                        (0.1 + (0.02 * hero.Spellbook.GetSpell(SpellSlot.W).Level)), enemy);
                            }
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            if (hero.Spellbook.GetSpell(SpellSlot.E).Name == "PrimalSurge")
                            {
                                return 0; // no exception as switchting won't change the name directly
                            }
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 75)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);

                    }
                    //Champ = Nidalee;
                    break;
                case "Nocturne":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.75 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.2 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Nocturne;
                    break;
                case "Nunu":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.75 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.2 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Nunu;
                    break;
                case "Olaf":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45) +
                                    (0.4 * (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)));

                    }
                    //Champ = Olaf;
                    break;
                case "oriannanoball":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (60.0 + (30.0 * hero.Spellbook.GetSpell(SpellSlot.Q).Level) +
                                     (0.5 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (70.0 + (45.0 * hero.Spellbook.GetSpell(SpellSlot.W).Level) +
                                     (0.7 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (60.0 + (30.0 * hero.Spellbook.GetSpell(SpellSlot.E).Level) +
                                     (0.3 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (150.0 + (75.0 * hero.Spellbook.GetSpell(SpellSlot.R).Level) +
                                     (0.7 * hero.FlatMagicDamageMod)), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Orianna;
                    break;
                case "Orianna":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (60.0 + (30.0 * hero.Spellbook.GetSpell(SpellSlot.Q).Level) +
                                     (0.5 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (70.0 + (45.0 * hero.Spellbook.GetSpell(SpellSlot.W).Level) +
                                     (0.7 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (60.0 + (30.0 * hero.Spellbook.GetSpell(SpellSlot.E).Level) +
                                     (0.3 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (150.0 + (75.0 * hero.Spellbook.GetSpell(SpellSlot.R).Level) +
                                     (0.7 * hero.FlatMagicDamageMod)), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();

                    }
                    //Champ = Orianna;
                    break;
                case "Pantheon":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (1.4 * hero.FlatPhysicalDamageMod) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (3.6 * hero.FlatPhysicalDamageMod), enemy);

                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 300)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Pantheon;
                    break;
                case "Poppy":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                                    (1.0 * (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage)) +
                                    (0.6 * hero.FlatMagicDamageMod) + (0.08 * enemy.MaxHealth), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);


                    }

                    //Champ = Poppy;
                    break;
                case "Quinn":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.65 * hero.FlatPhysicalDamageMod) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.20 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    ((70 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                                     (0.50 * hero.FlatPhysicalDamageMod)) * (2 - enemy.Health / enemy.MaxHealth),
                                    enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Quinn;
                    break;
                case "Rammus":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                                    (0.1 * hero.Armor), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (520 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 520)) +
                                    (2.4 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Rammus;
                    break;
                case "Renekton":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    ((0.8 * hero.FlatPhysicalDamageMod)), enemy); // basic q

                        case SpellSlot.W:

                            return
                                CalcPhysicalDmg(
                                    -10 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 20) +
                                    ((1.5 *
                                      (hero.FlatPhysicalDamageMod +
                                       hero.BaseAttackDamage))), enemy); // basic w

                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    0 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30) +
                                    ((0.9 * (hero.FlatPhysicalDamageMod))), enemy); // basic e

                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (450 * hero.Spellbook.GetSpell(SpellSlot.R).Level) +
                                    (1.5 * hero.FlatMagicDamageMod), enemy); // complete dmg

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Renekton;
                    break;
                case "Rengar":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return CalcPhysicalDmg(
                                    30 + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    ((0.95 * (5 * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level)) +
                                     (ObjectManager.Player.FlatPhysicalDamageMod + ObjectManager.Player.BaseAttackDamage)),
                                    enemy);
                            //30+(120)+(20)+
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 30) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy); // basic w

                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                                    (0.7 * hero.FlatPhysicalDamageMod), enemy); // basic e


                    }
                    //Champ = Rengar;
                    break;
                case "Riven":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcPhysicalDmg(
                                    30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 60) +
                                    (105 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) *
                                    (hero.FlatPhysicalDamageMod + hero.BaseAttackDamage),
                                    enemy);

                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 30) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            if ((enemy.Health / enemy.MaxHealth) * 100 > 25)
                            {
                                return
                                    CalcPhysicalDmg(
                                        40 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 40) +
                                        (0.6 * hero.FlatPhysicalDamageMod), enemy);
                            }
                            return
                                CalcPhysicalDmg(
                                    120 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 120) +
                                    (1.8 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Riven;
                    break;
                case "Rumble":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 60) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    325 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 325) +
                                    (1.5 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Rumble;
                    break;
                case "Ryze":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25) +
                                    (0.4 * hero.FlatMagicDamageMod) + (0.065 * hero.MaxMana),
                                    enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35) +
                                    (0.6 * hero.FlatMagicDamageMod) + (0.045 * hero.MaxMana),
                                    enemy);
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    90 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 60) +
                                    (1.05 * hero.FlatMagicDamageMod) +
                                    (0.03 * hero.MaxMana), enemy);
                    }
                    //Champ = Ryze;
                    break;
                case "Sejuani":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    (2 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 2)) *
                                    (enemy.MaxHealth / 100), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    60 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 60) +
                                    (0.9 * hero.FlatMagicDamageMod) +
                                    ((hero.ScriptHealthBonus / 100) * 10), enemy);

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Sejuani;
                    break;
                case "Shaco":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    0.20 +
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.20) *
                                    (hero.FlatPhysicalDamageMod + hero.BaseAbilityDamage), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 15) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy);

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40) +
                                    (1.0 * hero.FlatMagicDamageMod) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    150 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 150) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Shaco;
                    break;
                case "Shen":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);

                    }
                    //Champ = Shen;
                    break;
                case "Shyvanna":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    0.75 +
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                                    (hero.FlatPhysicalDamageMod + hero.BaseAbilityDamage), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    35 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 105) +
                                    (1.4 * hero.FlatPhysicalDamageMod), enemy);

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Shyvana;
                    break;
                case "Singed":
                    switch (slot)
                    {
                        case SpellSlot.Q:

                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 12)) +
                                    (0.3 * hero.FlatMagicDamageMod), enemy); // per sec

                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);

                    }
                    //Champ = Singed;
                    break;
                case "Sion":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (12 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 57.5)) +
                                    (0.9 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.9 * hero.FlatMagicDamageMod), enemy);


                    }
                    //Champ = Sion;
                    break;
                case "Sivir":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    ((0.6 + (0.1 * hero.Spellbook.GetSpell(SpellSlot.Q).Level)) *
                                     hero.FlatPhysicalDamageMod) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy); // basic physical dmg
                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod) *
                                    (0.45 + (0.05 * hero.Spellbook.GetSpell(SpellSlot.W).Level)), enemy);
                        // for each of 3

                    }
                    //Champ = Sivir;
                    break;
                case "Skarner":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (8 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                                    (0.4 * hero.FlatPhysicalDamageMod), enemy); // basic bonus dmg
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Skarner;
                    break;
                case "Sona":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Sona;
                    break;
                case "Soraka":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.4 * hero.FlatMagicDamageMod) + (0.5 * hero.MaxMana),
                                    enemy);


                    }
                    //Champ = Soraka;
                    break;
                case "Swain":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                                    (0.3 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.8 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy); //x sec
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Swain;
                    break;
                case "Syndra":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (45 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 45)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy); // for each orb

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Syndra;
                    break;
                case "Talon":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (0 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (1.3 * (hero.FlatPhysicalDamageMod)), enemy); // bonus dmg
                        case SpellSlot.W:
                            return
                                CalcPhysicalDmg(
                                    (5 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                                    (0.60 * (hero.FlatPhysicalDamageMod)), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (70 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                                    (0.75 * (hero.FlatPhysicalDamageMod)), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Talon;
                    break;
                case "Taric":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.W).Level * 40) +
                                    (0.2 * hero.Armor), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.2 * hero.FlatMagicDamageMod), enemy); // min e dmg
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Taric;
                    break;
                case "Teemo":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.80 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (0 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 34)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy); // total dmg for one hit
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Teemo;
                    break;
                case "Thresh":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.4 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (100 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Thresh;
                    break;
                case "Tristana":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.80 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:

                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // active

                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (200 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.5 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Tristana;
                    break;
                case "Trundle":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (0 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    ((0.95 + (0.05 * hero.Spellbook.GetSpell(SpellSlot.Q).Level)) *
                                     hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            double basepercent = 16 + (4 * hero.Spellbook.GetSpell(SpellSlot.R).Level);
                            double additionalpercentper100ap = 0;
                            if (hero.FlatMagicDamageMod < 100)
                            {
                                additionalpercentper100ap = 0;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                            {
                                additionalpercentper100ap = 2;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                            {
                                additionalpercentper100ap = 4;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                            {
                                additionalpercentper100ap = 6;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                            {
                                additionalpercentper100ap = 8;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                            {
                                additionalpercentper100ap = 10;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                            {
                                additionalpercentper100ap = 12;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                            {
                                additionalpercentper100ap = 14;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                            {
                                additionalpercentper100ap = 16;
                            }
                            else if (hero.FlatMagicDamageMod >= 900)
                            {
                                additionalpercentper100ap = 18;
                            }
                            var healthbase = enemy.MaxHealth / 100 * (basepercent + additionalpercentper100ap);
                            return CalcMagicDmg(healthbase, enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Trundle;
                    break;
                case "Tryndamere":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (1.2 * (hero.FlatPhysicalDamageMod)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);


                    }
                    //Champ = Tryndamere;
                    break;
                case "TwistedFate":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (10 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                                    (0.5 * hero.FlatMagicDamageMod) +
                                    (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod),
                                    enemy); // Red Card


                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.5 * hero.FlatMagicDamageMod), enemy);

                    }
                    //Champ = TwistedFate;
                    break;
                case "Twitch":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            var basedmg = CalcPhysicalDmg(
                                5 + (15 * hero.Spellbook.GetSpell(SpellSlot.E).Level), enemy);
                            var perstack =
                                CalcPhysicalDmg(
                                    10 + (5 * hero.Spellbook.GetSpell(SpellSlot.E).Level) +
                                    (0.2 * hero.FlatMagicDamageMod) +
                                    (0.25 * hero.FlatPhysicalDamageMod), enemy);

                            return basedmg + (5 * perstack); // complete dmg 5 stacks


                    }
                    //Champ = Twitch;
                    break;
                case "Udyr":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            var percentadbonus = 1.1 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 0.1);
                            return
                                CalcPhysicalDmg(
                                    (-20 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (percentadbonus *
                                     (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                                    (1.25 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Udyr;
                    break;
                case "Urgot":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30) - 20 +
                                    (0.85 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                                    (0.60 * (hero.FlatPhysicalDamageMod)), enemy);

                    }
                    //Champ = Urgot;
                    break;
                case "Varus":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (-40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (1.6 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy); // max dmg, first target_
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (6 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 4)) +
                                    (0.25 * hero.FlatMagicDamageMod), enemy); // passive magic dmg
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.60 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Varus;
                    break;
                case "Vayne":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            var percentofbonusad = 0.25 + (0.05 * hero.Spellbook.GetSpell(SpellSlot.Q).Level);
                            return
                                CalcPhysicalDmg(
                                    percentofbonusad *
                                    (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod), enemy);
                        // ony the bonus dmg
                        case SpellSlot.W:
                            double flattruedmg = 10 + (10 * hero.Spellbook.GetSpell(SpellSlot.W).Level);
                            double percentofenemyhp = 3 + (hero.Spellbook.GetSpell(SpellSlot.W).Level);
                            return flattruedmg + ((enemy.MaxHealth / 100) * percentofenemyhp);
                        case SpellSlot.E:

                            return
                                CalcPhysicalDmg(
                                    (20 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 70)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy);
                        // Damage when knock + against wall


                    }
                    //Champ = Vayne;
                    break;
                case "Veigar":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (70 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (125 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.2 * hero.FlatMagicDamageMod) + (0.8 * enemy.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Veigar;
                    break;
                case "Velkoz":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.625 * hero.FlatMagicDamageMod), enemy); // complete dmg

                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                                    (0.06 * hero.FlatMagicDamageMod), enemy); //x 0,25sec
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Velkoz;
                    break;
                case "Vi":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.80 * (hero.FlatPhysicalDamageMod)), enemy);
                        case SpellSlot.W:
                            var percentage = 2.5 * (1.5 * hero.Spellbook.GetSpell(SpellSlot.W).Level);
                            var bonusadpercentage = percentage + (hero.FlatPhysicalDamageMod / 34);
                            return (enemy.MaxHealth / 100 * bonusadpercentage);
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (-10 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                                    (0.70 * hero.FlatMagicDamageMod) +
                                    (1.15 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (75 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.4 * hero.FlatPhysicalDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Vi;
                    break;
                case "Viktor":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (55 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35) +
                                     (0.60 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.70 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (70 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 120)) +
                                    (0.79 * hero.FlatMagicDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Viktor;
                    break;
                case "Vladimir":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35) +
                                     (0.60 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.W:

                            return
                                CalcMagicDmg(
                                    (25 + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level * 55) +
                                     (15 * ObjectManager.Player.ScriptHealthBonus)), enemy); // complete w dmg

                        case SpellSlot.E:
                            var edmg =
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.45 * hero.FlatMagicDamageMod), enemy);

                            return edmg;


                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.70 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Vladimir;
                    break;
                case "Volibear":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return CalcPhysicalDmg((hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30), enemy);
                        case SpellSlot.W:
                            var basedmg =
                                CalcPhysicalDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.15 * hero.ScriptHealthBonus), enemy);
                            double percentmissinghealth = 100 - ((enemy.Health / enemy.MaxHealth) * 100);
                            return basedmg * percentmissinghealth;
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.R).Level * 80) - 5 +
                                    (0.30 * hero.FlatMagicDamageMod), enemy); //RÃœBERGUCKEN
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Volibear;
                    break;
                case "Warwick":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            var percentagedmg =
                                CalcMagicDmg(
                                    (enemy.MaxHealth / 100 *
                                     (6 + (2 * hero.Spellbook.GetSpell(SpellSlot.Q).Level))) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                            var flatdmg =
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy);
                            if (percentagedmg > flatdmg)
                            {
                                return percentagedmg;
                            }
                            return flatdmg;
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (165 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 85)) +
                                    (2.0 * hero.FlatPhysicalDamageMod), enemy);

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Warwick;
                    break;
                case "MonkeyKing":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    (1.1 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.60 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (0.8 * (hero.FlatPhysicalDamageMod)), enemy);
                        case SpellSlot.R:

                            return
                                CalcPhysicalDmg(
                                    (-280 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 360)) +
                                    (4.4 *
                                     (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy); // max damage

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = MonkeyKing;
                    break;
                case "Xerath":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.75 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (60 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.90 * hero.FlatMagicDamageMod), enemy); //NOT EMPOWERED
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.45 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:

                            return
                                CalcMagicDmg(
                                    (405 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 165)) +
                                    (1.3 * hero.FlatMagicDamageMod), enemy); // 3 hits on target_

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Xerath;
                    break;
                case "XinZhao":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 15) +
                                    (1.0 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy); // per hit
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (-25 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.0 * hero.FlatPhysicalDamageMod) + ((enemy.Health / 100) * 15), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = XinZhao;
                    break;
                case "Yasuo":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                                    (1.0 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (50 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.6 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.R).Level * 100) +
                                    (1.5 * (hero.FlatPhysicalDamageMod)), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Yasuo;
                    break;
                case "Yorick":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    (1.2 * (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod)),
                                    enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35) +
                                     (1.0 * hero.FlatMagicDamageMod)), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (1.0 * (hero.FlatPhysicalDamageMod)), enemy);

                    }
                    //Champ = Yorick;
                    break;
                case "Zac":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            var basedmg =
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                            double percentofmaxhealth = (3 + (hero.Spellbook.GetSpell(SpellSlot.W).Level));
                            double additionalpercentper100ap = 0;
                            if (hero.FlatMagicDamageMod < 100)
                            {
                                additionalpercentper100ap = 0;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(100, 199))
                            {
                                additionalpercentper100ap = 2;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(200, 299))
                            {
                                additionalpercentper100ap = 4;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(300, 399))
                            {
                                additionalpercentper100ap = 6;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(400, 499))
                            {
                                additionalpercentper100ap = 8;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(500, 599))
                            {
                                additionalpercentper100ap = 10;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(600, 699))
                            {
                                additionalpercentper100ap = 12;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(700, 799))
                            {
                                additionalpercentper100ap = 14;
                            }
                            else if (hero.FlatMagicDamageMod.IsBetween(800, 899))
                            {
                                additionalpercentper100ap = 16;
                            }
                            else if (hero.FlatMagicDamageMod >= 900)
                            {
                                additionalpercentper100ap = 18;
                            }
                            var healthbase = enemy.MaxHealth / 100 *
                                             (percentofmaxhealth + additionalpercentper100ap);
                            return basedmg + CalcMagicDmg(healthbase, enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (40 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.7 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:


                            return
                                CalcMagicDmg(
                                    (175 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (1.0 * hero.FlatMagicDamageMod), enemy); // all jumps on enemy
                        //default:
                        //throw new InvalidSpellSlotException();

                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Zac;
                    break;
                case "Zed":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcPhysicalDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (1.0 * hero.FlatPhysicalDamageMod), enemy); // 1 hit
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcPhysicalDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.8 * hero.FlatPhysicalDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcPhysicalDmg(
                                    1.0 * (hero.FlatMagicDamageMod + hero.BaseAttackDamage),
                                    enemy);
                        // base dmg
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Zed;
                    break;
                case "Ziggs":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (30 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (15 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.30 * hero.FlatMagicDamageMod), enemy); // per mine
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (125 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (0.35 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    //Champ = Ziggs;
                    break;
                case "Zilean":
                    switch (slot)
                    {

                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (0.9 * hero.FlatMagicDamageMod), enemy);
                    }
                    //Champ = Zilean;
                    break;
                case "Zyra":
                    switch (slot)
                    {
                        case SpellSlot.Q:
                            return
                                CalcMagicDmg(
                                    (35 + (hero.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.65 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.W:
                        //throw new InvalidSpellSlotException();
                        case SpellSlot.E:
                            return
                                CalcMagicDmg(
                                    (25 + (hero.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.50 * hero.FlatMagicDamageMod), enemy);
                        case SpellSlot.R:
                            return
                                CalcMagicDmg(
                                    (95 + (hero.Spellbook.GetSpell(SpellSlot.R).Level * 85)) +
                                    (0.70 * hero.FlatMagicDamageMod), enemy);
                        //default:
                        //throw new InvalidSpellSlotException();
                    }
                    break;
            }
            //Champ = Zyra;
            return 0;



        }


        public static void Game_OnGameUpdate(EventArgs args)
        {

        }

        public static double CalcMagicDmg(double dmg, Obj_AI_Base enemy)
        {
            double additionaldmg = 0;

            if (enemy.CombatType == GameObjectCombatType.Melee)
            {
                additionaldmg = dmg * 0.02;
            }
            else
            {
                additionaldmg = dmg * 0.015;
            }


            additionaldmg += dmg * 0.03;


            if ((enemy.Health / enemy.MaxHealth) * 100 < 50)
            {
                additionaldmg += dmg * 0.05;
            }



            var reducedmg = 0;

            double newspellblock = enemy.SpellBlock * enemy.PercentMagicPenetrationMod;
            var dmgreduction = 100 / (100 + newspellblock - enemy.FlatMagicPenetrationMod);
            return (((dmg + additionaldmg) * dmgreduction)) - reducedmg;
        }

        public static double CalcPhysicalDmg(double dmg, Obj_AI_Base enemy)
        {
            double additionaldmg = 0;

            if (enemy.CombatType == GameObjectCombatType.Melee)
            {
                additionaldmg += dmg * 0.02;
            }
            else
            {
                additionaldmg += dmg * 0.015;
            }



            additionaldmg += dmg * 0.03;



            if ((enemy.Health / enemy.MaxHealth) * 100 < 50)
            {
                additionaldmg += dmg * 0.05;
            }


            var reducedmg = 0;



            double newarmor = enemy.Armor * enemy.PercentArmorPenetrationMod;
            var dmgreduction = 100 / (100 + newarmor - enemy.FlatArmorPenetrationMod);
            return (((dmg + additionaldmg) * dmgreduction)) - reducedmg;
        }
    }
}
