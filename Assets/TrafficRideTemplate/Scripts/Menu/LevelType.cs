// Used to manage an control item (drive Type) selection    

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelType : MonoBehaviour {

	public GameObject[] selectionIcons;

	public GameObject[] locks;

	MainMenu mainMenu;

	int currentID;

	public int[] itemPrices;

	public Text [] itemPriceTexts;

	public GameObject buyButton;

	public GameObject nextMenu;

	void Start()
	{

		// Read last selected item
		currentID = PlayerPrefs.GetInt("CurrentDriveType");

		// Find mainMenu script to activated shop offer if needed
		mainMenu = GameObject.FindObjectOfType<MainMenu>();

		// Check each item unlocked or not 
		for (int a = 0; a < locks.Length; a++) {
			if(PlayerPrefs.GetInt("DriveType"+a.ToString())  ==3)
				locks[a].SetActive(false);
			else
				locks[a].SetActive(true);

			itemPriceTexts[a].text = itemPrices[a].ToString();

		}

		UpdateSelection(currentID);

		if(PlayerPrefs.GetInt("DriveType" + currentID.ToString()) == 3) // 3=>true , 0=>false	
			buyButton.SetActive(false);
	}

	public void SelectItem(int id)
	{

		currentID = id;

		UpdateSelection(currentID);

		if(PlayerPrefs.GetInt("DriveType" + currentID.ToString()) == 3) // 3=>true , 0=>false	
		{
			PlayerPrefs.SetInt("CurrentDriveType",currentID);
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

	public void BuySelectedItem()
	{
		if(PlayerPrefs.GetInt("DriveType"+currentID.ToString()) != 3)
		{
			if(PlayerPrefs.GetInt("Coins")>=itemPrices[currentID])
			{
				locks[currentID].SetActive(false);
				PlayerPrefs.SetInt("DriveType"+currentID.ToString(),3);
				PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")-itemPrices[currentID]);
				mainMenu.totalCoins.text = PlayerPrefs.GetInt("Coins").ToString();
				PlayerPrefs.SetInt("CurrentDriveType",currentID);
				buyButton.SetActive(false);
			}
			else
				mainMenu.shopWindow.SetActive(true);
		}
	}


	public void SelectDriveType()
	{
		if(PlayerPrefs.GetInt("DriveType" + currentID.ToString()) == 3)
		{
			PlayerPrefs.SetInt ("SelectedDriveType", currentID);
			PlayerPrefs.SetInt ("CurrentDriveType", currentID);
			nextMenu.SetActive (true);
			gameObject.SetActive (false);
		}

	}
}