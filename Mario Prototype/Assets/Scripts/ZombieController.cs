using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this sccript is used to control zombie

public class ZombieController : MonoBehaviour
{

    public float Speed = 2f;
    Rigidbody2D rb;
    public Animator anim;
    public int movingdirection = -1;
    Vector3 facingdirection;
    public GameObject Obj;
    public bool IsDead = false;    // if dead or not
    public static bool Wait = false;  // if have to wait to move or not

    public AudioSource DieSound , BobDied;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(!IsDead && !Wait)  // if not dead and not have to wait
        {
            rb.velocity = new Vector2(movingdirection * Speed, rb.velocity.y);
            transform.eulerAngles = facingdirection;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetType() == typeof(BoxCollider2D) && collision.collider.gameObject.CompareTag("Player") && !IsDead)  // when zombie hit player from side collider
        {
            if (!collision.collider.gameObject.GetComponent<BobController>().IfPowered)  // if player is powered
            {
                collision.collider.gameObject.GetComponent<BobController>().IsDead = true;  
                ZombieController.Wait = true; // stop zombie movement for a while
                BobDied.Play();
            }
            else  // if not
            {
                collision.collider.GetComponent<BobController>().OnInvincible();  // call on invincible funtion to make player normal
            }
        }
        if(collision.collider.GetType() == typeof(BoxCollider2D) && !collision.collider.gameObject.CompareTag("Player"))  // zombie collide with any collide than player start walking in oppse direction
        {
            if(movingdirection == -1)
            {
                movingdirection = 1;
                facingdirection = new Vector3(0, 0, 0);
            }
            else
            {
                movingdirection = -1;
                facingdirection = new Vector3(0, 180, 0);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<BobController>().IsDead)   // if zombie not dead and player hit zombie on head
        {
            IsDead = true;
            DieSound.Play();
            anim.SetTrigger("ZombieDied");  // zombie dies
            StartCoroutine(died());
           
        }
    }
    IEnumerator died()
    {
        yield return new WaitForSeconds(0.4f);   // destory zombie after 0.4sec
        Destroy(this.Obj);
    }
}
