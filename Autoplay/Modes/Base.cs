using System;
using System.Linq;
using AIM.Autoplay.Util;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using LeagueSharp;
using LeagueSharp.Common;
using AutoLevel = LeagueSharp.Common.AutoLevel;

namespace AIM.Autoplay.Modes
{
    public abstract class Base
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static bool IsInDanger = false;
        public static AutoLevel AutoLevel;
        public static Menu Menu;
        public Obj_AI_Minion LeadingMinion;

        protected Base()
        {
            ObjConstants = new Constants();
            ObjHeroes = new Heroes();
            ObjMinions = new Minions();
            ObjTurrets = new Turrets();
            OrbW = new Orbwalker();
        }

        public static Constants ObjConstants { get; protected set; }
        public static Heroes ObjHeroes { get; protected set; }
        public static Minions ObjMinions { get; protected set; }
        public static Turrets ObjTurrets { get; protected set; }
        public static Orbwalker OrbW { get; set; }
        public virtual void OnGameLoad(EventArgs args) {}
        public virtual void OnGameUpdate(EventArgs args) {}

        #region Minions

        public void RefreshMinions()
        {
            ObjMinions.UpdateMinions();
            if (Utility.Map.GetMap().Type == Utility.Map.MapType.SummonersRift)
            {
                LeadingMinion = ObjMinions.GetLeadMinion(SummonersRift.BottomLane.Bottom_Zone.CenterOfPolygone().To3D());
            }
            else
            {
                LeadingMinion = ObjMinions.GetLeadMinion();
            }
        }

        public bool InDangerUnderEnemyTurret()
        {
            var nearestTurret = Turrets.EnemyTurrets.FirstOrDefault(t => t.Distance(Player) < 800);
            if (nearestTurret != null)
            {
                return
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Count(minion => minion.IsAlly && !minion.IsDead && minion.Distance(nearestTurret) < 650) <= 2;
            }
            return false;
        }

        #endregion Minions
    }
}