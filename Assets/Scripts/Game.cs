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
    }
}