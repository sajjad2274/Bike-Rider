using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	[Header("Visuals")]
	public GameObject healthImage;
	public GameObject finishMenu;
	public GameObject timeTrialMenu;
	public Text totalScoreText;
	public Text currentSpeedText;
	public Text distanceText;
	public Text endMenuScore;
	public Text timeCountText;

	[Header("Score")]
	public int totalHealth = 100;
	public int totalScore;

	[Header("Sound")]
	public AudioClip timeDownSound;
	public AudioClip windSound;
	AudioSource audioSourceTime;
	AudioSource audioSourceWind;

	[Header("Revive UI")]
	
    public Text reviveCountdownText;
	private bool isWaitingForRevive = false;
	private float reviveCountdown = 15f;

	[Header("Level Settings")]
	public float[] levelDistances; // Example: [1000f, 1200f, 1500f]
	public int currentLevel = 0;
	public GameObject levelCompletePanel;
	public Text levelText;
	public GameObject[] Levels;

	[Header("Time Trial")]
	public int checkpointTime = 30;

	BikeController bikeController;

	IEnumerator Start()
	{
		currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
		levelText.text = "LEVEL " + (currentLevel + 1).ToString();
		Levels[currentLevel].SetActive(true);
		Time.timeScale = 1f;
		if (PlayerPrefs.GetInt ("SelectedDriveType") == 0 || PlayerPrefs.GetInt ("SelectedDriveType") == 2) {
			timeCountText.text = checkpointTime.ToString () + " s";
			timeTrialMenu.SetActive (true);
		} else {
			timeTrialMenu.SetActive (false);
		}

		audioSourceTime = gameObject.AddComponent<AudioSource>();
		audioSourceTime.hideFlags = HideFlags.None;
		audioSourceTime.loop = true;
		audioSourceTime.playOnAwake = false;
		audioSourceTime.clip = timeDownSound;
		audioSourceTime.spatialBlend = 0;
		audioSourceTime.volume = 0.7f;

		audioSourceWind = gameObject.AddComponent<AudioSource>();
		audioSourceWind.hideFlags = HideFlags.HideInInspector;
		audioSourceWind.loop = false;
		audioSourceWind.playOnAwake = false;
		audioSourceWind.clip = windSound;
		audioSourceWind.pitch = 1.7f;
		audioSourceWind.spatialBlend = 0;

		totalScore = PlayerPrefs.GetInt("Coins");
		totalScoreText.text = totalScore.ToString();

		yield return new WaitForEndOfFrame();
		bikeController = GameObject.FindObjectOfType<BikeController>();

		if (PlayerPrefs.GetInt ("SelectedDriveType") == 1 || PlayerPrefs.GetInt ("SelectedDriveType") == 3)
			yield break;


		while (true) {
			yield return new WaitForSeconds (1f);
			checkpointTime -= 1;
			timeCountText.text = checkpointTime.ToString() + " s";
			if (checkpointTime < 10) {
				if (!audioSourceTime.isPlaying)
					audioSourceTime.Play ();
				

			}
		}

	}
	void Update()
	{
		if (isWaitingForRevive)
		{
			reviveCountdown -= Time.unscaledDeltaTime;
			reviveCountdownText.text = Mathf.Ceil(reviveCountdown).ToString() + "s";

			if (reviveCountdown <= 0)
			{
				isWaitingForRevive = false;
				finishMenu.SetActive(false);
				Time.timeScale = 1;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

	}
	public void AddScore(int Score)
	{
		audioSourceWind.PlayOneShot (windSound);
		totalScore += Score+((int)Mathf.Floor(bikeController.currentSpeed));
		PlayerPrefs.SetInt("Coins",totalScore);
		totalScoreText.text = totalScore.ToString();
	}

	public void EndGame()
	{
		Time.timeScale = 0;
		isWaitingForRevive = true;
		healthImage.SetActive(false);

		finishMenu.SetActive(true); 
		endMenuScore.text = totalScore.ToString(); 
	}
	public void reviveBtn()
	{
	CollisionController.instance.ReSpawn();
	finishMenu.SetActive(false);
		isWaitingForRevive = false;
	}

	public void CheckpointTime(int timeAdd)
	{
		checkpointTime = timeAdd;

		audioSourceTime.Stop ();
	}

	public void LoadNextLevel()
	{
		currentLevel++;
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
		PlayerPrefs.Save();
		Time.timeScale = 1;
		levelText.text = "LEVEL " + (currentLevel+1).ToString();	
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}


