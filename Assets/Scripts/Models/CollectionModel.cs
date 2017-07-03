using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class CollectionModel
    {
        public int id;
        public string name;
        public string baseImagesUrl;
        public List<ItemModel> items;
    }

}
