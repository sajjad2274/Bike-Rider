using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour {

	public float maxAngle = 100.0f;// max needle excursion in degrees 
	public float maxVel = 200.0f; // max velocity
	public float xAxisDegree = 21f;

	BikeController bike;

	public Vector3 rotationAxis;

	void Start()
	{ 

		bike = GameObject.FindObjectOfType<BikeController>();

	}

	float speed;

	void Update()
	{
		speed = Mathf.Lerp(speed,bike.currentSpeed,Time.deltaTime*10);

		SetNeedle(speed);

	}

	void SetNeedle(float vel)
	{
		var newAngle = vel*maxAngle/maxVel;
		transform.localRotation = Quaternion.Euler( xAxisDegree, 0, newAngle*rotationAxis.z+transform.rotation.z) ;

	}
}
