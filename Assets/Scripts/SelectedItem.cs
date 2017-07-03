using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;
using SwipeMenu;

public class SelectedItem : MonoBehaviour, IPointerClickHandler {

	public float scaleTime = 8;
	public float timeToMove = 0.5f;

	public Vector3 targetScale = new Vector3(3.2f, 3.2f, 0);
	public Vector3 targetPosition = new Vector3(0, 1.1f,-5);

	public GameObject descriptionText;
	public GameObject back;
	public GameObject image;
	public GameObject[] rateStars;

	private GameObject _item;
	private ItemGrades _selectedGrade;

	Vector3 _targetPosition;
	Vector3 _targetScale;
	Vector3 _originalPosition;
	Vector3 _originalScale;

	public void ShowItem(GameObject item, ItemGrades grade, string description)
	{
		_item = item;
		_selectedGrade = grade;
		image.transform.position = _item.transform.position;

		descriptionText.GetComponentInChildren<Text> ().text = description;

		_originalPosition = _item.transform.position;
		_originalScale = image.transform.localScale;

		gameObject.SetActive (true);

		var spriteRenderer = image.GetComponentInChildren<SpriteRenderer> ();
		spriteRenderer.sprite = _item.GetComponentInChildren<SpriteRenderer> ().sprite;
		PositionItem ();
	}

	void PositionItem ()
	{
		_targetScale = targetScale;
		_targetPosition = targetPosition;
		StartCoroutine (ScaleParent(true));
		StartCoroutine (MoveToCenter());
	}

	void HideItem () 
	{
		DeactivateAttributes ();
		_targetScale = _originalScale;
		_targetPosition = _originalPosition;
		StartCoroutine (ScaleParent(false));
		StartCoroutine (MoveToCenter());

		SwipeHandler.Instance.handleSwitchMenu = true;
	}

	public IEnumerator ScaleParent(bool show)
	{
		var scale = image.transform.localScale;
		var t = 0f;
		while(t < 1)
		{
			t += Time.deltaTime / timeToMove;
			image.transform.localScale = Vector3.Lerp(scale, _targetScale, t);
			yield return null;
		}
		if (show) {
			ActivateAttributes ();
		} else {
			gameObject.SetActive (false);
		}
	}

	void ActivateAttributes () {
		descriptionText.SetActive (true);
		back.SetActive (true);
		for (int i = 0; i <= (int)_selectedGrade; i++) {
			rateStars [i].SetActive (true);
		}
	}

	void DeactivateAttributes () {
		descriptionText.SetActive (false);
		for (int i = 0; i < rateStars.Length; i++) {
			rateStars [i].SetActive (false);
		}
	}

	public IEnumerator MoveToCenter()
	{
		var currentPos = image.transform.position;
		currentPos.z = 0;
		var t = 0f;
		while(t < 1)
		{
			t += Time.deltaTime / timeToMove;
			image.transform.position = Vector3.Lerp(currentPos, _targetPosition, t);
			yield return null;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		HideItem ();
	}
}
