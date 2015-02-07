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
            if (Game.MapId == GameMapId.HowlingAbyss)
            {
                if (Util.AllyZone() != null)
                {
                    var allyZonePathList = Util.AllyZone().OrderBy(p => Randoms.Rand.Next()).FirstOrDefault();
                    var allyZoneVectorList = new List<Vector2>();
                    
                    //create vectors from points and remove walls
                    foreach (var point in allyZonePathList)
                    {
                        var v2 = new Vector2(point.X, point.Y);
                        if (!v2.IsWall())
                        {
                            allyZoneVectorList.Add(v2);
                        }
                    }
                    var pointClosestToEnemyHQ =
                        allyZoneVectorList.OrderBy(p => p.Distance(HQ.EnemyHQ.Position)).FirstOrDefault();

                    //remove people that just respawned from the equation
                    foreach (var v2 in allyZoneVectorList)
                    {
                        if (v2.Distance(pointClosestToEnemyHQ) > 2000)
                        {
                            allyZoneVectorList.Remove(v2);
                        }
                    }

                    //return a random orbwalk pos candidate from the list
                    return allyZoneVectorList.FirstOrDefault();
                }

                //if somehow stuff doesn't go our way(which it shouldn't, laneclear behavior would be more appropiate here :s)
                var hfPos =
                    Utils.GetAllyPosList().OrderByDescending(h => h.Distance(HQ.AllyHQ.Position)).FirstOrDefault();
                hfPos.X = hfPos.X + Randoms.Rand.Next(-200, 200);
                hfPos.Y = hfPos.Y + Randoms.Rand.Next(-200, 200);
                return hfPos;
            }
            
            //for SR :s
            var minion =
                ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.Distance(HQ.EnemyHQ)).FirstOrDefault().Position.To2D();
            var turret = ObjectManager.Get<Obj_AI_Turret>().OrderByDescending(m => m.Distance(HQ.AllyHQ)).FirstOrDefault().Position.To2D();
            return (minion != null && minion.IsValid()) ? minion : turret;
        }
    }
}
