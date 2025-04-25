using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsLoader : MonoBehaviour 
{

	[Header("Modify script to activate image effect support")]

	public AudioSource musicSource;
	public Toggle musicToggle;

	void Start ()
	{
		Camera.main.aspect = 16f/9f;

		if (PlayerPrefs.GetInt ("Music") == 3)
			musicToggle.isOn = true;
		else
			musicToggle.isOn = false;

		if (musicToggle.isOn) {
			
			if (!musicSource.isPlaying)
				musicSource.Play ();
		} else {
			
			if (musicSource.isPlaying)
				musicSource.Pause ();
		}




		musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");

		/*
		if (PlayerPrefs.GetInt ("Effect") != 3)
			Camera.main.GetComponent<AmplifyColorEffect> ().enabled = false;
		else
			Camera.main.GetComponent<AmplifyColorEffect> ().enabled = true;
*/

	}

	public void toggleMusic()
	{
		StopCoroutine ("chekcMusic");
		StartCoroutine ("chekcMusic");
	}

	IEnumerator chekcMusic()
	{
		yield return new WaitForEndOfFrame ();
		if (musicToggle.isOn) {
			PlayerPrefs.SetInt ("Music", 3);
			if (!musicSource.isPlaying)
				musicSource.Play ();
		} else {
			PlayerPrefs.SetInt ("Music", 0);
			if (musicSource.isPlaying)
				musicSource.Pause ();
		}
	}
}