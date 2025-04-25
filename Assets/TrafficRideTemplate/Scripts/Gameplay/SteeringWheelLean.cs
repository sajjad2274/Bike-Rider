using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelLean : MonoBehaviour {

	public float leanValue = 0.14f;

	float leanSpeed;

	[HideInInspector]public float currentSpeed;

	public Rigidbody rigid;

	public Transform target;

	float steerInput;

	BikeController racer;

	Quaternion orginalRotation; 

	float accelInput;
	bool isAccel;

	void Start()
	{

		if (GameObject.FindObjectOfType<InputSystem> ().steerMode != SteerMode.Accelerometer) {
			accelInput = 1f;
			isAccel = false;
		}
		else
			isAccel = true;
		
		racer = GetComponentInParent<BikeController> ();
		orginalRotation  = target.localRotation;
	}

	void Update () 
	{

		if (isAccel)
			accelInput = Input.acceleration.x;
		
		steerInput = racer.steerInput;

		currentSpeed = racer.currentSpeed/10;

		leanSpeed = racer.steerSpeed/10;

		if (steerInput > 0) {

			target.Rotate(new Vector3(0, leanValue*Mathf.Abs(accelInput), 0), Space.Self);

		}
		if (steerInput < 0) {
			target.Rotate(new Vector3(0, -leanValue*Mathf.Abs(accelInput), 0), Space.Self);

		}

		// Recoil back the car 
		target.transform.localRotation = Quaternion.Lerp (target.transform.localRotation, orginalRotation, Time.deltaTime *leanSpeed );

	}
}



