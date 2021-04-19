using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to control bob player

public class BobController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    public  BoxCollider2D groundcheck;   // if bob is on ground
    public BoxCollider2D ground;  // which ground is bob on
    public float speed;
    public double jumpforce;
    public bool IsDead = false;                     // if dead or not
    public GamPlayManager Scene_Manager_Script;
    public GameObject Obj;  // this gameobject

    public bool IfPowered = false;    // if powerup is still on
    public static bool respawn = false;  // if just have been respawned

    public AudioSource BobDied, jump, powerup;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        Scene_Manager_Script = FindObjectOfType<GamPlayManager>();
        if(respawn)  // respawn after death
        {
            StartCoroutine(Invincible(2f)); // call invincible coroutine
        }
    }

    private void Update()  // calls every frame like loop
    {
        if(IsDead && !IfPowered)
        {
            anim.SetTrigger("Died");
            StartCoroutine(Ondied());  // if dead and not powered
        }

        if(gameObject.transform.position.y <= -6)
        {
            IsDead = true;
            BobDied.Play();  // player falls below the ground
        }
    }
    void FixedUpdate()  // call every fixed frame
    {
        if(!IsDead)  // if not dead player able to move jump
        {

            float Movement = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(Movement * speed, rb.velocity.y);  // apply motion

            if (Movement == 0f)
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Left", false);
            }
            else
            {
                anim.SetBool("Left", true);
                anim.SetBool("Idle", false);
            }

            if (Movement > 0f)  // rotate the sprite
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (Movement < 0f)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.Space) && groundcheck.IsTouching(ground))  // jump on space if touching the ground
            {
                rb.velocity = Vector2.up * (float) jumpforce;
                jump.Play();
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.CompareTag("Finish"))
        {
            transform.position = new Vector3(transform.position.x, -3.3f ,transform.position.z);   //if  bob collide with gameobject tag as finish like flag pr house
            GamPlayManager.GamePlayMusic.Stop(); // play sounds from different scripts
            GamPlayManager.EndMusic.Play();
            anim.SetTrigger("Finish");
            GameObject.FindObjectOfType<Finish>().EndIt();  // call endit function in finish script
            Destroy(rb);
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground") && !collision.gameObject.CompareTag("Powerup"))  // ground store the object player stand on to be used for touchingground part
        {
            ground = collision.gameObject.GetComponent<BoxCollider2D>();
        }
    }

    IEnumerator Ondied()
    {
        Destroy(rb);
        yield return new WaitForSeconds(1f);  // when bob dies call gameplay bob died function
        Destroy(this.Obj);
        Scene_Manager_Script.OnBobDied();
    }

    public void OnInvincible()
    {
        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);  // back to normal and call the invincible coroutine when powered
        IfPowered = false;
        BobDied.Play();
        IsDead = false;
        StartCoroutine(Invincible(0.3f));
    }

    IEnumerator Invincible(float wait)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        gameObject.layer = 3;
        yield return new WaitForSeconds(0.3f);
        sprite.enabled = true;
        yield return new WaitForSeconds(0.3f);
        sprite.enabled = false;                                      // create blinking effect and stop collision with zombies for a given time
        yield return new WaitForSeconds(0.3f);
        sprite.enabled = true;
        yield return new WaitForSeconds(wait);
        sprite.enabled = false;
        yield return new WaitForSeconds(0.3f);
        sprite.enabled = true;
        gameObject.layer = 7;
    }


    #region PowerUp Functions

      public void OrangePower()
      {
        transform.localScale = new Vector3(4.2f, 4.2f, 1);    // when orange potion is taken make player bigger in size
        powerup.Play();
      }
      public void BluePower()
      {
         jumpforce = jumpforce * 1.2;   // when bluish potion is taken increase the jump force 
         StartCoroutine(NormalBack());
        powerup.Play();
      }

      IEnumerator NormalBack()
      {
        yield return new WaitForSeconds(5f);  // call from bluepowerfunction normal jumpforce after 5 sec
        jumpforce = jumpforce / 1.2;  
      }

    #endregion
}
