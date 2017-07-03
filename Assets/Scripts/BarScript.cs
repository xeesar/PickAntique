using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField]
	private float fillAmount;
	[SerializeField]
	private Image PBLine;
	[SerializeField]
	private Text PBText;
	[SerializeField]
	public float CardsValue = 0;

	// Use this for initialization
	void Start () {
		CardsValue = ProgressManager.progressManager.GetCollectionProgress ();
	}
	
	// Update is called once per frame
	void Update () {
		HandleBar ();
	}

	private void HandleBar()
	{
		var text = (int)(CardsValue / 66 * 100);
		PBText.text = text + " %";
		PBLine.fillAmount = Map(CardsValue, 0, 66, 0, 1);
	}

	private float Map(float value, float inMin, float inMax, float outMin, float outMax)
	{
		return ((value - inMin) * (outMax - outMin) / (inMax - inMin)) + outMin;
	}
}
