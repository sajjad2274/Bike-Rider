using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelie : MonoBehaviour {

	BikeController controller;

	public Transform carBody;
	public float leanValue = 0.3f;
	public float targetSpeed = 43f;
	bool state;
	float steerInput;
	float accelInput;
	bool isAccel;

	void Start ()
	{
		if (GameObject.FindObjectOfType<InputSystem> ().steerMode != SteerMode.Accelerometer) {
			accelInput = 1f;
			isAccel = false;
		}
		else
			isAccel = true;
		
		controller = GetComponentInParent<BikeController> ();

	}

	private void Update()
	{

		if (isAccel)
			accelInput = Input.acceleration.x;
		
		steerInput = controller.steerInput;

		if (controller.currentSpeed >= targetSpeed) {
			canWheelie = true;
		}
		else {
			canWheelie = false;
			state = false;
			CheckWheelie ();
		}

		if (state)
		{
			if (steerInput > 0) 
			{
				carBody.Rotate(new Vector3(0, 0, -leanValue*Mathf.Abs(accelInput)), Space.Self);
			}
			if (steerInput < 0)
			{
				
				carBody.Rotate(new Vector3(0, 0, leanValue * Mathf.Abs(accelInput)), Space.Self);
			}

			carBody.transform.rotation = Quaternion.Lerp (carBody.transform.rotation, transform.rotation, Time.deltaTime * 4.3f);

		}
	}


	void CheckWheelie()
	{
		if (state) {
			controller.isWheelie = true;
		} else {
			controller.isWheelie = false;
		}

		GetComponent<Animator> ().SetBool ("Wheelie", state);
	}

	bool canWheelie;
	public void WheelieInput()
	{
		if (canWheelie) 
		{
			state = !state;

			CheckWheelie ();
		}
	}


}
