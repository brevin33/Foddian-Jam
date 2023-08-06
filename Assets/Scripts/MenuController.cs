using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    Slowmo s;


    bool activePauseMenu = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activePauseMenu = !activePauseMenu;
            pauseMenu.SetActive(activePauseMenu);
            s.freezeTime(activePauseMenu);
        }
    }
}
