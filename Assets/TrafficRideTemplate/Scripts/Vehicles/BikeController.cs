//
//  Orginally written by ALIyerEdon - Spring 2017 - Unity 5.5 64 bit
//

// Used to controll the player car

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BikeController : MonoBehaviour {


	[Space(3)]
	[Header("Vehicle Setup")]
	public float maxPower;
	public float maxSteer;
	public float steerSpeed = 30f;
	public float distanceFromRoad = 0.43f;

	[HideInInspector] public  float speedFactor;
	public Transform COM;
	Rigidbody rigid;
	float maxPowerTemp;
	Camera mainCamera;

	// Private values o store input values for stter and throttle
	[HideInInspector]public float steerInput;
	[HideInInspector]public float motorInput;
	[HideInInspector]public bool Boost;
	[HideInInspector]public float currentSpeed;

	public float moveSpeed;
	float moveSpeedTemp;
	public bool isWheelie;
	[Header("Visuals")]	
	public Transform carBody;
	public float leanValue = 0.43f;
	public float leanSpeed = 4.3f;
	Quaternion rotationTemp;
	float accelInput;
	public TextMesh speedText;
	[HideInInspector]  public Transform carParent;

	[Header("Animations")]	
	public Animation handANim;
	public float animDelay = 1f;
	public float animSpeed = 1f;

	// Current speed without rigidbody
	Vector3 previous;
	float velocity;

	GameManager manager;

	[HideInInspector] public bool isThrottle;
	[HideInInspector] public bool isBrake;

	public Transform parentCar;

	bool isAccel;

	// distance passed
	float distanceTravelled = 0;
	Vector3 lastPosition;

	void Start () 
	{
		// Calculate distance passed by player   
		lastPosition = transform.position;

		if (GameObject.FindObjectOfType<InputSystem> ().steerMode != SteerMode.Accelerometer) {
			accelInput = 1f;
			isAccel = false;
		} else
			isAccel = true;
		
		carParent = transform;

		steerSpeed = PlayerPrefs.GetFloat("SteerSpeed");

		handANim["Throttle"].speed = animSpeed;

		mainCamera = Camera.main;

		rigid = GetComponent<Rigidbody> ();

		rigid.centerOfMass = COM.localPosition;

		moveSpeedTemp = moveSpeed;

		rotationTemp = rigid.transform.rotation;

		manager = GameObject.FindObjectOfType<GameManager>();

	}

	void Update () 
	{

		distanceTravelled += Vector3.Distance(transform.position, lastPosition);
		lastPosition = transform.position;

		if (isAccel)
			accelInput = Input.acceleration.x;

		if (transform.position.x != distanceFromRoad)
			transform.position = new Vector3 (transform.position.x, distanceFromRoad, transform.position.z);
		
		manager.currentSpeedText.text = Mathf.Floor(currentSpeed).ToString() + " KMH";
		if(distanceTravelled>1000)
			manager.distanceText.text  = Mathf.Floor(distanceTravelled/1000).ToString() + "." + Mathf.Floor(distanceTravelled/10).ToString().Substring(1) + " KM" ;
		else
			manager.distanceText.text  = "0" + "." + Mathf.Floor(distanceTravelled).ToString() + " KM" ;
		
		speedText.text = Mathf.Floor(currentSpeed).ToString() + " KM ";  


		if (Boost)
			rigid.AddForce (rigid.transform.forward * moveSpeedTemp * 2 * Time.deltaTime);
		 else 
			rigid.AddForce (rigid.transform.forward * (moveSpeed+speedFactor) * Time.deltaTime * 10);

		if (steerInput > 0) 
		{
			if (!isWheelie)
				carBody.Rotate(new Vector3(0, 0, -leanValue*Mathf.Abs(accelInput)), Space.Self);

			rigid.AddForce (rigid.transform.right * (430*steerInput)*10  * Time.deltaTime * 10000);

		}

		if (steerInput < 0)
		{
			if (!isWheelie) 
				carBody.Rotate(new Vector3(0, 0, leanValue*Mathf.Abs( accelInput)), Space.Self);

				rigid.AddForce (rigid.transform.right * (430 * steerInput) * 10 * Time.deltaTime * 10000);


		}

		rigid.transform.rotation = rotationTemp;

		if (!isWheelie) {
			carBody.transform.rotation = Quaternion.Lerp (carBody.transform.rotation, parentCar.rotation, Time.deltaTime * leanSpeed);
		}
		if (isThrottle) // Player pressed throttle button
		{
			if (speedFactor < moveSpeedTemp * 10)
				speedFactor = speedFactor + 100 * Time.deltaTime * 3000;

			handANim["Throttle"].wrapMode = WrapMode.ClampForever;
			handANim.CrossFade("Throttle");
		} 
		else // is not throttle has been pressed
		{
			if (isBrake)// Braking button is held down
			{
				if (speedFactor > moveSpeedTemp) 
					speedFactor = speedFactor - 1000 * Time.deltaTime * 3000;
			}
			else // Idle state (is not thrrotle or brake button helt down)
			{
				if (speedFactor > moveSpeedTemp) 
					speedFactor = speedFactor - 74 * Time.deltaTime * 743;

			}
		}

		if (distanceTravelled >= manager.levelDistances[manager.currentLevel])
		{
			distanceTravelled = 0; // Reset for the next level
		LevelComplete();
		}
	}
	public void LevelComplete()
	{
		Time.timeScale = 0;
		manager.levelCompletePanel.SetActive(true);
	}
	// Calculate current speed without rigidbody (is smoother than rigidbody in this controller  )    
	void FixedUpdate()
	{
		currentSpeed = ((new Vector3(0,0,transform.position.z)) - (new Vector3(0,0,previous.z))).magnitude / Time.deltaTime;
		previous = transform.position;

	}

	// Calculate camera field of view in boost on and off
	void CameraFOV()
	{
		if(Boost)
			mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView, 67f, Time.deltaTime * 0.1f );
		else
			mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView, 60f, Time.deltaTime * 1f);
	}

	public void ChangeGearAnimation()
	{
		StopCoroutine("ChangeAnim");
		StartCoroutine("ChangeAnim");
	}

	IEnumerator ChangeAnim()
	{
		handANim["Throttle"].wrapMode = WrapMode.ClampForever;
		handANim["Throttle"].speed = -animSpeed*2;
		handANim.CrossFade("Throttle");

		yield return new WaitForSeconds(animDelay);
		handANim["Throttle"].wrapMode = WrapMode.Once;
		handANim["Throttle"].speed = animSpeed;
		handANim.CrossFade("Throttle");
	}
}