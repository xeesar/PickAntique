using UnityEngine;
using SwipeMenu;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject OptionsPage1;						//Store a reference to the Game Object MenuPanel 
	public GameObject OptionsPage2;
	//public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 


	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		SwipeHandler.Instance.handleSwitchMenu = false;
		optionsPanel.SetActive(true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel()
	{
		optionsPanel.SetActive(false);
		optionsTint.SetActive(false);
		SwipeHandler.Instance.handleSwitchMenu = true;
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		menuPanel.SetActive (true);
	}

	public void StartTutorialMain()
	{
		SceneManager.LoadScene("TutorialMain", LoadSceneMode.Single);
	}

	public void RestoreNoAd(){
		ProgressManager.progressManager.playerData.IsAdOff = true;
	}

	public void StartTutorialCore()
	{
		ProgressManager.progressManager.chestGrade = Assets.Scripts.Enums.ChestGrades.Base;
		SceneManager.LoadScene("TutorialCore", LoadSceneMode.Single);
	}

	public void ShowTutorialPanel()
	{
		OptionsPage1.SetActive (false);
		OptionsPage2.SetActive (true);
	}
	public void HideTutorialPanel()
	{
		OptionsPage1.SetActive (true);
		OptionsPage2.SetActive (false);
	}
	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}
	
	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		//pausePanel.SetActive (true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		//pausePanel.SetActive (false);
		optionsTint.SetActive(false);

	}
}
