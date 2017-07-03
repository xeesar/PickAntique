using UnityEngine;
using System.Collections;
using Assets.Scripts.Models;
using UnityEngine.EventSystems;
using System;

public class CollectionItem : MonoBehaviour,
IPointerClickHandler
{
    private ItemModel itemData;
	public GameObject[] progresses;
	public delegate void OnTouchedDelegate( GameObject touchedObject );

	OnTouchedDelegate touchDelegate;
	public OnTouchedDelegate TouchDelegate
	{
		set { touchDelegate = value; }
	}

    // Use this for initialization
    void Start () {
	}

	// Update is called once per frame
	void Update () {
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		touchDelegate (gameObject);
	}

}
