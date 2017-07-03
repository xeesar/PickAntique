using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace AppAdvisory.StopTheLock
{
	public class TimeBarScript : MonoBehaviourHelper
	{
		public Image CircleImage;
		public Color start;
		public Color end;

		void Start()
		{
			CircleImage.type = Image.Type.Filled;
			CircleImage.fillMethod = Image.FillMethod.Radial360;
			CircleImage.fillOrigin = 0;
		}

		void Update () {
			var value = gameManager.timeLeft / gameManager.totalTimeLeft;
			CircleImage.fillAmount = Mathf.Max( value , 0.001f);
			if (gameManager.timeLeft <= 3) {
				CircleImage.color = end;
			} else {
				CircleImage.color = start;
			}
		}
	}
}