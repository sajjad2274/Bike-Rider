// Used to manage an control bike selection    

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BikeSelect : MonoBehaviour {

	public GameObject[] selectionIcons;

	public GameObject[] locks;

	MainMenu mainMenu;

	public GameObject[] bikes;

	public Transform spawnPoint;

	public GameObject buyButton;

	int currentID;

	public int[] bikePrices;

	public Text currentBikePrice;

	void Start()
	{

		// Read last selected bike
		currentID = PlayerPrefs.GetInt("CurrentBike");

		// Find mainMenu script to activated shop offer if needed
		mainMenu = GameObject.FindObjectOfType<MainMenu>();

		// Check each bike unlocked or not 
		for (int a = 0; a < locks.Length; a++) {
			if(PlayerPrefs.GetInt("Bike"+a.ToString())  ==3)
				locks[a].SetActive(false);
			else
				locks[a].SetActive(true);
		}

		if(PlayerPrefs.GetInt("Bike"+currentID.ToString()) == 3) // 3=>true , 0=>false		
			buyButton.SetActive(false);
		else
			buyButton.SetActive(true);
		
		currentBikePrice.text = bikePrices[currentID].ToString() + " COINS";

		if(GameObject.FindGameObjectWithTag("Player"))
			Destroy(GameObject.FindGameObjectWithTag("Player")); 
		
		Instantiate(bikes[currentID],spawnPoint.position,spawnPoint.rotation);

		UpdateSelection(currentID);

	}

	public void SelectBike(int id)
	{

		currentID = id;

		currentBikePrice.text = bikePrices[currentID].ToString() + " COINS";

		UpdateSelection(currentID);

		Destroy(GameObject.FindGameObjectWithTag("Player")); 

		Instantiate(bikes[currentID],spawnPoint.position,spawnPoint.rotation);

		if(PlayerPrefs.GetInt("Bike" + currentID.ToString()) == 3) // 3=>true , 0=>false	
		{
			PlayerPrefs.SetInt("CurrentBike",currentID);

			buyButton.SetActive(false);
		}
		else
		{
			buyButton.SetActive(true);
		}

	}

	public void UpdateSelection(int id)
	{
		for(int a = 0; a<selectionIcons.Length;a++)
		{
			selectionIcons[a].SetActive(false);

		}
		selectionIcons[id].SetActive(true);
	}

	public void BuyCurrentBike()
	{
		
		if(PlayerPrefs.GetInt("Bike"+currentID.ToString()) != 3)
		{
			if(PlayerPrefs.GetInt("Coins")>=bikePrices[currentID])
			{
				locks[currentID].SetActive(false);
				PlayerPrefs.SetInt("Bike"+currentID.ToString(),3);
				PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")-bikePrices[currentID]);
				mainMenu.totalCoins.text = PlayerPrefs.GetInt("Coins").ToString();
				buyButton.SetActive(false);
				PlayerPrefs.SetInt("CurrentBike",currentID);
			}
			else
				mainMenu.shopWindow.SetActive(true);
		}

	}

}