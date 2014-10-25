namespace Test
open LeagueSharp;
open LeagueSharp.Common;
open SharpDX;
open System.Windows.Input;
open System.Collections.Generic;
open System.Linq;
open Microsoft.FSharp.Reflection;

module Test =
    let enemies = 
        let isEnemy (hero : Obj_AI_Hero) = hero.IsEnemy && hero.IsValid && not hero.IsDead in //&& Vector3.Distance(ObjectManager.Player.Position, hero.Position) < 1500.f in
        Seq.filter isEnemy <| ObjectManager.Get<Obj_AI_Hero> ()

    let isQWER (sp : SpellDataInst) =
        match sp.Slot with
        | SpellSlot.Q | SpellSlot.W | SpellSlot.E | SpellSlot.R -> true
        | _                                                     -> false

    let rec menu = new Menu ("WalkBlock", "block", true)
    and bgMenu = menu.AddItem(new MenuItem("stop", "Disable")).SetValue(new KeyBind(32u,KeyBindType.Press))
    let getMenu st = menu.Item(st).GetValue<Slider>().Value

    (*
    let dangerousSpells (enemies : IEnumerable<Obj_AI_Hero>) dest = seq {
        for enemy in enemies do
            //Game.PrintChat ("dS for " + enemy.ChampionName)
            let dist = Option.map (enemy.Position.Distance >> int) dest
            for spell in enemy.Spellbook.Spells do
                Game.PrintChat("in")
                if isQWER spell && spell.CooldownExpires - Game.Time < 1.f && not enemy.IsDead && enemy.IsVisible then
                    Game.PrintChat("in2")
                    Game.PrintChat((menu.Item(enemy.ChampionName + spell.Slot.ToString()).GetValue<Slider>().Value).ToString())
                    match dist with
                         | Some dist -> 
                            let range = (menu.Item (enemy.ChampionName + spell.Slot.ToString ())).GetValue<Slider>().Value
                            if (range > dist && range < 1500)
                            then yield spell
                         | None      -> yield spell
    }
    *)

    let theMostDangerousSpell (enemy : Obj_AI_Hero) dest = 
        if enemy.IsDead || not enemy.IsVisible then None else
        let dist = Option.map enemy.Position.Distance dest
        let danger spell =
            if isQWER spell && spell.CooldownExpires - Game.Time < 1.f
            then match dist with
                 | Some dist -> let range = float32 <| menu.Item(enemy.ChampionName + spell.Slot.ToString()).GetValue<Slider>().Value in
                                if range > dist then range else 0.f
                 | None -> float32 <| menu.Item(enemy.ChampionName + spell.Slot.ToString ()).GetValue<Slider>().Value
            else 0.f
        Some (Seq.max <| Seq.map danger enemy.Spellbook.Spells)

    let rec onGameLoad args =
        let addEnemy (enemy : Obj_AI_Hero) =
            let sm = menu.AddSubMenu (new Menu(enemy.ChampionName, enemy.ChampionName))
            ignore <| sm.AddItem(let txt = enemy.ChampionName + "AA" in new MenuItem (txt, txt))
                        .SetValue(new Slider ((int enemy.AttackRange), 0, 1500))
            let addSpell (spell : SpellDataInst) =
                ignore <| (sm.AddItem (let txt = enemy.ChampionName + spell.Slot.ToString () in new MenuItem (txt, txt)))
                             .SetValue (new Slider ((int spell.SData.CastRange.[0]), 0, 1500))
            Seq.filter isQWER enemy.Spellbook.Spells |> Seq.iter addSpell
        Seq.iter addEnemy enemies
        menu.AddToMainMenu ()

        GameSendPacket onGameSendPacket |> Game.add_OnGameSendPacket
        Draw onGameUpdate |> Drawing.add_OnDraw
        
    and onGameSendPacket = function
    | args when ((args.PacketData.[0] = byte 0x72 && not (bgMenu.GetValue<KeyBind>().Active))) -> //(not (menu.Item("stop").GetValue<bool>()))) -> //(not (LeagueSharp.Common.Orbwalking.OrbwalkingMode() = LeagueSharp.Common.Orbwalking.OrbwalkingMode.Combo))) -> //(LeagueSharp.Common.Orbwalking.OrbwalkingMode() = LeagueSharp.Common.Orbwalking.OrbwalkingMode.Mixed || LeagueSharp.Common.Orbwalking.OrbwalkingMode() =LeagueSharp.Common.Orbwalking.OrbwalkingMode.LaneClear || LeagueSharp.Common.Orbwalking.OrbwalkingMode() =LeagueSharp.Common.Orbwalking.OrbwalkingMode.LastHit)) -> 
        let parsed = Packet.C2S.Move.Decoded args.PacketData 
        let dest = new Vector3 (parsed.X, parsed.Y, ObjectManager.Player.Position.Z)
        let notDangerous = function | None -> true | Some x -> x = 0.f
        let canAA = Seq.exists id <| seq { for e in enemies -> e.Position.Distance dest < (float32 <| getMenu (e.ChampionName + "AA")) }
        //for eni in enemies do
            //if 
        let inRange rng (enemy : Obj_AI_Hero) =
            ObjectManager.Player.Position.Distance enemy.Position < rng
        let drawCircle (enemy : Obj_AI_Hero) =
            let sizeOfDick = max enemy.AttackRange <| defaultArg (theMostDangerousSpell enemy None) 0.f
            //Game.PrintChat((menu.Item(enemy.ChampionName + spell.Slot.ToString()).GetValue<int> ()).ToString())
            //Drawing.DrawCircle (enemy.Position, float32 sizeOfDick, System.Drawing.Color.Red)
            if ObjectManager.Player.Distance(enemy.Position) < sizeOfDick || not (Packet.C2S.Move.Decoded(args.PacketData).MoveType = byte 2) then
                args.Process <- true
            else
                args.Process <- ((Seq.map (fun en -> theMostDangerousSpell en (Some dest)) enemies |> Seq.forall notDangerous))  
        //Game.PrintChat("OGU: {0}", Seq.length enemies)
        let eir = Seq.filter (inRange 2500.f) enemies
        for enemy in eir do
            //Game.PrintChat ("inRange " + enemy.ChampionName)
            drawCircle enemy
        //args.Process <- ((Seq.map (fun en -> theMostDangerousSpell en (Some dest)) enemies |> Seq.forall notDangerous)) 
    | _ -> ()
    
    and onGameUpdate args =
        let inRange rng (enemy : Obj_AI_Hero) =
            ObjectManager.Player.Position.Distance enemy.Position < rng
        let drawCircle (enemy : Obj_AI_Hero) =
            let sizeOfDick = max enemy.AttackRange <| defaultArg (theMostDangerousSpell enemy None) 0.f
            //Game.PrintChat((menu.Item(enemy.ChampionName + spell.Slot.ToString()).GetValue<int> ()).ToString())
            Drawing.DrawCircle (enemy.Position, float32 sizeOfDick, System.Drawing.Color.Red)
        //Game.PrintChat("OGU: {0}", Seq.length enemies)
        let eir = Seq.filter (inRange 2500.f) enemies
        for enemy in eir do
            //Game.PrintChat ("inRange " + enemy.ChampionName)
            drawCircle enemy