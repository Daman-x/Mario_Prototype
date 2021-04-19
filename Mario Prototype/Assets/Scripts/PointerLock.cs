using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script control behaviour of camera

public class PointerLock : MonoBehaviour
{
    
    public GameObject ToFollow;  // what to follow
    public GameObject Tofollow;   // if follow is null then what to follow
    public BobController bob; //player script
    public float PositionY;
    void Start()
    {
         bob = FindObjectOfType<BobController>();
         ToFollow = bob.gameObject;
         PositionY = gameObject.transform.position.y;
    }
    void LateUpdate()
    {
        if(ToFollow == null)
        {
            ToFollow = Tofollow;  // if to follow or player is not alive
        }
        else
        {
            if (ToFollow.transform.position.x > 0.6 && !bob.IsDead)
            {
                transform.position = new Vector3(ToFollow.transform.position.x,transform.position.y,transform.position.z);  // while player is alive follow
            }
        }
    }

    public void ReAssign()
    {
        ToFollow = GameObject.FindGameObjectWithTag("Player");   // when player respawn set the camera psoition again to the player
        bob = ToFollow.GetComponent<BobController>();
    }
}
