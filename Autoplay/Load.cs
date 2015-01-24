using System;
using AIM.Autoplay.Modes;
using AIM.Autoplay.Util.Helpers;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay
{
    internal class Load
    {
        private static int _loadTickCount;
        public static bool ModeLoaded = false;

        public Load()
        {
            Game.OnWndProc += OnWndProc;
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            Game.OnGameUpdate += OnGameUpdate;
        }

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

            FileHandler.DoChecks();

            Game.PrintChat("AIM {0} Loaded!", Program.Version);
            Game.PrintChat("Don't panic, the bot will stard at 60 seconds in the game.");
        }

        public static void OnGameUpdate(EventArgs args)
        {
            if (Utility.Map.GetMap().Type == Utility.Map.MapType.HowlingAbyss)
            {
                UsePorosnax();

                try
                {
                    if (!ModeLoaded &&
                        (Environment.TickCount - _loadTickCount > 60000 || ObjectManager.Player.Level > 3))
                    {
                        var carryMode = new Carry();
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

        public static bool UsePorosnax()
        {
            return SpellSlot.Trinket.IsReady() && ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Trinket);
        }
    }
}