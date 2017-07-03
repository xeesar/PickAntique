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

public class WinResultsManagerOld : MonoBehaviour,
    IPointerClickHandler
{
    public GameObject newCollectionItemPrefab;
	public GameObject goldItemPrefab;
	public GameObject gemItemPrefab;
    public List<GameObject> jewelsPrefabs;
	public Text scoreText;
	public Image circleImage;


    private bool _touchStarted;
    // private bool _isGeneratinng;
    private bool _moved;

    private ChestConfigFactory _configsFactory;
    private ChestConfig _chestConfig;
    private int _countToGenerate;
    private GameObject _currentItem;
	private float _score;
	private ChestGrades _chestGrade;
	private int _prizesCount;
	private WinSceneStates _currentState = WinSceneStates.None;
	private float _currentScoreInCount = 0;

	private Dictionary<ChestGrades, int> _scoresForPrize = new Dictionary<ChestGrades, int>{
		{ChestGrades.Base, 15},
		{ChestGrades.Rare, 10},
		{ChestGrades.Epic, 5}
	};

    // Use this for initialization
    void Awake()
    {
		_configsFactory = new ChestConfigFactory();
		//
		var results = ProgressManager.progressManager.winResults;//new WinResults{ Score = 17, ChestGrade = ChestGrades.Base };
		_score = results.Score;
		_chestGrade = results.ChestGrade;

		scoreText.text = _score.ToString();
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
			RemoveCurrent();
			_currentScoreInCount = 0;
			_currentState = WinSceneStates.TimeBar;
			break;
		case WinSceneStates.TimeBar:
			var dt = Time.deltaTime * 30;
			_currentScoreInCount += dt;
			UpdateText (_score - _currentScoreInCount);
			if (_score < _scoresForPrize [_chestGrade] && (int)_currentScoreInCount >= _score) {
				_score = 0;
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
			_currentState = WinSceneStates.GenerateEnd;
			break;
		case WinSceneStates.GenerateEnd:
			break;
		case WinSceneStates.None:
			break; 
		}

	}
	private void UpdateText(float score){
		scoreText.text = (Math.Round(score)).ToString ();
	
	}

    public void GenerateResults()
    {
		var chestGrade = _chestGrade;

		_chestConfig = _configsFactory.GetChestConfigs(chestGrade);

		_countToGenerate = UnityEngine.Random.Range(_chestConfig.MinItemsCount, _chestConfig.MaxItemsCount);
    }

    private void GenerateNewItem()
    {
        RemoveCurrent();

		var random = UnityEngine.Random.Range(1, 100);
		if (random < _chestConfig.GoldPossibility)
		{
			GenerateGold(random);
        }
		else if (random < _chestConfig.JewelPossibility)
		{
			GenerateGold(random);
        }
		else if (random < _chestConfig.CardPossibility)
        {
            var newCard = ProgressManager.progressManager.GetNewItemProgress();
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

    void GenerateGold(int random)
    {
        var cost = random * 10;
        GenerateGoldCard(cost);
    }

	void AddGoldToScore (int cost) {
		ProgressManager.progressManager.playerData.Gold += cost;
		ProgressManager.progressManager.Save ();
	}

    void GenerateGoldCard(int cost)
    {
        _currentItem = Instantiate(goldItemPrefab);
		var costText = _currentItem.GetComponentInChildren<Text>();
		costText.text = cost.ToString();
		_currentItem.transform.SetParent(transform);
		_currentItem.gameObject.SetActive (false);
		_currentItem.gameObject.SetActive (true);
    }

    void GenerateJewel(int random)
    {
    }

    void GenerateCollectionCard(ItemProgressModel card)
    {
        var itemData = CollectionService.Instance.GetItemFromCollection(card.id);
        _currentItem = Instantiate(newCollectionItemPrefab);
        var text = _currentItem.GetComponentInChildren<Text>();
        var descr = itemData.grades[(int)card.grade].description;
        text.text = descr;
        var sprite = GetItemImage(card.id, card.grade);
        _currentItem.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
		_currentItem.transform.SetParent(transform);
    }

    void RemoveCurrent() {
        Destroy(_currentItem);
    }

	void GenerateGem() {
		_currentItem = Instantiate(gemItemPrefab);
		_currentItem.transform.SetParent(transform);
		_currentItem.gameObject.SetActive (false);
		_currentItem.gameObject.SetActive (true);
		ProgressManager.progressManager.playerData.Gems++;
		ProgressManager.progressManager.Save ();
	}

    public void Close()
    {
		StartCoroutine(Fading.fader.LoadScene("MainScene"));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
		if (_currentState == WinSceneStates.TimeBar) {
			_currentState = WinSceneStates.TimeBarEnd;		
		} else if (_score <= 0) {
			Close ();
		} else {			
			_currentState = WinSceneStates.TimeBarStart;
		}
    }
}

public enum WinSceneStatesOld {
	None,

	GenerateStart,
	Generate,
	GenerateEnd,

	TimeBarStart,
	TimeBar,
	TimeBarEnd,

	WinStart,
	Win,
	WinEnd,

	Done
}
