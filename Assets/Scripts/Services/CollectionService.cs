using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CollectionService
    {
        private const string CollectionUrl = @"Data/Collections/{0}";
        private CollectionModel _collection; // TODO: handle many collections
        
        private static CollectionService instance;

        private CollectionService() { }

        public static CollectionService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollectionService();
                }
                return instance;
            }
        }

        public void InitCollectionData(string name)
        {
            if (_collection == null || _collection.name != name)
            {
                var json = Resources.Load<TextAsset>(string.Format(CollectionUrl, name));
                //var json = File.ReadAllText(Application.dataPath + string.Format(CollectionUrl, name));
                _collection = JsonUtility.FromJson<CollectionModel>(json.text);
            }
        }

		public Dictionary<int ,String> GetItemsNames(){
			var result = new Dictionary<int, String> ();

			foreach (var item in _collection.items) {
				result.Add (item.id, item.name);
			}

			return result;
		}

        public ItemModel GetItemFromCollection(int id)
        {
            return this._collection.items.FirstOrDefault(i => i.id == id);
        }

        public string GetBaseImageUrl()
        {
            return _collection.baseImagesUrl;
        }

        public string GetItemImage(int itemId, ItemGrades grade)
        {
            var itemModel = GetItemFromCollection(itemId);
            return itemModel.grades[(int)grade].image;
        }
    }
}
