using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballFalling : MonoBehaviour
{

	public GameObject[] balls;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		StartCoroutine(BallSpawnerController(0.8f, collision));
	}

	IEnumerator BallSpawnerController(float waitTime, Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//for loop for creating rotating obstacles
			for (int i = 0; i < balls.Length; i++)
			{
				balls[i].SetActive(true);
				yield return new WaitForSeconds(waitTime);
				Destroy(balls[i], 5f);
			}
		}

		//IEnumerator BallDeactivator(float waitTime)
		//{
		//	yield return new WaitForSeconds(waitTime);

		//	Debug.Log("toplar deaktif olcak");

		//	gameObject.SetActive(false);

		//}

	}
}
