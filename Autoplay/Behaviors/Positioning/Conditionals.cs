using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Util.Objects;
using BehaviorSharp.Components.Conditionals;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay.Behaviors.Positioning
{
    internal class Conditionals
    {
        private static Heroes Heroes = new Heroes();
        internal static Conditional ShouldPushLane = new Conditional(() =>
        {
            return (Heroes.EnemiesInRange(1400) < 2 && Heroes.AlliesInRange(1400) > 2) ||
                   Heroes.AllyHeroes.All(h => h.InFountain());
        });

        internal static Conditional ShouldTryToKill = new Conditional(
            () =>
            {
                var spells = new List<SpellSlot> { SpellSlot.Q, SpellSlot.W, SpellSlot.E };
                return (Heroes.EnemyHeroes.Any(h => h.Health < Heroes.Me.GetComboDamage(h, spells) + Heroes.Me.GetAutoAttackDamage(Heroes.Me) * 2));
            });

        internal static Conditional ShouldCollectHealthRelic = new Conditional(
            () => Relics.ClosestRelic() != null &&
                  Heroes.Me.Health < Modes.Base.Menu.Item("LowHealth").GetValue<Slider>().Value);
        internal static Conditional LowHealth = new Conditional(() => Heroes.Me.HealthPercentage() < Modes.Base.Menu.Item("LowHealth").GetValue<Slider>().Value);
    }
}
