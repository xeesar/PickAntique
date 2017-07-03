﻿
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;


namespace AppAdvisory.StopTheLock
{
	public class ScreenShake : MonoBehaviour
	{
		private static Vector3 originPosition;
		private static Quaternion originRotation;

		private static float shakeDecay = 0.002f;
		private static float shakeIntensity;

		public static bool isShaking = false;

		public static IEnumerator Shake(Transform t){

			isShaking = true;


			originPosition = t.position;
			originRotation = t.rotation;
			shakeIntensity = 0.3f;
			while (shakeIntensity > 0) {
				t.position = originPosition + Random.insideUnitSphere * shakeIntensity;
				t.rotation = new Quaternion (
					originRotation.x + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.y + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.z + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.w + Random.Range (-shakeIntensity, shakeIntensity) * .2f);
				shakeIntensity -= shakeDecay;
				yield return false;
			}


			isShaking = false;
		}

		public static IEnumerator Shake(Transform t, float i){

			isShaking = true;


			originPosition = t.position;
			originRotation = t.rotation;
			shakeIntensity = i;
			while (shakeIntensity > 0) {
				t.position = originPosition + Random.insideUnitSphere * shakeIntensity;
				t.rotation = new Quaternion (
					originRotation.x + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.y + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.z + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
					originRotation.w + Random.Range (-shakeIntensity, shakeIntensity) * .2f);
				shakeIntensity -= shakeDecay;
				yield return false;
			}

			isShaking = false;
		}
	}
}