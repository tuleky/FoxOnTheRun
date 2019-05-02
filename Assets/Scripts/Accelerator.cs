using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : MonoBehaviour {

	public Animator anim;
	public Camera cam;
	public int acceleratedSpeed;

	private void Start()
	{
		acceleratedSpeed = 70;
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			collision.gameObject.GetComponent<charMovement>().speed = acceleratedSpeed;
			cam.orthographicSize = 40f;
			anim.SetBool("camIn", true);
		}
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<charMovement>().speed = 35;
			cam.orthographicSize = 33.75f;
			anim.SetBool("camIn", false);
		}
	}

}
