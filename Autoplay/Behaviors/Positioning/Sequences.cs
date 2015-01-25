using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorSharp.Components.Composites;

namespace AIM.Autoplay.Behaviors.Positioning
{
    internal class Sequences
    {
        internal static Sequence LanePush = new Sequence(Actions.PushLane, Conditionals.ShouldPushLane, Inverters.LowHealth);
        internal static Sequence StayWithinExpRange = new Sequence(Actions.StayWithinExpRange, Conditionals.ShouldPlaySafe);
        internal static Sequence CollectHealthPack = new Sequence(Actions.CollectHealthRelic, Conditionals.ShouldCollectHealthRelic);
        internal static Sequence TryToKill = new Sequence(Actions.KillEnemy);
    }
}
