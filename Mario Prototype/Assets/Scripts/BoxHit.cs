using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// used this script when bob(player) hit the boxes

public class BoxHit : MonoBehaviour
{
    [SerializeField]
    Text txt;
    public  Animation anim;
    public GameObject sprite;
    public Sprite Used;

    public float HeightTo = 2.5f;
    public AudioSource hitsound, Collectable;  // hit sound and sound of pick coin or powerup
    public bool IsUsed = false;               //if already used or not
    static int CoinsCount = 0;
    // Start is called before the first frame update

    private void Start()
    {
        txt = GameObject.FindGameObjectWithTag("CoinAmount").GetComponent<Text>(); // point to gameplay ui coins amount
        CoinsCount = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetType() == typeof(CircleCollider2D) && !sprite.CompareTag("Powerup"))
        {
            anim.Play();
            hitsound.Play();
            if(!IsUsed)
            {
                StartCoroutine(CoinOut());                                           // play hit animation and if not used play coin animation and collect coin
                Collectable.Play();
                OnCoinCollect();     
                gameObject.GetComponent<SpriteRenderer>().sprite = Used;
                this.IsUsed = true;
            }  
        }

        if(collision.collider.GetType() == typeof(CircleCollider2D) && sprite.CompareTag("Powerup"))
        {
            anim.Play();
            hitsound.Play();
            if (!IsUsed)
            {
                StartCoroutine(PowerupsOut()); // call powerupsout()
                Collectable.Play();
                gameObject.GetComponent<SpriteRenderer>().sprite = Used;    // play hit animation and if not used collect the powerup depend upon the tag set
                this.IsUsed = true;
            }
        }
    }

    IEnumerator CoinOut()
    {

        sprite.transform.position += new Vector3(0,HeightTo , 0);   // if coin is inside the box
        yield return new WaitForSeconds(0.3f);
        sprite.SetActive(false);
    }

    IEnumerator PowerupsOut()
    {
        sprite.transform.position += new Vector3(0, 1.2f, 0);  // if powerup is inside the box
        yield return null;
    }

    void OnCoinCollect()
    {
        CoinsCount = CoinsCount + 1;
        txt.text =  string.Format("<b> {0} </b>  X", CoinsCount);  // update amount of no.of coin in ui
    }
}
