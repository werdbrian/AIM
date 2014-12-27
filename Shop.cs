#region

using System;
using System.Collections.Generic;
using BehaviorSharp;
using BehaviorSharp.Components.Actions;
using BehaviorSharp.Components.Composites;
using BehaviorSharp.Components.Conditionals;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace AIM
{
    internal static class Shop
    {
        public enum Stage
        { };

        //private static Dictionary<Stage, List<ShopItem>> _itemList = new Dictionary<Stage, List<ShopItem>>();
        private static List<ItemId> _itemList = new List<ItemId>();
        //private static List<ShopItem> _currentItemList = new List<ShopItem>();

        public static Sequence GetSequence()
        {
            try
            {
                _itemList = Program.ChampData.GetItemList();
                //    var shop = new LeagueLib.Shop();
                LeagueLib.Shop.AddList(_itemList);

                var CanShop = new Conditional(
                    () =>
                    {
                        if (!Utility.InShopRange() && !ObjectManager.Player.IsDead)
                        {
                            Console.WriteLine("Cant Shop");
                            return false;
                        }
                        Console.WriteLine("CAN SHOP");
                        Program.FinishedShopping = false;
                        return true;
                    });

                var Shop = new BehaviorAction(
                    () =>
                    {
                        Console.WriteLine("TICK");
                      // if (LeagueLib.Shop.Tick())
                      //  {
                            Console.WriteLine("FINISHED");
                            Program.FinishedShopping = true;
                            Program.LastShop = Environment.TickCount;
                      //  }
                        return BehaviorState.Success;
                    });

                var Finished = new BehaviorAction(
                    () =>
                    {
                        if ((int) ObjectManager.Player.HealthPercentage() == 100)
                        {
                            Program.FinishedShopping = true;
                        }

                        return BehaviorState.Success;
                    });

                var shopSequence = new Sequence(CanShop, Shop);

                return shopSequence;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new Sequence();
        }
    }
}