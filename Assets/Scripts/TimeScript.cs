using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;
public class TimeScript : MonoBehaviour {

	public bool isServerAvailable = false;

	public string timeUrl = "http://104.199.49.232/game/server/timecontroller.php";

	private static readonly TimeScript instance = new TimeScript();
	private TimeScript() {}

	public static TimeScript Instance
	{
		get 
		{
			return instance;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator GetTimeFromServer(string url, Action<string> onSuccess, Action<string> onError )
	{
		WWW www = new WWW(url);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			isServerAvailable = false;
			onError (www.error);
			//noWifi.SetActive (true);
		} else {
			isServerAvailable = true;
			onSuccess (www.text);		
		}
	}
}
