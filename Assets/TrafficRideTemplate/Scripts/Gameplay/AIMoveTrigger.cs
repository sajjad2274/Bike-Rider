using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveTrigger : MonoBehaviour {



	public bool isRight;
	public TrafficCar tCar;

	void Start () {
		tCar = GetComponentInParent<TrafficCar> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (isRight) {
			if (col.CompareTag ("Traffic Car") || col.CompareTag ("Road"))
				tCar.canRight = false;
		} else {
			if (col.CompareTag ("Traffic Car"))
				tCar.canLeft = false;
		}
			
	}
	void OnTriggerExit(Collider col)
	{
		if (isRight) {
			if (col.CompareTag ("Traffic Car") || col.CompareTag ("Road"))
				tCar.canRight = true;
		} else {
			if (col.CompareTag ("Traffic Car"))
				tCar.canLeft = true;
		}
	}

}
