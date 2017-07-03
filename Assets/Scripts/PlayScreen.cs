using UnityEngine;
using SwipeMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;

public class PlayScreen : MonoBehaviour
{
	public GameObject chestView;
	public Text userGoldText;
	public Text userGemsText;
	public Text userHighscoreText;
	public GameObject modal;
	public Button buyGemsButton;
	//public Button buyGemsTimerButton;
	public Button playButton;
	public ScenesManager scenesManager;

	public Color NotActiveChestColor;

	public Text timerText;
	public Text timerTextRare;
	public Text timerTextEpic;
	public Text buyTimertext;
	public Text buyText;

	public GameObject rareChestPlane;
	public GameObject epicChestPlane;
	public GameObject rareChestButton;
	public GameObject epicChestButton;

	public GameObject noWifi;

	public int rareCostGold;
	public int rareCostGems;
	public int epicCostGold;
	public int epicCostGems;

	private int _currentCostGold;
	private int _currentCostGems;
	private bool _payByGold;

	private float[] _timers;

	private ChestGrades _currentGrade;

	private List<GameObject> chests = new List<GameObject>();
	// Use this for initialization
	void Start()
	{ 
		UpdateUI ();

		StartCoroutine (TimeScript.Instance.GetTimeFromServer(TimeScript.Instance.timeUrl, GetTime, OnServerError));

		foreach (Transform chest in chestView.transform) {
			chests.Add (chest.gameObject);
		}
		ProgressManager.progressManager.chestGrade = ChestGrades.Base;
	}

	public void UpdateUI () {
		userGoldText.text = ProgressManager.progressManager.playerData.Gold.ToString();
		userGemsText.text = ProgressManager.progressManager.playerData.Gems.ToString();
		userHighscoreText.text = ProgressManager.progressManager.playerData.Statistics.BestScoreInGame.ToString();
	}

	// Update is called once per frame
	void Update()
	{
		buyGemsButton.interactable = ProgressManager.progressManager.playerData.Gems >= _currentCostGems;
		//buyGemsTimerButton.interactable = ProgressManager.progressManager.playerData.Gems >= _currentCostGems;
	}

	void OnServerError(string error){
		ToggleTimers (false);
	}

	void ToggleTimers(bool activate){
		noWifi.SetActive (!activate);
		timerTextRare.gameObject.SetActive (activate);
		timerTextEpic.gameObject.SetActive (activate);
	}

	public void SetActiveChest (GameObject chestObject) {
		for (int i = 0; i < 3; i++) {
			chests[i].SetActive (false);		
		}
		chestObject.SetActive (true);
		var grade = (ChestGrades)chests.IndexOf (chestObject);
		_currentGrade = grade;
		ProgressManager.progressManager.chestGrade = grade;
		if (grade == ChestGrades.Rare) {
			_currentCostGold = 10000;
			_currentCostGems = 1;
			ToggleBuyButtons (true);
		} else if (grade == ChestGrades.Epic) {
			_currentCostGold = 30000;
			_currentCostGems = 3;
			ToggleBuyButtons (true);
		} else {
			ToggleBuyButtons (false);
		}

		buyText.text = _currentCostGems.ToString ();
		//buyTimertext.text = _currentCostGems.ToString ();

		UpdateTimers ();
	}

	void ToggleBuyButtons(bool activate){
		buyGemsButton.gameObject.SetActive (activate);
		playButton.gameObject.SetActive (!activate);
	}

	public void OpenModalGold () {
		SwipeHandler.Instance.handleSwitchMenu = false;
		_payByGold = true;
		modal.SetActive (true);
	}

	public void OpenModalGems () {
		SwipeHandler.Instance.handleSwitchMenu = false;
		_payByGold = false;
		modal.SetActive (true);
	}

	public void Buy () {
		if (_payByGold) {
			ProgressManager.progressManager.playerData.Gold -= _currentCostGold;			
		} else {
			ProgressManager.progressManager.playerData.Gems -= _currentCostGems;
		}
		ProgressManager.progressManager.Save ();
		SwipeHandler.Instance.handleSwitchMenu = true;
		scenesManager.OpenCore ();
	}

	public void ExitModal () {
		modal.SetActive (false);
		SwipeHandler.Instance.handleSwitchMenu = true;
	}

	void OnApplicationFocus( bool hasFocus )
	{
		StartCoroutine (TimeScript.Instance.GetTimeFromServer(TimeScript.Instance.timeUrl, GetTime, OnServerError));
	}

	public void GetTime(string dateString) {
		ToggleTimers (true);
		var timeObject = DateTime.ParseExact (dateString , "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
		ProgressManager.progressManager.SetTimers (dateString);
		_timers = ProgressManager.progressManager.currentTimers;
		CancelInvoke ("UpdateTimers");
		InvokeRepeating ("UpdateTimers", 0.0f, 1.0f);
	}

	void UpdateTimers(){
		if(_timers != null){
			UpdateRareTimer ();
			UpdateEpicTimer ();
		}		
	}

	void UpdateRareTimer () {
		var ended = _timers [0] <= 60;
		if (!ended) {
			--_timers [0];
		}
		float minutesRare = 0, hoursRare = 0;
		hoursRare = Mathf.Floor (_timers [0] / 3600);
		minutesRare = Mathf.Floor ((_timers [0] % 3600) / 60);
		if (_currentGrade == ChestGrades.Rare) {
			ToggleBuyButtons (!ended);
			//timerText.text = "/ " + hoursRare.ToString ("0") + "h " + minutesRare.ToString ("0") + "min";
		}
		rareChestPlane.SetActive (!ended);
		rareChestButton.SetActive (ended);
		timerTextRare.gameObject.SetActive (!ended && TimeScript.Instance.isServerAvailable);
		timerTextRare.text = hoursRare.ToString ("0") + "h " + minutesRare.ToString ("0") + "min";
	}
	void UpdateEpicTimer(){
		var ended = _timers [1] <= 60;
		if (!ended) {
			--_timers [1];
		}
		float hoursEpic = Mathf.Floor (_timers [1] / 3600);
		float minutesEpic = Mathf.Floor ((_timers [1] % 3600) / 60);
		if (_currentGrade == ChestGrades.Epic) {
			ToggleBuyButtons (!ended);
			//timerText.text = "/ " + hoursEpic.ToString ("0") + "h " + minutesEpic.ToString ("0") + "min";
		}
		epicChestPlane.SetActive (!ended);
		epicChestButton.SetActive (ended);
		timerTextEpic.gameObject.SetActive (!ended && TimeScript.Instance.isServerAvailable);
		timerTextEpic.text = hoursEpic.ToString ("0") + "h " + minutesEpic.ToString ("0") + "min";
	}
}