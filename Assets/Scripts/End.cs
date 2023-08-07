using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    [SerializeField]
    GameObject getName;

    [SerializeField]
    LeaderBoard learderBoard;

    [SerializeField]
    TMP_InputField nameInput;

    [SerializeField]
    TextMeshProUGUI timeText;

    [SerializeField]
    Slowmo slowmo;

    string name;

    public bool nameSubmited = false;

    float time = 0;

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        timeText.text = time.ToString();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        slowmo.freezeTime(true);
        MenuController.won = true;
        Reset.work = false;
        StartCoroutine(getNameThenShowLeaderBoard());
    }
    

    IEnumerator getNameThenShowLeaderBoard()
    {
        getName.SetActive(true);
        name = null;
        yield return new WaitUntil(hasName);
        Reset.work = true;
        getName.SetActive(false);
        learderBoard.setPlayer(name);
        learderBoard.submitScore((int)time);
        yield return new WaitUntil(submitted);
        learderBoard.showLeaderBoardText();
    }


    bool hasName()
    {
        return name != null;
    }

    bool submitted()
    {
        return nameSubmited;
    }

    public void gotName() {
        name = nameInput.text;
    }




}
