using UnityEngine;
using System.Collections;

public class mermi : MonoBehaviour {

    public int bulletSpeed;
	int dir = 0;
	Rigidbody2D rb2d;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void Start()
    {
        Destroy(gameObject, 3.5f);

        if (transform.eulerAngles.y >0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
    }

	private void FixedUpdate()
	{
		rb2d.velocity = new Vector2(bulletSpeed * dir, 0f);
	}

	//private void OnCollisionEnter2D(Collision2D coll)
	//{
	//	if (coll.gameObject.CompareTag("floor"))
	//	{
	//		coll.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
	//	}
	//}

	private IEnumerator OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
			Debug.Log("player ile etkileşime geçti");
            coll.gameObject.GetComponent<charMovement>().jumpPower = 10000;

			yield return new WaitForSeconds(1f);
			coll.gameObject.GetComponent<charMovement>().jumpPower = 7000;
        }

    }

	//private IEnumerator OnCollisionExit2D(Collision2D coll)
 //   {
 //       if (coll.gameObject.CompareTag("Player"))
 //       {
	//		yield return new WaitForSeconds(1f);
	//		coll.gameObject.GetComponent<charMovement>().jumpPower = 7000;
 //       }
 //   }
}
