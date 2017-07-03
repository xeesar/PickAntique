using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;

public class WinResultsManager : MonoBehaviour,
    IPointerClickHandler
{
    public GameObject newCollectionItemPrefab;
	public GameObject goldItemPrefab;
	public GameObject gemItemPrefab;
	public GameObject jewelItemPrefab;

	public Material baseMat;
	public Material rareMat;
	public Material epicMat;
	public Sprite baseSprt;
	public Sprite rareSprt;
	public Sprite epicSprt;

	public AudioSource audioSource;
	public AudioClip winGoldSound;
	public AudioClip winGemSound;
	public AudioClip winCardSound;
	public AudioClip winJewelSound;


	public SpriteRenderer itemCard;
	public Renderer goldCard;
	public Renderer gemCard;
	public Renderer jewelCard;

    public List<GameObject> jewelsPrefabs;
	public Text scoreText;
	public Text centerText;
	public Image circleImage;
	public GameObject collectionItemHolder;
	public Text cardName;
	//public Text goldAmountText;

	public GameObject goldScoreObject;
	public GameObject gemsScoreObject;
	public GameObject[] rateStars;
	public GameObject miasHead;
//	public GameObject sharePopUpObject;

	public int waitBeforeGemScoreCounted = 150;

	public float moveScoreSpeed = 2.0f;

    private bool _touchStarted;
    // private bool _isGeneratinng;
    private bool _moved;

	private int _currentCost;

    private ChestConfigFactory _configsFactory;
    private ChestConfig _chestConfig;
    private int _countToGenerate;
    private GameObject _currentItem;
	private float _score;
	private ChestGrades _chestGrade;
	private int _prizesCount;
	private WinSceneStates _currentState = WinSceneStates.None;
	private float _currentScoreInCount = 0;

	private int _currentScoreGold;
	private int _currentScoreGems;

	private int _generatedItems = 0;
	private int _generatedCardsNumber = 0;
	private int _generatedGold = 0;
	private int _maxGoldInCard = 0;

	private bool fullCollection = false;

	private ProgressManager pm = ProgressManager.progressManager;

	private int _movedScore = 0;

	private Dictionary<ChestGrades, int> _scoresForPrize = new Dictionary<ChestGrades, int>{
		{ChestGrades.Base, 10},
		{ChestGrades.Rare, 10},
		{ChestGrades.Epic, 10}
	};

	private string _inFaceVideo = "video";

    // Use this for initialization
    void Awake()
    {

		_configsFactory = new ChestConfigFactory();
		var results = pm.winResults;///new WinResults{ Score = 170, ChestGrade = ChestGrades.Base };////
		_score = results.Score;
		_chestGrade = results.ChestGrade;//ProgressManager.progressManager.winResults;

		switch (_chestGrade) {
		case ChestGrades.Base:
			itemCard.sprite = baseSprt;
			goldCard.material = baseMat;
			gemCard.material = baseMat;
			break;
		case ChestGrades.Rare:
			itemCard.sprite = rareSprt;
			goldCard.material = rareMat;
			gemCard.material = rareMat;
			break;
		case ChestGrades.Epic:
			itemCard.sprite = epicSprt;
			goldCard.material = epicMat;
			gemCard.material = epicMat;
			break;
		default:
			break;
		}

		scoreText.text = _score.ToString();
    }

//	void ShowSharePopUp () {
//		sharePopUpObject.SetActive (true);
//	}

	void UpdateStatistics () {

		pm.playerData.Statistics.GamesNumber++;
		pm.playerData.Statistics.TotalCards = pm.playerData.Statistics.TotalCards + _generatedItems;
		switch (pm.playerData.Statistics.GamesNumber) {
		case 5:
			LoggingService.Instance.Games (5);
			break;
		case 10:
			LoggingService.Instance.Games (10);
			break;
		case 15:
			LoggingService.Instance.Games (15);
			break;
		case 20:
			LoggingService.Instance.Games (20);
			break;
		case 25:
			LoggingService.Instance.Games (25);
			break;
		case 30:
			LoggingService.Instance.Games (30);
			break;
		case 40:
			LoggingService.Instance.Games (40);
			break;
		case 50:
			LoggingService.Instance.Games (50);
			break;
		case 75:
			LoggingService.Instance.Games (75);
			break;
		case 100:
			LoggingService.Instance.Games (100);
			break;
		case 150:
			LoggingService.Instance.Games (150);
			break;
		case 200:
			LoggingService.Instance.Games (200);
			break;
		case 250:
			LoggingService.Instance.Games (250);
			break;
		case 300:
			LoggingService.Instance.Games (300);
			break;
		case 400:
			LoggingService.Instance.Games (400);
			break;
		case 500:
			LoggingService.Instance.Games (500);
			break;
		case 750:
			LoggingService.Instance.Games (750);
			break;
		case 1000:
			LoggingService.Instance.Games (1000);
			break;
		default:
			break;
		}

		var isNewRecord = pm.winResults.Score > pm.playerData.Statistics.BestScoreInGame;
		if (isNewRecord) {
			var prevScore = pm.playerData.Statistics.BestScoreInGame;
			if (pm.winResults.Score >= 500 && prevScore < 500) {
				LoggingService.Instance.Records (500);
			} else if (pm.winResults.Score >= 400 && prevScore < 400) {
				LoggingService.Instance.Records (400);
			} else if (pm.winResults.Score >= 350 && prevScore < 350) {
				LoggingService.Instance.Records (350);
			} else if (pm.winResults.Score >= 300 && prevScore < 300) {
				LoggingService.Instance.Records (300);
			} else if (pm.winResults.Score >= 250 && prevScore < 250) {
				LoggingService.Instance.Records (250);
			} else if (pm.winResults.Score >= 200 && prevScore < 200) {
				LoggingService.Instance.Records (200);
			} else if (pm.winResults.Score >= 175 && prevScore < 175) {
				LoggingService.Instance.Records (175);
			} else if (pm.winResults.Score >= 150 && prevScore < 150) {
				LoggingService.Instance.Records (150);
			} else if (pm.winResults.Score >= 125 && prevScore < 125) {
				LoggingService.Instance.Records (125);
			} else if (pm.winResults.Score >= 100 && prevScore < 100) {
				LoggingService.Instance.Records (100);
			} else if (pm.winResults.Score >= 75 && prevScore < 75) {
				LoggingService.Instance.Records (75);
			} else if (pm.winResults.Score >= 50 && prevScore < 50) {
				LoggingService.Instance.Records (50);
			} else if (pm.winResults.Score >= 40 && prevScore < 40) {
				LoggingService.Instance.Records (40);
			} else if (pm.winResults.Score >= 30 && prevScore < 30) {
				LoggingService.Instance.Records (30);
			} else if (pm.winResults.Score >= 25 && prevScore < 25) {
				LoggingService.Instance.Records (25);
			} else if (pm.winResults.Score >= 20 && prevScore < 20) {
				LoggingService.Instance.Records (20);
			} else if (pm.winResults.Score >= 15 && prevScore < 15) {
				LoggingService.Instance.Records (15);
			} else if (pm.winResults.Score >= 10 && prevScore < 10) {
				LoggingService.Instance.Records (10);
			} else if (pm.winResults.Score >= 5 && prevScore < 5) {
				LoggingService.Instance.Records (5);
			}

			pm.playerData.Statistics.BestScoreInGame = pm.winResults.Score;
		}

//		if (pm.winResults.ScoreInRaw > pm.playerData.Statistics.BestScoreInRaw) {
//			pm.playerData.Statistics.BestScoreInRaw = pm.winResults.ScoreInRaw;
//		}

		if (_generatedGold > pm.playerData.Statistics.MaxGoldInGame) {
			pm.playerData.Statistics.MaxGoldInGame = _generatedGold;
		}

		if (_maxGoldInCard > pm.playerData.Statistics.MaxGoldPerItem) {
			pm.playerData.Statistics.MaxGoldPerItem = _maxGoldInCard;
		}

		switch (pm.winResults.ChestGrade) {
		case ChestGrades.Base:
			pm.playerData.Statistics.NumberOfBaseGames++;
			pm.playerData.Statistics.TotalCardsBase = pm.playerData.Statistics.TotalCardsBase + _generatedItems;

			if (pm.winResults.Score > pm.playerData.Statistics.BestScoreBase) {
				pm.playerData.Statistics.BestScoreBase = pm.winResults.Score;
			}

//			if (pm.winResults.ScoreInRaw > pm.playerData.Statistics.BestScoreInRawBase) {
//				pm.playerData.Statistics.BestScoreInRawBase = pm.winResults.ScoreInRaw;
//			}

			if (_generatedGold > pm.playerData.Statistics.MaxGoldBase) {
				pm.playerData.Statistics.MaxGoldBase = _generatedGold;
			}

			if (_maxGoldInCard > pm.playerData.Statistics.MaxGoldPerItemBase) {
				pm.playerData.Statistics.MaxGoldPerItemBase = _maxGoldInCard;
			}
			break;

		case ChestGrades.Rare:
			pm.playerData.Statistics.NumberOfRareGames++;
			pm.playerData.Statistics.TotalCardsRare = pm.playerData.Statistics.TotalCardsRare + _generatedItems;

			if (pm.winResults.Score > pm.playerData.Statistics.BestScoreRare) {
				pm.playerData.Statistics.BestScoreRare = pm.winResults.Score;
			}

//			if (pm.winResults.ScoreInRaw > pm.playerData.Statistics.BestScoreInRawRare) {
//				pm.playerData.Statistics.BestScoreInRawRare = pm.winResults.ScoreInRaw;
//			}

			if (_generatedGold > pm.playerData.Statistics.MaxGoldRare) {
				pm.playerData.Statistics.MaxGoldRare = _generatedGold;
			}

			if (_maxGoldInCard > pm.playerData.Statistics.MaxGoldPerItemRare) {
				pm.playerData.Statistics.MaxGoldPerItemRare = _maxGoldInCard;
			}
			break;

		case ChestGrades.Epic:
			pm.playerData.Statistics.NumberOfEpicGames++;
			pm.playerData.Statistics.TotalCardsEpic = pm.playerData.Statistics.TotalCardsEpic + _generatedItems;

			if (pm.winResults.Score > pm.playerData.Statistics.BestScoreEpic) {
				pm.playerData.Statistics.BestScoreEpic = pm.winResults.Score;
			}

//			if (pm.winResults.ScoreInRaw > pm.playerData.Statistics.BestScoreInRawEpic) {
//				pm.playerData.Statistics.BestScoreInRawEpic = pm.winResults.ScoreInRaw;
//			}

			if (_generatedGold > pm.playerData.Statistics.MaxGoldEpic) {
				pm.playerData.Statistics.MaxGoldEpic = _generatedGold;
			}

			if (_maxGoldInCard > pm.playerData.Statistics.MaxGoldPerItemEpic) {
				pm.playerData.Statistics.MaxGoldPerItemEpic = _maxGoldInCard;
			}
			break;
		}
		pm.Save ();
//		if (isNewRecord) {
//			ShowSharePopUp ();
//		} else {
			Close ();
		//}
	}

    Sprite GetItemImage(int itemId, ItemGrades grade)
    {
        return Resources.Load<Sprite>(CollectionService.Instance.GetBaseImageUrl() +
            CollectionService.Instance.GetItemImage(itemId, grade));
    }

    void Start()
    {
        GenerateResults();
    }

	void Update () {
		switch (_currentState) {
		case WinSceneStates.TimeBarStart:
			DisableScores ();
			RemoveCurrent ();
			_currentScoreInCount = 0;
			_currentState = WinSceneStates.TimeBar;
			break;
		case WinSceneStates.TimeBar:
			var dt = Time.smoothDeltaTime;
			_currentScoreInCount += dt * 30;
			UpdateText (_score - _currentScoreInCount);
			if (_score < _scoresForPrize [_chestGrade] && (int)_currentScoreInCount >= _score) {
				centerText.text = "Need more points to get a card";
				centerText.gameObject.SetActive (true);
				_score = 0;
				UpdateText (0);
				_currentState = WinSceneStates.Done;
				break;
			}
			var value = _currentScoreInCount / _scoresForPrize [_chestGrade];
			circleImage.fillAmount = Mathf.Max (value, 0.001f);
			if (value >= 1) {
				_currentState = WinSceneStates.TimeBarEnd;
			} 
			break;
		case WinSceneStates.TimeBarEnd:
			if (_score >= _scoresForPrize [_chestGrade]) {
				_score -= _scoresForPrize [_chestGrade];
				circleImage.fillAmount = 1.0f;
				_currentState = WinSceneStates.GenerateStart;
			} else {
				circleImage.fillAmount = Mathf.Max (_score/_scoresForPrize [_chestGrade], 0.001f);
				_score = 0;
				_currentState = WinSceneStates.Done;
			}
			UpdateText (_score);
			break;
		case WinSceneStates.GenerateStart: 
			GenerateNewItem ();
			_currentScoreGold = pm.playerData.Gold;
			_currentScoreGems = pm.playerData.Gems;
			pm.playerData.Gold += _currentCost;
			break;
		case WinSceneStates.GenerateGold:
			var goldText = goldScoreObject.transform.FindChild ("GoldText");
			var delt = (int)(Time.deltaTime * _currentCost * moveScoreSpeed);
			_movedScore += delt > 0 ? delt : 1;
			if (_movedScore < _currentCost) {
				//_currentCost -= movedScore;
				//_goldScore += movedScore;
				goldText.GetComponent<Text> ().text = (_currentScoreGold + _movedScore).ToString();
			} else {
				_currentScoreGold += _currentCost;
				goldText.GetComponent<Text> ().text = _currentScoreGold.ToString();
				_currentState = WinSceneStates.GenerateEnd;				
			}
			break;
		case WinSceneStates.GenerateGem:
			pm.playerData.Gems++;
			System.Threading.Thread.Sleep (waitBeforeGemScoreCounted);
			var gemText = gemsScoreObject.transform.FindChild ("GemsText");
			gemText.GetComponent<Text> ().text = (++ _currentScoreGems).ToString();
			_currentState = WinSceneStates.GenerateEnd;				
			break;
		case WinSceneStates.GenerateCollectionItem:
			_currentState = WinSceneStates.GenerateEnd;
			break;
		case WinSceneStates.GenerateEnd:
			_movedScore = 0;
			pm.Save ();
			break;
		case WinSceneStates.None:
			break; 
		}

	}
	private void UpdateText (float score) {
		scoreText.text = (Math.Round(score)).ToString ();	
	}

	private void DisableScores () {
		goldScoreObject.SetActive (false);
		gemsScoreObject.SetActive (false);		
		for (int i = 0; i < rateStars.Length; i++) {
			rateStars [i].SetActive (false);
		}
		miasHead.SetActive (false);
	}

    public void GenerateResults()
    {
		var chestGrade = _chestGrade;

		_chestConfig = _configsFactory.GetChestConfigs(chestGrade);

		_countToGenerate = UnityEngine.Random.Range(_chestConfig.MinItemsCount, _chestConfig.MaxItemsCount);
    }

    private void GenerateNewItem()
	{
		_generatedItems++;
		if (ProgressManager.progressManager.playerData.Statistics.TotalCards == 0 && _generatedItems == 0) {
			ItemProgressModel newCard = null;
			newCard = pm.GetNewItemProgress ();
			if (newCard != null) {
				GenerateCollectionCard (newCard);
			} else {
				GenerateGem();
			}
			return;
		} 

		RemoveCurrent();

		//////////////////////////////////////////
		/// FOR TESTING CARDS GENERATION
		/// //////////////////////////////////////
//		ItemProgressModel newCard = null;
//		newCard = pm.GetNewItemProgress ();
//		if (newCard != null) {
//			GenerateCollectionCard (newCard);
//		} else {
//			//GenerateGold(random);
//		}
		/////////////////////////////////////////

		var random = UnityEngine.Random.Range(1, 100);
		if (random < _chestConfig.GoldPossibility)
		{
			GenerateGold(random);
        }
		else if (random < _chestConfig.JewelPossibility)
		{
			GenerateGold(UnityEngine.Random.Range(500, 2000), true);
        }
		else if (random <= _chestConfig.CardPossibility)
        {
			ItemProgressModel newCard = null;
			newCard = pm.GetNewItemProgress ();
			if (newCard != null) {
				GenerateCollectionCard (newCard);
			} else {
				GenerateGem();
			}
        }
		else if (random < _chestConfig.GemPossibility)
		{
			GenerateGem();
        }
    }

	void GenerateGold(int random, bool isJewel = false)
    {
		_currentCost = random;
		_generatedGold += _currentCost;
		if (_currentCost >= _maxGoldInCard) {
			_maxGoldInCard = _currentCost;
		}
		if (isJewel) {
			GenerateJewelCard (_currentCost);			
		} else {
			GenerateGoldCard (_currentCost);
		}
    }

	void AddGoldToScore (int cost) {
		pm.playerData.Gold += cost;
		pm.Save ();
	}

    void GenerateCollectionCard(ItemProgressModel card)
    {

		//this.fullCollection = true;
		audioSource.PlayOneShot (winCardSound,1f);
		var cardsNumber = ProgressManager.progressManager.GetCollectionProgress ();
		switch (cardsNumber) {
		case 1:
			LoggingService.Instance.Cards (1);
			break;
		case 2:
			LoggingService.Instance.Cards (2);
			break;
		case 3:
			LoggingService.Instance.Cards (3);
			break;
		case 10:
			LoggingService.Instance.Cards (10);
			break;
		case 15:
			LoggingService.Instance.Cards (15);
			break;
		case 20:
			LoggingService.Instance.Cards (20);
			break;
		case 30:
			LoggingService.Instance.Cards (30);
			break;
		case 40:
			LoggingService.Instance.Cards (40);
			break;
		case 50:
			LoggingService.Instance.Cards (50);
			break;
		case 60:
			LoggingService.Instance.Cards (60);
			break;
		case 66:
			this.fullCollection = true;
			LoggingService.Instance.Cards (66);
			break;
		default:
			break;
		}

		_generatedCardsNumber++;
		var intGrade = (int)card.grade;
		for (int i = 0; i <= intGrade; i++) {
			rateStars [i].SetActive (true);
		}
		miasHead.SetActive (true);

        var itemData = CollectionService.Instance.GetItemFromCollection(card.id);
        var sprite = GetItemImage(card.id, card.grade);

		collectionItemHolder.GetComponent<SpriteRenderer>().sprite = sprite;
		cardName.gameObject.SetActive (true);
		cardName.text = itemData.name;
		//collectionItemHolder.GetComponentInChildren<Text> ().text = itemData.name;
		collectionItemHolder.SetActive (true);

		_currentState = WinSceneStates.GenerateCollectionItem;
    }

	void RemoveCurrent() {
		//goldAmountText.gameObject.SetActive (false);
		cardName.gameObject.SetActive (false);
		goldItemPrefab.SetActive (false);
		gemItemPrefab.SetActive (false);
		jewelItemPrefab.SetActive (false);
		collectionItemHolder.SetActive (false);
    }
	void GenerateGoldCard(int cost)
	{
		audioSource.PlayOneShot (winGoldSound,1f);
		goldItemPrefab.gameObject.SetActive (true);
		var tm = goldItemPrefab.GetComponentInChildren<TextMesh> ();
		tm.text = cost.ToString ();
		var animator = goldItemPrefab.GetComponent<Animator> ();
		animator.Play ("Turn");
		goldScoreObject.SetActive (true);
		cardName.gameObject.SetActive (true);
		cardName.text = "Coins";
		_currentState = WinSceneStates.GenerateGold;
	}
	void GenerateJewelCard(int cost)
	{
		audioSource.PlayOneShot (winJewelSound,1f);
		jewelItemPrefab.gameObject.SetActive (true);
		var tm = jewelItemPrefab.GetComponentInChildren<TextMesh> ();
		tm.text = cost.ToString ();
		var animator = jewelItemPrefab.GetComponent<Animator> ();
		animator.Play ("Turn");
		goldScoreObject.SetActive (true);
		cardName.gameObject.SetActive (true);
		cardName.text = "Coins";
		_currentState = WinSceneStates.GenerateGold;
	}
	void GenerateGem() {
		audioSource.PlayOneShot (winGemSound,1f);
		gemItemPrefab.gameObject.SetActive (true);
		miasHead.SetActive (true);
		var animator = gemItemPrefab.GetComponent<Animator> ();
		animator.Play ("Turn");
		gemsScoreObject.SetActive (true);
		cardName.gameObject.SetActive (true);
		cardName.text = "Gem";
		_currentState = WinSceneStates.GenerateGem;
	}

    public void Close()
    {
		AdMobScript.Instance.HideBanner ();
		ProgressManager.progressManager.ResetTimers (_chestGrade);
		ShowAd ();
		if (fullCollection) {
			StartCoroutine (Fading.fader.LoadScene ("FullCollectionScene"));
		} else {
			StartCoroutine(Fading.fader.LoadScene("MainScene"));
		}
    }

	private void ShowAd(){
		if (Advertisement.isSupported) {
			Advertisement.Initialize ("1258582", true);
			Advertisement.Initialize ("1258583", true);
		}
		var gamesNumber = ProgressManager.progressManager.playerData.Statistics.GamesNumber;
		if (gamesNumber > 0 && gamesNumber % 3 == 0 && !ProgressManager.progressManager.playerData.IsAdOff) {
			if (Advertisement.IsReady (_inFaceVideo)) {
				Advertisement.Show (_inFaceVideo);
			}
		}
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		centerText.gameObject.SetActive (false);
		switch (_currentState) {
		case WinSceneStates.GenerateGem:
		case WinSceneStates.TimeBarEnd:
		case WinSceneStates.GenerateGold:
		case WinSceneStates.GenerateCollectionItem:
		case WinSceneStates.TimeBar:
			break;
		default:		
			if (_score <= 0) {
				UpdateStatistics ();
			} else {			
				_currentState = WinSceneStates.TimeBarStart;
			}
			break;
		}
    }

}

public enum WinSceneStates {
	None,

	GenerateStart,
	GenerateGold,
	GenerateGem,
	GenerateCollectionItem,
	GenerateEnd,

	TimeBarStart,
	TimeBar,
	TimeBarEnd,

	WinStart,
	Win,
	WinEnd,

	Done
}
