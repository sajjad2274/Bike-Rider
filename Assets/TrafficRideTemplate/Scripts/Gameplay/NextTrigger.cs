//
//  Orginally written by ALIyerEdon - Winter 2017 - Unity 5.4.1 64 bit
//

// In thsi script we want to spawn next road prefab based on his name from resources folder

using UnityEngine;
using System.Collections;

public class NextTrigger : MonoBehaviour {

	// List of the road prefab names from resources folder
	public string[] roadPrefabs;

	// Rote object to find next prefab position and rotation
	public Transform root;

	// ROad manager to store current and next road index
	RoadManager roadManager;

	GameManager gameManager;

	// Wait some secounds and start moving traffic cars
	public float startDelay = 1f;
	// Refull time in time terial mode
	public int chechpointTime = 30;

	public Transform vehicles;

	IEnumerator Start()
	{


		vehicles.parent = null;

		roadManager = GameObject.FindObjectOfType<RoadManager> ();
		gameManager = GameObject.FindObjectOfType<GameManager> ();

		// We want to pause traffic cars movement until player has been reached close of them    
		// If you don't dely, Traffic cars will be falling from end of the road 
		// (the next road prefab is not instantiated untill player car has been reached to next trigger )

		yield return new WaitForSeconds (startDelay);

		TrafficCar[] tf = GetComponentsInChildren<TrafficCar> ();

		foreach (TrafficCar t in tf)
			t.isActive = true;
		
	}
	void OnTriggerEnter(Collider col)   
	{
	
		if (col.CompareTag ("Player")) 
		{

			if (roadManager.roadIndex < roadPrefabs.Length )
				roadManager.roadIndex++;
			
			if (roadManager.roadIndex >= roadPrefabs.Length)
				roadManager.roadIndex = 0;

			gameManager.CheckpointTime (chechpointTime);

			GameObject nextPatern = new GameObject ();

			nextPatern =  Resources.Load (roadPrefabs[roadManager.roadIndex]) as GameObject;
				Instantiate (nextPatern, new Vector3(root.position.x,root.position.y,
				root.position.z+root.GetComponent<MeshRenderer>().bounds.size.z), root.transform.rotation);
		}
	}
}

