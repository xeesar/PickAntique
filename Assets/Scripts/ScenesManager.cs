using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using AppAdvisory.StopTheLock;
using Assets.Scripts.Enums;

public class ScenesManager : MonoBehaviourHelper {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }


	public void OpenCore () {
		LoadCoreScene();
	}

	private void LoadCoreScene()
	{		
		switch (ProgressManager.progressManager.chestGrade) {
		case ChestGrades.Base: 
			if (ProgressManager.progressManager.playerData.IsFirstRun) {
				StartCoroutine (Fading.fader.LoadScene ("TutorialCore"));
				ProgressManager.progressManager.playerData.IsFirstRun = false;
			} else {
				StartCoroutine (Fading.fader.LoadScene ("NewCoreBaseScene"));
			}
			break;
		case ChestGrades.Rare: 
			StartCoroutine (Fading.fader.LoadScene ("NewCoreRareScene"));
			break;
		case ChestGrades.Epic: 
			StartCoroutine (Fading.fader.LoadScene ("NewCoreEpicScene"));
			break;
		}
    }

    public void WinGenerate()
    {
        ProgressManager.progressManager.generateItems = true;
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
