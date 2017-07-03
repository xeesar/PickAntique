using UnityEngine;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class SoundManager : MonoBehaviourHelper 
	{
		public AudioSource audioSourceIntro;
		public AudioSource audioSourceLoop;
		public AudioSource audioSourceEffects;

		private bool startedLoop;

		public AudioClip soundSuccess;
		public AudioClip soundFail;
		public AudioClip soundTouch;
		public AudioClip soundBonus;
		public AudioClip soundSuperBonus;

		public void PlaySuccess()
		{
			audioSourceEffects.PlayOneShot (soundSuccess,1f);
		}

		public void PlayFail()
		{
			audioSourceEffects.PlayOneShot (soundFail,1f);
		}

		public void PlayBonus()
		{
			audioSourceEffects.PlayOneShot (soundBonus,1f);
		}

		public void PlaySuperBonus()
		{
			audioSourceEffects.PlayOneShot (soundSuperBonus,1f);
		}

		public void PlayTouch()
		{
			audioSourceEffects.PlayOneShot (soundTouch,1f);
		}

		void FixedUpdate()
		{
			if (!audioSourceIntro.isPlaying && !startedLoop)
			{
				audioSourceLoop.Play();
				Debug.Log("Done playing");
				startedLoop = true;
			}
		}
	}
}