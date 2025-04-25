using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollisionController : MonoBehaviour {


	public AudioClip collisionSound,deadSound;
	AudioSource collisionSource,deadSource;

	public static CollisionController instance;
	public int maxDead = 3;

	public int collisionDamage = 10;

	public float deadVelocity = 30f;

	GameManager manager;

	public Animator deadAnimation;

	public float respawnTime = 3f;

	public GameObject scorePrefab;

	public Transform scorePosition ;

	VehicleSpawner vehicleSpawner;

	InputSystem inputSystem;

	void Start()
	{
		instance = this;
		manager = GameObject.FindObjectOfType<GameManager>();
		inputSystem = GameObject.FindObjectOfType<InputSystem>();
		vehicleSpawner = GameObject.FindObjectOfType<VehicleSpawner>();

		collisionSource = gameObject.AddComponent<AudioSource> ();
		collisionSource.spatialBlend = 0;
		collisionSource.playOnAwake = false;
		collisionSource.volume = 1f;
		collisionSource.hideFlags = HideFlags.HideInInspector;

		deadSource = gameObject.AddComponent<AudioSource> ();
		deadSource.spatialBlend = 0;
		deadSource.playOnAwake = false;
		deadSource.volume = 0.7f;
		deadSource.hideFlags = HideFlags.HideInInspector;
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.collider.CompareTag("Road"))
		{
			collisionSource.PlayOneShot (collisionSound);

			if(col.relativeVelocity.magnitude > deadVelocity)
				Dead () ;    

		}

		if(col.collider.CompareTag("Traffic Car"))
		{
			collisionSource.PlayOneShot (collisionSound);

			if(col.relativeVelocity.magnitude > deadVelocity)
				Dead () ;    

			col.collider.GetComponent<TrafficCar>().enabled = false;

		}
	}

	GameObject[] trafficVehicles;

	void Dead()
	{
		manager.healthImage.SetActive(true);
		deadAnimation.SetBool("Dead",true);
		GetComponent<BikeController>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<VehicleAudio>().engineSource.volume = 0;
		maxDead--;


		deadSource.PlayOneShot (deadSound);

		trafficVehicles = GameObject.FindGameObjectsWithTag("Traffic Car");

		for(int a = 0;a<trafficVehicles.Length;a++)
		{
			trafficVehicles[a].GetComponent<Rigidbody>().isKinematic = true;
		}

		vehicleSpawner.isActive = false;

		//StartCoroutine(ReSpawn());
		manager.EndGame();

	}


	public void ReSpawn()
	{
		collisionSource.PlayOneShot (collisionSound);
		inputSystem.controlsParent.SetActive (false);
		//yield return new WaitForSeconds(1);
		Time.timeScale = 1;
		inputSystem.controlsParent.SetActive (true);
		
			transform.position = new Vector3(0,transform.position.y,transform.position.z);
			manager.healthImage.SetActive(false);
			deadAnimation.SetBool("Dead",false);
			GetComponent<VehicleAudio>().engineSource.volume = PlayerPrefs.GetFloat("FXVolume") - 0.3f;
			GetComponent<BikeController>().enabled = true;
			GetComponent<BikeController>().speedFactor = 0;
			GetComponent<Rigidbody>().isKinematic = false;

			for(int a = 0;a<trafficVehicles.Length;a++)
			{
				if(trafficVehicles[a])
					trafficVehicles[a].GetComponent<Rigidbody>().isKinematic = false;
			}

			vehicleSpawner.isActive = true;

		
	}


	public void SpawnScorePrefab(int score)
	{
		GameObject temp = (GameObject)Instantiate(scorePrefab,scorePosition.position,scorePosition.rotation);
		temp.GetComponent<TextMesh> ().text = score + " +";
	}

}
