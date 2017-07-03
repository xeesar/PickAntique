﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;	
	public float fadeSpeed = 0.8f;		
	public float waitTime = 0.2f;
	public Font font;

	private int drawDepth = -1000;		
	private float alpha = 1.0f;			
	private int fadeDir = -1;			


	public static Fading fader;

	void Awake()
	{
		if (fader == null)
		{
			fader = this;
		}
	}

	void OnGUI()
	{
		GUI.skin.font = font;
		GUI.skin.label.fontSize = 100;
		// fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		// set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;																// make the black texture render on top (drawn last)
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);		// draw the texture to fit the entire screen area
		GUI.Label(new Rect(Screen.width/2 - 220, Screen.height/2 - 50, 500, 100), "Loading...");
	}

	// sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
	public float BeginFade (int direction)
	{
		fadeDir = direction;
		return (waitTime);
	}

	public IEnumerator LoadScene (string sceneName, int wait = 0) {
		var speed = Fading.fader.BeginFade (1);
		yield return new WaitForSeconds(speed + wait);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	// OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
	void OnLevelWasLoaded(int i)
	{
		// alpha = 1;		// use this if the alpha is not set to 1 by default
		BeginFade(-1);		// call the fade in function
	}
}