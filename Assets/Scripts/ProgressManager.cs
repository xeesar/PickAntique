using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Models;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Linq;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
	private const string UserDataUrl = "/playerInfoV0500.dat";
    private List<int> availableIds = new List<int>();

	public LoggingService logger;

    public delegate void UpdatedEventHandler(ItemProgressModel items);
    public event UpdatedEventHandler OnUpdateCollection;
    //public delegate void GenerateEventHandler(ItemProgressModel item);
    //public event GenerateEventHandler OnItemGenerated;
    public static ProgressManager progressManager;
    public bool loaded = false;
    public string defaultCollectionName = "egypt";
    public int cardsInCollection = 22;

	public ChestGrades chestGrade = ChestGrades.Base;

    //public delegate void LoadEvent();
    //public static event LoadEvent OnLoaded;
    public WinResults winResults;
    public bool generateItems;

	public string dateTimeFormat = "yyyy-MM-dd HH:mm";
	public float[] currentTimers;

    public PlayerData playerData;

    void Awake()
    {
        if (progressManager == null)
        {
            DontDestroyOnLoad(gameObject);
            progressManager = this;
        }
        else if (progressManager != this)
        {
            Destroy(gameObject);
		}
		Load();
    }

	public int GetCollectionProgress () {
		var totalCardsCount = 0;
		foreach(var item in playerData.Collections[0].items){
			totalCardsCount += (int)item.grade + 1;
		}
		return totalCardsCount;
	}

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        //userGoldText.text = "Gold: " + playerData.gold;
        //GUI.Label(new Rect(10, 10, 100, 30), "Gold: " + playerData);
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void ResetTimers(ChestGrades grade){
		if (grade == ChestGrades.Rare) {
			StartCoroutine(TimeScript.Instance.GetTimeFromServer(TimeScript.Instance.timeUrl, ResetRareTimer, OnError));
		} else if (grade == ChestGrades.Epic) {
			StartCoroutine(TimeScript.Instance.GetTimeFromServer(TimeScript.Instance.timeUrl, ResetEpicTimer, OnError));
		}

	}

	void OnError(string error){
	}

	public void ResetRareTimer(string timeString){
		var rareTimerDate = DateTime.ParseExact (timeString, dateTimeFormat, CultureInfo.InvariantCulture);
		rareTimerDate = rareTimerDate.AddMinutes (60*4+1);
		playerData.RareTimer = rareTimerDate.ToString (dateTimeFormat, CultureInfo.InvariantCulture);
		Save ();
	}

	public void ResetEpicTimer(string timeString){
		var epicTimerDate = DateTime.ParseExact (timeString, dateTimeFormat, CultureInfo.InvariantCulture);
		epicTimerDate = epicTimerDate.AddMinutes (60*12+1);
		playerData.EpicTimer = epicTimerDate.ToString (dateTimeFormat, CultureInfo.InvariantCulture);
		Save ();
	}

	public float[] SetTimers(string timeString){
		var result = new float [2];
		var timerRareString = playerData.RareTimer;
		var currentDateTime = DateTime.ParseExact (timeString, dateTimeFormat, CultureInfo.InvariantCulture);
		if (timerRareString == null) {
			//var nextOpen = currentDateTime.AddHours (4);
			var nextOpen = currentDateTime.AddMinutes(60*4+1);
			playerData.RareTimer = nextOpen.ToString (dateTimeFormat, CultureInfo.InvariantCulture);
			result [0] = (float)(nextOpen - currentDateTime).TotalSeconds;
		} else {
			var timerRare = DateTime.ParseExact (timerRareString, dateTimeFormat, CultureInfo.InvariantCulture);
			result [0] = (float)(timerRare - currentDateTime).TotalSeconds;
		}

		var timerEpicString = playerData.EpicTimer;
		if (timerEpicString == null) {
			//var nextOpen = currentDateTime.AddHours (12);
			var nextOpen = currentDateTime.AddMinutes(60*12+1);
			playerData.EpicTimer = nextOpen.ToString (dateTimeFormat, CultureInfo.InvariantCulture);
			result [1] = (float)(nextOpen - currentDateTime).TotalSeconds;
		} else {
			var timerEpic = DateTime.ParseExact (timerEpicString, dateTimeFormat, CultureInfo.InvariantCulture);
			result [1] = (float)(timerEpic - currentDateTime).TotalSeconds;
		}
		Save ();
		currentTimers = result;
		return result;
	}

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + UserDataUrl))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + UserDataUrl, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(fs);
            fs.Close();          

            //TODO handle corrupted load file
            this.playerData = data;
        }
        else
        {			
            playerData = InitPlayerData();
			Save();
			LoggingService.Instance.FirstLogging ();
			SceneManager.LoadScene("TutorialMain", LoadSceneMode.Single);
        }
        loaded = true;
    }

	List<int> GetAvailableIds () {
		var availableIds = InitAvailableIds();
		var ids = availableIds.Except(this.playerData.Collections[0].items.Where(i => i.grade >= ItemGrades.High).Select(i=>i.id)).ToList();
		return ids;
	}

	List<int> InitAvailableIds ()
	{
		int lowBorder = 0, highBorder = 0;
		switch (winResults.ChestGrade) {
		case ChestGrades.Base:
			highBorder = 11;
			break;
		case ChestGrades.Rare: 
			lowBorder = 12;
			highBorder = 17;
			break;
		case ChestGrades.Epic:
			lowBorder = 18;
			highBorder = 21;
			break;
		}
        var availableIds = new List<int>();
		for (int i = lowBorder; i <= highBorder; i++)
        {
            availableIds.Add(i);
        }
		return availableIds;
    }

    PlayerData InitPlayerData()
    {
        var data = new PlayerData();
		data.Collections = new List<CollectionProgressModel> {
			new CollectionProgressModel(defaultCollectionName)
		};
        data.Gems = 0;
		data.Gold = 0;
		data.IsFirstRun = true;
		data.Shared = false;
		data.IsAdOff = false;
		data.Statistics = new Statistics ();
		data.Statistics.BestScoreInGame = 0;

        //InitAvailableIds();
        return data;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + UserDataUrl);

        bf.Serialize(fs, playerData);
        fs.Close();
    }

    public void Reset()
    {
        this.playerData = InitPlayerData();
        OnUpdateCollection(null);
    }

    //public void UpdateCollectionProgress(CollectionProgressModel collectionProgress)
    //{
    //    playerData.collection = collectionProgress;
    //}

    public ItemProgressModel GetNewItemProgress()
    {
		var availableIds = GetAvailableIds ();
        if (availableIds.Count <= 0)
        {
            return null;
        }
        var progressModel = ProgressManager.progressManager.playerData.Collections[0];
//        List<int> progressHighIds = new List<int>();
//        if (progressModel != null)
//        {
//            var progressHighItems = progressModel.items.Where(i => i.grade == ItemGrades.High).ToList();
//            if (progressHighItems.Count() > 0)
//            {
//                progressHighIds = progressHighItems.Select(i => i.id).ToList();
//            }
//
//        }
        var index = UnityEngine.Random.Range(0, availableIds.Count());
        Debug.Log(index);
        var newItemId = availableIds[index];
        Debug.Log(newItemId);

        ItemProgressModel currentProgress = null;
        var grade = ItemGrades.Low;
        //TODO: handle many collections set active, etc.
        currentProgress = progressModel.items.FirstOrDefault(i => i.id == newItemId);
        if (currentProgress != null)
        {
            grade = ++currentProgress.grade;
			if (grade == ItemGrades.High)
            {
                availableIds.RemoveAll(i => i == newItemId);
                Debug.Log("remove" + newItemId);
            }
        }
        else
        {
            Debug.Log("new" + newItemId);
            currentProgress = new ItemProgressModel
            {
                id = newItemId,
                grade = grade
            };
            progressModel.items.Add(currentProgress);
        }
        ProgressManager.progressManager.Save();
        //if (OnUpdateCollection != null)
        //{
        //    OnUpdateCollection(new List<ItemProgressModel> { currentProgress });
        //}
        // OnUpdateCollection(currentProgress);
        return currentProgress;
    }

    public void Unsubscribe()
    {
        if (OnUpdateCollection != null)
            foreach (var d in OnUpdateCollection.GetInvocationList())
                OnUpdateCollection -= (d as UpdatedEventHandler);
    }

}
