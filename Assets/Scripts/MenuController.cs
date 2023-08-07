using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    Slowmo s;

    public static bool won = false;

    bool activePauseMenu = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activePauseMenu = !activePauseMenu;
            pauseMenu.SetActive(activePauseMenu);
            if (!won)
            {
                s.freezeTime(activePauseMenu);
            }
        }
    }
}
