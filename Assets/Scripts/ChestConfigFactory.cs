using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ChestConfigFactory
    {
        public int goldPossibility = 90;
        public int jewelPossibility = 91;
        public int cardPossibility = 98;
        public int gemPossibility = 100;

        public int minItemsBase = 1;
        public int maxItemsBase = 3;
        
        public int minItemsRare = 3;
        public int maxItemsRare = 5;
        
        public int minItemsEpic = 5;
        public int maxItemsEpic = 8;

        public ChestConfig GetChestConfigs(ChestGrades chestGrade)
        {
            var config = new ChestConfig()
            {
                GoldPossibility = goldPossibility,
                JewelPossibility = jewelPossibility,
                CardPossibility = cardPossibility,
                GemPossibility = gemPossibility
            };
            switch (chestGrade)
            {
                case ChestGrades.Base:
                    config.MinItemsCount = minItemsBase;
                    config.MaxItemsCount = maxItemsBase;
                    break;
                case ChestGrades.Rare:
                    config.MinItemsCount = minItemsRare;
                    config.MaxItemsCount = maxItemsRare;
                    break;
                case ChestGrades.Epic:
                    config.MinItemsCount = minItemsEpic;
                    config.MaxItemsCount = maxItemsEpic;
                    break;
            }

            return config;
        }
    }

    public class ChestConfig
    {
        public int MinItemsCount { get; set; }
        public int MaxItemsCount { get; set; }

        public int GoldPossibility { get; set; }
        public int JewelPossibility { get; set; }
        public int CardPossibility { get; set; }
        public int GemPossibility { get; set; }
    }
}
