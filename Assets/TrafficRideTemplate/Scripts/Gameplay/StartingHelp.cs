using UnityEngine;
using System.Collections;

public class StartingHelp : MonoBehaviour {


	void Start()
	{
		if (PlayerPrefs.GetInt ("FirstHelp") != 3)
			StartCoroutine ("disableHelp");
		else
			gameObject.SetActive (false);
	}

	IEnumerator disableHelp()
	{
		yield return new WaitForSeconds (10f);
		PlayerPrefs.SetInt  ("FirstHelp", 3);
		gameObject.SetActive (false);
	}

}
