using UnityEngine;
using UnityEngine.UI;
using SwipeMenu;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;

public class HighscoresScript : MonoBehaviour {

	public Text cardsScore;
	public Text bestScore;
	public Text games;
	public Text maxGold;
	public Text maxGoldInCard;

	public Toggle totalsToggle;

	//public GameObject statsToggleGroup;

	private Statistics _stats;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {	
	}

	public void ShowHighscoresPanel () {
		SwipeHandler.Instance.handleSwitchMenu = false;
		_stats = ProgressManager.progressManager.playerData.Statistics;
		gameObject.SetActive (true);
		totalsToggle.isOn = true;
		ShowTotals ();
		//statsToggleGroup.SetActive (true);
	}

	public void HideHighscoresPanel () {
		gameObject.SetActive (false);
		SwipeHandler.Instance.handleSwitchMenu = true;
	}

	private void ShowTotals(){
		cardsScore.text = _stats.TotalCards.ToString();
		//scoreInRaw.text = _stats.BestScoreInRaw.ToString ();
		bestScore.text = _stats.BestScoreInGame.ToString ();
		games.text = _stats.GamesNumber.ToString ();
		maxGold.text = _stats.MaxGoldInGame.ToString ();
		maxGoldInCard.text = _stats.MaxGoldPerItem.ToString ();
	}

	public void ShowScores (int grade) {
		
		if (grade == -1) {
			ShowTotals ();
		} else {

			switch (grade) {
			case (int)ChestGrades.Base:
				cardsScore.text = _stats.TotalCardsBase.ToString();
				//scoreInRaw.text = _stats.BestScoreInRawBase.ToString ();
				bestScore.text = _stats.BestScoreBase.ToString ();
				games.text = _stats.NumberOfBaseGames.ToString ();
				maxGold.text = _stats.MaxGoldBase.ToString ();
				maxGoldInCard.text = _stats.MaxGoldPerItemBase.ToString ();
			
				break;
			case (int)ChestGrades.Rare:
				cardsScore.text = _stats.TotalCardsRare.ToString();
				//scoreInRaw.text = _stats.BestScoreInRawRare.ToString ();
				bestScore.text = _stats.BestScoreRare.ToString ();
				games.text = _stats.NumberOfRareGames.ToString ();
				maxGold.text = _stats.MaxGoldRare.ToString ();
				maxGoldInCard.text = _stats.MaxGoldPerItemRare.ToString ();
				break;
			case (int)ChestGrades.Epic:
				cardsScore.text = _stats.TotalCardsEpic.ToString();
				//scoreInRaw.text = _stats.BestScoreInRawEpic.ToString ();
				bestScore.text = _stats.BestScoreEpic.ToString ();
				games.text = _stats.NumberOfEpicGames.ToString ();
				maxGold.text = _stats.MaxGoldEpic.ToString ();
				maxGoldInCard.text = _stats.MaxGoldPerItemEpic.ToString ();
				break;
			}
		}
	}	
	}