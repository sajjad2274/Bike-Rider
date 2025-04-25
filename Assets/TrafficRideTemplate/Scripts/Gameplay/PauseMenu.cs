//
//  Orginally written by ALIyerEdon - Winter 2017 - Unity 5.4.1 64 bit
//

// Game pause system    

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	// Activate this game object when game is paused
	public GameObject pauseMenu;

	// Garage scene name (car selection) 
	public string garageName = "Garage";

	// Used to disabl pausing on finishing state or car is destroyed    
	[HideInInspector]	public bool isFinished;

	[Header("Settings Menu")]
	public Slider steerSpeed;
	public Text steerValueText;
	public Dropdown throttleMode;
	public Dropdown steerMode; 
	public Toggle useKeyboard;
	CameraShake camShake;


	IEnumerator Start()
	{
		
		steerSpeed.value = PlayerPrefs.GetFloat("SteerSpeed");
		steerValueText.text = steerSpeed.value.ToString(); 

		steerMode.value = PlayerPrefs.GetInt ("SteerMode");
		throttleMode.value = PlayerPrefs.GetInt ("ThrottleMode");

		if (PlayerPrefs.GetInt ("UseKeyboard") == 3)
			useKeyboard.isOn = true;
		else
			useKeyboard.isOn = false;

		GetComponent<InputSystem> ().useKeyboard = useKeyboard.isOn;
		GetComponent<InputSystem> ().UpdateSettings ();

		yield return new WaitForEndOfFrame ();
		camShake = GameObject.FindObjectOfType<CameraShake> ();


	}

	void Update()
	{
		// Pause with escape key (back button in android)
		if (Input.GetKeyDown (KeyCode.Escape))
			Pause ();

		}

	// Public function used in resume ui button
	public void Resume()
	{
		if (PlayerPrefs.GetInt ("Vibrate") == 3)
			camShake.isActive = true;
		else
			camShake.isActive = false;


		GameObject.FindObjectOfType<VehicleAudio>().engineSource.volume = PlayerPrefs.GetFloat("FXVolume") - 0.3f;
		pauseMenu.SetActive (false);
		Time.timeScale = 1f;    
	}

	// Public function used in retry ui button
	public void Retry()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

	}

	// Public function used in exit ui button
	public void Exit()
	{
		Time.timeScale = 1f;
		SceneManager.LoadSceneAsync (garageName);
	}

	// Pause the game
	public void Pause()
	{
		if (!isFinished) 
		{
			camShake.isActive = false;
			GameObject.FindObjectOfType<VehicleAudio>().engineSource.volume = 0;
			pauseMenu.SetActive (true);
			Time.timeScale = 0;
		}


	}

	public void SetTrue(GameObject temp)
	{
		temp.SetActive(true );
	}
	public void SetFalse(GameObject temp)
	{
		temp.SetActive(false);
	}


	public void SetSteerSpeed()
	{
		PlayerPrefs.SetFloat("SteerSpeed",steerSpeed.value);
		GameObject.FindObjectOfType<BikeController>().steerSpeed = steerSpeed.value;
		steerValueText.text = steerSpeed.value.ToString(); 

	}

	public void SelectSteerMode()
	{
		PlayerPrefs.SetInt ("SteerMode", steerMode.value);
		Debug.Log (PlayerPrefs.GetInt ("SteerMode"));
		GetComponent<InputSystem> ().UpdateSettings ();
	}
	public void SelectThrottleMode()
	{
		PlayerPrefs.SetInt ("ThrottleMode", throttleMode.value);
		GetComponent<InputSystem> ().UpdateSettings ();
	}

	public void SetUseKeyboard()
	{
		StartCoroutine (save_UseKeyboard ());
	}

	IEnumerator save_UseKeyboard()
	{
		yield return new WaitForEndOfFrame ();
		if (useKeyboard.isOn)
			PlayerPrefs.SetInt ("UseKeyboard", 3);
		else
			PlayerPrefs.SetInt ("UseKeyboard", 0);

		GetComponent<InputSystem> ().useKeyboard = useKeyboard.isOn;
		GetComponent<InputSystem> ().UpdateSettings ();
	}
}