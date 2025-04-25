using UnityEngine;
using System.Collections;

public class TimedObjectDestroyer : MonoBehaviour {


	public float lifeTime = 14f;

	IEnumerator Start () {
	

		yield return new WaitForSeconds (lifeTime);
		Destroy (gameObject);
	}
}
