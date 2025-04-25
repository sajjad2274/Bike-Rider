// Used to spawn traffic cars on highway lines    
// Randomized spawn time based on player speed. Higher speed, results faster spawn


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour {

	// Highway Lines Used To Spawn Traffic Cars
	public Transform[] Lines;

	// List of the traffci cars prefabs
	public GameObject[] vehicles;

	// Min and max spawn time (randomized)    
	public float minSpwanTime = 1f,maxSpawnTime = 3f;

	// Internal usage
	int lineNumber;
	int vehicleNumber;

	Transform player;

	// keep distance from player
	public float DistanceFromPlayer = 1000f;

	public bool isActive;

	BikeController bikeController;

	IEnumerator Start () 
	{
		if (PlayerPrefs.GetInt ("SelectedDriveType") == 2 || PlayerPrefs.GetInt ("SelectedDriveType") == 3)
			Destroy (gameObject);
		yield return new WaitForEndOfFrame();

		player = GameObject.FindGameObjectWithTag("Player").transform;

		bikeController = player.GetComponent<BikeController>();

		// Main spawner body codes
		while(true)
		{
			yield return new WaitForSeconds(RandomizeTime());
			if(isActive)
			{
				lineNumber++;
				vehicleNumber++;

				if(lineNumber>=Lines.Length-1)
					lineNumber = 0;

				if(vehicleNumber>vehicles.Length-1)
					vehicleNumber = 0;
						
				Instantiate(vehicles[vehicleNumber],Lines[lineNumber].position,Lines[lineNumber].rotation);
			}

		}

	}
	
	void Update () 
	{
		if(!player)
			return;


		// Keep spawner distance from player 
		transform.position = new Vector3(transform.position.x,transform.position.y,player.position.z+DistanceFromPlayer);

	}

	// Randomize spawn time    
	float RandomizeTime()
	{
		return Random.Range(minSpwanTime,maxSpawnTime  -  (bikeController.currentSpeed/100));
	}


	void OnDrawGizmos()
	{
		
		Gizmos.color = Color.green;

		for(int a = 0;a<Lines.Length;a++)
			Gizmos.DrawSphere(Lines[a].position,1f);
		
	}

}
