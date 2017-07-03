using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class PlayerData
    {
		public int Gold {
			get;
			set;
		}
		public int Gems {
			get;
			set;
		}
		public List<CollectionProgressModel> Collections {
			get;
			set;
		}
		public Statistics Statistics {
			get;
			set;
		}
		public string RareTimer {
			get;
			set;
		}

		public string EpicTimer {
			get;
			set;
		}

		public bool IsAdOff {
			get;
			set;
		}
		public bool IsFirstRun {
			get;
			set;
		}
		public bool Shared {
			set;
			get;
		}
        //public PlayerData(string collectionName)
        //{
        //    this.collection = new CollectionProgressModel(collectionName);
        //}
    }

	[Serializable]
	public class Statistics{
		public int BestScoreInGame {
			get;
			set;
		}
		public int BestScoreBase {
			get;
			set;
		}		
		public int BestScoreRare {
			get;
			set;
		}
		public int BestScoreEpic {
			get;
			set;
		}
		public int MaxGoldInGame {
			get;
			set;
		}
		public int MaxGoldBase {
			get;
			set;
		}
		public int MaxGoldRare {
			get;
			set;
		}
		public int MaxGoldEpic {
			get;
			set;
		}


		public int MaxGoldPerItem {
			get;
			set;
		}
		public int MaxGoldPerItemBase {
			get;
			set;
		}
		public int MaxGoldPerItemRare {
			get;
			set;
		}
		public int MaxGoldPerItemEpic {
			get;
			set;
		}

		public int TotalScore {
			get;
			set;
		}

		public int TotalScoreBase {
			get;
			set;
		}

		public int TotalScoreRare {
			get;
			set;
		}

		public int TotalScoreEpic {
			get;
			set;
		}

		public int TotalCards {
			get;
			set;
		}

		public int TotalCardsBase {
			get;
			set;
		}

		public int TotalCardsRare {
			get;
			set;
		}

		public int TotalCardsEpic {
			get;
			set;
		}

//		public int BestScoreInRaw {
//			get;
//			set;
//		}		
//		public int BestScoreInRawBase {
//			get;
//			set;
//		}		
//		public int BestScoreInRawRare {
//			get;
//			set;
//		}		
//		public int BestScoreInRawEpic {
//			get;
//			set;
//		}


		public int GamesNumber {
			get;
			set;
		}

		public int NumberOfBaseGames {
			get;
			set;
		}
		public int NumberOfRareGames {
			get;
			set;
		}
		public int NumberOfEpicGames {
			get;
			set;
		}
	}
}
