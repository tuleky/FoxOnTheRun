using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaScript : MonoBehaviour
{
	public Text remainingTimeText;
	public GameObject closingUI;

	public GameObject[] enemies;
	public GameObject[] cherries;

	void Start()
	{
		Time.timeScale = 0f;
	}

	//When Go button pressed this function starts
	public void Go()
	{
		//starts the game
		Time.timeScale = 1f;

		//closes the instructions UI
		closingUI.SetActive(false);

		StartCoroutine(ArenaTimer());
		StartCoroutine(Spawner(5f, enemies));
		StartCoroutine(Spawner(20f, cherries));
	}

	//RemainingTime Timer
	int leftTime = 70;
	IEnumerator ArenaTimer()
	{
		while (leftTime > 0)
		{
			yield return new WaitForSeconds(1f);
			leftTime--;
			remainingTimeText.text = "Remaining Time: " + leftTime.ToString();
		}
	}


	IEnumerator Spawner(float waitTime, GameObject[] gameObjects)
	{
		int i = 0;
		while (leftTime > 0)
		{
			yield return new WaitForSeconds(waitTime);
			gameObjects[i].SetActive(true);
			i++;
		}
	}


	void Win()
	{
		if (leftTime == 0)
		{
			nextLevel.GoNextLevel();
		}
	}
}
