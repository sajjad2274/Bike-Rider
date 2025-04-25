using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour 
{

	GameManager manager;

	// Score given to player
	public int awardedScore = 100;

	// target speed of player to earn score
	public float scoreSpeed = 43f;

	void Start () 
	{
		manager = GameObject.FindObjectOfType<GameManager>();	
	}

	// just give one time score
	bool entered;
	int scoreTemp;

	void OnTriggerEnter (Collider col) {

		if(!entered)
		{
			if(col.tag == "Player")
			{
				if(col.GetComponent<BikeController>().currentSpeed >= scoreSpeed)
				{
					if(col.GetComponent<BikeController>().isWheelie)
						scoreTemp = awardedScore + ((int)col.GetComponent<BikeController> ().currentSpeed) * 4;
					else
						scoreTemp = awardedScore + ((int)col.GetComponent<BikeController> ().currentSpeed) * 2;
					
					manager.AddScore(scoreTemp);
						entered = true;
					col.GetComponent<CollisionController>().SpawnScorePrefab(scoreTemp);

				}
			}
		}

	}
}
