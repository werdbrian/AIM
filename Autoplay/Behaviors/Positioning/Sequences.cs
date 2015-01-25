using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorSharp.Components.Composites;
using BehaviorSharp.Components.Conditionals;

namespace AIM.Autoplay.Behaviors.Positioning
{
    internal class Sequences
    {
        internal static Sequence LanePush = new Sequence(new Actions().PushLane, new Conditionals().ShouldPushLane, new Inverters().LowHealth);
        internal static Sequence StayWithinExpRange = new Sequence(new Actions().StayWithinExpRange);
        internal static Sequence CollectHealthPack = new Sequence(new Actions().CollectHealthRelic, new Conditionals().ShouldCollectHealthRelic);
        internal static Sequence TryToKill = new Sequence(new Actions().KillEnemy);
    }
}
