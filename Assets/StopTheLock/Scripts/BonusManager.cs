using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppAdvisory.StopTheLock;

public class BonusManager : MonoBehaviourHelper {

	public GameObject[] bonusesObjects;

	public GameObject freezeTimerIcon;
	public GameObject sloMoTimerIcon;
	public GameObject x2TimerIcon;
	public GameObject[] targets;

	public Image freezeTimerImage;
	public Image slowMoTimerImage;
	public Image x2TimerImage;
	public float timeToMoveBonus = 0.0f;

	private float timer = 5.0f;
	private float sloMoKoef = 0.5f;
	private float speedToStartSloMo = 1.6f;

	private List<GameObject> bonusesOnScreen = new List<GameObject>();

	public Dictionary<int, BonusTypes> bonuses = new Dictionary<int, BonusTypes>();
	public List<BonusModel> activeBonuses = new List<BonusModel>();

	void Start () {
	}

	public void InitBonus (int index, Vector3 position) {
		//if (bonuses.Count == 0) {
		var random = Random.Range (0, 99);
		GameObject bonusPrefab = new GameObject ();
		if (random < 30) {
			if (!activeBonuses.Any (b => b.type == BonusTypes.Freeze)) {
				bonusPrefab = (GameObject)Instantiate (bonusesObjects [0], position, Quaternion.identity);
				bonuses.Add (index, BonusTypes.Freeze);
			}
		} else if (random < 60) {
			if (player.speed >= speedToStartSloMo && !activeBonuses.Any (b => b.type == BonusTypes.SlowMo)) {
				bonusPrefab = (GameObject)Instantiate (bonusesObjects [1], position, Quaternion.identity);
				bonuses.Add (index, BonusTypes.SlowMo);
			}
		} else {
			if (!activeBonuses.Any (b => b.type == BonusTypes.X2)) {
				bonusPrefab = (GameObject)Instantiate (bonusesObjects [2], position, Quaternion.identity);
				bonuses.Add (index, BonusTypes.X2);
			}
		}
		bonusesOnScreen.Add (bonusPrefab);
		bonusPrefab.transform.SetParent (transform);
		//}
	}

	void Update () {
		if (gameManager.superBonus) {
			return;
		}
		//if (activeBonuses.Count > 0) {
			var slowMoBonus = activeBonuses.FirstOrDefault (b => b.type == BonusTypes.SlowMo);
			if (slowMoBonus != null) {
				if (Time.timeScale > sloMoKoef) {
					Time.timeScale -= Time.deltaTime;
				}
				if (slowMoBonus.timer > 0) {
					slowMoBonus.timer -= Time.deltaTime;
					slowMoTimerImage.fillAmount = slowMoBonus.timer / 5.0f;
					
					//sloMoTimerText.text = (Mathf.CeilToInt(slowMoBonus.timer)).ToString();
				} else {
					activeBonuses.Remove (slowMoBonus);
					sloMoTimerIcon.SetActive (false);
				}
			} else {
				if (Time.timeScale < 1.0f) {
					Time.timeScale += Time.deltaTime;
				}
			}
			var freezeBonus = activeBonuses.FirstOrDefault (b => b.type == BonusTypes.Freeze);
			if (freezeBonus != null) {
				if (freezeBonus.timer > 0) {
					freezeBonus.timer -= Time.deltaTime;
					freezeTimerImage.fillAmount = freezeBonus.timer / 5.0f;
					//freezeTimerText.text = (Mathf.CeilToInt(freezeBonus.timer)).ToString();
				} else {
					activeBonuses.Remove (freezeBonus);
					freezeTimerIcon.SetActive (false);
				}
			}
			var x2Bonus = activeBonuses.FirstOrDefault (b => b.type == BonusTypes.X2);
			if (x2Bonus != null) {
				if (x2Bonus.timer > 0) {
					x2Bonus.timer -= Time.deltaTime;
					x2TimerImage.fillAmount = x2Bonus.timer / 5.0f;
					//x2TimerText.text = (Mathf.CeilToInt(x2Bonus.timer)).ToString();
				} else {
					signsHolder.targetsCount = 1;
					activeBonuses.Remove (x2Bonus);
					x2TimerIcon.SetActive (false);
				}
			}
	}

	public void ReleaseBonus (BonusTypes type) {
		var toRemove = activeBonuses.FirstOrDefault (b => b.type == type);
		if (toRemove != null) {
			activeBonuses.Remove (toRemove);
		}
	}

	public bool CheckOnBonus (int index) {
		return bonuses.ContainsKey(index);
	}

	public bool CheckOnActiveBonus (BonusTypes type) {
		return activeBonuses.Any(b => b.type == type);
	}

	public void ActivateBonus (int index) {
		//currentBonus = BonusTypes.Freeze;
		BonusTypes bonusType;
		bonuses.TryGetValue (index, out bonusType);
		var currBonus = activeBonuses.FirstOrDefault (b => b.type == bonusType);
		GameObject target = null;
		if (currBonus == null) {

			if (bonusType == BonusTypes.Freeze) {
				freezeTimerIcon.SetActive (true);
				signsHolder.MoveCountTrail (player.currentSectorIndex, freezeTimerIcon);
			} else if (bonusType == BonusTypes.SlowMo) {
				sloMoTimerIcon.SetActive (true);
				signsHolder.MoveCountTrail (player.currentSectorIndex, sloMoTimerIcon);
			} else if (bonusType == BonusTypes.X2) {
				signsHolder.targetsCount = 2;
				x2TimerIcon.SetActive (true);	
				signsHolder.MoveCountTrail (player.currentSectorIndex, x2TimerIcon);
			}
			activeBonuses.Add (new BonusModel {
				type = bonusType,
				timer = timer
			});
			bonuses.Remove (index);
		}
	}

	public void ResetBonuses () {
		foreach (var bonus in bonusesOnScreen) {
			GameObject.Destroy (bonus);
		}
		bonuses = new Dictionary<int, BonusTypes> ();
	}

	void OnDestroy(){
		Time.timeScale = 1.0f;
	}
}

public enum BonusTypes {
	None,
	Freeze,
	SlowMo,
	Death,
	X2
}


public class BonusModel {
	public BonusTypes type;
	public float timer;
}