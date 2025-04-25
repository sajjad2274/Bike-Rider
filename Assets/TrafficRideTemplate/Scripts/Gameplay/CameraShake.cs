///Daniel Moore (Firedan1176) - Firedan1176.webs.com/
///26 Dec 2015
///
///Shakes camera parent object

using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public float speedMultiplier = 14000f;

	// How long the object should shake for.
	float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	float shakeAmount = 0.7f;

	float decreaseFactor = 1.0f;

	Vector3 originalPos;

	BikeController racer;

	float currentSpeed;

	public bool isActive;

	void Awake()
	{
		shakeDuration = 100000f;
		decreaseFactor = 0.1f;

		if (PlayerPrefs.GetInt ("Vibrate") == 3)
			isActive = true;
		else
			isActive = false;
		
	}

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		racer = GameObject.FindGameObjectWithTag("Player").GetComponent<BikeController>();
	}

	void OnEnable()
	{
		originalPos = transform.localPosition;
	}

	void Update()
	{
		if(!racer)
			return;

		if (!isActive)
			return;
		
		currentSpeed = racer.currentSpeed;

		shakeAmount = (currentSpeed/(speedMultiplier*100));


		if (shakeDuration > 0)
		{
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			transform.localPosition = originalPos;
		}
	}
}