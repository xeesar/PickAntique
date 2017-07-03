using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class CollectionProgressModel
    {
        public string name;
        public List<ItemProgressModel> items;

        public CollectionProgressModel(string collectionName)
        {
            name = collectionName;
            items = new List<ItemProgressModel>();
        }
    }

    [Serializable]
    public class ItemProgressModel
    {
        public int id;
        public int price;
        public ItemGrades grade;
    }
}
