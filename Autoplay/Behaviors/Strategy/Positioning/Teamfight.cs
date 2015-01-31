using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace AIM.Autoplay.Behaviors.Strategy.Positioning
{

    /// <summary>
    /// Stuff used to determine the teamfight behavior ^^
    /// </summary>
    internal class Teamfight
    {
        private static Util Utils = new Util();
        

        /// <summary>
        /// Returns a random position in the team zone or the position of the ally champion farthest from base
        /// </summary>
        internal static Vector2 GetPos()
        {
            if (Utils.AllyZone() != null)
            {
                var randomPoint = Utils.AllyZone().OrderBy(p => Randoms.Rand.Next()).FirstOrDefault().FirstOrDefault();
                var v2 = new Vector2();
                v2.X = randomPoint.X;
                v2.Y = randomPoint.Y;
                return v2;
            }
            Console.WriteLine("Utils.AllyZone is null");
            return Utils.GetAllyPosList().OrderByDescending(h => h.Distance(HQ.AllyHQ.Position)).FirstOrDefault();
        }
    }
}
