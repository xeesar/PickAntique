using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour {
	
	void Start()
	{
		StartCoroutine(Example());
	}

	IEnumerator Example()
	{
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
