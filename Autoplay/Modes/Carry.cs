using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AIM.Autoplay.Util.Data;
using AIM.Autoplay.Util.Objects;
using BehaviorSharp;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using AIM.Autoplay.Util.Helpers;
using AIM.Autoplay.Util.Data;
using BehaviorSharp.Components.Actions;
using AutoLevel = LeagueSharp.Common.AutoLevel;

namespace AIM.Autoplay.Modes
{
    class Carry : Base
    {
        public Carry():base()
        {
            Game.OnGameUpdate += OnGameUpdate;
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Load.ModeLoaded = true;
        }

        public override void OnGameLoad(EventArgs args)
        {
            try
            {
                //
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void OnGameUpdate(EventArgs args)
        {
            ObjHeroes.SortHeroesListByDistance();
            ObjTurrets.UpdateTurrets();

            ImpingAintEasy();
            RefreshMinions();

            if (ObjectManager.Player.UnderTurret(true))
            {
                if (InDangerUnderEnemyTurret())
                {
                    IsInDanger = true;
                }
            }
            if (!ObjectManager.Player.UnderTurret(true))
            {
                IsInDanger = false;
            }

        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && sender.UnderTurret(true) && args.Target.IsEnemy && args.Target.Type == GameObjectType.obj_AI_Hero)
            {
                IsInDanger = true;
            }

            if (sender is Obj_AI_Turret && args.Target.IsMe)
            {
                IsInDanger = true;
            }

            if (sender is Obj_AI_Minion && args.Target.IsMe)
            {
                Vector2 orbwalkingPos = new Vector2();
                orbwalkingPos.X = ObjectManager.Player.Position.X + ObjConstants.DefensiveAdditioner;
                orbwalkingPos.Y = ObjectManager.Player.Position.Y + ObjConstants.DefensiveAdditioner;
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D());
            }
        }

        public void ImpingAintEasy()
        {
            new AutoLevel(Util.Data.AutoLevel.GetSequence());
            MetaHandler.DoChecks(); //#TODO rewrite MetaHandler with BehaviorSharp
            #region OrbwalkAtLeadingMinionLocation
            new BehaviorAction(
                () =>
                {
                    try
                    {
                        if (IsInDanger)
                        {
                            Vector2 orbwalkingPos = new Vector2();
                            orbwalkingPos.X = ObjectManager.Player.Position.X + ObjConstants.DefensiveAdditioner;
                            orbwalkingPos.Y = ObjectManager.Player.Position.Y + ObjConstants.DefensiveAdditioner;
                            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, orbwalkingPos.To3D());
                            return BehaviorState.Success;
                        }
                        if (LeadingMinion != null && !IsInDanger)
                        {
                            Vector2 orbwalkingPos = new Vector2();
                            orbwalkingPos.X = LeadingMinion.Position.X + ObjConstants.DefensiveAdditioner;
                            orbwalkingPos.Y = LeadingMinion.Position.Y + ObjConstants.DefensiveAdditioner;
                            OrbW.ExecuteMixedMode(orbwalkingPos.To3D());
                            return BehaviorState.Success;
                        }
                        return BehaviorState.Failure;
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e);
                    }
                    return BehaviorState.Failure;

                }).Tick();
            #endregion OrbwalkAtLeadingMinionLocation
        }
    }
}
