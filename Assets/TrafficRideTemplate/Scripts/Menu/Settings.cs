
// This script used for game settings menu

using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{

	public Toggle Effect,camVibrate;

	public Dropdown qualityDrop;

	public Slider musicVolume, fxVolume ;

	public Dropdown throttleMode;
	public Dropdown steerMode; 

	public InputField cheatBox;

	void Start ()
	{
		
		if (PlayerPrefs.GetInt ("Effect") == 3)
			Effect.isOn = true;
		else
			Effect.isOn = false;

		if (PlayerPrefs.GetInt ("Vibrate") == 3)
			camVibrate.isOn = true;
		else
			camVibrate.isOn = false;
		
		qualityDrop.value =PlayerPrefs.GetInt ("Quality");

		musicVolume.value = PlayerPrefs.GetFloat ("MusicVolume");

		fxVolume.value = PlayerPrefs.GetFloat ("FXVolume");

		steerMode.value = PlayerPrefs.GetInt ("SteerMode");

		throttleMode.value = PlayerPrefs.GetInt ("ThrottleMode");
	}

	public void Set_Effect ()
	{
		StartCoroutine (Effect_Save ());
	}

	public void SetQualityLevel ()
	{
		PlayerPrefs.SetInt ("Quality", qualityDrop.value);
		QualitySettings.SetQualityLevel (qualityDrop.value, false);
	}

	IEnumerator Effect_Save ()
	{
		yield return new WaitForEndOfFrame ();
		if (Effect.isOn)
			PlayerPrefs.SetInt ("Effect", 3);  //3 = true;
		else
			PlayerPrefs.SetInt ("Effect", 0);//0 = false;

	}

	public void Set_CamVibrate ()
	{
		StartCoroutine (Vibrate_Save ());
	}

	IEnumerator Vibrate_Save ()
	{
		yield return new WaitForEndOfFrame ();
		if (camVibrate.isOn)
			PlayerPrefs.SetInt ("Vibrate", 3);  //3 = true;
		else
			PlayerPrefs.SetInt ("Vibrate", 0);//0 = false;

	}

	public void Set_MusicVolume()
	{
		PlayerPrefs.SetFloat ("MusicVolume", musicVolume.value);
	}

	public void Set_FXVolume()
	{
		PlayerPrefs.SetFloat ("FXVolume", fxVolume.value);
	}

	public void SelectSteerMode()
	{
		PlayerPrefs.SetInt ("SteerMode", steerMode.value);

		if(GetComponent<InputSystem> ())
			GetComponent<InputSystem> ().UpdateSettings ();
	}
	public void SelectThrottleMode()
	{
		PlayerPrefs.SetInt ("ThrottleMode", throttleMode.value);
		if(GetComponent<InputSystem> ())
			GetComponent<InputSystem> ().UpdateSettings ();
	}

	public void CheckCheat()
	{
		if (cheatBox.text == "AddScore10000") {
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + 10000);
			GameObject.FindObjectOfType<MainMenu> ().totalCoins.text = PlayerPrefs.GetInt ("Coins").ToString ();
		}
	}

}

