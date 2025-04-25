//--------------------------------------------------------------
//
//       Contact me : aliyeredon@gmail.com
//
//--------------------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class VehicleAudio : MonoBehaviour
{
	public AudioClip EngineSound;

	[HideInInspector]public  AudioSource gearSource,engineSource;

	public AudioClip gearShiftClip,horn; 
	public float gearVolume = 1f;
	public float pitchMultiplier = 1f;
	public float PitchMin = 0.7f;
	public float PitchMax = 1.7f;

	// Gear sound (Engine sound based on gears ) management, orginally edited from unity standard car sample project
	private  BikeController m_CarController;   
	// Private variables
	public int totalGears = 7;
	public float GearShiftDelay = 0.3f;
	public float nextGearSpeed = 300f;
	int currentGear;
	float GearFactor;
	float Revs;

	private void Start ()
	{
		
		m_CarController = GetComponent<BikeController> ();

		gearSource = gameObject.AddComponent<AudioSource> ();
		gearSource.loop = false;
		gearSource.playOnAwake = false;
		gearSource.volume = gearVolume;  

		engineSource = gameObject.AddComponent<AudioSource>();
		engineSource.clip = EngineSound;
		engineSource.loop = true;
		engineSource.Play ();
		engineSource.volume = PlayerPrefs.GetFloat("FXVolume") - 0.3f;
		engineSource.spatialBlend = 0.43f;

		StartCoroutine(GearChanging());

	}

	private void Update ()
	{
			CalculateGearFactor ();

			CalculateRevs();

			// The pitch is interpolated between the min and max values, according to the car's revs.
			float pitch = ULerp (PitchMin, PitchMax, Revs);

			// clamp to minimum pitch (note, not clamped to max for high revs while burning out)
			pitch = Mathf.Min (PitchMax, pitch);

			engineSource.pitch = pitch * pitchMultiplier;

	}

	public void ChangeGear()
	{
		if (gearShiftClip) 
		{
			gearSource.clip = gearShiftClip;
			gearSource.Play ();
		}

	}

	// Engine sound system calculation
	// Gear changing only used for sound system      
	IEnumerator GearChanging ()
	{
		while (true) {
			yield return new WaitForSeconds (0.01f);

			float f = Mathf.Abs (m_CarController.currentSpeed / nextGearSpeed);
			float upgearlimit = (1 / (float)totalGears) * (currentGear + 1);
			float downgearlimit = (1 / (float)totalGears) * currentGear;

			// Changinbg gear down
			if (currentGear > 0 && f < downgearlimit) {
				// Reduce engine audio volume when changing gear
				// Delay time for changing gear down
				yield return new WaitForSeconds (0);


				currentGear--;
			}

			// Changing gear Up
			if (f > upgearlimit && (currentGear < (totalGears - 1))) {
				// Reduce engine audio volume when changing gear
				//	audioCar.audioSource.volume = 0.3f;
				ChangeGear ();
				// Delay before changing gear up
				m_CarController.ChangeGearAnimation();
				yield return new WaitForSeconds (GearShiftDelay);
				//audioCar.audioSource.volume = 1f;
				currentGear++;
			}

		}
	}

	// simple function to add a curved bias towards 1 for a value in the 0-1 range
	private static float CurveFactor (float factor)
	{
		return 1 - (1 - factor) * (1 - factor);
	}

	// unclamped version of Lerp, to allow value to exceed the from-to range
	private static float ULerp (float from, float to, float value)
	{
		return (1.0f - value) * from + value * to;
	}

	// Used for engine sound system    
	private void CalculateGearFactor ()
	{
		float f = (1 / (float)totalGears);
		// gear factor is a normalised representation of the current speed within the current gear's range of speeds.
		// We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
		var targetGearFactor = Mathf.InverseLerp (f * currentGear, f * (currentGear + 1), Mathf.Abs (m_CarController.currentSpeed / nextGearSpeed));
		GearFactor = Mathf.Lerp (GearFactor, targetGearFactor, Time.deltaTime * 5f);
	}     

	// Used for engine sound system
	private void CalculateRevs ()
	{
		// calculate engine revs (for display / sound)
		// (this is done in retrospect - revs are not used in force/power calculations)
		CalculateGearFactor ();
		var gearNumFactor = currentGear / (float)totalGears;
		var revsRangeMin = ULerp (0f, 1f, CurveFactor (gearNumFactor));
		var revsRangeMax = ULerp (1f, 1f, gearNumFactor);
		Revs = ULerp (revsRangeMin, revsRangeMax, GearFactor);
	}
}

