using UnityEngine;
using System.Collections;

public class ScoreAnimation : MonoBehaviour {

	public float value;

	void Update () {
	
		transform.position = new Vector3 (transform.position.x, transform.position.y + value * Time.deltaTime,
			transform.position.z);
	}
}
