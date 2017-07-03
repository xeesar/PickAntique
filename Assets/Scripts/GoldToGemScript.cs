using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts;

public class GoldToGemScript : MonoBehaviour, IPointerClickHandler {

	public int goldToActivate = 100;
	public int gemsAmount = 1;
	public PlayScreen playScreen;

	public AdScript adScript;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Button> ().interactable = ProgressManager.progressManager.playerData.Gold >= goldToActivate;
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (gameObject.GetComponent<Button> ().interactable) {
			ProgressManager.progressManager.playerData.Gold -= goldToActivate;
			ProgressManager.progressManager.playerData.Gems += gemsAmount;
			ProgressManager.progressManager.Save ();
			LoggingService.Instance.GemsExchange (gemsAmount);
			playScreen.UpdateUI ();
		}
	}
}
