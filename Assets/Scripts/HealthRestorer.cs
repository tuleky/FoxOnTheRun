using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestorer : MonoBehaviour
{
	charMovement charMovement;

	private void Start()
	{
		charMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<charMovement>();
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && charMovement.health < 3)
		{
			HealthIncrementer();
			Destroy(gameObject);
		}
	}

	void HealthIncrementer()
	{
		charMovement.health++;
		charMovement.HeartUI();
	}
}
