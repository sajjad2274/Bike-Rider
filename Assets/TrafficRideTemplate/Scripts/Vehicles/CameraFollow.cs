//
//  Orginally written by ALIyerEdon - Winter 2017 - Unity 5.4.1 64 bit
//

// Used to camera follow the car

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Find player car as target
	[HideInInspector]public Transform target;

	// Distance offset in z axis from player 
	public float zOffset = -14f;

	// Gme is finished
	[HideInInspector]public bool isFinished = false;

	IEnumerator Start () {
	
		// Wait a frame to car has been spawned
		yield return new WaitForEndOfFrame ();

		// Find player car with his tag
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		started = true;

	}

	bool started;

	void Update () 
	{
		
		if (!started)
			return;
		
		if (!isFinished)
		{
			transform.position = new Vector3 (transform.position.x, transform.position.y,
				target.position.z + zOffset);
		} else 
		{

			transform.position = new Vector3 (transform.position.x, transform.position.y,
				transform.position.z - zOffset *Time.deltaTime);
		}
	}
}
