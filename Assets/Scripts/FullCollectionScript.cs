using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FullCollectionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadMain () {
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}
}
