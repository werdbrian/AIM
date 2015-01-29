using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace AIM.Autoplay.Util.Objects
{
    public class HQ
    {
        public static Obj_HQ AllyHQ = ObjectManager.Get<Obj_HQ>().FirstOrDefault(hq => hq.IsAlly);
        public static Obj_HQ EnemyHQ = ObjectManager.Get<Obj_HQ>().FirstOrDefault(hq => hq.IsEnemy);
    }
}
