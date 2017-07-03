using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;

public class ShareScript : MonoBehaviour 
{
	public GameObject button;
	public int gamesNumber;

	//Twitter Share Link
	string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

	//Language
	string TWEET_LANGUAGE = "en";

	//This is the text which you want to show
	string textToDisplay="Hey Guys! Check out my score: ";

	string AppID = "1973282939568281";

	//This link is attached to this post
	string Link = "https://google.com";

	//The URL of a picture attached to this post. The Size must be atleat 200px by 200px.
	string Picture = "http://i-cdn.phonearena.com/images/article/85835-thumb/Google-Pixel-3-codenamed-Bison-to-be-powered-by-Andromeda-OS.jpg";

	//The Caption of the link appears beneath the link name
	string Caption = "Check out My New Score: ";

	//The Description of the link
	string Description = "Enjoy Fun, free games! Challenge yourself or share with friends. Fun and easy to use games.";

	void Start () {
		if (!ProgressManager.progressManager.playerData.Shared) {
			button.SetActive (true);
		}
	}
		
	void Awake()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback, OnHideUnity);
		}
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		else
		{
			Debug.Log("Failed to Initialize the FaceBookSDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	// Facebook Share Button
	public void ShareScoreOnFacebook ()
	{
		if (!FB.IsLoggedIn) {
			FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" }, LogInCallback);
		} else {
			StartCoroutine (ShareImage());
		}
//		Application.OpenURL ("https://www.facebook.com/dialog/feed?" + "app_id=" + AppID + "&link=" + Link + "&picture=" + Picture
//			+ "&caption=" + Caption + ProgressManager.progressManager.playerData.Statistics.BestScoreInGame + "&description=" + Description);
	}

	IEnumerator ShareImage(){
		yield return new WaitForEndOfFrame();

		var width = Screen.width;
		var height = Screen.height;
		var twx = new Texture2D(width, height/2,TextureFormat.RGB24, false);

		twx.ReadPixels(new Rect(0, height/2, width, height), 0, 0);
		twx.Apply();
		byte[] screenshot = twx.EncodeToPNG();

		var wwwForm = new WWWForm();

		wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
		FB.API("me/photos", HttpMethod.POST, Callback, wwwForm);
	}

	private void LogInCallback(IResult result)
	{
		if (!result.Cancelled && string.IsNullOrEmpty (result.Error)) {
			StartCoroutine (ShareImage());
		}
	}
	private void Callback(IResult result)
	{
		if (!result.Cancelled && string.IsNullOrEmpty (result.Error)) {
			ProgressManager.progressManager.playerData.Gems++;
			ProgressManager.progressManager.playerData.Shared = true;
			ProgressManager.progressManager.Save ();
			button.SetActive (false);
		}
	}
}
