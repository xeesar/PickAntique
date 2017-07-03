
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class ColorManager : MonoBehaviour 
	{
		public Color[] colors;

		public Image m_background;
		public Image m_clock;
		public Image m_lock;

		Color color;

		public float timeChangeColor = 10;

		void OnEnable()
		{
			//m_background.color = GetLastColor ();
			UpdateCircleColor ();
		}

		void OnDisable(){
			StopAllCoroutines ();
			SaveLastColor ();
			PlayerPrefs.Save ();
		}

		Color GetLastColor()
		{
			if (PlayerPrefs.HasKey ("COLOR_R")) 
			{
				float r = PlayerPrefs.GetFloat ("COLOR_R");
				float g = PlayerPrefs.GetFloat ("COLOR_G");
				float b = PlayerPrefs.GetFloat ("COLOR_B");

				Color c = new Color (r, g, b, 1f);

				return c;
			}

			return colors [0];
		}

		void SaveLastColor()
		{
			Color c = m_background.color;

			float r = c.r;
			float g = c.g;
			float b = c.b;

			PlayerPrefs.SetFloat ("COLOR_R", r);
			PlayerPrefs.SetFloat ("COLOR_G", g);
			PlayerPrefs.SetFloat ("COLOR_B", b);
		}


		public void ChangeColor()
		{
			Color colorTemp = colors [UnityEngine.Random.Range (0, colors.Length)];

			StartCoroutine(DoLerp(m_background.color, colorTemp, 1f));
		}


		public IEnumerator DoLerp(Color from, Color to, float time)
		{
			float timer = 0;
			while (timer <= time)
			{
				timer += Time.deltaTime;
				m_background.color = Color.Lerp(from, to, timer / time);
				SaveLastColor ();
				UpdateCircleColor ();
				yield return null;
			}
			m_background.color = to;
			UpdateCircleColor ();
			PlayerPrefs.Save ();
		}

		void UpdateCircleColor()
		{
			Color c = m_background.color;

			Color temp = new Color (c.r / 2f, c.g / 2f, c.b / 2f, 1f);
			Color temp2 = new Color (c.r / 2f, c.g / 2f, c.b / 2f, 0.6f);

			m_clock.color = temp;
			//m_lock.color = temp2;
		}



	}
}