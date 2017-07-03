using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;

public class UIScript : MonoBehaviour
{

	public Text userGoldText;

	void OnGUI()
	{
		userGoldText.text = ProgressManager.progressManager.playerData.Gold.ToString();
	}

	// Update is called once per frame
	void Update()
	{
	}
}