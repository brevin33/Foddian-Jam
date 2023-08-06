using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmo : MonoBehaviour
{

    [SerializeField]
    float slowmoGameSpeed = 0.05f;

    bool froze;

    float baseFixedDeltaTime;

    private void Awake()
    {
        baseFixedDeltaTime = Time.fixedDeltaTime;
    }
    void Update()
    {
        if (froze)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = slowmoGameSpeed;
            Time.fixedDeltaTime = slowmoGameSpeed * Time.fixedDeltaTime;
        }
        else if (Input.GetMouseButtonUp(0)) {
            Time.timeScale = 1;
            Time.fixedDeltaTime = baseFixedDeltaTime;
        }
    }

    public void freezeTime(bool f)
    {
        froze = f;
        if (f)
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0 * Time.fixedDeltaTime;
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = baseFixedDeltaTime;
        }
    }
}
