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
        internal static BehaviorAction PushLane = new BehaviorAction(
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

        internal static BehaviorAction StayWithinExpRange = new BehaviorAction(
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
                    var orbwalkingPos = new Vector2();
                    var expRange = 1100 + Randoms.Rand.Next(-100, 100);
                    orbwalkingPos.X = Modes.Base.ClosestEnemyMinion.Position.X +
                                      (Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) < expRange
                                          ? expRange + Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) * objConstants.DefensiveMutiplier
                                          : expRange - Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) * objConstants.DefensiveMutiplier);
                    orbwalkingPos.Y = Modes.Base.ClosestEnemyMinion.Position.Y +
                                      (Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) < expRange
                                          ? expRange + Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) * objConstants.DefensiveMutiplier
                                          : expRange - Modes.Base.ClosestEnemyMinion.Distance(Heroes.Me) * objConstants.DefensiveMutiplier);
                    Modes.Base.OrbW.ExecuteMixedMode(orbwalkingPos.To3D());
                    return BehaviorState.Success;
                }
                return BehaviorState.Failure;
            });

        internal static BehaviorAction KillEnemy = new BehaviorAction(
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
        internal static BehaviorAction CollectHealthRelic = new BehaviorAction(
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
