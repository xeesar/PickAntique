using UnityEngine;
using Assets.Scripts.Enums;
using Assets.Scripts;
using Assets.Scripts.Models;
using SwipeMenu;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Collection : MonoBehaviour
{

    // private CollectionService _collectionService;
    private CollectionProgressModel _collectionProgress;
    private List<GameObject> _itemsObjects;
	public SelectedItem selectedItem;


    //public GameObject _itemToShow = null;

    public static bool itemSelected = false;

    //public GameObject selectedItemPrefab;
    //public GameObject selectedItemPlaceholder;

    // Use this for initialization
    void Awake()
    {
        //_collectionService = new CollectionService();
        _itemsObjects = new List<GameObject>();
        foreach (Transform itemTransform in transform)
        {
            _itemsObjects.Add(itemTransform.gameObject);
			var script = itemTransform.gameObject.GetComponent<CollectionItem> ();
			if(script != null){
				script.TouchDelegate = ShowSelectedItem;
			}
        }
    }

	void ShowSelectedItem (GameObject item) {
		SwipeHandler.Instance.handleSwitchMenu = false;
		var index = _itemsObjects.IndexOf (item);
		var itemProgress = _collectionProgress.items.FirstOrDefault (i => i.id == _itemsObjects.Count - index - 1);
		if (itemProgress != null) {
			var itemData = CollectionService.Instance.GetItemFromCollection (_itemsObjects.Count - index - 1);
			selectedItem.ShowItem (item, itemProgress.grade, itemData.grades[(int)itemProgress.grade].description);			
		}
	}

    void Start()
    {
        if (!ProgressManager.progressManager.loaded)
        {
            ProgressManager.progressManager.Load();
        }
        ProgressManager.progressManager.OnUpdateCollection += OnUpdated;
		InitCollection();			
		SetItemsNames ();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnUpdated(ItemProgressModel item)
    {
        if (item == null)
        {
            ClearItemsObjects();
        }
        else
        {
            InitItem(item.id, item.grade);
        }
        _collectionProgress = ProgressManager.progressManager.playerData.Collections[0];
    }

    public void InitCollection()
    {
        ClearItemsObjects();
        _collectionProgress = ProgressManager.progressManager.playerData.Collections[0];
        CollectionService.Instance.InitCollectionData(_collectionProgress.name);

        foreach (var item in _collectionProgress.items)
        {
            InitItem(item.id, item.grade);
        }
    }

    void ClearItemsObjects()
    {
        foreach (var item in _itemsObjects)
        {
            SetItemImage(item, null);

			var script = item.gameObject.GetComponent <CollectionItem>();
			if (script != null) {
				script.enabled = false;
			}
        }
    }

    void InitItem(int itemId, ItemGrades grade)
    {
		var script = _itemsObjects[_itemsObjects.Count - itemId - 1].GetComponent <CollectionItem>();
		if (script != null) {
			script.enabled = true;
			switch (grade) {
			case ItemGrades.Low:
				script.progresses [0].SetActive (true);
				break;
			case ItemGrades.Medium:
				script.progresses [1].SetActive (true);
				break;
			case ItemGrades.High:
				script.progresses [2].SetActive (true);
				script.progresses [3].SetActive (true);
				break;
			}
		}
        var sprite = GetItemImage(itemId, grade);
		var item = _itemsObjects [_itemsObjects.Count - itemId - 1];
		SetItemImage(item, sprite);
		//SetItemName (item, itemId);
    }

    Sprite GetItemImage(int itemId, ItemGrades grade)
    {
        return Resources.Load<Sprite>(CollectionService.Instance.GetBaseImageUrl() +
            CollectionService.Instance.GetItemImage(itemId, grade));
    }

    void SetItemImage(GameObject item, Sprite sprite)
    {
        //var itemSprite = item.transform.Fin("ItemSprite");
        var renderer = item.GetComponentsInChildren<SpriteRenderer>()[0];
        renderer.sprite = sprite;
    }

	void SetItemName(GameObject item, int itemId){
		var itemaData = CollectionService.Instance.GetItemFromCollection (itemId);
		var textComponent = item.transform.FindChild("Name").GetComponent<Text>();
		textComponent.text = itemaData.name;
	}

	void SetItemsNames(){
		var names = CollectionService.Instance.GetItemsNames();
		foreach (var item in _itemsObjects) {
			var textComponent = item.transform.FindChild("Name").GetComponent<Text>();
			textComponent.text = names[_itemsObjects.Count - _itemsObjects.IndexOf(item) - 1];
		}
	}
}