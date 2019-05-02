using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class charMovement : MonoBehaviour
{

    public int speed;
    public int jumpPower;
    public int jumpSize;

    public int health;

	private static bool isDead = false;

    public Image[] hearts;

	public GameObject canvasObject;


    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer sprt;
    private groundedCheck groundedCheck;


    private AudioSource shotSound;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprt = GetComponent<SpriteRenderer>();
        groundedCheck = GetComponentInChildren<groundedCheck>();
        shotSound = GetComponent<AudioSource>();

    }
    
    private void Update()
    {
		AdControl();
        YenidenBaslama();
        Ziplama();
        AtesEtme();
        YonTayini();
    }
    private void FixedUpdate()
    {
        Move();
        Ziplat();
        AnimasyonOynat();
        Fire();
    }

    private void Start(){
        health = 3;
		isDead = false;

        //Time.timeScale = 1;
        uiElement[0].SetActive(false);
        uiElement[1].SetActive(false);
        uiElement[2].SetActive(true);
        uiElement[3].SetActive(true);
        uiElement[4].SetActive(true);
        uiElement[5].SetActive(true);

		HeartUI();
    }


    

    public void HeartUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    
    private bool fired = false;
    private bool leftPress = false;
    private bool rightPress = false;
    private bool jumpConditions = false;

    private float moveMobileDirection;
    private float movePCDirection;

    public GameObject[] uiElement;


    IEnumerator YenidenBaslamaOncesıBekletme(float waitTime){
		//1 saniye boyunca playerin olmesi izlenir, sonrasinda reklam gösterilir.
        yield return new WaitForSeconds(waitTime);
		Ads.ShowAd();
	
        Time.timeScale = 0;
        uiElement[0].SetActive(true);
        uiElement[1].SetActive(true);
        uiElement[2].SetActive(false);
        uiElement[3].SetActive(false);
        uiElement[4].SetActive(false);
        uiElement[5].SetActive(false);
    }

    private void AdControl()
	{
		if (Ads.CheckForAd())
		{
			canvasObject.SetActive(false);
		}
		else
		{
			canvasObject.SetActive(true);
		}
	}

    private void YenidenBaslama()
    {
		Debug.Log(isDead);
		//Öldüğünde yeniden başlama
		if (!isDead && (transform.position.y < -50 || health <= 0))
        {
            //If player dies, then disable box collider and little dying effects
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(Vector2.up * (2 * jumpPower / 3));
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.Rotate(new Vector3(0,0, Random.Range(140,220)));
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);


			isDead = true;

			//Then wait a # seconds to show game menu
			StartCoroutine(YenidenBaslamaOncesıBekletme(1f));
        }
    }


    //Check for jump conditions
    private void Ziplama()
    {
        if ((CrossPlatformInputManager.GetButtonDown("Zipla") || Input.GetKeyDown(KeyCode.W)) && jumpSize > 0) //eğer zıplama hakkımız varsa ve tuşa basılmışsa
        {
            jumpConditions = true;
        }
    }



    //Check for shooting conditions
    private void AtesEtme()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            fired = true;
        }
        else
        {
            fired = false;
        }
    }




    //Get axis for movement and apply them to bool variables
    private void YonTayini()
    {
        moveMobileDirection = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        movePCDirection = Input.GetAxisRaw("Horizontal");

        if (moveMobileDirection < 0 || movePCDirection < 0)
        {
            leftPress = true;
            rightPress = false;
        }
        else if (moveMobileDirection > 0 || movePCDirection > 0)
        {
            rightPress = true;
            leftPress = false;
        }
        else
        {
            rightPress = false;
            leftPress = false;
        }
    }

    //If he goes left or right then move, but if he doesnt then stop him absolutely
    private void Move()
    {
        if (rightPress)
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            sprt.flipX = false;
        }
        else if (leftPress)
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
            sprt.flipX = true;
        } else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }


    private void Ziplat()
    {
        if (jumpConditions)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(new Vector2(0, jumpPower));
            jumpSize--;
            jumpConditions = false;
			jumpPower = 7000;
        }
    }

    private void AnimasyonOynat()
    {
        //yürüme animasyonu
        if ((rb2D.velocity.x > 2 || rb2D.velocity.x < -2) && groundedCheck.grounded)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //düşme animasyonu
        if ((rb2D.velocity.y < -1) && (!groundedCheck.grounded))
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }

        //zıplama animasyonu
        if ((rb2D.velocity.y > 1) && (!groundedCheck.grounded))
        {
            anim.SetBool("isRising", true);
        }
        else
        {
            anim.SetBool("isRising", false);
        }
    }

    public GameObject bulletPrefab;
    public Transform bulletRightSpawn;
    public Transform bulletLeftSpawn;


    //Instantiate a bullet and then throw it
    public int maksMermiSayisi;
    void Fire()
    {
        if (fired)
        {

            int bulletCount = GameObject.FindGameObjectsWithTag("mermi").Length;
            //GameObject bullet;
            // Add velocity to the 
            //if he looks right and we have enough bullet
            if (sprt.flipX && bulletCount < maksMermiSayisi)
            {
                //sola mermi
                Instantiate(bulletPrefab, bulletLeftSpawn.position, bulletLeftSpawn.rotation);
                shotSound.Play();
            }
            //if he looks left and we have enough bullet
            else if (!sprt.flipX && bulletCount < maksMermiSayisi)
            {
                //sağa mermi
                Instantiate(bulletPrefab, bulletRightSpawn.position, bulletRightSpawn.rotation);
                shotSound.Play();
            }

            fired = false;
        }
    }

	//Hurt should only works one at the same time
    public IEnumerator Hurt()
    {
		//layer değiştirerek düşman collisionlarından kaç
		gameObject.layer = 9;
		//canı azalt
		health--;
		HeartUI(); //show the new health on UI
		//karakter görüntüsü yanıp sönsün
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		//playerı zıplat
		rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
		rb2D.AddForce(new Vector2(0, jumpPower / 1.2f));

        //2 saniyeliğine colliderları kapalı kalsın, sonra eski haline çevir
        yield return new WaitForSeconds(2);
        gameObject.layer = 0;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    }

    //Yeniden başla butonuna basıldığında çalışan fonksiyon
    public void LevelRestart()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
	}
}
