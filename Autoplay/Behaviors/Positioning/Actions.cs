using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Util;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using BehaviorSharp;
using BehaviorSharp.Components.Actions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace AIM.Autoplay.Behaviors.Positioning
{
    internal class Actions
    {
        internal BehaviorAction PushLane = new BehaviorAction(
            () =>
            {
                try
                {
                    var objConstants = new Constants();
                    var isInDanger = ObjectManager.Player.UnderTurret(true) && Modes.Base.InDangerUnderEnemyTurret();
                    if (isInDanger)
                    {
                        var orbwalkingPos = new Vector2();
                        orbwalkingPos.X = ObjectManager.Player.Position.X + objConstants.DefensiveAdditioner;
                        orbwalkingPos.Y = ObjectManager.Player.Position.Y + objConstants.DefensiveAdditioner;
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D());
                        return BehaviorState.Success;
                    }
                    if (Modes.Base.LeadingMinion != null)
                    {
                        var orbwalkingPos = new Vector2();
                        orbwalkingPos.X = Modes.Base.LeadingMinion.Position.X + objConstants.DefensiveAdditioner;
                        orbwalkingPos.Y = Modes.Base.LeadingMinion.Position.Y + objConstants.DefensiveAdditioner;
                        Modes.Base.OrbW.ExecuteMixedMode(orbwalkingPos.To3D());
                        return BehaviorState.Success;
                    }
                    return BehaviorState.Failure;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
                return BehaviorState.Failure;
            });

        internal BehaviorAction StayWithinExpRange = new BehaviorAction(
            () =>
            {
                var objConstants = new Constants();
                var isInDanger = ObjectManager.Player.UnderTurret(true) && Modes.Base.InDangerUnderEnemyTurret();
                if (isInDanger)
                {
                    var orbwalkingPos = new Vector2();
                    orbwalkingPos.X = ObjectManager.Player.Position.X + objConstants.DefensiveAdditioner;
                    orbwalkingPos.Y = ObjectManager.Player.Position.Y + objConstants.DefensiveAdditioner;
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D());
                    return BehaviorState.Success;
                }
                if (Modes.Base.ClosestEnemyMinion != null)
                {
                    //this is a temp workaround untill the planned smart positioning
                    var orbwalkingPos = new Vector2();
                    if (Heroes.Me.Distance(Modes.Base.ClosestEnemyMinion) > 1300)
                    {
                        orbwalkingPos.X = Modes.Base.ClosestEnemyMinion.Position.X + objConstants.AggressiveAdditioner;
                        orbwalkingPos.Y = Modes.Base.ClosestEnemyMinion.Position.Y + objConstants.AggressiveAdditioner;
                        Console.WriteLine("too far from exp range, go to X {0} Y {1}", orbwalkingPos.X, orbwalkingPos.Y);
                        Utility.DelayAction.Add(
                            Randoms.Rand.Next(1000, 2000),
                            () => Heroes.Me.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D()));
                        return BehaviorState.Running;
                    }
                    if (Heroes.Me.Distance(Modes.Base.ClosestEnemyMinion) < 750)
                    {
                        orbwalkingPos.X = Modes.Base.ClosestEnemyMinion.Position.X + objConstants.DefensiveAdditioner;
                        orbwalkingPos.Y = Modes.Base.ClosestEnemyMinion.Position.Y + objConstants.DefensiveAdditioner;
                        Console.WriteLine("In Dangerzone, move to: X {0} Y {1}", orbwalkingPos.X, orbwalkingPos.Y);
                        Utility.DelayAction.Add(
                            Randoms.Rand.Next(1000, 2000),
                            () => Heroes.Me.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D()));
                        return BehaviorState.Running;
                    }
                    if (Heroes.Me.Distance(Modes.Base.ClosestEnemyMinion) > 750 &&
                        Heroes.Me.Distance(Modes.Base.ClosestEnemyMinion) < 1300)
                    {
                        Utility.DelayAction.Add(6000, () => Modes.Base.OrbW.WalkAround(Heroes.Me));
                        Game.PrintChat("Safe");
                        return BehaviorState.Success;
                    }
                }
                return BehaviorState.Failure;
            });

        internal BehaviorAction KillEnemy = new BehaviorAction(
            () =>
            {
                var spells = new List<SpellSlot> { SpellSlot.Q, SpellSlot.W, SpellSlot.E };
                var heroes = new Heroes();
                var killableEnemy = heroes.EnemyHeroes.FirstOrDefault(h => h.Health < Heroes.Me.GetComboDamage(h, spells) + Heroes.Me.GetAutoAttackDamage(Heroes.Me));
                if (killableEnemy == null || killableEnemy.IsDead || !killableEnemy.IsValidTarget() || killableEnemy.IsInvulnerable ||
                                              killableEnemy.UnderTurret(true) || Heroes.Me.IsDead)
                {
                    Orbwalker.Stop = false;
                    return BehaviorState.Success;
                }
                Orbwalker.Stop = true;
                Heroes.Me.IssueOrder(GameObjectOrder.AutoAttack, killableEnemy);
                return BehaviorState.Running;
            });
        internal BehaviorAction CollectHealthRelic = new BehaviorAction(
            () =>
            {
                if (Heroes.Me.Position != Relics.ClosestRelic().Position)
                {
                    Orbwalker.Stop = true;
                    Heroes.Me.IssueOrder(GameObjectOrder.MoveTo, Relics.ClosestRelic().Position);
                    return BehaviorState.Running;
                }
                Orbwalker.Stop = false;
                return BehaviorState.Success;
            });
    }
}
