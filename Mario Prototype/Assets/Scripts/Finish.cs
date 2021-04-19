using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script change the sprite and show hell yeah text on victory

public class Finish : MonoBehaviour
{
    public Sprite sprite;
    public GameObject txt;
    public GamPlayManager gamPlayManager;

    public SpriteRenderer spriteToChange;

    public void EndIt()
    {
        StartCoroutine(End()); // start coroutine on victory
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(2.7f);
        spriteToChange.sprite = sprite;            // change house sprite and show txt and then restart the game
        txt.SetActive(true);
        yield return new WaitForSeconds(5f);
        gamPlayManager.RestartGam();
    }
}
