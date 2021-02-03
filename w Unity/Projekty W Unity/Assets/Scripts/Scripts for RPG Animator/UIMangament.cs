using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMangament : MonoBehaviour
{

    [SerializeField] GameObject playerEQ;
    [SerializeField] GameObject pauseMenu;
    public bool activateEQ = false;
    bool activatePauseMenu = false;
    void Start()
    {
        playerEQ.SetActive(false);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PanelsUIMangament();
    }

    void PanelsUIMangament()
    {
        if(Input.GetKeyDown(KeyCode.E) && pauseMenu.activeSelf == false)
        {
            activateEQ = !activateEQ;
            playerEQ.SetActive(activateEQ);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuMangament();
        }
    }

    public void PauseMenuMangament()
    {
        activatePauseMenu = !activatePauseMenu;
        pauseMenu.SetActive(activatePauseMenu);

        if(activatePauseMenu == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Wyszed³êœ z Gry :D");
    }
}
