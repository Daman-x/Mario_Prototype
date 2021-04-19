using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is used on collecting powerup, this script is only attach topowerup boxes

public class PowerUp : MonoBehaviour
{
    public int Power;  // which power box have 0 for orange 1 for blue/greenish

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))  // when player collide with powerup
        {
            if(Power == 0)
            {
                if( !collision.gameObject.GetComponent<BobController>().IfPowered)   // if 0 then call orange power function in bobcontroller script
                {
                    collision.gameObject.GetComponent<BobController>().OrangePower();
                    collision.gameObject.GetComponent<BobController>().IfPowered = true;

                }
            }
            if(Power == 1)
            {
                collision.gameObject.GetComponent<BobController>().BluePower();  // if 1 then call blue power function in bobcontroller script
            }

            this.gameObject.SetActive(false);  
        }
    }
}
