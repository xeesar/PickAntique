using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;

public class AdScript : MonoBehaviour {

	public float timeToShowAd = 5.0f;
	public int rewardAmountGold = 500;
	public int rewardAmountGem = 0;
	private string _rewardedAdLabel = "rewardedVideo";

	public PlayScreen playScreen;
	public GameObject watchAdButtonGold;
	public GameObject watchAdButtonGem;
	public int gamesNumber = 5;

	private bool _isRightGamesNumber;

	void Start () {
		var gamesCount = ProgressManager.progressManager.playerData.Statistics.GamesNumber;
		_isRightGamesNumber = gamesCount > 0 && gamesCount % gamesNumber == 0;
	}

	// Update is called once per frame
	void Update () {
		if (watchAdButtonGem != null) {
			watchAdButtonGem.gameObject.SetActive (Advertisement.IsReady (_rewardedAdLabel) && _isRightGamesNumber);
		} else {
			watchAdButtonGold.GetComponent<Button> ().interactable = Advertisement.IsReady (_rewardedAdLabel);
		}
	}

	public void ShowAdForGold(){
		ShowRewardedAd (RevardGold);
	}

	public void ShowAdForGem(){
		ShowRewardedAd (RevardGem);
	}

	public void ShowRewardedAd (Action callback) {
		if (Advertisement.IsReady (_rewardedAdLabel)) {
			Advertisement.Show (_rewardedAdLabel, new ShowOptions {
				resultCallback = result => {
					if (result == ShowResult.Finished) {
						callback();
						ProgressManager.progressManager.Save ();
						playScreen.UpdateUI ();
						LoggingService.Instance.RewardedVideo();
					}
				}
			});
		}
		//StartCoroutine (ShowAdsCOR());
	}

	private void RevardGold () {
		ProgressManager.progressManager.playerData.Gold += rewardAmountGold;
	}

	private void RevardGem () {
		ProgressManager.progressManager.playerData.Gems += rewardAmountGem;
		_isRightGamesNumber = false;
		watchAdButtonGem.gameObject.SetActive (false);
	}

	IEnumerator ShowAdsCOR () {
		while (!Advertisement.IsReady(_rewardedAdLabel)) {
			yield return null;
		}
		Advertisement.Show (_rewardedAdLabel);
		yield return new WaitForSeconds (timeToShowAd);
		yield break;
	}
}
