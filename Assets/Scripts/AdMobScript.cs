using UnityEngine;
using System.Collections; 
using admob;

public class AdMobScript : MonoBehaviour {

	public static AdMobScript Instance {set; get;}

	void Awake () {
		
	}
	// Use this for initialization
	void Start () {
		Instance = this;
		#if UNITY_ANDROID
		Admob.Instance().initAdmob("ca-app-pub-4244949877462631/5699246103", "ca-app-pub-4244949877462631/8551320901");
		#elif UNITY_IOS
		Admob.Instance().initAdmob("ca-app-pub-4244949877462631/9670588509", "ca-app-pub-4244949877462631/5862510906");
		#endif
		Admob.Instance().setTesting (false);
	}

	public void ShowBanner () {
		if (!ProgressManager.progressManager.playerData.IsAdOff) {
			Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.BOTTOM_CENTER, 0);
		}
	}

	public void HideBanner () {
		Admob.Instance().removeBanner();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
