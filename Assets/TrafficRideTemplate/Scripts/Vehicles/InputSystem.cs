using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ThrottleMode
{
	Auto,
	Manual
}
public enum SteerMode
{
	Accelerometer,
	Buttons,
	SteeringWheel
}

public class InputSystem : MonoBehaviour {


	public ThrottleMode throttleMode;
	public SteerMode steerMode;

	// Catch current spawned car controller from scene
	BikeController bikeRider;

	// Wheelie input controller
	Wheelie wheelie;

	// Automaticlly switch between keyboard and touch in target device
	public bool useKeyboard;

	public GameObject throttleRight,throttleSide,steerButtons,steeringWheel,controlsParent;

	bool isBraking;

	AudioSource hornSource;

	IEnumerator Start () {


		// Wait a frame to car has been spawned
		yield return new WaitForEndOfFrame ();

		// Find car controller based on his type    (We have only one RacerController in the scene in the same time    )    
		bikeRider = GameObject.FindObjectOfType<BikeController> ();

		wheelie = GameObject.FindObjectOfType<Wheelie> ();

		UpdateSettings ();

		hornSource = gameObject.AddComponent<AudioSource> ();
		hornSource.loop = false;
		hornSource.spatialBlend = 0;
		hornSource.playOnAwake = false;
		hornSource.volume = PlayerPrefs.GetFloat ("FXVolume");
		hornSource.clip = bikeRider.transform.GetComponent<VehicleAudio>().horn;

	}   

	void Update ()
	{

		if (!bikeRider) {
			return;

		}
		
		// Read keyboard inputs + default motor power 
		GetInput ();

	}

	void GetInput()
	{
		// Motor input is always 1 (Always in max input, Always is running by default with low speed    ) 
		bikeRider. motorInput = 1f;

		// Read keyborad input values
		if (useKeyboard) {
			bikeRider.steerInput = Input.GetAxis ("Horizontal") * Time.deltaTime * bikeRider.steerSpeed * 2;

			// Control throttling (mobile)
			if (throttleMode == ThrottleMode.Auto) {
				if (!isBraking)
					bikeRider.isThrottle = true;
				else
					bikeRider.isThrottle = false;
			} else {
				if (Input.GetKey (KeyCode.W))
					Throttle (true);

				if (Input.GetKeyUp (KeyCode.W))
					Throttle (false);
			}
			if (Input.GetKey (KeyCode.S))
				Brake (true);
			
			if (Input.GetKeyUp (KeyCode.S))
				Brake (false);

			if (Input.GetKeyDown (KeyCode.LeftShift))
				StartWheelie ();

			if (Input.GetKey (KeyCode.Space))
				Horn (true);

			if (Input.GetKeyUp (KeyCode.Space))
				Horn (false);
		} else {

			// Controll steering (mobile)
			if (steerMode == SteerMode.Accelerometer) {
				if (Input.acceleration.x > 0.2f || Input.acceleration.x < -0.2f) {
					bikeRider.steerInput = Input.acceleration.x * Time.deltaTime * bikeRider.steerSpeed;
				} else {
					bikeRider.steerInput = 0;
				}
			}

			// Control throttling (mobile)
			if (throttleMode == ThrottleMode.Auto) {
				if (!isBraking)
					bikeRider.isThrottle = true;
				else
					bikeRider.isThrottle = false;
			}
		}
	}

	// Used for mobile touch controlls
	public void Steer(float value)
	{
		if(steerMode == SteerMode.Buttons || steerMode == SteerMode.SteeringWheel)
			bikeRider. steerInput = Mathf.Lerp (bikeRider. steerInput, value, Time.deltaTime * bikeRider. steerSpeed );
	}

	// Used for mobile touch control
	public void SteerUp()
	{
			bikeRider. steerInput = 0 ;
	}


	public void Throttle(bool state)
	{
			bikeRider.isThrottle = state;
	}

	public void Brake(bool state)
	{
		bikeRider.isBrake = state;
		isBraking = state;
	}

	public void StartWheelie()
	{
		wheelie.WheelieInput ();
	}



	public void Horn(bool state)
	{
		if (state) {
			if (!hornSource.isPlaying)
				hornSource.Play ();
		} else {


			if (hornSource.isPlaying)
				hornSource.Stop ();
		}

	}
	public void UpdateSettings()
	{
		// Load last steer mode that's selected from settings menu    
		if (PlayerPrefs.GetInt ("SteerMode") == 0)
			steerMode = SteerMode.Accelerometer;
		if (PlayerPrefs.GetInt ("SteerMode") == 1)
			steerMode = SteerMode.Buttons;
		if (PlayerPrefs.GetInt ("SteerMode") == 2)
			steerMode = SteerMode.SteeringWheel;

		// Load last throttle mode that's selected from settings menu 
		if (PlayerPrefs.GetInt ("ThrottleMode") == 0)
			throttleMode = ThrottleMode.Auto;
		if (PlayerPrefs.GetInt ("ThrottleMode") == 1) {
			throttleMode = ThrottleMode.Manual;
			if(bikeRider)
				bikeRider.isThrottle = false;
		}

		if (useKeyboard) {
			controlsParent.SetActive (false);
		} else {
			controlsParent.SetActive (true);
			if (steerMode == SteerMode.Accelerometer) {
				steerButtons.SetActive (false);
				steeringWheel.SetActive (false);
				throttleRight.SetActive (false);
				throttleSide.SetActive (true);
			} 
			if (steerMode == SteerMode.Buttons) {
				steerButtons.SetActive (true);
				steeringWheel.SetActive (false);
				throttleRight.SetActive (true);
				throttleSide.SetActive (false);
			}
			if (steerMode == SteerMode.SteeringWheel) {
				steerButtons.SetActive (false);
				steeringWheel.SetActive (true);
				throttleRight.SetActive (true);
				throttleSide.SetActive (false);
			}
		}
	}
}