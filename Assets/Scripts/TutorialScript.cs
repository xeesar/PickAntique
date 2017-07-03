using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialScript : MonoBehaviour {

	public GameObject[] panels;
	public GameObject questionPanel;

	// Use this for initialization
	void Start () {

	}

	public void ShowPanel(int index){
		for (int i = 0; i < panels.Length; i++) {
			panels [i].SetActive (index == i);
		}
	}

	public void StartTutorial(){
		questionPanel.SetActive (false);
		panels[0].SetActive (true);
	}

	public void Skip(){
	
	}

	public void LoadMain () {
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}

	public void LoadPlaySceneTutorial () {
		ProgressManager.progressManager.chestGrade = Assets.Scripts.Enums.ChestGrades.Base;
		SceneManager.LoadScene("TutorialCore", LoadSceneMode.Single);
	}

	public void LoadPlayScene () {
		SceneManager.LoadScene("NewCoreBaseScene", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
