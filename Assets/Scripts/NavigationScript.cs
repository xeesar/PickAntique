using UnityEngine;
using System.Collections;

public class NavigationScript : MonoBehaviour {

	public GameObject pointerImage;
	public float timeToMove;

	public GameObject leftPointerPos;
	public GameObject centerPointerPos;
	public GameObject rightPointerPos;

	private Vector3 currentPointerPos;

	public IEnumerator MoveToPosition(Vector3 position)
	{
		var currentPos = pointerImage.transform.position;
		var t = 0f;
		while(t < 1)
		{
			t += Time.deltaTime / timeToMove;
			var val = Mathf.Exp (t * 1.5f)-1;
			pointerImage.transform.position = Vector3.Lerp(currentPos, position, val);
			yield return null;
		}
	}

	public void MoveToTargetRight (bool value) {
		if(value){
			StopCoroutine ("MoveToPosition");
			StartCoroutine (MoveToPosition(rightPointerPos.transform.position));
		}
	}	
	public void MoveToTargetLeft (bool value) {
		if(value){
			StopCoroutine ("MoveToPosition");
			StartCoroutine (MoveToPosition(leftPointerPos.transform.position));
		}
	}	
	public void MoveToTargetCenter (bool value) {
		if(value){
			StopCoroutine ("MoveToPosition");
			StartCoroutine (MoveToPosition(centerPointerPos.transform.position));
		}
	}	

	public void OnValueChanged (bool value){
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
