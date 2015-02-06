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
using Color = System.Drawing.Color;

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
            if (Util.AllyZone() != null)
            {
                var randomPoint = Util.AllyZone().OrderBy(p => Randoms.Rand.Next()).FirstOrDefault().FirstOrDefault();
                if (randomPoint != null)
                {
                    var v2 = new Vector2();
                    v2.X = randomPoint.X;
                    v2.Y = randomPoint.Y;
                    if (!v2.IsWall())
                    {
                        return v2;
                    }
                }
            }
            //Console.WriteLine("Utils.AllyZone is null");
            var hfPos = Utils.GetAllyPosList().OrderByDescending(h => h.Distance(HQ.AllyHQ.Position)).FirstOrDefault();
            hfPos.X = hfPos.X + Randoms.Rand.Next(-200, 200);
            hfPos.Y = hfPos.Y + Randoms.Rand.Next(-200, 200);
            return hfPos;
        }
    }
}
