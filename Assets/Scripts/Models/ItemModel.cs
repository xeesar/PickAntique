using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ItemModel
    {
        public string name;
        public int id;
        public ItemGrade[] grades;
    }

    [Serializable]
    public class ItemGrade
    {
        public ItemGrades grade;
        public string image;
        public string description;
        public int minPrice;
        public int maxPrice;
    }
}
