using UnityEngine;
using System.Collections;

public class LightFlasher : MonoBehaviour {

	public float t1,t2;
	public Light l1,l2;

	IEnumerator Start () {

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name.Contains ("Garage"))
			Destroy (gameObject);


		while (true) {
			yield return new WaitForSeconds (t1);
			l1.intensity = 1f;
			l2.intensity = 0;
			yield return new WaitForSeconds (t2);
			l1.intensity = 0;
			l2.intensity = 1f;
		}
	}

}
