using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Enums;
using AppAdvisory.StopTheLock;

public class SignsHolder : MonoBehaviourHelper
{
    public List<DotPosition> signs;
//    public List<LatchScript> latches;
    public Color regColor;
	public Color activeColor;
	public Color bonusRegColor;
	public Color bonusActiveColor;
	public GameObject fireworksPrefab;
	public float timeToMoveSparks = 1.0f;
	public float timeToMoveCounter = 1.5f;
	public GameObject targetForSparksCenter;
	//public GameObject currentTargetForSparks;

	public Color counterIncreaseColor = Color.green;
	public Color counterDecreaseColor = Color.red;

	public List<int> targetSignIndexes = new List<int>();
	public List<int> freeSignIndexes = new List<int>();
	public int targetBonusIndex;
    public int currentIndex = 1;
	public int minSignSize = 50;
	public int maxSignSize = 70;

	public int targetsCount = 1;

    // Use this for initialization
    void Awake()
	{
		//currentTargetForSparks = targetForSparksCenter;
        var signsParent = transform.Find("Signs");
        foreach (Transform dp in signsParent.transform)
        {
            var sign = dp.gameObject.GetComponent<DotPosition>();
            signs.Add(sign);
        }
        PositionSigns();
        PopulateSignsText();
        SetRandomSign();
	}

    void PositionSigns()
    {

        var signsCount = signs.Count;
        var segmentSize = 360 / signsCount;
        for (var i = 1; i < signsCount; i++)
        {
            var sign = signs[i];
            var signVector = new Vector3(0, 0, -segmentSize * (i));
            sign.transform.eulerAngles = signVector;
            sign.transform.Find("Text").eulerAngles = Vector3.zero;
        }
    }

	public void SetRandomSign()
    {
		targetSignIndexes = new List<int> ();
		PopulateFreeSigns ();
        //int index;
		FadeSigns();
		for (int i = 0; i < targetsCount; i++) {
			AddSign ();
		}
		HighlightTargetSigns ();
    }

	void PopulateFreeSigns()
	{
		freeSignIndexes = new List<int> ();
		for (int i = 0; i < signs.Count; i++) {
			freeSignIndexes.Add (i);
		}
	}

	public void AddSign()
	{
		int index;
		bool samePlaceCheck = false; 
		do {
			index = freeSignIndexes[UnityEngine.Random.Range (0, freeSignIndexes.Count)];
			if (targetSignIndexes.Count > 0 && targetSignIndexes.Count < 2) {
				samePlaceCheck = Mathf.Abs (index - targetSignIndexes [0]) <= 1 || Mathf.Abs (index - targetSignIndexes [0]) >= signs.Count;
			}
		} while(samePlaceCheck);
		targetSignIndexes.Add (index);
		freeSignIndexes.Remove (index);
	}

	public void SetRandomBonus ()
	{
		bonusManager.ResetBonuses ();
		int index;
		do
		{
			index = UnityEngine.Random.Range(0, signs.Count);
		} while (CheckForTarget(index) || bonusManager.CheckOnBonus(index));
		bonusManager.InitBonus (index, signs[index].transform.Find("Spotlight").position);
		targetBonusIndex = index;
	}

	public void ChangeToBonus(){
		regColor = bonusRegColor;
		activeColor = bonusActiveColor;
	}

	public void InitFireworks (int index) {
		var spotlight = signs [index].transform.Find ("Spotlight");
		if (spotlight != null) {
			//var fwPrefab = Instantiate(fireworksPrefab, signs[index].transform.Find("Spotlight").transform, false);
			//GameObject.Destroy (fwPrefab, 1);
		} else {
			Debug.LogError ("Spotlight is null");
		}

		StopCoroutine ("CounterChange");

		StartCoroutine (CounterChange (index, counterIncreaseColor, "+1Sec"));
	}

	public void MoveCountTrail (int index, GameObject target = null) {
		StopCoroutine ("CountTrail");
		StartCoroutine (CountTrail(index, target));
	}

	public void DecreaseCounter (int index){
		StartCoroutine (CounterChange(index, counterDecreaseColor, "-1Sec"));
	}

	IEnumerator CounterChange (int index, Color color, string counterText) {
		var parent = signs [index];
		var signText = parent.transform.FindChild ("Text");
		var textPref = parent.transform.FindChild("Counter");
		var textObject = (Transform)Instantiate (textPref, parent.transform);


		textObject.position = signText.position;
		textObject.rotation = signText.rotation;
		var oldPos = textObject.transform.position;

		textObject.gameObject.SetActive (true);
		var text = textObject.GetComponent<Text> ();

		text.text = counterText;
		text.color = color;
		var t = 0.0f;
		while(t < 1)
		{
			t += Time.deltaTime / timeToMoveCounter;
			var curPos = textObject.transform.position;
			textObject.transform.position = new Vector3 (curPos.x, curPos.y + Time.deltaTime * 100, curPos.z);
			var currColor = text.color;
			text.color = Color.Lerp (currColor, new Color(currColor.r, currColor.g, currColor.b, 0), t*0.3f);
			yield return null;
		}
		GameObject.Destroy (textObject.gameObject, 1);
	}

	public IEnumerator CountTrail (int index, GameObject target) {
		if (target == null) {
			target = targetForSparksCenter;
		}
		var trail = signs [index].magicTrail;
		if (trail != null) {
			trail.SetActive (false);
			var signPos = signs [index].transform.Find ("Spotlight").position;
			trail.transform.position = new Vector3 (signPos.x, signPos.y, signPos.z - 200);
			trail.SetActive (true);
			var t = 0f;
			while (t < 1) {
				t += Time.deltaTime / timeToMoveSparks;
				//target - (target - current) * Mathf.Exp(coeff * Time.deltaTime); 
				var val = t * t * t * 5;//Mathf.Exp (t * 1.5f) - 1;
				var curPos = Vector3.Lerp (trail.transform.position, target.transform.position, val);
				//curPos.y += curPos.y * Mathf.Sin(t* Mathf.PI * 0.4f);
				//curPos.x += curPos.x * Mathf.Sin(t* Mathf.PI * 0.3f);
				trail.transform.position = curPos;

				yield return null;
			}
		} else {
			Debug.LogError ("Trail is null");
		}
	}

    public void HighlightCurrent()
    {
        FadeSigns();
		HighlightSign(currentIndex);
    }

    void HighlightTargetSigns()
    {
		foreach(var i in targetSignIndexes) {
			HighlightTargetSign (i);
		}
    }

	void HighlightTargetSign( int index)
	{
		signs [index].transform.Find ("Spotlight").gameObject.SetActive (true);
		//TargetSign.transform.Find("AreaLight").gameObject.SetActive(true);
		var text = signs [index].transform.GetComponentInChildren<Text> ();
		text.color = activeColor;
		text.resizeTextMaxSize = maxSignSize;
	}

    public void HighlightSign(int index)
    {
        var selectedSign = signs[index];
		var text = selectedSign.GetComponentInChildren<Text> ();
		text.color = activeColor;
		text.resizeTextMaxSize = maxSignSize;
        //selectedSign.transform.Find("Spotlight").gameObject.SetActive(true);
    }

	public bool CheckForTarget (int index) 
	{
		return targetSignIndexes.IndexOf(index) >= 0;//targetSignIndexes[0] == index || targetSignIndexes[1] == index;
		//var firstCheck = Mathf.Abs(index - targetSignIndex) <= 1 || Mathf.Abs(index - targetSignIndex) >= signs.Count;
		//var secondCheck = targetSignIndexes[1] >= 0 ? Mathf.Abs (index - targetSignIndex) <= 1 || Mathf.Abs (index - targetSignIndex) >= signs.Count : false;
		//return firstCheck || secondCheck;
	}

    void FadeTargets()
    {
		foreach (var i in targetSignIndexes) {
			signs[i].GetComponentInChildren<Text>().color = regColor;
		}
    }

    public void FadeSigns()
    {
		for (var i = 0; i < signs.Count; i++)
        {			
			var sign = signs [i];
			//if (i != targetBonusIndex) {
				//sign.gameObject.SetActive (true);
			//}
			//if (!CheckForTarget(i)) {
				sign.transform.Find ("Spotlight").gameObject.SetActive (false);
				//sign.transform.Find("AreaLight").gameObject.SetActive (false);
				var text = sign.transform.GetComponentInChildren<Text> ();
				text.color = regColor;
				text.resizeTextMaxSize = minSignSize;
			//}
        }
    }
    void PopulateSignsText()
    {
		List<char> characters = null;
		switch (ProgressManager.progressManager.chestGrade) {
		case ChestGrades.Base:
			characters = new List<char> { 'A', 'C', 'I', 'J', 'K', 'M', 'N', 'O', 'Q', 'R', 'U', 'V', 'W', 'Y', 'Z' };
			break;
		case ChestGrades.Rare:
			characters = new List<char> { 'A', 'C', 'I', 'J', 'K', 'M', 'N', 'O', 'Q', 'R', 'U', 'V', 'W', 'Y', 'Z' };
			break;
		case ChestGrades.Epic:
			characters = new List<char> { 'A', 'C', 'I', 'J', 'K', 'M', 'N', 'O', 'Q', 'R', 'U', 'V', 'W', 'Y', 'Z' };
			break;
		}
		//var characters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

		foreach (var sign in signs)
        {
            var random = Random.Range(0, characters.Count);
            var chr = characters[random];
            sign.GetComponentInChildren<Text>().text = chr.ToString();
            characters.Remove(chr);
        }
    }
}
