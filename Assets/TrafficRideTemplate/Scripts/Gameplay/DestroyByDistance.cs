using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByDistance : MonoBehaviour {

	public float updateInterval = 1f;

	public float distanceToDestroy = 300f;

	public float distance;

	Transform target;

	IEnumerator Start () {

		target = GameObject.FindGameObjectWithTag("Player").transform;

		while(true)
		{

			distance = Vector3.Distance(transform.position,target.position);

			yield return new WaitForSeconds(updateInterval);

			if(distance>=distanceToDestroy)
				Destroy(gameObject);
			
		}

	}
}

