using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// this script control all the gameplay flow 

public class GamPlayManager : MonoBehaviour
{
    public GameObject diedpanel, menu, gameplay_menu;
    public Vector2[] SpawnPoints;
    public Vector3 PlayerSpawnPoint;
    public Text LifeCount;
    public Text LifeCount2;

    public GameObject Level;                // all the variables used in script
    public GameObject Player;
    public GameObject hoard;
    public GameObject BobLifes, GameOver;

    public PointerLock pointerLock; // point to camera script to set posiiton 

    public static AudioSource GamePlayMusic , EndMusic;
    public AudioSource GameOverSound;
    public int TotalLifes = 3;           // total no of lifes

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // lock mouse pointer and hide it
    }

    public void OnPlay()
    {
        menu.SetActive(false);
        gameplay_menu.SetActive(true);
        Player.transform.position = PlayerSpawnPoint; // when player like play on main menu
        SpawnEnemies();
        LoadLevel();
        PlayerSpawn();
    }

    public void OnBobDied()
    {
        diedpanel.SetActive(true);
        GamePlayMusic.Stop();
        if (TotalLifes > 1)
        {
            this.TotalLifes = this.TotalLifes - 1;
            LifeCount.text = string.Format("X  <b> {0} </b>", TotalLifes);  // when bob (Player) dies
            LifeCount2.text = string.Format("X  <b> {0} </b>", TotalLifes);
            PlayerSpawn();
            BobController.respawn = true;
            StartCoroutine(Restart());
        }
        else
        {
            BobLifes.SetActive(false);
            GameOver.SetActive(true);
            GameOverSound.Play();
            RestartGam();
        }
    }
    void SpawnEnemies()
    {
        foreach (Vector2 i in SpawnPoints)
        {
            Instantiate(hoard, i, Quaternion.identity); // spawn zombie on runtime
        }
    }

    void LoadLevel()
    {
        Level.SetActive(true);
        GamePlayMusic = GameObject.Find("Gameplay UI").GetComponent<AudioSource>();   // load the level 
        EndMusic = GameObject.Find("House").GetComponent<AudioSource>();
    }
    void PlayerSpawn()
    {
        Instantiate(Player, PlayerSpawnPoint, Quaternion.identity);   // spawn player
    }

    public void RestartGam()
    {
        StartCoroutine(RestartGame());  // start the coroutine restartgame 
    }

    IEnumerator Restart()
    {
        
        yield return new WaitForSeconds(2f);
        pointerLock.ReAssign();                     //reset player position and camera settings after player dies
        diedpanel.SetActive(false);
        GamePlayMusic.Play();
        ZombieController.Wait = false;
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        Scene scene = SceneManager.GetActiveScene();  // restart the whole scene/game on gameover ir victory
        ZombieController.Wait = false;
        SceneManager.LoadScene(scene.name);

    }
}
