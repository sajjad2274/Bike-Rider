using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLean : MonoBehaviour {

	public float leanValue = 0.3f;

	public float leanSpeed = 3f;

	[HideInInspector]public float currentSpeed;

	public Rigidbody rigid;

	public Transform target;

	float steerInput;

	BikeController racer;

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
	}

	void Update () 
	{
		if (isAccel)
			accelInput = Input.acceleration.x;

		steerInput = racer.steerInput;

		currentSpeed = racer.currentSpeed;

		if (steerInput > 0) {

			if((leanValue-currentSpeed/74)>0.1f)
			{
				
				target.Rotate(new Vector3(0, 0, -((leanValue*Mathf.Abs(accelInput))-currentSpeed/74)), Space.Self);
			}
			else
			{
				target.Rotate(new Vector3(0, 0, -0.1f), Space.Self);

			}

		}
		if (steerInput < 0) {
			if((leanValue-currentSpeed/74)>0.1f)
			{

				target.Rotate(new Vector3(0, 0, ((leanValue*Mathf.Abs(accelInput)) - currentSpeed / 74)), Space.Self);

			}
			else
			{

				target.Rotate(new Vector3(0, 0, 0.1f), Space.Self);

			}


		}

		// Recoil back the car 
		target.transform.rotation = Quaternion.Lerp (target.transform.rotation, target.parent.rotation, Time.deltaTime *leanSpeed );

	}
}



