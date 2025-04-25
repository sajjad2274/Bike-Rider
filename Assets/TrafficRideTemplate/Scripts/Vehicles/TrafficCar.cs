using UnityEngine;
using System.Collections;

public class TrafficCar : MonoBehaviour {

	public float moveSpeed,startSpeed;
	Rigidbody car;
	public bool isActive;

	// Used for randome forward force    
	public float minForce,maxForce;
	float random;

	[Header("Raycasting")]
	public Transform raycastSpot;
	public float raycastLength = 30f;
	public float raycastUpdate = 0;
	public float randomTime = 10f;
	public float delayToNextLine = 3f;
	public float sideMovingRandomize = 7f;
	public float  sideMoveAmount = 10f;
	public float sideMoveTemp;

	void Start () 
	{
		
		canRight = canLeft = true;

		car = GetComponent<Rigidbody> ();

		moveSpeed = Random.Range(minForce,maxForce) ;

		startSpeed = moveSpeed;

		StartCoroutine(RayCast());

		StartCoroutine (AI_State ());

	}
	
	void Update () 
	{
		
		if (!isActive)
			return;
		
		car.AddForce (car.transform.forward * moveSpeed*100000 * Time.deltaTime);

	
		if (moveRight) {
			if (transform.position.x < sideMoveAmount + sideMoveTemp)
				transform.position = new Vector3 (transform.position.x + sideMoveAmount * Time.deltaTime, transform.position.y, transform.position.z);
		} else {
			if (moveLeft) {
				if (transform.position.x > sideMoveAmount - sideMoveTemp)
					transform.position = new Vector3 (transform.position.x - sideMoveAmount * Time.deltaTime, transform.position.y, transform.position.z);
				
			}
		}
			

	}

	RaycastHit hitInfo;
	Color rayColor;
	IEnumerator RayCast()
	{
		while(true)
		{
			yield return new WaitForSeconds(raycastUpdate );

			Vector3 dir = raycastSpot.TransformDirection(Vector3.forward);
			Debug.DrawRay(raycastSpot.position,dir * raycastLength,rayColor);

			if(Physics.Raycast(raycastSpot.position,dir,out hitInfo,raycastLength))
			{
				rayColor = Color.red;
				moveSpeed = Mathf.Lerp(moveSpeed,Vector3.Distance(raycastSpot.position,hitInfo.collider.transform.position),
					Time.deltaTime*10f);
				car.drag = Mathf.Lerp(car.drag,3f,Time.deltaTime*10);
			}
			else
			{
				rayColor = Color.green;
				moveSpeed = startSpeed;
				car.drag = Mathf.Lerp(car.drag,1f,Time.deltaTime*10);
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.collider.CompareTag("Traffic Car"))
			Destroy(gameObject);
	}

	// AI System    
	public bool canRight;
	public bool canLeft;
	public bool moveRight;
	public bool moveLeft;
	int randomValue;

	IEnumerator AI_State()
	{
		while (true)
		{
			
			yield return new WaitForSeconds (sideMovingRandomize);
			randomValue = (int)Mathf.Floor (Random.Range (0, 1.4f));
			if (randomValue == 0) {
				if (canRight) {
					moveRight = true;
					moveLeft = false;
					sideMoveTemp = transform.position.x;
				} else {
					if (canLeft) {
						sideMoveTemp = transform.position.x;
						moveLeft = true;
						moveRight = false;
					} else {
						moveLeft = false;
						moveRight = false;
					}
				}
			} else {
				if (canLeft) {
					moveRight = false;
					moveLeft = true;
					sideMoveTemp = transform.position.x;
				} else {
					if (canRight) {
						sideMoveTemp = transform.position.x;
						moveLeft = false;
						moveRight = true;
					} else {
						moveLeft = false;
						moveRight = false;
					}
				}
			}

			yield break;
		}
	}

}
