using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//add this import statement for Random number generation
using Random = UnityEngine.Random;

public class ComputerTankController : MonoBehaviour
{
    private Vector3 velocity;

    private SpriteRenderer rend;
    //private Animator anim;
    public GameObject bullet;


    public float speed = 1.0f;
    private bool canFire = true;

    // Use this for initialization
    void Start()
    {
        int startx = Random.Range(-1, 1);
        if (startx == 0)
            startx = 1;

        velocity = new Vector3((float)startx, 0f, 0f);
        rend = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        //anim.Play("RedGhostRight");
    }

    // Update is called once per frame
    void Update()
    {

        //... steps for controlling tank
        //shooting the bullet from the player
        if (canFire)
        {
            Debug.Log("FIRE");
            Vector3 offset = new Vector3(0f, 2f, 0f);
            GameObject b = Instantiate(bullet, new Vector3(0f, 0f, 0f), Quaternion.AngleAxis(180, Vector2.left));

            b.GetComponent<BulletController>().InitPosition(transform.position + offset, new Vector3(0f, -2f, 0f));
            canFire = false;

            //this starts a coroutine... a non-blocking function
            StartCoroutine(ComputerCanFireAgain());
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

        //1% of the time, switch the direction: 
        int change = Random.Range(0, 100);
        if (change == 0)
        {
            velocity = new Vector3(-velocity.x, 0, 0);
            //not coding the animation control for this. 

        }

        //make sure the obect is inside the borders... if edge is hit reverse direction
        if ((transform.position.x <= leftBorder + width / 2.0) && velocity.x < 0f)
        {
            velocity = new Vector3(1f, 0f, 0f);
            //anim.Play("RedGhostRight");
        }
        if ((transform.position.x >= rightBorder - width / 2.0) && velocity.x > 0f)
        {
            velocity = new Vector3(-1f, 0f, 0f);
            //anim.Play("RedGhostLeft");
        }
        transform.position = transform.position + velocity * Time.deltaTime * speed;
    }

    //will wait 3 seconds and then will reset the flag
    IEnumerator ComputerCanFireAgain()
    {
        //this will pause the execution of this method for 3 seconds without blocking
        yield return new WaitForSecondsRealtime(3);
        canFire = true;
    }
}