using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int whenToDieY;
    public int moveSpeed;
	private float rayDistanceForWall = 1f;
	private float rayDistanceForGround = 40f;
    float time = 0.3f;


    private bool movingRight = true;
	bool canRun = false;
	public bool alwaysWalk = false; //editörden ayarlanabilir, arena gibi levellarda açık olmalı
	//private bool hitted = false;

	private Rigidbody2D rb2d;

    public Transform frontController;
    public Transform groundController;

	private GameObject playerObject;
	private charMovement charMovementRef;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

		playerObject = GameObject.FindGameObjectWithTag("Player");
		charMovementRef = playerObject.GetComponent<charMovement>();
	}


	public LayerMask ObstacleLayer;
    public LayerMask groundLayer;



    // Update is called once per frame
    void Update()
    {
		Run();
		CheckforDie();
    }

    #region CameraVisible Properties
    //if always walk is turned off and when its visible it can run
    private void OnBecameVisible()
	{
		if (!alwaysWalk)
		{
			canRun = true;
		}
	}

    //if always walk is turned off and when its not visible then it cant run
	private void OnBecameInvisible()
	{
		if (!alwaysWalk)
		{
			canRun = false;
		}
	}
    #endregion

    private void Run()
    {
        time -= Time.deltaTime;

        //Eğer koşabiliyorsa
        if (canRun || alwaysWalk)
		{
			//Eğer düşman duvar görüyorsa
			RaycastHit2D hitInfoForWall = Physics2D.Raycast(frontController.position, Vector2.left, rayDistanceForWall, ObstacleLayer);
			RaycastHit2D hitInfoForGround = Physics2D.Raycast(groundController.position, Vector2.down, rayDistanceForGround, groundLayer);

			//debugging
			Debug.DrawRay(frontController.position, Vector2.left * rayDistanceForWall, Color.red);
			Debug.DrawRay(groundController.position, Vector2.down * rayDistanceForGround, Color.cyan);



			//Eğer yere düşeceğini fark ederse dönsün
			if (!hitInfoForGround)
			{
				rayDistanceForGround = 50f;
				movingRight = !movingRight;
			}

            if (hitInfoForWall)
            {
                time = 0.3f;
            }

            //Eğer önünde engel varsa ve yerdeyse tırmansın
            if ((time > 0) && hitInfoForGround)
			{
               
                //Tırmanma
                rb2d.velocity = new Vector2(rb2d.velocity.x, moveSpeed);
				rayDistanceForGround = 40f;

            }



			//Hareket ettirme ve döndürme
			if (movingRight)
			{
				rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
				transform.eulerAngles = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
				rayDistanceForWall = -3f;
			}
			else if (!movingRight)
			{
				rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
				transform.eulerAngles = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
				rayDistanceForWall = 3f;
			}
		}
    }

    void CheckforDie()
    {
        if (transform.position.y < whenToDieY)
        {
            Destroy(this);
        }
    }


    //Oyuncuya hasar verme
    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
		bool x = true;
        if (collision.gameObject.CompareTag("Player"))
        {
			//burası kediler üst üste bindiginde birden fazla kez calisabiliyor
			charMovementRef.StartCoroutine(charMovementRef.Hurt());

			Debug.Log("hasar yedi");
        }

		if (collision.gameObject.CompareTag("mermi") && x)
		{
			//mermi ile temas eden kedilerin layerını bir süreliğine değiştir ki
			//mermi içinden geçip diğer kedilere gitsin, sonrasında eski haline çevir ve tekrar mermi yiyebilsin
			//bu alttaki kodda merminin layer değişiyor, bu yüzden bir mermiyi sadece bir kedi yiyebiliyor, sonraısnda mermi iptal oluyor.
			x = false;
			StartCoroutine(MakeEnemyDisable(collision));

			yield return new WaitForSeconds(1f);
			x = true;


			//collision.gameObject.layer = 11;
			//rb2d.velocity = new Vector2(rb2d.velocity.x * 3, rb2d.velocity.y);
		}
    }


	void OnCollisionStay(Collision collision)
	{
		NewColliderGenerator();
	}

	void NewColliderGenerator()
	{
		if (rb2d.OverlapPoint(transform.position))
		{
			Debug.Log("kediler iç içe girdi");
		}
	}


	IEnumerator MakeEnemyDisable(Collision2D coll)
	{
		yield return new WaitForSeconds(0.1f);
		gameObject.layer = 11;
		canRun = false;
		yield return new WaitForSeconds(0.3f);
		gameObject.layer = 9;
		yield return new WaitForSeconds(1f);
		canRun = true;
	}
}