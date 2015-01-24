using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AIM.Autoplay.Modes;
using BehaviorSharp;
using BehaviorSharp.Components.Conditionals;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay
{
    internal class Load
    {
        public Load()
        {
            Game.OnWndProc += OnWndProc;
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            Game.OnGameUpdate += OnGameUpdate;
        }

        private static int _loadTickCount;
        public static bool ModeLoaded = false;

        public static void OnWndProc(EventArgs args)
        {
            //Draw AIMLoading.jpg
        }

        public void OnGameLoad(EventArgs args)
        {
            _loadTickCount = Environment.TickCount;
            
            Base.Menu = new Menu("AIM", "AIM", true);
            Base.Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(new KeyBind(32, KeyBindType.Toggle)));
            Base.Menu.AddItem(new MenuItem("LowHealth", "Self Low Health %").SetValue(new Slider(20, 10, 50)));
            Base.Menu.AddToMainMenu();

            Util.Helpers.FileHandler.DoChecks();

            Game.PrintChat("AIM {0} Loaded!", Program.Version);
            Game.PrintChat("Don't panic, the bot will stard at 60 seconds in the game.");

        }

        public static void OnGameUpdate(EventArgs args)
        {
            if (Utility.Map.GetMap().Type == Utility.Map.MapType.HowlingAbyss)
            {
                try
                {
                    if (!ModeLoaded &&
                        (Environment.TickCount - _loadTickCount > 60000 || ObjectManager.Player.Level > 3))
                    {
                        var carryMode = new Modes.Carry();
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            else
            {
                Console.WriteLine("Map not yet supported, use AutoSharpporting ;)");
            }
        }
    }
}
