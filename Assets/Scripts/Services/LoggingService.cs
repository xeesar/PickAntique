using UnityEngine;
using SwipeMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;

namespace Assets.Scripts
{
	public class LoggingService : MonoBehaviour
	{
		private string _firstLogging = "http://104.199.49.232/game/logging/firstlogin.php";

		private string _cards = "http://104.199.49.232/game/logging/cards{0}.php";

		private string _games = "http://104.199.49.232/game/logging/games{0}.php";

		private string _record = "http://104.199.49.232/game/logging/record{0}.php";

		private string _gemsExchange = "http://104.199.49.232/game/logging/GemExchange{0}.php";

		private string _rewardedVideo = "http://104.199.49.232/game/logging/rewardedvideo.php";
		
		public bool isServerAvailable;
		//private static LoggingService instance;

		private static readonly LoggingService instance = new LoggingService();
		private LoggingService() {}

		public static LoggingService Instance
		{
			get 
			{
				return instance;
			}
		}

		public void RewardedVideo () {
			CallServer(_rewardedVideo);
		}

		public void FirstLogging () {
			CallServer(_firstLogging);
		}
		public void Cards (int count) {
			CallServer(String.Format (_cards, count));
		}

		public void Games(int count){
			CallServer (String.Format (_games, count));
		}

		public void Records(int count){
			CallServer (String.Format (_record, count));
		}

		public void GemsExchange(int count){
			CallServer (String.Format (_gemsExchange, count));
		}

		public static void CallServer(string url)
		{
			WWW www = new WWW(url);
		}

	}
}

