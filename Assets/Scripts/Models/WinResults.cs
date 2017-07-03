using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class WinResults
    {
        public ChestGrades ChestGrade { get; set; }
        public int Score { get; set; }
		public int ScoreInRaw { get; set; }
    }
}
