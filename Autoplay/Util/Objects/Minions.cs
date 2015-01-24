using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace AIM.Autoplay.Util.Objects
{
    public class Minions
    {
        public Minions()
        {
            UpdateMinions();
        }
        public List<Obj_AI_Minion> AllMinions;
        public List<Obj_AI_Minion> AllyMinions;
        public List<Obj_AI_Minion> EnemyMinions;

        public void UpdateMinions()
        {
            AllMinions = ObjectManager.Get<Obj_AI_Minion>().ToList();
            AllyMinions = AllMinions.FindAll(minion => minion.IsAlly);
            EnemyMinions = AllMinions.FindAll(minion => !minion.IsAlly);
        }

        public Obj_AI_Minion GetLeadMinion()
        {
            var enemyTurretsSortedByDistance = Turrets.EnemyTurrets.OrderByDescending(t => t.Distance(ObjectManager.Player));
            var closestEnemyTurret = enemyTurretsSortedByDistance.First();
            var allyMinionsSortedByDistToClosestEnemyTurret = AllyMinions.OrderBy(x => x.Distance(closestEnemyTurret.Position));
            return allyMinionsSortedByDistToClosestEnemyTurret.First();
        }

        public Obj_AI_Minion GetLeadMinion(Vector3 lane)
        {
            var enemyTurretsSortedByDistance = Turrets.EnemyTurrets.OrderByDescending(t => t.Distance(lane));
            var closestEnemyTurret = enemyTurretsSortedByDistance.First();
            var allyMinionsSortedByDistToClosestEnemyTurret = AllyMinions.OrderBy(x => x.Distance(closestEnemyTurret.Position));
            return allyMinionsSortedByDistToClosestEnemyTurret.First();
        }
    }
}
