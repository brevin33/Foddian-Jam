using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    [SerializeField]
    Slowmo s;
    public static bool work = true;
    private void Update()
    {
        if (Input.GetKeyDown("r") && work)
        {
            s.setDefault();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
