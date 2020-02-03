using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankController : MonoBehaviour
{
    private Vector3 velocity;

    private SpriteRenderer rend;
    public GameObject bullet;
    public Text playerLives;
    public GameController gameController;
    public AudioSource gunAudio;

    public float speed = 1.0f;
    private bool canFire = true;
    private int hitCount = 0;
    private string LOSE_TEXT = "Player Loses";

    

    // Use this for initialization
    void Start()
    {
        gunAudio = GetComponent<AudioSource>();
        velocity = new Vector3(0f, 0f, 0f);
        rend = GetComponent<SpriteRenderer>();
        playerLives.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        //... steps for controlling tank
        //shooting the bullet from the player
        if (Input.GetButtonDown("Fire1") && canFire)
        {
            //the offset 
            gunAudio.Play();
            Vector3 offset = new Vector3(0f, 2f, 0f);
            //create a bullet pointing in its natural direction 
            GameObject b = Instantiate(bullet, new Vector3(0f, 0f, 0f), Quaternion.identity);

            b.GetComponent<BulletController>().InitPosition(transform.position + offset, new Vector3(0f, 2f, 0f));
            canFire = false;

            //this starts a coroutine... a non-blocking function
            StartCoroutine(PlayerCanFireAgain());
        }

        // calculate location of screen borders
        // this will make more sense after we discuss vectors and 3D
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        //get the width of the object
        float width = rend.bounds.size.x;
        float height = rend.bounds.size.y;

        velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        //make sure the obect is inside the borders... if edge is hit reverse direction
        if ((transform.position.x <= leftBorder + width / 2.0) && velocity.x < 0f)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }
        if ((transform.position.x >= rightBorder - width / 2.0) && velocity.x > 0f)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }
        transform.position = transform.position + velocity * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collision detected");
      
        hitCount++;
        if (hitCount >= 3)
        {
            Destroy(gameObject);
            gameController.GameOver(LOSE_TEXT);
        }
        playerLives.text += "X";
    }

    //will wait 3 seconds and then will reset the flag
    IEnumerator PlayerCanFireAgain()
    {
        //this will pause the execution of this method for 3 seconds without blocking
        yield return new WaitForSecondsRealtime(3);
        canFire = true;
    }
}