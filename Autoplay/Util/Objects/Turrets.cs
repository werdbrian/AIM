using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay.Util.Objects
{
    public class Turrets
    {

        public Turrets()
        {
            UpdateTurrets();
            SortTurretsByDistance();
        }

        public static List<Obj_AI_Turret> AllTurrets;
        public static List<Obj_AI_Turret> AllyTurrets;
        public static List<Obj_AI_Turret> EnemyTurrets;

        public void UpdateTurrets()
        {
            AllTurrets = ObjectManager.Get<Obj_AI_Turret>().ToList();
            AllyTurrets = AllTurrets.FindAll(turret => turret.IsAlly);
            EnemyTurrets = AllTurrets.FindAll(turret => !turret.IsAlly);
        }

        public void SortTurretsByDistance()
        {
            AllTurrets = AllTurrets.OrderBy(turret => turret.Distance(Heroes.Me)).ToList();
            AllyTurrets = AllyTurrets.OrderBy(turret => turret.Distance(Heroes.Me)).ToList();
            EnemyTurrets = EnemyTurrets.OrderBy(turret => turret.Distance(Heroes.Me)).ToList();
        }
    }
}