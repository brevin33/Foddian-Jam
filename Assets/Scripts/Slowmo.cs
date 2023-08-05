using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmo : MonoBehaviour
{

    [SerializeField]
    float slowmoGameSpeed = 0.05f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = slowmoGameSpeed;
            Time.fixedDeltaTime = slowmoGameSpeed * Time.fixedDeltaTime;
        }
        else if (Input.GetMouseButtonUp(0)) {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.fixedDeltaTime/slowmoGameSpeed;
        }
    }
}
