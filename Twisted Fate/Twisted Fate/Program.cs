#region
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace TwistedFate
{
    internal class Program
    {
        private static Menu Config;

        private static Spell Q;
        private static readonly float Qangle = 28 * (float)Math.PI / 180;
        private static Orbwalking.Orbwalker SOW;
        private static Vector2 PingLocation;
        private static int LastPingT = 0;
        private static Obj_AI_Hero Player;
        private static int CastQTick;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Ping(Vector2 position)
        {
            if (Utils.TickCount - LastPingT < 30 * 1000)
            {
                return;
            }

            LastPingT = Utils.TickCount;
            PingLocation = position;
            SimplePing();

            Utility.DelayAction.Add(150, SimplePing);
            Utility.DelayAction.Add(300, SimplePing);
            Utility.DelayAction.Add(400, SimplePing);
            Utility.DelayAction.Add(800, SimplePing);
        }

        private static void SimplePing()
        {
            Game.ShowPing(PingCategory.Fallback, PingLocation, true);
        }

        private static int timeChangeW;
        private static int timeToggleGold;

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != "TwistedFate") return;
            Player = ObjectManager.Player;
            Q = new Spell(SpellSlot.Q, 1450);
            Q.SetSkillshot(0.25f, 40f, 1000f, false, SkillshotType.SkillshotLine);

            //Make the menu
            Config = new Menu("Twisted Fate", "TwistedFate", true);

            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            var SowMenu = new Menu("Orbwalking", "Orbwalking");
            SOW = new Orbwalking.Orbwalker(SowMenu);
            Config.AddSubMenu(SowMenu);

            /* Q */
            var q = new Menu("Q - Wildcards", "Q");
            {
                q.AddItem(new MenuItem("AutoQI", "Auto-Q immobile").SetValue(true));
                q.AddItem(new MenuItem("AutoQD", "Auto-Q dashing").SetValue(true));
                q.AddItem(
                    new MenuItem("CastQ", "Cast Q (tap)").SetValue(new KeyBind('U', KeyBindType.Press)));
                Config.AddSubMenu(q);
            }

            /* W */
            var w = new Menu("W - Pick a card", "W");
            {
                w.AddItem(new MenuItem("SelectCard", "Card to Pick").SetValue(new Slider(2, 0, 2)));
                w.AddItem(new MenuItem("Info", "0 = blue, 1 = red, 2 = gold"));
                //w.AddItem(
                //    new MenuItem("SelectYellow", "Select Yellow").SetValue(new KeyBind("W".ToCharArray()[0],
                //        KeyBindType.Press)));
                //w.AddItem(
                //    new MenuItem("SelectBlue", "Select Blue").SetValue(new KeyBind("E".ToCharArray()[0],
                //        KeyBindType.Press)));
                w.AddItem(
                    new MenuItem("ChangeCard", "Change Card Pick").SetValue(new KeyBind('T', KeyBindType.Press)));
                Config.AddSubMenu(w);

                w.AddItem(new MenuItem("WFarm", "W Farm Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                w.AddItem(
                    new MenuItem("AlwaysBlue", "Always use Blue card in farm if mana < x%").SetValue(new Slider(20, 0,
                        100)));
                w.AddItem(new MenuItem("RedMinions", "Only use Red Card for farm if > X minions in range"))
                    .SetValue(new Slider(1, 0, 5));

                w.AddItem(new MenuItem("AlwaysGold", "Always use Gold Card in Combo?").SetValue(false));
                w.AddItem(new MenuItem("ToggleGold", "^ toggle on/off").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            }

            var menuItems = new Menu("Items", "Items");
            {
                menuItems.AddItem(new MenuItem("itemBotrk", "Botrk").SetValue(true));
                menuItems.AddItem(new MenuItem("itemYoumuu", "Youmuu").SetValue(true));
                menuItems.AddItem(
                    new MenuItem("itemMode", "Use items on").SetValue(
                        new StringList(new[] { "No", "Mixed mode", "Combo mode", "Both" }, 2)));
                Config.AddSubMenu(menuItems);
            }

            var r = new Menu("R - Destiny", "R");
            {
                r.AddItem(new MenuItem("AutoY", "Select yellow card after R").SetValue(true));
                Config.AddSubMenu(r);
            }

            var misc = new Menu("Misc", "Misc");
            {
                misc.AddItem(new MenuItem("PingLH", "Ping low health enemies (Only local)").SetValue(true));
                misc.AddItem(new MenuItem("DisplayLH", "Notify on low health enemies").SetValue(false));
                Config.AddSubMenu(misc);
            }

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit = ComboDamage;
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate (object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            /*Drawing*/
            var drawings = new Menu("Drawings", "Drawings");
            {
                drawings.AddItem(
                    new MenuItem("Qcircle", "Q Range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("Rcircle", "R Range").SetValue(new Circle(true, Color.FromArgb(100, 255, 255, 255))));
                drawings.AddItem(
                    new MenuItem("Rcircle2", "R Range (minimap)").SetValue(new Circle(true,
                        Color.FromArgb(255, 255, 255, 255))));
                drawings.AddItem(dmgAfterComboItem);
                Config.AddSubMenu(drawings);
            }

            Config.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddToMainMenu();

            Config.Item("SelectCard").SetValue(new Slider(0, 0, 2));
            timeChangeW = Environment.TickCount;
            timeToggleGold = Environment.TickCount;
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += DrawingOnOnEndScene;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
                args.Process = CardSelector.Status != SelectStatus.Selecting &&
                               Utils.TickCount - CardSelector.LastWSent > 300;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "gate" && Config.Item("AutoY").GetValue<bool>())
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
        }

        private static void DrawingOnOnEndScene(EventArgs args)
        {
            var rCircle2 = Config.Item("Rcircle2").GetValue<Circle>();
            if (rCircle2.Active)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 5500, rCircle2.Color, 1, 23, true);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qCircle = Config.Item("Qcircle").GetValue<Circle>();
            var rCircle = Config.Item("Rcircle").GetValue<Circle>();

            if (qCircle.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, qCircle.Color);
            }

            if (rCircle.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, 5500, rCircle.Color);
            }


            Vector2 screenPos = Drawing.WorldToScreen(Player.Position);
            switch (Config.Item("SelectCard").GetValue<Slider>().Value)
            {
                case 0:
                    Drawing.DrawText(screenPos.X,screenPos.Y,Color.Blue,"Blue Card");
                    break;
                case 1:
                    Drawing.DrawText(screenPos.X,screenPos.Y,Color.Red,"Red Card");
                    break;
                case 2:
                    Drawing.DrawText(screenPos.X, screenPos.Y, Color.Yellow, "Yellow Card");
                    break;
            }

            if (Config.Item("AlwaysGold").GetValue<bool>())
            {
                Drawing.DrawText(screenPos.X, screenPos.Y+13, Color.Yellow, "Always Combo Gold");
            }

            if (Config.Item("DisplayLH").GetValue<bool>())
            {
                var ydiff = 13;
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready &&
                                    h.IsValidTarget() && ComboDamage(h) > h.Health))
                {
                    Drawing.DrawText(screenPos.X, screenPos.Y + ydiff + 13, Color.White, enemy.Name);
                    ydiff += 13;
                }
            }
        }


        private static int CountHits(Vector2 position, List<Vector2> points, List<int> hitBoxes)
        {
            var result = 0;

            var startPoint = ObjectManager.Player.ServerPosition.To2D();
            var originalDirection = Q.Range * (position - startPoint).Normalized();
            var originalEndPoint = startPoint + originalDirection;

            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];

                for (var k = 0; k < 3; k++)
                {
                    var endPoint = new Vector2();
                    if (k == 0) endPoint = originalEndPoint;
                    if (k == 1) endPoint = startPoint + originalDirection.Rotated(Qangle);
                    if (k == 2) endPoint = startPoint + originalDirection.Rotated(-Qangle);

                    if (point.Distance(startPoint, endPoint, true, true) <
                        (Q.Width + hitBoxes[i]) * (Q.Width + hitBoxes[i]))
                    {
                        result++;
                        break;
                    }
                }
            }

            return result;
        }

        private static void CastQ(Obj_AI_Base unit, Vector2 unitPosition, int minTargets = 0)
        {
            var points = new List<Vector2>();
            var hitBoxes = new List<int>();

            var startPoint = ObjectManager.Player.ServerPosition.To2D();
            var originalDirection = Q.Range * (unitPosition - startPoint).Normalized();

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsValidTarget() && enemy.NetworkId != unit.NetworkId)
                {
                    var pos = Q.GetPrediction(enemy);
                    if (pos.Hitchance >= HitChance.Medium)
                    {
                        points.Add(pos.UnitPosition.To2D());
                        hitBoxes.Add((int)enemy.BoundingRadius);
                    }
                }
            }

            var posiblePositions = new List<Vector2>();

            for (var i = 0; i < 3; i++)
            {
                if (i == 0) posiblePositions.Add(unitPosition + originalDirection.Rotated(0));
                if (i == 1) posiblePositions.Add(startPoint + originalDirection.Rotated(Qangle));
                if (i == 2) posiblePositions.Add(startPoint + originalDirection.Rotated(-Qangle));
            }


            if (startPoint.Distance(unitPosition) < 900)
            {
                for (var i = 0; i < 3; i++)
                {
                    var pos = posiblePositions[i];
                    var direction = (pos - startPoint).Normalized().Perpendicular();
                    var k = (2 / 3 * (unit.BoundingRadius + Q.Width));
                    posiblePositions.Add(startPoint - k * direction);
                    posiblePositions.Add(startPoint + k * direction);
                }
            }

            var bestPosition = new Vector2();
            var bestHit = -1;

            foreach (var position in posiblePositions)
            {
                var hits = CountHits(position, points, hitBoxes);
                if (hits > bestHit)
                {
                    bestPosition = position;
                    bestHit = hits;
                }
            }

            if (bestHit + 1 <= minTargets)
                return;

            Q.Cast(bestPosition.To3D(), true);
        }

        private static float ComboDamage(Obj_AI_Hero hero)
        {
            var dmg = 0d;
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q) * 2;
            dmg += Player.GetSpellDamage(hero, SpellSlot.W);
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q);

            if (ObjectManager.Player.GetSpellSlot("SummonerIgnite") != SpellSlot.Unknown)
            {
                dmg += ObjectManager.Player.GetSummonerSpellDamage(hero, Damage.SummonerSpell.Ignite);
            }

            return (float)dmg;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("PingLH").GetValue<bool>())
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready &&
                                    h.IsValidTarget() && ComboDamage(h) > h.Health))
                {
                    Ping(enemy.Position.To2D());
                }

            if (Config.Item("CastQ").GetValue<KeyBind>().Active)
            {
                CastQTick = Utils.TickCount;
            }
            var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (Utils.TickCount - CastQTick < 500)
            {   
                if (qTarget != null)
                {
                    Q.Cast(qTarget);
                }
            }

            var combo = Config.Item("Combo").GetValue<KeyBind>().Active;

            //Select cards.
            if (Environment.TickCount - timeChangeW > 120)
            {
                timeChangeW = Environment.TickCount;
                if (Config.Item("ChangeCard").GetValue<KeyBind>().Active)
                {
                    var oldCard = Config.Item("SelectCard").GetValue<Slider>().Value;
                    if (oldCard != 2)
                    {
                        Config.Item("SelectCard").SetValue(new Slider(oldCard + 1, 0, 2));
                    }
                    else
                    {
                        Config.Item("SelectCard").SetValue(new Slider(0, 0, 2));
                    }
                }
            }

            if (Environment.TickCount - timeToggleGold > 120)
            {
                timeToggleGold = Environment.TickCount;
                if (Config.Item("ToggleGold").GetValue<KeyBind>().Active)
                {
                    var CardToggle = Config.Item("AlwaysGold").GetValue<bool>();
                    Config.Item("AlwaysGold").SetValue(!CardToggle);
                    Config.Item("ToggleGold").SetValue(new KeyBind(Config.Item("ToggleGold").GetValue<KeyBind>().Key,KeyBindType.Toggle));
                }
            }

            if (combo)
            {
                if (!Config.Item("AlwaysGold").GetValue<bool>())
                {
                    switch (Config.Item("SelectCard").GetValue<Slider>().Value)
                    {
                        case 0:
                            CardSelector.StartSelecting(Cards.Blue);
                            break;
                        case 1:
                            CardSelector.StartSelecting(Cards.Red);
                            break;
                        case 2:
                            CardSelector.StartSelecting(Cards.Yellow);
                            break;
                    }
                }
                else
                {
                    CardSelector.StartSelecting(Cards.Yellow);
                }

                if (qTarget != null && (qTarget.MoveSpeed < 250 || qTarget.IsStunned || !qTarget.CanMove || qTarget.IsRooted ||
                    qTarget.IsCharmed || qTarget.Distance(Player) < 400))
                {
                    Q.Cast(qTarget);
                }
            }

            if (Config.Item("WFarm").GetValue<KeyBind>().Active)
            {
                if (Player.ManaPercent < Config.Item("AlwaysBlue").GetValue<Slider>().Value)
                {
                    CardSelector.StartSelecting(Cards.Blue);
                }
                else
                {
                    switch (Config.Item("SelectCard").GetValue<Slider>().Value)
                    {
                        case 0:
                            CardSelector.StartSelecting(Cards.Blue);
                            break;
                        case 1:
                            if (
                                MinionManager.GetMinions(SOW.GetTarget().Position, 125f, MinionTypes.All,
                                    MinionTeam.Enemy).Count > Config.Item("RedMinions").GetValue<Slider>().Value)
                            {
                                CardSelector.StartSelecting(Cards.Red);
                            }
                            break;
                        case 2:
                            if (
                                MinionManager.GetMinions(SOW.GetTarget().Position, 125f, MinionTypes.All,
                                    MinionTeam.Enemy).Count > Config.Item("RedMinions").GetValue<Slider>().Value)
                            {
                                CardSelector.StartSelecting(Cards.Red);
                            }
                            break;
                    }
                }
            }

            //if (Config.Item("SelectYellow").GetValue<KeyBind>().Active ||
            //    combo)
            //{
            //    CardSelector.StartSelecting(Cards.Yellow);
            //}

            //if (Config.Item("SelectBlue").GetValue<KeyBind>().Active)
            //{
            //    CardSelector.StartSelecting(Cards.Blue);
            //}

            //if (Config.Item("SelectRed").GetValue<KeyBind>().Active)
            //{
            //    CardSelector.StartSelecting(Cards.Red);
            //}
            /*
                        if (CardSelector.Status == SelectStatus.Selected && combo)
                        {
                            var target = SOW.GetTarget();
                            if (target.IsValidTarget() && target is Obj_AI_Hero && Items.HasItem("DeathfireGrasp") && ComboDamage((Obj_AI_Hero)target) >= target.Health)
                            {
                                Items.UseItem("DeathfireGrasp", (Obj_AI_Hero) target);
                            }
                        }
            */

            //Auto Q
            var autoQI = Config.Item("AutoQI").GetValue<bool>();
            var autoQD = Config.Item("AutoQD").GetValue<bool>();


            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready && (autoQD || autoQI))
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget(Q.Range * 2))
                    {
                        var pred = Q.GetPrediction(enemy);
                        if ((pred.Hitchance == HitChance.Immobile && autoQI) ||
                            (pred.Hitchance == HitChance.Dashing && autoQD))
                        {
                            CastQ(enemy, pred.UnitPosition.To2D());
                        }
                    }
                }


            var useItemModes = Config.Item("itemMode").GetValue<StringList>().SelectedIndex;
            if (
                !((SOW.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                   (useItemModes == 2 || useItemModes == 3))
                  ||
                  (SOW.ActiveMode == Orbwalking.OrbwalkingMode.Mixed &&
                   (useItemModes == 1 || useItemModes == 3))))
                return;

            var botrk = Config.Item("itemBotrk").GetValue<bool>();
            var youmuu = Config.Item("itemYoumuu").GetValue<bool>();
            var target = SOW.GetTarget() as Obj_AI_Base;

            if (botrk)
            {
                if (target != null && target.Type == ObjectManager.Player.Type &&
                    target.ServerPosition.Distance(ObjectManager.Player.ServerPosition) < 450)
                {
                    var hasCutGlass = Items.HasItem(3144);
                    var hasBotrk = Items.HasItem(3153);

                    if (hasBotrk || hasCutGlass)
                    {
                        var itemId = hasCutGlass ? 3144 : 3153;
                        var damage = ObjectManager.Player.GetItemDamage(target, Damage.DamageItems.Botrk);
                        if (hasCutGlass ||
                            ObjectManager.Player.Health + damage < ObjectManager.Player.MaxHealth &&
                            Items.CanUseItem(itemId))
                            Items.UseItem(itemId, target);
                    }
                }
            }

            if (youmuu && target != null && target.Type == ObjectManager.Player.Type &&
                Orbwalking.InAutoAttackRange(target) && Items.CanUseItem(3142))
                Items.UseItem(3142);
        }
    }
}