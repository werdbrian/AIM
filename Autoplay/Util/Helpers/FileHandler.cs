using System;
using System.IO;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace AIM.Autoplay.Util.Helpers
{
    internal class FileHandler
    {
        private static string CustomBuildsPath = Config.LeagueSharpDirectory + @"\AIM\";
        public static string TheFile;
        public static ItemId[] CustomShopList;

        public static void DoChecks()
        {
            TheFile = CustomBuildsPath + Utility.Map.GetMap().Type + @"\" + Objects.Heroes.Me.BaseSkinName + ".txt";
            if (!Directory.Exists(CustomBuildsPath))
            {
                Directory.CreateDirectory(CustomBuildsPath);
                Directory.CreateDirectory(CustomBuildsPath + Utility.Map.MapType.CrystalScar);
                Directory.CreateDirectory(CustomBuildsPath + Utility.Map.MapType.HowlingAbyss);
                Directory.CreateDirectory(CustomBuildsPath + Utility.Map.MapType.SummonersRift);
                Directory.CreateDirectory(CustomBuildsPath + Utility.Map.MapType.TwistedTreeline);
                Directory.CreateDirectory(CustomBuildsPath + Utility.Map.MapType.Unknown);
            }
            if (File.Exists(TheFile))
            {
                AIM.Util.Helpers.PrintMessage("Loaded: " + TheFile);
                string[] itemsStringArray = File.ReadAllLines(TheFile);
                int[] itemsIntArray = new int[itemsStringArray.Count()];
                CustomShopList = new ItemId[itemsStringArray.Count()];
                for(var i = 0; i < itemsStringArray.Count(); i++)
                {
                    itemsIntArray[i] = Convert.ToInt32(itemsStringArray[i]);
                }
                for (var i = 0; i < itemsIntArray.Count(); i++)
                {
                    CustomShopList[i] = (ItemId)itemsIntArray[i];
                }
                if (CustomShopList[0] == (ItemId) 3157 && CustomShopList[1] == (ItemId) 3089 &&
                    CustomShopList[2] == (ItemId) 3165)
                {
                    CustomShopList = CustomShopList.OrderBy(item => Data.Randoms.Rand.Next()).ToArray();
                }
            }
            if (!File.Exists(TheFile) && Utility.Map.GetMap().Type == Utility.Map.MapType.SummonersRift)
            {
                var newfile = File.Create(TheFile);
                newfile.Close();
                var content = "3157\n3089\n3165\n3174\n3116\n3222\n3092\n3151\n3100\n3190\n3027\n3135\n3146\n3020";
                var separator = new[] { "\n" };
                string[] lines = content.Split(separator, StringSplitOptions.None);
                File.WriteAllLines(TheFile, lines);
                AIM.Util.Helpers.PrintMessage("Created custom shoplist at: " + TheFile);
            }
        }
    }
}
