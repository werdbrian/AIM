using System;
using System.Linq;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace AIM.Autoplay.Util
{
    public class Orbwalker
    {
        public Orbwalker()
        {
            Game.PrintChat("AIM Orbwalker Init Successful");
            Game.PrintChat("Enjoy.");
        }

        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        public void ExecuteMixedMode(Vector3 pos)
        {
            if (Player.UnderTurret(true))
            {
                var nearestTurret =
                    Turrets.EnemyTurrets.Where(t => t.IsValid).OrderBy(r => r.Distance(Player, true)).FirstOrDefault();

                if (nearestTurret.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player)))
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, nearestTurret);
                }
            }
            else
            {
                var spellbook = Player.Spellbook;

                if (spellbook.IsChanneling || spellbook.IsCharging || spellbook.IsCastingSpell)
                {
                    return;
                }

                WalkAround(pos);

                if (!CanLastHit())
                {
                    var target = TargetSelector.GetTarget(Player.AttackRange, TargetSelector.DamageType.Physical);
                    if (target != null && target.IsValid && !target.IsDead && State.IsBotSafe() &&
                        !target.UnderTurret(true) && !Variables.OverrideAttackUnitAction)
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                }
                else
                {
                    LastHit();
                }
            }
        }

        private static void WalkAround(Vector3 pos)
        {
            Randoms.RandRange = Randoms.Rand.Next(-267, 276);
            Randoms.RandSeconds = Randoms.Rand.Next(1000, 4000);

            if (Environment.TickCount - Variables.StepTime < Randoms.RandSeconds || Variables.OverrideAttackUnitAction)
            {
                return;
            }

            if (Player.Team == GameObjectTeam.Order)
            {
                var orbwalkingAdditionInteger = Randoms.RandRange * (-1);
                Variables.OrbwalkingPos.X = pos.X + orbwalkingAdditionInteger;
                Variables.OrbwalkingPos.Y = pos.Y + orbwalkingAdditionInteger;
            }
            else
            {
                var orbwalkingAdditionInteger = Randoms.RandRange;
                Variables.OrbwalkingPos.X = pos.X + orbwalkingAdditionInteger;
                Variables.OrbwalkingPos.Y = pos.Y + orbwalkingAdditionInteger;
            }
            if (Variables.OrbwalkingPos == Vector2.Zero)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Variables.OrbwalkingPos.To3D());
            Variables.StepTime = Environment.TickCount;
        }

        private void WalkAround(Obj_AI_Hero follow)
        {
            Randoms.RandRange = Randoms.Rand.Next(-367, 376);
            Randoms.RandSeconds = Randoms.Rand.Next(500, 3500);
            if (Environment.TickCount - Variables.StepTime >= Randoms.RandSeconds && !Variables.OverrideAttackUnitAction)
            {
                if (Player.Team == GameObjectTeam.Order)
                {
                    var orbwalkingAdditionInteger = Randoms.RandRange * (-1);
                    Variables.OrbwalkingPos.X = follow.Position.X + orbwalkingAdditionInteger;
                    Variables.OrbwalkingPos.Y = follow.Position.Y + orbwalkingAdditionInteger;
                }
                else
                {
                    var orbwalkingAdditionInteger = Randoms.RandRange;
                    Variables.OrbwalkingPos.X = follow.Position.X + orbwalkingAdditionInteger;
                    Variables.OrbwalkingPos.Y = follow.Position.Y + orbwalkingAdditionInteger;
                }
                if (Variables.OrbwalkingPos != Vector2.Zero && Player.Distance(follow) < 550)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Variables.OrbwalkingPos.To3D());
                    Variables.StepTime = Environment.TickCount;
                }
            }
        }

        public bool CanLastHit()
        {
            return
                ObjectManager.Get<Obj_AI_Minion>()
                    .Any(
                        minion =>
                            minion.IsValidTarget() && Player.Distance(minion) < Player.AttackRange &&
                            minion.Health < 2 * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod));
        }

        public void LastHit()
        {
            var target =
                ObjectManager.Get<Obj_AI_Minion>()
                    .FirstOrDefault(
                        minion =>
                            minion.IsValidTarget() && Player.Distance(minion) < Player.AttackRange &&
                            minion.Health < 2 * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod));

            if (target != null && target.IsValidTarget(Player.AttackRange))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }
    }
}