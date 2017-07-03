using UnityEngine;
using System.Collections;

public class PopUpPanelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowPanel () {
		this.gameObject.SetActive (true);
	}

	public void HidePanel () {
		this.gameObject.SetActive (false);
	}
}
