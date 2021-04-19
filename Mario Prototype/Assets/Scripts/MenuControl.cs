using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this script control the main menu

public class MenuControl : MonoBehaviour
{

    GameObject MenuPanel;
    public GameObject MenuPointer;
    public GameObject[] options;
    private int _selectedOption;
    public GamPlayManager scene_manager_script;
    public PointerLock PointerLock;

    public Vector3 pointerposition;
    public AudioSource selectSound;
    
    void Start()
    {
        MenuPanel = gameObject; // MenuPanel
        _selectedOption = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedOption = (_selectedOption >= 1) ? 0 : 1;  // if down arrow select 1 if 1 already then 0
            selectSound.Play();
            
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedOption = (_selectedOption > 0) ? 0 : 1;  // oppose to the above logic
            selectSound.Play();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            selectSound.Play();
            options[_selectedOption].gameObject.GetComponent<Button>().onClick.Invoke(); // call onclick event on the button
        }

        MenuPointer.transform.position = options[_selectedOption].transform.position - new Vector3(pointerposition.x ,pointerposition.y ,options[_selectedOption].transform.position.z);//\position of pointer in the menu
    }



    #region MenuOnClickFunctions

    public void OnClickPlay()
    {
        scene_manager_script.OnPlay();  // call gamplay manager onplay function
        PointerLock.enabled = true;
    }

    public void OnClickQuit()
    {
        Application.Quit(); //quit game or application
    }

    #endregion
}
