using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {

	public GameObject loading;

	public string[] levelNames;

	public Toggle realisticControl;

	void Start()
	{
		if (PlayerPrefs.GetInt ("RealisticControl") == 3)
			realisticControl.isOn = true;
		else
			realisticControl.isOn = false;
	}

	public void SelectDifficulty(int level)
	{
		PlayerPrefs.SetInt ("Difficulty", level);

		loading.SetActive (true);

		SceneManager.LoadSceneAsync (levelNames [PlayerPrefs.GetInt ("LevelID")]);

	}


	public void SetRealisticControl()
	{
		StartCoroutine ("save_Control");
	}

	IEnumerator save_Control()
	{

		yield return new WaitForEndOfFrame ();

		if(realisticControl.isOn)
			PlayerPrefs.SetInt ("RealisticControl", 3); // 3=>true , 0 => false
		else
			PlayerPrefs.SetInt ("RealisticControl", 0); // 3=>true , 0 => false
	}

}
