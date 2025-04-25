using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour {


	public string levelName = "Garage";
	void Start () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (levelName);
	}

	/*
	void Update () {
		
	}*/
}
