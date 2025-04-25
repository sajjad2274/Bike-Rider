using UnityEngine;
using System.Collections;

public class TrafficCar_Wheel : MonoBehaviour {

	public float moveSpeed;
	Rigidbody car;
	public Transform COM;

	public WheelCollider[] wheel_Colliders;



	void Start () 
	{
		car = GetComponent<Rigidbody> ();
		car.centerOfMass = COM.localPosition;

	}
	

	void Update () 
	{
		car.AddForce (transform.forward * moveSpeed);


		wheel_Colliders [2].steerAngle = 0;
		wheel_Colliders [3].steerAngle = 0;
		wheel_Colliders [0].steerAngle = 0;
		wheel_Colliders [1].steerAngle = 0;

		wheel_Colliders [2].motorTorque = Input.GetAxis ("Vertical") * moveSpeed;
		wheel_Colliders [3].motorTorque = Input.GetAxis ("Vertical") * moveSpeed;
		wheel_Colliders [2].motorTorque = Mathf.Clamp (wheel_Colliders [2].motorTorque, -moveSpeed, moveSpeed);
		wheel_Colliders [3].motorTorque = Mathf.Clamp (wheel_Colliders [3].motorTorque, -moveSpeed, moveSpeed);

	}
}




