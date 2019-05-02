using UnityEngine;

public class groundedCheck : MonoBehaviour {

    [SerializeField]
    GameObject dustCloud;

    //private Rigidbody2D rb2D;

    private charMovement charMovement;

    public bool grounded;

	// Use this for initialization
	void Awake () {
        //rb2D = GetComponentInParent<Rigidbody2D>();
        charMovement = GetComponentInParent<charMovement>();
    }




    //karakterin sekmesini önlemek için velocity.y = 0 yaptık

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("mermi") || collision.gameObject.CompareTag("obstacle"))
        {
            charMovement.jumpSize = 2;
            grounded = true;
            Instantiate(dustCloud, transform.position, dustCloud.transform.rotation);
        }

    }

    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("mermi")) || collision.gameObject.CompareTag("obstacle"))
        {
            grounded = false;

            if (charMovement.jumpSize > 2)
            {
                charMovement.jumpSize --;
            }
        }
    }
}
