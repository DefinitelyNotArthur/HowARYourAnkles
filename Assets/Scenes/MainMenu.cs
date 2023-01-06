using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public Button playButton;
    public void PlayGame()
	{
		SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
	}
	public void QuitGame()
	{
		print("Quit");
		Application.Quit();
	}
}
