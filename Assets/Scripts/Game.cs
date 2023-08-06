using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class Game : MonoBehaviour
{
    [SerializeField]
    LeaderBoard leaderBoard;
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
        leaderBoard.setPlayer("20");
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            leaderBoard.showLeaderBoardText();
        }
        if (Input.GetMouseButtonDown(2))
        {
            leaderBoard.submitScore();
        }
    }
}