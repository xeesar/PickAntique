using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {

	public AudioMixer mainMixer;					//Used to hold a reference to the AudioMixer mainMixer

	private string _musicString = "musicVol";
	private string _sfxString = "sfxVol";

	public Slider sliderMusic;
	public Slider sliderEffects;


	//Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
	public void SetMusicLevel(float musicLvl)
	{
		mainMixer.SetFloat(_musicString, musicLvl);
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
	public void SetSfxLevel(float sfxLevel)
	{
		mainMixer.SetFloat(_sfxString, sfxLevel);
	}

	public float GetSfxLevel(){
		float level;
		mainMixer.GetFloat (_sfxString, out level);
		return level;
	}

	public float GetMusicLevel(){
		float level;
		mainMixer.GetFloat (_musicString, out level);
		return level;
	}

	public void Start(){
		var sliders = transform.GetComponentsInChildren<Slider> ();
		var fxlvl = GetSfxLevel ();
		var mlvl = GetMusicLevel ();
		sliderEffects.value = fxlvl;
		sliderMusic.value = mlvl;

	}
	public void ResetSettings(){
		var sliders = transform.GetComponentsInChildren<Slider> ();//("SfxVolSliderOptions");
		//sfx.GetComponent<Slider> ().value = 0;
		sliders[0].value = 0;
		sliders [1].value = 0;

	}
}
