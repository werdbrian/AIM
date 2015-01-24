using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Util;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Helpers;
using AIM.Autoplay.Util.Objects;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using AutoLevel = LeagueSharp.Common.AutoLevel;

namespace AIM.Autoplay.Modes
{
    public abstract class Base
    {
        public Base()
        {
            ObjConstants = new Constants();
            ObjHeroes = new Heroes();
            ObjMinions = new Minions();
            ObjTurrets = new Turrets();
            OrbW = new Util.Orbwalker();
        }

        public Obj_AI_Minion LeadingMinion;
        public virtual void OnGameLoad(EventArgs args) { }
        public virtual void OnGameUpdate(EventArgs args) { }
        public static Constants ObjConstants { get; protected set; }
        public static Heroes ObjHeroes { get; protected set; }
        public static Minions ObjMinions { get; protected set; }
        public static  Turrets ObjTurrets { get; protected set; }
        public static Autoplay.Util.Orbwalker OrbW { get; set; }
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static bool IsInDanger = false; 

        #region Menu
        public static Menu Menu;
        public void LoadMenu()
        {
            Menu = new Menu("AIM", "AIM", true);
            Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(new KeyBind(32, KeyBindType.Toggle)));
            Menu.AddItem(new MenuItem("LowHealth", "Self Low Health %").SetValue(new Slider(20, 10, 50)));
            Menu.AddToMainMenu();
        }
        #endregion Menu

        #region Minions

        public void RefreshMinions()
        {
            ObjMinions.UpdateMinions();
            if (Utility.Map.GetMap().Type == Utility.Map.MapType.SummonersRift)
            {
                LeadingMinion = ObjMinions.GetLeadMinion(SummonersRift.BottomLane.Bottom_Zone.CenterOfPolygone().To3D());
                Game.PrintChat("Leading minion assigned");
            }
            else
            {
                LeadingMinion = ObjMinions.GetLeadMinion();
                Game.PrintChat("Leading minion assigned");
            }
        }

        public bool InDangerUnderEnemyTurret()
        {
            var nearestTurret = Turrets.EnemyTurrets.FirstOrDefault(t => t.Distance(Player) < 800);
            if (nearestTurret != null)
                return
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Count(minion => minion.IsAlly && !minion.IsDead && minion.Distance(nearestTurret) < 650) <= 2;
            else
                return false;
        }

        #endregion Minions

    }
}
