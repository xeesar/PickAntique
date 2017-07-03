using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Enums;
#if UNITY_5_3_OR_NEWER
#endif
#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif
#if VS_SHARE
using AppAdvisory.SharingSystem;
#endif
#if APPADVISORY_LEADERBOARD
using AppAdvisory.social;
#endif



namespace AppAdvisory.StopTheLock
{
	public class GameManager : MonoBehaviourHelper
	{
		public int numberOfPlayToShowInterstitial = 5;

		public string VerySimpleAdsURL = "http://u3d.as/oWD";

		public bool RESET_PLAYER_PREF = false;

		public bool isGameOver = false;

		public Image fillingSuperBonus;
		public Image closedSuperBonus;

		public GameObject levelCenterScreen;
		public Text levelTopScreen;
		public Text timerText;
		public int scoreInRawToBonus = 10;
		public int scoreBeforeBonus = 10;

		public int superBonusSignsCount = 12;

		public float blackoutSpeed = 4.0f;

		public Transform theGame;

		public RectTransform lockRect;

		public bool gameIsStarted = false;
		public bool missed = false;
		public float timeLeft = 10.0f;
		public float totalTimeLeft = 10.0f;
		public float bonusTimeLimit = 10;
		public float bonusScore;
		public GameObject bonusMagic;

		public GameObject[] earnedCards;
		public Text earnedCardsText;

		public Animator timerAnimator;

		//public Material bonusBackMaterial;
		public GameObject bonusBack;
		public SpriteRenderer blackBack;
		public float superBonusTimer = 10.0f;
		private float _superBonusTimer;

		private Animator _centerAnimator;
		private Text _centerText;

		public Material[,] BackMaterials = new Material[3,2];

		public float timerSpeed = 1.0f;

		public int score = 0;
		private int _scoreInRaw = 0;
		private int _gotScoreInRawToBonus = 0;
		private int _bestScoreInRaw = 0;
		public bool isLastChance;

		private bool _startBonus = false;

		public bool superBonus = false;

		CanvasScaler canvasScaler;


		public bool isSuccess
		{
			get
			{
				bool success = false;
				return success;
			}
		}

		public void LoadWinScene()
		{
			StartCoroutine(Fading.fader.LoadScene("WinScene", 2));
		}

		public void LoadMainScene()
		{
			StartCoroutine(Fading.fader.LoadScene("MainScene"));
		}

		void Awake()
		{

			if (RESET_PLAYER_PREF)
				PlayerPrefs.DeleteAll();

			RESET_PLAYER_PREF = false;

			SetNewGame();

			gameIsStarted = true;
			AdMobScript.Instance.ShowBanner ();
		}

		void Start()
		{
			_centerAnimator = levelCenterScreen.GetComponent<Animator> ();
			_centerText = levelCenterScreen.GetComponent<Text> ();
		}

		void Update()
		{
			if (!player.firstMove)
			{
				if (!bonusManager.CheckOnActiveBonus(BonusTypes.Freeze) && !isLastChance && !superBonus) {
					timeLeft -= Time.deltaTime * timerSpeed;
					SetTimerText (timeLeft);
				}
				if (superBonus) {
					if (_superBonusTimer > 0) {
						_superBonusTimer -= Time.deltaTime;
					} else {
						QuitBonus ();
					}
				}
				if (timeLeft <= 3) {
					timerAnimator.Play ("TimerAnimation");
				} else {
					timerAnimator.Play ("Stable");
				}
				if (timeLeft <= 0 && !isGameOver)
				{
					isLastChance = true;
					bonusManager.ResetBonuses ();
					levelTopScreen.text = "LAST CHANCE";
				}
			} else {
				levelTopScreen.text = "TAP TO START";
			}
		}

		void SetNewGame()
		{
			isGameOver = false;

			gameIsStarted = false;

			player.transform.eulerAngles = Vector3.zero;
		}

		private void SetTimerText (float time) {
			if (time <= 0) {
				time = 0;
			} else if(time > 10) {
				time = 10;
			}
			timerText.text = time.ToString ("f0");		
		}

		IEnumerator IncreaseCounter (int score) {
			yield return new WaitForSeconds (0.3f);
			_centerAnimator.Play ("CenterCount", 0, 0.167f);
			_centerText.text = score.ToString();
			if (isLastChance) {
				isGameOver = true;
				Win ();
				//return ;
			}
			if (timeLeft < 10) {
				timeLeft += 1;
				SetTimerText (timeLeft);		
			}
			player.IncreaseSpeed ();
			soundManager.PlaySuccess();
			bool success = false;

			if (success)
			{
				Win();
			}
			else
				soundManager.PlayTouch();
		}

		public void MoveDone()
		{
			_scoreInRaw ++;
			score++;
			ProgressManager.progressManager.playerData.Statistics.TotalScore++;
			if (superBonus) {
				//score += 3;
			} else if (score > scoreBeforeBonus) {
				_gotScoreInRawToBonus ++;
			}
			var earnedCardsNumber = 0;
			switch (ProgressManager.progressManager.chestGrade) {
			case ChestGrades.Base:
				ProgressManager.progressManager.playerData.Statistics.TotalScoreBase++;
				earnedCardsNumber = (int)(score / 10);
				break;
			case ChestGrades.Rare:
				ProgressManager.progressManager.playerData.Statistics.TotalScoreRare++;
				earnedCardsNumber = (int)(score / 10);
				break;
			case ChestGrades.Epic:
				ProgressManager.progressManager.playerData.Statistics.TotalScoreEpic++;
				earnedCardsNumber = (int)(score / 10);
				break;
			}
			if (earnedCardsNumber > 0) {
				earnedCardsText.text = earnedCardsNumber.ToString();
				if (earnedCardsNumber < 6) {
					earnedCards [earnedCardsNumber - 1].SetActive (true);
				}
			}

			if (score == scoreInRawToBonus) {
				StartCoroutine ("OpenSuper");
			}

			if (score > scoreInRawToBonus) {
				var fillAmountTarget = (_gotScoreInRawToBonus) % 10 / 10.0f;
				StartCoroutine (FillSuper(fillingSuperBonus.fillAmount, fillAmountTarget));
			}
			if (_gotScoreInRawToBonus == scoreInRawToBonus && !superBonus) {
				StartBonus ();
				_gotScoreInRawToBonus = 0;
			}
			StartCoroutine (IncreaseCounter(score));
		
		}
		IEnumerator OpenSuper () {
			for (float f = 1f; f >= 0; f -= 0.05f) {
				closedSuperBonus.fillAmount = f;
				yield return new WaitForSeconds(.05f);
			}
		}

		IEnumerator FillSuper (float from, float to) {
			if (from > to) {
				for (float f = from; f >= to; f -= 0.01f) {
					fillingSuperBonus.fillAmount = f;
					yield return new WaitForSeconds (.005f);
				}
			} else {
				for (float f = from; f <= to; f += 0.005f) {
					fillingSuperBonus.fillAmount = f;
					yield return new WaitForSeconds (.005f);
				}
			}
		}

		public void MissDone()
		{
			if (superBonus) {
				QuitBonus ();
			}
			if (_scoreInRaw > _bestScoreInRaw) {
				_bestScoreInRaw = _scoreInRaw;
			}
			_scoreInRaw = 0;
			StartCoroutine (FillSuper(fillingSuperBonus.fillAmount, 0));
			_gotScoreInRawToBonus = 0;
			if (isLastChance) {
				isGameOver = true;
				Win ();
				return;
			}
			levelCenterScreen.GetComponent<Text>().text = score.ToString();
			if (!bonusManager.CheckOnActiveBonus(BonusTypes.Freeze) && !superBonus) {
				timeLeft--;
			}
			soundManager.PlayFail();
			missed = true;
		}

		public void Win()
		{
			var winResults = new WinResults
			{
				ChestGrade = ProgressManager.progressManager.chestGrade,
				Score = score,
				ScoreInRaw = _bestScoreInRaw
			};
			ProgressManager.progressManager.winResults = winResults;
			LoadWinScene();
		}

		void StartBonus()
		{
			Time.timeScale = 1f;
			soundManager.PlaySuperBonus ();
			_superBonusTimer = superBonusTimer;
			var bMagic = (GameObject) Instantiate (bonusMagic, new Vector3(0, 0, -200), Quaternion.identity);
			bMagic.transform.SetParent(transform, false);
			GameObject.Destroy (bMagic, 1);
			//transform.Find ("Back").GetComponent<MeshRenderer> ().material = bonusBackMaterial;
			bonusBack.SetActive(true);
			//levelTopScreen.text = "BONUS GAME";
			superBonus = true;
			signsHolder.targetsCount = superBonusSignsCount;
			signsHolder.SetRandomSign ();
		}

		void QuitBonus() {
			signsHolder.targetsCount = 1;
			signsHolder.SetRandomSign ();
			levelTopScreen.text = string.Empty;
			//bonusBack.SetActive(false);
			StartCoroutine(FadeBackBack());
			superBonus = false;
		}

		public IEnumerator FadeBackBack()
		{
			var value = blackBack.color.a ;

			while(value < 1)
			{
				value += Time.deltaTime * blackoutSpeed;
				blackBack.color = new Color (0f,0f,0f,value);
				yield return null;
			}
			bonusBack.SetActive(false);
			while(value >= 0)
			{
				value -= Time.deltaTime * blackoutSpeed;
				blackBack.color = new Color (0f,0f,0f,value);
				yield return null;
			}
		}
	}
}