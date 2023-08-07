using UnityEngine;

public class Slowmo : MonoBehaviour
{

    [SerializeField]
    float slowmoGameSpeed = 0.05f;

    bool froze;

    public bool slow = false;

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
            slow = true;
        }
        else if (Input.GetMouseButtonUp(0)) {
            slow = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = baseFixedDeltaTime;
        }
    }

    public void setDefault()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = baseFixedDeltaTime;
    }

    public void freezeTime(bool f)
    {
        froze = f;
        if (f)
        {
            slow = true;
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0 * Time.fixedDeltaTime;
        }
        else
        {
            slow = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = baseFixedDeltaTime;
        }
    }
}
