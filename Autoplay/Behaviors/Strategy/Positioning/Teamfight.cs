using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using LeagueSharp.Common;
using SharpDX;

namespace AIM.Autoplay.Behaviors.Strategy.Positioning
{
    internal class Teamfight
    {
        private static Util Utils = new Util();

        internal static Vector2 GetPos()
        {
            return Utils.GetAllyPosList().OrderByDescending(h => h.Distance(HQ.AllyHQ.Position)).FirstOrDefault();
        }
    }
}
